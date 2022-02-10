namespace Jack.Model;

public class ArgCommand : ValueHolder
{
    public string Datatype { get; set; }
    
    public ArgCommand() : base(CommandType.Arg)
    {
    }
}