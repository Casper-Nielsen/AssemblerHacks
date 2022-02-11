namespace Jack.Model;

public class VarCommand : Command
{
    public string VarName { get; set; }
    public string DataType { get; set; }
    public VarType Type { get; set; }

    public VarCommand() { }
    public VarCommand(string type) : base(CommandType.VAR)
    {
        Type = Enum.Parse<VarType>(type.ToUpper());
    }
    
    public VarCommand(string type, string dataType) : this(type)
    {
        DataType = dataType;
    }

    public enum VarType
    {
        STATIC,
        VAR,
        FIELD 
    }
}