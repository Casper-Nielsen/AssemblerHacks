using Jack.Model.VMModels;

namespace Jack.Converter;

/// <summary>
/// A Class used to hold the information about variables to be used to create vmModels like push or pop
/// </summary>
public class Field
{
    public string Address { get; }
    public string DataType { get; }
    public int Position { get; set; }
    public DataLocation Location { get; }

    public Field() { }
    public Field(string address, string dataType, DataLocation location, int position)
    {
        DataType = dataType;
        Address = address;
        Location = location;
        Position = position;
    }
}