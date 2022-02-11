using System.Collections.Generic;

namespace Assembler.HackAssembly.Scraper
{
    /// <summary>
    /// Scrapes spaces from the assembly code
    /// </summary>
    public class SpaceScraper : IScraper
    {
        public void Scrap(ref string[] hackStringArray, ref Dictionary<string, int> namedPointers, ref List<int> usedNumbers)
        {
            for (var i = 0; i < hackStringArray.Length; i++)
            {
                hackStringArray[i] = hackStringArray[i].Replace(" ", "");
            }
        }
    }
}