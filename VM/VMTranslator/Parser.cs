using VM.Model;
using VM.VMTranslator.CommandFinder;

namespace VM.VMTranslator
{
    /// <summary>
    /// Parses a vm code line to commands 
    /// </summary>
    public class Parser
    {
        private readonly ICommandFinder[] _commandFinders;
        private readonly FileStream _file;
        private readonly StreamReader _sr;

        public bool HasMore => !_sr.EndOfStream;

        public Parser(string path)
        {
            _commandFinders = new ICommandFinder[]
            {
                new ArithmeticFinder(),
                new MemoryFinder(),
                new LabelFinder(),
                new GoToFinder(),
                new FunctionFinder(),
                new CallFinder(),
                new ReturnFinder()
                
            };
            
            _file = new FileStream(path, FileMode.Open, FileAccess.Read);
            _sr = new StreamReader(_file);
        }

        /// <summary>
        /// Gets the next command from the vm code
        /// </summary>
        /// <returns>The next command</returns>
        public Command GetNext()
        {
            var line = _sr.ReadLine();
            if (line == null) return new Command();
            var removeIndex = line.IndexOf("//", StringComparison.Ordinal);
            line = removeIndex >= 0 ? line.Remove(removeIndex).Trim() : line.Trim();
            
            var found = false;
            var command = new Command();
            
            foreach (var commandFinder in _commandFinders)
            {
                commandFinder.Search(line,ref command, ref found);
                if(found)
                    break;
            }

            return command;
        }

        /// <summary>
        /// Closes the file stream
        /// </summary>
        public void Close()
        {
            _sr.Close();
            _file.Close();
        }
    }
}