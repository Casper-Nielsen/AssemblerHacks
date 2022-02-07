namespace Jack.Model;

internal class LetStatement : Command
{
    public Command VarName { get; set; }
    public Command? Expression { get; set; }
    
    public LetStatement() : base(CommandType.letStatement)
    {
    }
}