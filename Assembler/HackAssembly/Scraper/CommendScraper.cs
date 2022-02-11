using System;
using System.Collections.Generic;

namespace Assembler.HackAssembly.Scraper
{
    /// <summary>
    /// Scrapes the commends from the assembly code
    /// </summary>
    public class CommendScraper : IScraper
    {
        public void Scrap(ref string[] hackStringArray, ref Dictionary<string, int> namedPointers, ref List<int> usedNumbers)
        {
            for (var i = 0; i < hackStringArray.Length; i++)
            {
                var removeIndex = hackStringArray[i].IndexOf("//", StringComparison.Ordinal);
                hackStringArray[i] = removeIndex >= 0 ? hackStringArray[i].Remove(removeIndex).Trim() : hackStringArray[i].Trim();
            }
        }
    }
}