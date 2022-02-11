namespace Jack.Model;

public class ClassCommand : Command
{
    public ValueHolder ClassName { get; set; }
    public List<Command?> ClassBody { get; set; }
    
    public ClassCommand() : base(CommandType.CLASS)
    {
        ClassBody = new List<Command?>();
    }
}