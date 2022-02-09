using Jack.Converter;

namespace Jack.Model.VMModels;

public class Pop : IVmModel
{
    public string Address { get; }
    public int Position { get; set; }
    public DataLocation Location { get; }

    public Pop(Field field)
    {
        Address = field.Address;
        Location = field.Location;
        Position = field.Position;
    }
    
    public Pop(string address, DataLocation location, int position)
    {
        Address = address;
        Location = location;
        Position = position;
    }
    
    public override string ToString()
    {
        return $"pop {Location.ToString().ToLower()} {Position.ToString()}";
    }
}