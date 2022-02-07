namespace Jack.Model;

public class Operation : Command
{
    public string Value { get; set; }

    public Operation() { }
    public Operation(string value) : base(CommandType.Operation)
    {
        Value = value;
    }
}