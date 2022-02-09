namespace Jack.Model.VMModels;

public class Label : IVmModel
{
    public string Name { get; }

    public Label(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        return $"label {Name}";
    }
}