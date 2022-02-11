namespace Jack.Model;

public class ReturnCommand : Command
{
    public Command? Value { get; set; }
    public ReturnCommand() : base(CommandType.RETURN)
    {
    }
}