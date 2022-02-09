namespace Jack.Model.VMModels;

public class IfGoTo : GoTo
{
    public IfGoTo(string location) : base(location)
    { }
    
    public override string ToString()
    {
        return $"if-goto {Location}";
    }


}