using VM.Model;

namespace VM.VMTranslator.CommandFinder
{
    public class ReturnFinder : ICommandFinder
    {
        public void Search(string line, ref Command command, ref bool found)
        {
            if (found) return;
            if (!line.StartsWith("return")) return;
            command = new ReturnCommand();
            found = true;
        }
    }
}