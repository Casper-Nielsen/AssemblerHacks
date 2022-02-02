using System;
using System.Collections.Generic;
using VM.Model;

namespace VM.VMTranslator.CommandFinder
{
    public class FunctionFinder : ICommandFinder
    {
        private List<string> _functions;

        public FunctionFinder()
        {
            _functions = new List<string>();
        }
        
        public void Search(string line, ref Command command, ref bool found)
        {
            if (found) return;
            if (!line.StartsWith("function")) return;
            
            var lineArray = line.Split(' ');
            
            if (_functions.Contains(lineArray[1]))
                throw new Exception("2 functions can not have the same name");
            
            _functions.Add(lineArray[1]);
            
            command = new FunctionCommand() {Name = lineArray[1], ArgNumber = int.Parse(lineArray[2])};
            
            found = true;
        }
    }
}