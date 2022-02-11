namespace Jack.Model;

public class StringConstant : ValueHolder
{
    public StringConstant() : base(CommandType.STRING)
    {
        
    }
    public StringConstant(string value) : base(CommandType.STRING)
    {
        Value = value;
    }
}