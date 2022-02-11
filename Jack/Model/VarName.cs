namespace Jack.Model;

public class VarName : ValueHolder
{
    public bool Neg { get; set; }
    public VarName() : base(CommandType.VAR_NAME)
    { }
    public VarName(string value) : base(CommandType.VAR_NAME)
    {
        Value = value;
    }
}