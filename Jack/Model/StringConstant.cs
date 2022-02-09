namespace Jack.Model;

public class StringConstant : ValueHolder
{
    public StringConstant() : base(CommandType.String)
    {
        
    }
    public StringConstant(string value) : base(CommandType.String)
    {
        Value = value;
    }
}