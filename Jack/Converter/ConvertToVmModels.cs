using Jack.Model;
using Jack.Model.VMModels;
using Operation = Jack.Model.Operation;

namespace Jack.Converter;

public class ConvertToVmModels
{
    private string _currentClass = "";
    private int _uniCount;

    private List<Field> _tempLclList = new List<Field>();
    private List<Field> _tempArgList = new List<Field>();
    
    private List<Field> _lclList = new List<Field>();
    private List<Field> _fieldList = new List<Field>();
    private List<Field> _staticList = new List<Field>();
    private List<Field> _argList = new List<Field>();

    private Field? GetField(string name)
    {
        var lclField = _lclList.FirstOrDefault(f => f.Address == name);
        if (lclField != null) return lclField;
        var fieldField = _fieldList.FirstOrDefault(f => f.Address == name);
        if (fieldField != null) return fieldField;
        var staticField = _staticList.FirstOrDefault(f => f.Address == name);
        if (staticField != null) return staticField;
        var argField = _argList.FirstOrDefault(f => f.Address == name);
        return argField;
    }

    public List<IVmModel> Convert(IReadOnlyCollection<Command> commands)
    {
        var vmModels = new List<IVmModel>();
        foreach (var command in commands)
        {
            switch (command)
            {
                case LetStatement letStatement:
                    vmModels.AddRange(ConvertLet(letStatement));
                    break;
                case VarCommand varCommand:
                    ConvertVar(varCommand);
                    break;
                case IfStatement ifStatement:
                    vmModels.AddRange(ConvertIf(ifStatement));
                    break;
                case ElseCommand elseCommand:
                    var ifLabel = vmModels[^1];
                    vmModels.RemoveAt(vmModels.Count-1);
                    vmModels.AddRange(ConvertElse(elseCommand, ifLabel));
                    break;
                case WhileStatement whileStatement:
                    vmModels.AddRange(ConvertWhile(whileStatement));
                    break;
                case DoCommand doCommand:
                    vmModels.AddRange(ConvertDo(doCommand));
                    break;
                case FunctionCommand functionCommand:
                    vmModels.AddRange(ConvertFunction(functionCommand));
                    break;
                case ReturnCommand returnCommand:
                    vmModels.AddRange(ConvertReturn(returnCommand));
                    break;
                case ClassCommand classCommand:
                    vmModels.AddRange(ConvertClass(classCommand));
                    break;
                case CtorCommand ctorCommand:
                    vmModels.AddRange(ConvertCtor(ctorCommand));
                    break;
            }
        }
        return vmModels;
    }

    private List<IVmModel> ConvertCtor(CtorCommand ctorCommand)
    {
        var vmModels = new List<IVmModel>();
        
        var tempArgList = _argList;
        var tempLclList = _lclList;
        _argList = new List<Field>();
        _lclList = new List<Field>();

        vmModels.Add(new Push("CONSTANT", DataLocation.CONSTANT, _fieldList.Count));
        vmModels.Add(new Call("call Memory.alloc 1", 1));
        vmModels.Add(new Pop("POINTER",DataLocation.POINTER, 0));
        
        vmModels.AddRange(Convert(ctorCommand.Statements));
        
        _argList = tempArgList;
        _lclList = tempLclList;
        
        return vmModels;
    }
    
    private List<IVmModel> ConvertLet(LetStatement letStatement)
    {
        var vmModels = new List<IVmModel>();
        var field = GetField(((ValueHolder) letStatement.VarName).Value);
        if (letStatement.Expression is DoCommand doCommand)
        {
            vmModels.AddRange(ConvertDo(doCommand));
        }
        else
            vmModels.AddRange(ConvertExpression(letStatement.Expression));
        vmModels.Add(new Pop(field));
        return vmModels;
    }
    
    private List<IVmModel> ConvertExpression(Command command)
    {
        var vmModels = new List<IVmModel>();
        switch (command)
        {
            case Expression expression:
            {
                if(expression.Term is Constant expressionTermConstant)
                    if(int.Parse(expressionTermConstant.Value) >= 0)
                        vmModels.Add(new Push("constant", DataLocation.CONSTANT, int.Parse(expressionTermConstant.Value)));
                    else
                    {
                        vmModels.Add(new Push("constant", DataLocation.CONSTANT,  Math.Abs(int.Parse(expressionTermConstant.Value))));
                        vmModels.Add(new VMOperation(VMOperation.OperationEnum.NEG));
                    }
                else
                    vmModels.Add(new Push(GetField(((ValueHolder)expression.Term).Value)));
                var temp = ConvertExpression(expression.OpTerm);
                if(temp.Count > 1 && temp[1] is VMOperation {Operation: VMOperation.OperationEnum.NEG})
                {
                    vmModels.Add(temp[0]);
                    vmModels.Add(temp[1]);
                    temp.RemoveAt(0);
                }else
                    vmModels.Add(temp[0]);
                    
                temp.RemoveAt(0);
                
                vmModels.Add(new VMOperation(((Operation)expression.Operation).Value));

                vmModels.AddRange(temp);
                break;
            }
            case VarName varName:
                vmModels.Add(new Push(GetField(varName.Value)));
                break;
            case Constant vConstant:
                if(int.Parse(vConstant.Value) >= 0)
                    vmModels.Add(new Push("constant", DataLocation.CONSTANT, int.Parse(vConstant.Value)));
                else
                {
                    vmModels.Add(new Push("constant", DataLocation.CONSTANT,  Math.Abs(int.Parse(vConstant.Value))));
                    vmModels.Add(new VMOperation(VMOperation.OperationEnum.NEG));
                }
                break;
            case DoCommand doCommand:
                vmModels.AddRange(ConvertDo(doCommand));
                break;
        }
        return vmModels;
    }

    private void ConvertVar(VarCommand varCommand)
    {
        switch (varCommand.Type)
        {
            case VarCommand.VarType.VAR:
                _lclList.Add(
                    new Field(((ValueHolder)varCommand.VarName).Value, 
                        DataLocation.LOCAL, 
                        _lclList.Count));
                break;
            case VarCommand.VarType.FIELD:
                _fieldList.Add(
                    new Field(((ValueHolder) varCommand.VarName).Value, 
                        DataLocation.THIS, 
                        _fieldList.Count));
                break;
            case VarCommand.VarType.STATIC:
                _staticList.Add(
                    new Field(((ValueHolder) varCommand.VarName).Value, 
                        DataLocation.STATIC, 
                        _staticList.Count));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private List<IVmModel> ConvertDo(DoCommand doCommand)
    {
        var vmModels = new List<IVmModel>();
        foreach (var valueHolder in doCommand.ValueHolders)
        {
            vmModels.AddRange(ConvertValueHolder((ValueHolder)valueHolder));
        }
        vmModels.Add(new Call($"{_currentClass}.{doCommand.methodName}", doCommand.ValueHolders.Count));
        return vmModels;
    }    
    
    private List<IVmModel> ConvertIf(IfStatement ifStatement)
    {
        var vmModels = new List<IVmModel>();
        var uniLabel = UniqeLabel("If");
        vmModels.AddRange(ConvertExpression(ifStatement.Expression));
        vmModels.Add(new VMOperation(VMOperation.OperationEnum.NOT));
        vmModels.Add(new IfGoTo(uniLabel));
        vmModels.AddRange(Convert(ifStatement.Statements));
        vmModels.Add(new Label(uniLabel));
        return vmModels;
    }

    private List<IVmModel> ConvertClass(ClassCommand classCommand)
    {
        var vmModels = new List<IVmModel>();
        _currentClass = classCommand.ClassName.Value;
        vmModels.AddRange(Convert(classCommand.ClassBody));
        return vmModels;
    }
    
    private List<IVmModel> ConvertElse(ElseCommand elseCommand, IVmModel ifLabel)
    {
        var vmModels = new List<IVmModel>();
        var uniLabel = UniqeLabel("else");
        vmModels.Add(new GoTo(uniLabel));
        vmModels.Add(ifLabel);
        vmModels.AddRange(Convert(elseCommand.Statements));
        vmModels.Add(new Label(uniLabel));
        return vmModels;
    }

    private List<IVmModel> ConvertWhile(WhileStatement whileStatement)
    {
        var vmModels = new List<IVmModel>();
        var uniLabel = UniqeLabel("while");
        var uniLabelOut = UniqeLabel("whileOut");
        vmModels.Add(new Label(uniLabel));
        vmModels.AddRange(ConvertExpression(whileStatement.Expression));
        vmModels.Add(new VMOperation(VMOperation.OperationEnum.NOT));
        vmModels.Add(new IfGoTo(uniLabelOut));
        vmModels.AddRange(Convert(whileStatement.Statements));
        vmModels.Add(new GoTo(uniLabel));
        vmModels.Add(new Label(uniLabelOut));
        return vmModels;
    }

    private List<IVmModel> ConvertFunction(FunctionCommand functionCommand)
    {
        var vmModels = new List<IVmModel>();
        
        var tempArgList = _argList;
        var tempLclList = _lclList;
        _argList = new List<Field>();
        _lclList = new List<Field>();
        
        vmModels.Add(new Function($"{_currentClass}.{functionCommand.FunctionName.Value}", functionCommand.Statements.Where(s => s is VarCommand).ToArray().Length));
        foreach (var valueHolder in functionCommand.ValueHolders)
        {
            _argList.Add(new Field(valueHolder.Value, DataLocation.ARGUMENT, _argList.Count));    
        }
        vmModels.AddRange(Convert(functionCommand.Statements));
        
        _argList = tempArgList;
        _lclList = tempLclList;
        
        return vmModels;
    }

    private List<IVmModel> ConvertValueHolder(ValueHolder valueHolder)
    {
        var vmModels = new List<IVmModel>();
        
        if(valueHolder is Constant expressionTermConstant)
            if(int.Parse(expressionTermConstant.Value) >= 0)
                vmModels.Add(new Push("constant", DataLocation.CONSTANT, int.Parse(expressionTermConstant.Value)));
            else
            {
                vmModels.Add(new Push("constant", DataLocation.CONSTANT,  Math.Abs(int.Parse(expressionTermConstant.Value))));
                vmModels.Add(new VMOperation(VMOperation.OperationEnum.NEG));
            }
        else
            vmModels.Add(new Push(GetField(((ValueHolder)valueHolder).Value)));
        
        return vmModels;
    }
    
    private List<IVmModel> ConvertReturn(ReturnCommand returnCommand)
    {
        var vmModels = new List<IVmModel>();
        if (returnCommand.Value != null)
        {
            switch (returnCommand.Value)
            {
                case Expression expression:
                    vmModels.AddRange(ConvertExpression(expression));
                    break;
                case DoCommand doCommand:
                    vmModels.AddRange(ConvertDo(doCommand));
                    break;
                case ValueHolder valueHolder:
                    if (valueHolder.Value == "this")
                        vmModels.Add(new Push("POINTER", DataLocation.POINTER, 0));
                    else
                        vmModels.AddRange(ConvertValueHolder(valueHolder));
                    break;
            }
        }
        vmModels.Add(new Return());
        return vmModels;
    }

    private List<IVmModel> ConvertString(StringConstant stringConstant)
    {
        var vmModels = new List<IVmModel>();

        return vmModels;
    }

    private string UniqeLabel(string definer)
    {
        _uniCount++;
        return $"{definer}{System.Convert.ToString(_uniCount, 16)}";
    }
}