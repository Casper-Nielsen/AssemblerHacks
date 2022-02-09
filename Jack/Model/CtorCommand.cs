namespace Jack.Model;

public class CtorCommand : Command
{
    public ValueHolder FunctionName { get; set; }
    public List<ValueHolder> ValueHolders { get; set; }
    public List<Command?> Statements { get; set; }
    
    public CtorCommand() : base(CommandType.Ctor)
    {
        ValueHolders = new List<ValueHolder>();
        Statements = new List<Command?>();
    }
}