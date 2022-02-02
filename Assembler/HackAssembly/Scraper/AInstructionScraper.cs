using System;
using System.Collections.Generic;

namespace Assembler.HackAssembly.Scraper
{
    public class AInstructionScraper : IScraper
    {
        private int _nextNumber = 16;
        public void Scrap(ref string[] hackStringArray, ref Dictionary<string, int> namedPointers, ref List<int> usedNumbers)
        {
            for (var i = 0; i < hackStringArray.Length; i++)
            {
                if (!hackStringArray[i].StartsWith('@')) continue;
                
                var aInstruction = hackStringArray[i].TrimStart('@');
                if (int.TryParse(aInstruction, out var addr))
                {
                    if (!usedNumbers.Contains(addr))
                        usedNumbers.Add(addr);
                }else if (namedPointers.ContainsKey(aInstruction))
                {
                    addr = namedPointers[aInstruction];
                }
                else
                {
                    addr = FindNextFree(usedNumbers);
                    usedNumbers.Add(addr);
                    namedPointers.Add(aInstruction,addr);
                }

                hackStringArray[i] = Convert.ToString(addr, 2).PadLeft(16, '0');
            }
        }

        private int FindNextFree(List<int> used)
        {
            var number = _nextNumber;
            _nextNumber++;
            return number;
        }
    }
}