using VM.Model;

namespace VM.VMTranslator.CommandFinder
{
    public class CallFinder : ICommandFinder
    {
        public void Search(string line, ref Command command, ref bool found)
        {
            if (found) return;
            if (!line.StartsWith("call")) return;
            
            var lineArray = line.Split(' ');
            
            command = new CallCommand
            {
                MethodName = lineArray[1],
                Amount = int.Parse(lineArray[2])
            };
            
            found = true;
        }
    }
}