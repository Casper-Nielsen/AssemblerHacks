using System.Collections.Generic;
using System.Linq;

namespace Assembler.HackAssembly.Scraper
{
    /// <summary>
    /// Scrapes empty lines from the assembly code 
    /// </summary>
    public class EmptyLineScraper : IScraper
    {
        public void Scrap(ref string[] hackStringArray, ref Dictionary<string, int> namedPointers, ref List<int> usedNumbers)
        {
            hackStringArray = hackStringArray.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
        }
    }
}