using VM.Model;

namespace VM.VMTranslator.CommandFinder
{
    public class GoToFinder : ICommandFinder
    {
        public void Search(string line, ref Command command, ref bool found)
        {
            if (found) return;
            if (!(line.StartsWith("goto") || line.StartsWith("if-goto"))) return;
            var lineArray = line.Split(' '); 
            command = new GoToCommand() { LabelName = lineArray[1], JumpIf = line.StartsWith("if-goto")};
            found = true;
        }
    }
}