namespace Jack.Model;

internal class Operation : Command
{
    public string Value { get; set; }
    
    public Operation(string value) : base(CommandType.Operation)
    {
        Value = value;
    }
}