namespace Jack.Model.VMModels;

public class Function : IVmModel
{
    public int LclVariables { get; set; }
    public string Name { get; }

    public Function(string name, int lclVariables)
    {
        Name = name;
        LclVariables = lclVariables;
    }
    
    public override string ToString()
    {
        return $"function {Name} {LclVariables.ToString()}";
    }
}