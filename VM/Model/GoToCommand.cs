namespace VM.Model
{
    public class GoToCommand : Command
    {
        public bool JumpIf { get; set; }
        public string LabelName { get; set; }
    }
}