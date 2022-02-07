namespace Jack.Model;

public class Constant : ValueHolder
{
    public Constant() : base(CommandType.Constant)
    { }
    public Constant(int value) : base(CommandType.Constant)
    {
        Value = value.ToString();
    }
}