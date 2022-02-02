namespace VM.Model
{
    public class CallCommand : Command
    {
        public string MethodName { get; set; }
        public int Amount { get; set; }
    }
}