namespace Jack.Model;

public class VarName : ValueHolder
{
    public VarName(string value) : base(CommandType.VarName)
    {
        Value = value;
    }
}