namespace Jack.Model;

public class DoCommand : Command
{
    public bool NoReturn { get; set; }
    public string MethodName { get; set; }
    public List<Command> ValueHolders { get; set; }
    
    public DoCommand() : base(CommandType.DO)
    {
        ValueHolders = new List<Command>();
    }
}