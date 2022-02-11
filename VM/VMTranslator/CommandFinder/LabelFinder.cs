using VM.Model;

namespace VM.VMTranslator.CommandFinder
{
    /// <summary>
    /// Searches the line for a label command
    /// </summary>
    public class LabelFinder : ICommandFinder
    {
        public void Search(string line, ref Command command, ref bool found)
        {
            if (found) return;
            if (!line.StartsWith("label")) return;
            var lineArray = line.Split(' '); 
            command = new LabelCommand() { LabelName = lineArray[1]};
            found = true;
        }
    }
}