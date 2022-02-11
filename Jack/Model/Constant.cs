namespace Jack.Model;

public class Constant : ValueHolder
{
    public Constant() : base(CommandType.CONSTANT)
    { }
    public Constant(int value) : base(CommandType.CONSTANT)
    {
        Value = value.ToString();
    }
}