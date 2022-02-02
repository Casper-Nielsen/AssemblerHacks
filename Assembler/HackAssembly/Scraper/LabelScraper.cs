using System.Collections.Generic;

namespace Assembler.HackAssembly.Scraper
{
    public class LabelScraper : IScraper
    {
        public void Scrap(ref string[] hackStringArray, ref Dictionary<string, int> namedPointers, ref List<int> usedNumbers)
        {
            List<string> hacks = new List<string>();
            foreach (var line in hackStringArray)
            {
                if (line.Contains('(') && line.Contains(')'))
                {
                    var pointer = line.Trim(new[]{'(', ')'});
                    namedPointers.Add(pointer,hacks.Count);
                }
                else
                {
                    hacks.Add(line);
                }
            }
            hackStringArray = hacks.ToArray();
        }
    }
}