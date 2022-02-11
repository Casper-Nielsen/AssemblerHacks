using VM.Model;

namespace VM.VMTranslator.CommandFinder
{
    /// <summary>
    /// Searches the line for a arithmetic command
    /// </summary>
    public class ArithmeticFinder: ICommandFinder
    {
        private readonly List<string> _arithmeticCommands;

        public ArithmeticFinder()
        {
            _arithmeticCommands = new List<string>()
            {
                "add",
                "sub",
                "neg",
                "eq",
                "gt",
                "lt",
                "and",
                "or",
                "not"
            };
        }

        public void Search(string line, ref Command command, ref bool found)
        {
            if (found) return;
            if (!_arithmeticCommands.Contains(line)) return;
            command = new ArithmeticCommand() {Method = Enum.Parse<Method>(line.ToUpper())};
            found = true;
        }
    }
}