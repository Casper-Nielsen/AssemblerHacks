using System;
using System.Collections.Generic;
using VM.Model;

namespace VM.VMTranslator.CommandFinder
{
    public class ArithmeticFinder: ICommandFinder
    {
        private List<string> _arithmeticCommands;

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