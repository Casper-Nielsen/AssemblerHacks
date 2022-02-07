namespace Jack.Model;

public class Statement : Command
{
    public Command? Expression { get; set; }
    public List<Command?> Statements { get; set; }

    public Statement() { }
    public Statement(CommandType type) : base(type)
    {
        Statements = new List<Command?>();
    }
}