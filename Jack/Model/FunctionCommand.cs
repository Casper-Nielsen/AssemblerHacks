namespace Jack.Model;

public class FunctionCommand : Command
{
    public string FunctionName { get; set; }
    public bool IsFunction { get; }
    public List<ArgCommand> ValueHolders { get; set; }
    public List<Command?> Statements { get; set; }
    
    public FunctionCommand() : base(CommandType.FUNCTION)
    {
        ValueHolders = new List<ArgCommand>();
        Statements = new List<Command?>();
    }

    public FunctionCommand(string type) : this()
    {
        IsFunction = type == "function";
    }
}