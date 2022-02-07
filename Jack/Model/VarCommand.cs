namespace Jack.Model;

public class VarCommand : Command
{
    public Command VarName { get; set; }
    public VarType Type { get; set; }

    public VarCommand() { }
    public VarCommand(string type) : base(CommandType.Var)
    {
        Type = Enum.Parse<VarType>(type.ToUpper());
    }

    public enum VarType
    {
        STATIC,
        VAR,
        FIELD 
    }
}