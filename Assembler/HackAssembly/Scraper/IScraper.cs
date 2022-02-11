using System.Collections.Generic;

namespace Assembler.HackAssembly.Scraper
{
    /// <summary>
    /// A interface for scraping commands from the string array
    /// It can convert the commands to binary code
    /// </summary>
    public interface IScraper
    {
        public void Scrap(ref string[] hackStringArray, ref Dictionary<string, int> namedPointers,
            ref List<int> usedNumbers);
    }
}