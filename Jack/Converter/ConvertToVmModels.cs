using System.Text;
using Jack.Model;
using Jack.Model.VMModels;
using Operation = Jack.Model.Operation;

namespace Jack.Converter;

/// <summary>
/// Converts commands and statements to vm code
/// </summary>
public class ConvertToVmModels
{
    private string _currentClass = "";
    private int _uniCount;

    private List<string> _classes = new List<string>();
    
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
    
    public ConvertToVmModels()
    {
        // Adds known classes
        _classes.Add("Array");
        _classes.Add("Memory");
        _classes.Add("Screen");
        _classes.Add("Keyboard");
        _classes.Add("Sys");
    }
    
    /// <summary>
    /// Adds the classes to a list for later use
    /// </summary>
    /// <param name="commands">A collection of commands where the classes will be read from</param>
    public void AddClasses(IReadOnlyCollection<Command> commands)
    {
        foreach (var command in commands)
        {
            if (command is ClassCommand classCommand)
            {
                _classes.Add(classCommand.ClassName.Value);
            }
        }
    }
    
    /// <summary>
    /// Converts the list of commands and statements to vm code
    /// </summary>
    /// <param name="commands">the collection of commands and statements to be converted</param>
    /// <returns></returns>
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
                    vmModels.RemoveAt(vmModels.Count - 1);
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
                case StringConstant stringConstant:
                    vmModels.AddRange(ConvertString(stringConstant));
                    break;
            }
        }

        return vmModels;
    }

    /// <summary>
    /// Convertes a ctor command into vm code
    /// </summary>
    /// <param name="ctorCommand">The command that will be converted</param>
    /// <returns>A list of vm models</returns>
    private List<IVmModel> ConvertCtor(CtorCommand ctorCommand)
    {
        var vmModels = new List<IVmModel>();

        var tempArgList = _argList;
        var tempLclList = _lclList;
        _argList = new List<Field>();
        _lclList = new List<Field>();


        vmModels.Add(new Function($"{_currentClass}.{ctorCommand.FunctionName}",
            ctorCommand.Statements.Where(s => s is VarCommand).ToArray().Length));        
        vmModels.Add(new Push("CONSTANT", DataLocation.CONSTANT, _fieldList.Count));
        vmModels.Add(new Call("Memory.alloc", 1));
        vmModels.Add(new Pop("POINTER", DataLocation.POINTER, 0));

        vmModels.AddRange(Convert(ctorCommand.Statements));

        _argList = tempArgList;
        _lclList = tempLclList;

        return vmModels;
    }

    /// <summary>
    /// Converts a let command to vm models
    /// </summary>
    /// <param name="letStatement">The command that will be converted</param>
    /// <returns>A list of vm models</returns>
    private List<IVmModel> ConvertLet(LetStatement letStatement)
    {
        var vmModels = new List<IVmModel>();
        var field = new Field();
        
        if (letStatement.Expression is DoCommand doCommand)
            vmModels.AddRange(ConvertDo(doCommand));
        else
            vmModels.AddRange(ConvertExpression(letStatement.Expression));

        switch (letStatement.VarName)
        {
            case VarName varName:
                field = GetField(varName.Value);
                vmModels.Add(new Pop(field));
                break;
            case ArrayVarCommand arrayVarCommand:
                vmModels.AddRange(ConvertArray(arrayVarCommand));
                vmModels.Add(new Pop("THAT", DataLocation.THAT, 0));
                break;
        }

        return vmModels;
    }

    /// <summary>
    /// Converts a array var command to vm code
    /// </summary>
    /// <param name="arrayVarCommand">The command that will be converted</param>
    /// <returns>A list of vm models</returns>
    private List<IVmModel> ConvertArray(ArrayVarCommand arrayVarCommand)
    {
        var vmModels = new List<IVmModel>();
        var field = GetField(arrayVarCommand.Value);
        
        vmModels.Add(new Push(field));
        if (arrayVarCommand.index is Expression indexExpression)
        {
            vmModels.AddRange(ConvertExpression(indexExpression));    
        }
        else if (arrayVarCommand.index is Constant indexConstant)
        {
            vmModels.Add(new Push("CONSTANT", DataLocation.CONSTANT, int.Parse(indexConstant.Value)));
        }
        else if (arrayVarCommand.index is VarName indexName)
        {
            var indexField = GetField(indexName.Value);
            vmModels.Add(new Push(indexField));
        }
        vmModels.Add(new VMOperation(VMOperation.OperationEnum.ADD));
        vmModels.Add(new Pop("POINTER", DataLocation.POINTER, 1));
        
        return vmModels;
    }

    /// <summary>
    /// Converts a Expression command to vm code
    /// </summary>
    /// <param name="command">The command that will be converted</param>
    /// <returns>A list of vm models</returns>
    private List<IVmModel> ConvertExpression(Command command)
    {
        var vmModels = new List<IVmModel>();
        switch (command)
        {
            case Expression expression:
            {
                switch (expression.Term)
                {
                    case Constant expressionTermConstant when int.Parse(expressionTermConstant.Value) >= 0:
                        vmModels.Add(new Push("constant", DataLocation.CONSTANT,
                            int.Parse(expressionTermConstant.Value)));
                        break;
                    case Constant expressionTermConstant:
                        vmModels.Add(new Push("constant", DataLocation.CONSTANT,
                            Math.Abs(int.Parse(expressionTermConstant.Value))));
                        vmModels.Add(new VMOperation(VMOperation.OperationEnum.NEG));
                        break;
                    case VarName varName:
                        vmModels.Add(new Push(GetField(varName.Value)));
                        break;
                    case DoCommand doCommand:
                        vmModels.AddRange(ConvertDo(doCommand));
                        break;
                    case Expression expressionLeft:
                        vmModels.AddRange(ConvertExpression(expressionLeft));
                        break;
                    case ArrayVarCommand arrayVarCommand:
                        vmModels.AddRange(ConvertArray(arrayVarCommand));
                        vmModels.Add(new Push("THAT", DataLocation.THAT, 0));
                        break;
                }
                
                // Gets the 2nd part of the expression
                var temp = ConvertExpression(expression.OpTerm);
                
                // Checks if the 2nd part Should be calculated before the operation
                if (expression.OpTerm is Expression {First: true})
                {
                    vmModels.AddRange(temp);
                    vmModels.Add(new VMOperation(((Operation) expression.Operation).Value));
                }
                else
                {
                    if (temp.Count > 1 && temp[1] is VMOperation {Operation: VMOperation.OperationEnum.NEG})
                    {
                        vmModels.Add(temp[0]);
                        vmModels.Add(temp[1]);
                        temp.RemoveAt(0);
                    }
                    else
                        vmModels.Add(temp[0]);

                    temp.RemoveAt(0);

                    vmModels.Add(new VMOperation(((Operation) expression.Operation).Value));

                    vmModels.AddRange(temp);
                }
                break;
            }
            case VarName varName:
                if (!varName.Neg)
                    vmModels.Add(new Push(GetField(varName.Value)));
                else
                {
                    vmModels.Add(new Push(GetField(varName.Value)));
                    vmModels.Add(new VMOperation(VMOperation.OperationEnum.NEG));
                }
                break;
            case Constant vConstant:
                if (int.Parse(vConstant.Value) >= 0)
                    vmModels.Add(new Push("constant", DataLocation.CONSTANT, int.Parse(vConstant.Value)));
                else
                {
                    vmModels.Add(new Push("constant", DataLocation.CONSTANT, Math.Abs(int.Parse(vConstant.Value))));
                    vmModels.Add(new VMOperation(VMOperation.OperationEnum.NEG));
                }

                break;
            case DoCommand doCommand:
                vmModels.AddRange(ConvertDo(doCommand));
                break;
            case ArrayVarCommand arrayVarCommand:
                vmModels.AddRange(ConvertArray(arrayVarCommand));
                vmModels.Add(new Push("THAT", DataLocation.THAT, 0));
                break;
            case StringConstant stringConstant:
                vmModels.AddRange(ConvertString(stringConstant));
                break;
        }

        return vmModels;
    }

    /// <summary>
    /// Converts a var command and adds it to one of the variable-lists
    /// </summary>
    /// <param name="varCommand">The command that will be converted</param>
    private void ConvertVar(VarCommand varCommand)
    {
        switch (varCommand.Type)
        {
            case VarCommand.VarType.VAR:
                _lclList.Add(
                    new Field(varCommand.VarName,
                        varCommand.DataType,
                        DataLocation.LOCAL,
                        _lclList.Count));
                break;
            case VarCommand.VarType.FIELD:
                _fieldList.Add(
                    new Field(varCommand.VarName,
                        varCommand.DataType,
                        DataLocation.THIS,
                        _fieldList.Count));
                break;
            case VarCommand.VarType.STATIC:
                _staticList.Add(
                    new Field(varCommand.VarName,
                        varCommand.DataType,
                        DataLocation.STATIC,
                        _staticList.Count));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// Converts a do command to vm code
    /// </summary>
    /// <param name="doCommand">The command that will be converted</param>
    /// <returns>A list of vm models</returns>
    private List<IVmModel> ConvertDo(DoCommand doCommand)
    {
        var vmModels = new List<IVmModel>();

        var obj = GetField(doCommand.methodName.Split('.')[0]);
        
        if (!_classes.Contains(doCommand.methodName.Split('.')[0]))
        {
            vmModels.Add(obj == null ? new Push("POINTER", DataLocation.POINTER, 0) : new Push(obj));
        }

        // Converts the values that will go into the arguments
        foreach (var valueHolder in doCommand.ValueHolders)
        {
            switch (valueHolder)
            {
                case ThisHolder:
                    vmModels.AddRange(ConvertThis());
                    break;
                case Expression expression:
                    vmModels.AddRange(ConvertExpression(expression));
                    break;
                case ValueHolder trueValueHolder:
                    vmModels.AddRange(ConvertValueHolder(trueValueHolder));
                    break;
            }
        }
        if (_classes.Contains(doCommand.methodName.Split('.')[0]))
            vmModels.Add(new Call(
                $"{doCommand.methodName}", 
                doCommand.ValueHolders.Count));
        else if (obj == null)
            vmModels.Add(new Call(
                $"{_currentClass}.{doCommand.methodName}", 
                doCommand.ValueHolders.Count + 1));
        else
            vmModels.Add(new Call(
                $"{obj.DataType}.{string.Join(".", doCommand.methodName.Split('.')[1..])}",
                doCommand.ValueHolders.Count + 1));
        
        if(doCommand.NoReturn)
            vmModels.Add(new Pop("TEMP",DataLocation.TEMP, 0));

        return vmModels;
    }

    /// <summary>
    /// Converts a if statement to vm code
    /// </summary>
    /// <param name="ifStatement">The statement that will be converted</param>
    /// <returns>A list of vm models</returns>
    private List<IVmModel> ConvertIf(IfStatement ifStatement)
    {
        var vmModels = new List<IVmModel>();
        var uniLabel = UniqueLabel("If");
        vmModels.AddRange(ConvertExpression(ifStatement.Expression));
        vmModels.Add(new VMOperation(VMOperation.OperationEnum.NOT));
        vmModels.Add(new IfGoTo(uniLabel));
        vmModels.AddRange(Convert(ifStatement.Statements));
        vmModels.Add(new Label(uniLabel));
        return vmModels;
    }

    /// <summary>
    /// Converts a class command into vm code
    /// </summary>
    /// <param name="classCommand">The command that will be converted</param>
    /// <returns>A list of vm models</returns>
    private List<IVmModel> ConvertClass(ClassCommand classCommand)
    {
        var vmModels = new List<IVmModel>();
        _currentClass = classCommand.ClassName.Value;
        var tempFieldList = _fieldList;
        _fieldList = new List<Field>();
        vmModels.AddRange(Convert(classCommand.ClassBody));
        _fieldList = tempFieldList;
        return vmModels;
    }

    /// <summary>
    /// Converts a else command to vm code
    /// </summary>
    /// <param name="elseCommand">The command that will be converted</param>
    /// <param name="ifLabel">The label that is used to jump in the if command</param>
    /// <returns>A list of vm models</returns>
    private List<IVmModel> ConvertElse(ElseCommand elseCommand, IVmModel ifLabel)
    {
        var vmModels = new List<IVmModel>();
        var uniLabel = UniqueLabel("else");
        vmModels.Add(new GoTo(uniLabel));
        vmModels.Add(ifLabel);
        vmModels.AddRange(Convert(elseCommand.Statements));
        vmModels.Add(new Label(uniLabel));
        return vmModels;
    }

    /// <summary>
    /// Converts a while statement to vm code
    /// </summary>
    /// <param name="whileStatement">The statement that will be converted</param>
    /// <returns>A list of vm models</returns>
    private List<IVmModel> ConvertWhile(WhileStatement whileStatement)
    {
        var vmModels = new List<IVmModel>();
        var uniLabel = UniqueLabel("while");
        var uniLabelOut = UniqueLabel("whileOut");
        vmModels.Add(new Label(uniLabel));
        vmModels.AddRange(ConvertExpression(whileStatement.Expression));
        vmModels.Add(new VMOperation(VMOperation.OperationEnum.NOT));
        vmModels.Add(new IfGoTo(uniLabelOut));
        vmModels.AddRange(Convert(whileStatement.Statements));
        vmModels.Add(new GoTo(uniLabel));
        vmModels.Add(new Label(uniLabelOut));
        return vmModels;
    }

    /// <summary>
    /// Converts a function command to vm code 
    /// </summary>
    /// <param name="functionCommand">The command that will be converted</param>
    /// <returns>A list of vm models</returns>
    private List<IVmModel> ConvertFunction(FunctionCommand functionCommand)
    {
        var vmModels = new List<IVmModel>();

        var tempArgList = _argList;
        var tempLclList = _lclList;
        _argList = new List<Field>(){new Field()};
        _lclList = new List<Field>();

        vmModels.Add(new Function($"{_currentClass}.{functionCommand.FunctionName}",
            functionCommand.Statements.Where(s => s is VarCommand).ToArray().Length));
        foreach (var valueHolder in functionCommand.ValueHolders)
        {
            _argList.Add(new Field(
                valueHolder.Value,
                valueHolder.Datatype,
                DataLocation.ARGUMENT,
                _argList.Count));
        }

        if (!functionCommand.IsFunction)
        {
            vmModels.Add(new Push("ARGUMENT", DataLocation.ARGUMENT, 0));
            vmModels.Add(new Pop("POINTER", DataLocation.POINTER, 0));
        }
        vmModels.AddRange(Convert(functionCommand.Statements));

        _argList = tempArgList;
        _lclList = tempLclList;

        return vmModels;
    }

    /// <summary>
    /// Converts a value holder to vm code 
    /// </summary>
    /// <param name="valueHolder">the value older that should be converted</param>
    /// <returns>A list of vm models</returns>
    private List<IVmModel> ConvertValueHolder(ValueHolder valueHolder)
    {
        var vmModels = new List<IVmModel>();

        if (valueHolder is Constant expressionTermConstant)
            if (int.Parse(expressionTermConstant.Value) >= 0)
                vmModels.Add(new Push("constant", DataLocation.CONSTANT, int.Parse(expressionTermConstant.Value)));
            else
            {
                vmModels.Add(new Push("constant", DataLocation.CONSTANT,
                    Math.Abs(int.Parse(expressionTermConstant.Value))));
                vmModels.Add(new VMOperation(VMOperation.OperationEnum.NEG));
            }
        else
            vmModels.Add(new Push(GetField(valueHolder.Value)));

        return vmModels;
    }

    /// <summary>
    /// Converts a return command to vm code
    /// </summary>
    /// <param name="returnCommand">The command that will be converted</param>
    /// <returns>A list of vm models</returns>
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
                case ThisHolder:
                    vmModels.AddRange(ConvertThis());
                    break;
            }
        }else
        {
            vmModels.Add(new Push("CONSTANT", DataLocation.CONSTANT, 0));
        }

        vmModels.Add(new Return());
        return vmModels;
    }

    /// <summary>
    /// Converts a string constant to vm code
    /// </summary>
    /// <param name="stringConstant">The string constant that will be converted</param>
    /// <returns>A list of vm models</returns>
    private List<IVmModel> ConvertString(StringConstant stringConstant)
    {
        var vmModels = new List<IVmModel>();
        
        vmModels.Add(new Push("CONSTANT",DataLocation.CONSTANT, stringConstant.Value.Length));
        vmModels.Add(new Call("String.new", 1));
        
        foreach (var charPart in stringConstant.Value)
        {
            vmModels.Add(new Push("CONSTANT",DataLocation.CONSTANT, charPart));
            vmModels.Add(new Call("String.appendChar", 2));
        }
        return vmModels;
    }

    /// <summary>
    /// Converts this into vm code
    /// </summary>
    /// <returns>A list of vm models</returns>
    private List<IVmModel> ConvertThis()
    {
        var vmModels = new List<IVmModel>();
        vmModels.Add(new Push("POINTER", DataLocation.POINTER, 0));
        return vmModels;
    }
    
    /// <summary>
    /// Creates a unique
    /// </summary>
    /// <param name="definer"></param>
    /// <returns></returns>
    private string UniqueLabel(string definer)
    {
        _uniCount++;
        return $"{definer}{System.Convert.ToString(_uniCount, 16)}";
    }
}