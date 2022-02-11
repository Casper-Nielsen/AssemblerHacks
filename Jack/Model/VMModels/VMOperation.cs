namespace Jack.Model.VMModels;

public class VmOperation : IVmModel
{
    public OperationEnum Operation { get; set; }
    public VmOperation(OperationEnum operation)
    {
        Operation = operation;
    }
    public VmOperation(string operation)
    {
        switch (operation)
        {
            case "+":
                Operation = OperationEnum.ADD;
                break;
            case "-" :
                Operation = OperationEnum.SUB;
                break;
            case "&" :
                Operation = OperationEnum.AND;
                break;
            case "|" :
                Operation = OperationEnum.OR;
                break;
            case ">" :
                Operation = OperationEnum.GT;
                break;
            case "<" :
                Operation = OperationEnum.LT;
                break;
            case "=" :
                Operation = OperationEnum.EQ;
                break;
        }
    }
    
    public override string ToString()
    {
        return $"{Operation.ToString().ToLower()}";
    }

    public enum OperationEnum
    {

        ADD,
        SUB,
        NEG,
        EQ,
        GT,
        LT,
        AND,
        OR,
        NOT
    }
}