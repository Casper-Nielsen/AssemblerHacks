namespace Jack.Model;

public class FunctionCommand : Command
{
    public ValueHolder FunctionName { get; set; }
    public List<ValueHolder> ValueHolders { get; set; }
    public List<Command?> Statements { get; set; }
    
    public FunctionCommand() : base(CommandType.Function)
    {
        ValueHolders = new List<ValueHolder>();
        Statements = new List<Command?>();
    }
}