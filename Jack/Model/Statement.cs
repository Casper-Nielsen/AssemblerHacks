namespace Jack.Model;

public abstract class Statement : Command
{
    public Command? Expression { get; set; }
    public List<Command?> Statements { get; set; }
    
    protected Statement(CommandType type) : base(type)
    {
        Statements = new List<Command?>();
    }
}