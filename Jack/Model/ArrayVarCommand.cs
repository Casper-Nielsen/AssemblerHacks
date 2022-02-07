namespace Jack.Model;

public class ArrayVarCommand : ValueHolder
{
    public Command index { get; set; }
    
    public ArrayVarCommand() : base(CommandType.ArrayVar)
    {
    }
}