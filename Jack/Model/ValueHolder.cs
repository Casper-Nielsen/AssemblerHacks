namespace Jack.Model;

public abstract class ValueHolder : Command
{
    public string Value { get; set; }
    
    public ValueHolder(CommandType type) : base(type)
    {
    }
}