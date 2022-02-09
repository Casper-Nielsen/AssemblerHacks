using Jack.Model.VMModels;

namespace Jack.Converter;

public class Field
{
    public string Address { get; }
    public int Position { get; set; }
    public DataLocation Location { get; }

    public Field(string address, DataLocation location, int position)
    {
        Address = address;
        Location = location;
        Position = position;
    }
}