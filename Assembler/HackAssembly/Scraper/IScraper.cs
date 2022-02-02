using System.Collections.Generic;

namespace Assembler.HackAssembly.Scraper
{
    public interface IScraper
    {
        public void Scrap(ref string[] hackStringArray, ref Dictionary<string, int> namedPointers,
            ref List<int> usedNumbers);
    }
}