namespace Jack.Model.VMModels;

public class GoTo : IVmModel
{
    public string Location { get; }

    public GoTo(string location)
    {
        Location = location;
    }

    public override string ToString()
    {
        return $"goto {Location}";
    }
}