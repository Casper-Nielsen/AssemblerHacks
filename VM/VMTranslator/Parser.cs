using System;
using System.IO;
using VM.Model;
using VM.VMTranslator.CommandFinder;

namespace VM.VMTranslator
{
    public class Parser
    {
        private ICommandFinder[] _commandfinders;
        private FileStream _file;
        private StreamReader _sr;

        public bool HasMore
        {
            get => !_sr.EndOfStream;
        }
        
        public Parser(string path)
        {
            _commandfinders = new ICommandFinder[]
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

        public Command GetNext()
        {
            var line = _sr.ReadLine();
            if (line == null) return new Command();
            var removeIndex = line.IndexOf("//", StringComparison.Ordinal);
            line = removeIndex >= 0 ? line.Remove(removeIndex).Trim() : line.Trim();
            
            var found = false;
            Command command = new Command();
            
            foreach (var commandFinder in _commandfinders)
            {
                commandFinder.Search(line,ref command, ref found);
            }

            return command;
        }

        public void Close()
        {
            _sr.Close();
            _file.Close();
        }
    }
}