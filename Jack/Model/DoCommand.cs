namespace Jack.Model;

public class DoCommand : Command
{
    public Command? methodName { get; set; }
    public List<Command> ValueHolders { get; set; }
    
    public DoCommand() : base(CommandType.Do)
    {
        ValueHolders = new List<Command>();
    }
}