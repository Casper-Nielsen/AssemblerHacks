namespace Jack.Model;

public class ArrayVarCommand : ValueHolder
{
    public Command Index { get; set; }
    
    public ArrayVarCommand() : base(CommandType.ARRAY_VAR)
    {
    }
}