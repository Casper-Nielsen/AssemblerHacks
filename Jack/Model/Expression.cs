namespace Jack.Model;

public class Expression : Command
{
    public Command? Term { get; set; }
    public Command OpTerm { get; set; }
    public Command Operation { get; set; }
    public bool First { get; set; }

    public Expression() : base(CommandType.EXPRESSION)
    {
    }
}