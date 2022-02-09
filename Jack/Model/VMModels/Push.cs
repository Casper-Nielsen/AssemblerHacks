using Jack.Converter;

namespace Jack.Model.VMModels;

public class Push : IVmModel
{
    public string Address { get; }
    public int Position { get; set; }
    public DataLocation Location { get; }

    public Push(Field field)
    {
        Address = field.Address;
        Location = field.Location;
        Position = field.Position;
    }
    
    public Push(string address, DataLocation location, int position)
    {
        Address = address;
        Location = location;
        Position = position;
    }

    public override string ToString()
    {
        return $"push {Location.ToString().ToLower()} {Position.ToString()}";
    }
}