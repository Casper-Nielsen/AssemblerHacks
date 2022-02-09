namespace Jack.Model.VMModels;

public class Call : IVmModel
{
    public string Method { get; }
    public int ArgAmount { get; }

    public Call(string method, int argAmount)
    {
        Method = method;
        ArgAmount = argAmount;
    }

    public override string ToString()
    {
        return $"call {Method} {ArgAmount.ToString()}";
    }
}