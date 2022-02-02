using System;
using System.Collections.Generic;
using VM.Model;

namespace VM.VMTranslator.CommandFinder
{
    public class MemoryFinder : ICommandFinder
    {
        private readonly List<string> _memoryCommands;

        public MemoryFinder()
        {
            _memoryCommands = new List<string>()
            {
                "pop",
                "push"
            };
        }

        public void Search(string line, ref Command command, ref bool found)
        {
            if (found) return;
            var lineArray = line.Split(' ');
            if (!_memoryCommands.Contains(lineArray[0])) return;

            var location = Enum.Parse<Location>(lineArray[1].ToUpper());

            if (int.TryParse(lineArray[2], out var address))
            {
                command = new MemoryCommand()
                {
                    Method = Enum.Parse<MemoryMethod>(lineArray[0].ToUpper()),
                    Location = location,
                    Address = address
                };
            }

            found = true;
        }
    }
}