namespace Jack.Model;

public class CtorCommand : Command
{
    public string FunctionName { get; set; }
    public List<ValueHolder> ValueHolders { get; set; }
    public List<Command?> Statements { get; set; }
    
    public CtorCommand() : base(CommandType.CTOR)
    {
        ValueHolders = new List<ValueHolder>();
        Statements = new List<Command?>();
    }
}