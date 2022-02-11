using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assembler.HackAssembly.Scraper;

namespace Assembler.HackAssembly
{
    /// <summary>
    /// Convert Assembly to binary
    /// </summary>
    public class HackAssemblyConverter
    {
        /// <summary>
        /// Converts the Assembly to binary
        /// </summary>
        /// <param name="hackAssembly">The assembly file</param>
        /// <returns>the binary code in string format</returns>
        public static string ConvertFromHackAssembly(string hackAssembly)
        {
            var scrappers = new List<IScraper>()
            {
                new CommendScraper(),
                new EmptyLineScraper(),
                new SpaceScraper(),
                new LabelScraper(),
                new AInstructionScraper(),
                new CInstructionScraper()
            };
            var hackAssemblyArray = hackAssembly.Split('\n');

            // Addes known pointers
            var pointers = new Dictionary<string, int>()
            {
                {"R0", 0}, 
                {"R1", 1}, 
                {"R2", 2}, 
                {"R3", 3}, 
                {"R4", 4}, 
                {"R5", 5}, 
                {"R6", 6}, 
                {"R7", 7}, 
                {"R8", 8}, 
                {"R9", 9},
                {"R10", 10}, 
                {"R11", 11}, 
                {"R12", 12}, 
                {"R13", 13}, 
                {"R14", 14}, 
                {"R15", 15},
                {"SP", 0}, 
                {"LCL", 1}, 
                {"ARG", 2}, 
                {"THIS", 3}, 
                {"THAT", 4}, 
                {"SCREEN", 16384}, 
                {"KBD", 24576}
            };

            var usedAddr = new List<int>();

            // Run through each scraper and scrap the assembly code it knows
            foreach (var scraper in scrappers)
            {
                scraper.Scrap(ref hackAssemblyArray, ref pointers, ref usedAddr);
            }

            return string.Join("\n", hackAssemblyArray);
        }
    }
}