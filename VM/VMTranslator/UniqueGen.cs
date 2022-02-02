using System;

namespace VM.VMTranslator
{
    internal class UniqueGen
    {
        int _uniqueNumber = 0;
        
        internal string GenUniqueLabel(string name)
        {
            var unique = name + Convert.ToString(_uniqueNumber, 8);
            _uniqueNumber++;
            return unique;
        }
    }
}