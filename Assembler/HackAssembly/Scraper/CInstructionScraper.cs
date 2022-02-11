using System;
using System.Collections.Generic;

namespace Assembler.HackAssembly.Scraper
{
    /// <summary>
    /// Scrapes the c instruction from the assembly code
    /// converts them to binary code 
    /// </summary>
    public class CInstructionScraper: IScraper
    {
        private Dictionary<string, string> _cBits;
        private Dictionary<string, string> _jump;

        public CInstructionScraper()
        {
            _cBits = new Dictionary<string, string>()
            {
                {"M","1110000"},
                {"!M","1110001"},
                {"-M","1110011"},
                {"M+1","1110111"},
                {"M-1","1110010"},
                {"D+M","1000010"},
                {"M+D","1000010"},
                {"D-M","1010011"},
                {"M-D","1000111"},
                {"D&M","1000000"},
                {"M&D","1000000"},
                {"D|M","1010101"},
                {"M|D","1010101"},
                
                
                {"0","0101010"},
                {"1","0111111"},
                {"-1","0111010"},
                {"D","0001100"},
                {"A","0110000"},
                {"!D","0001101"},
                {"!A","0110001"},
                {"-D","00011q1"},
                {"-A","0110011"},
                {"D+1","0011111"},
                {"A+1","0110111"},
                {"D-1","0001110"},
                {"A-1","0110010"},
                {"D+A","0000010"},
                {"A+D","0000010"},
                {"D-A","0010011"},
                {"A-D","0000111"},
                {"D&A","0000000"},
                {"A&D","0000000"},
                {"D|A","0010101"},
                {"A|D","0010101"}
            };

            _jump = new Dictionary<string, string>()
            {
                {"JGT","001"},
                {"JEQ","010"},
                {"JGE","011"},
                {"JLT","100"},
                {"JNE","101"},
                {"JLE","110"},
                {"JMP","111"}
            };
        }
        public void Scrap(ref string[] hackStringArray, ref Dictionary<string, int> namedPointers, ref List<int> usedNumbers)
        {
            for (var i = 0; i < hackStringArray.Length; i++)
            {
                var dest = false;
                var cPos = 0;
                var jump = false;
                
                if (hackStringArray[i].Contains('='))
                {
                    cPos = 1;
                    dest = true;
                }
                if (hackStringArray[i].Contains(';')) jump = true;
                
                if(!(dest || jump)) continue;
                
                var bits = "111";
                var spited = hackStringArray[i].Split(new[]{'=',';'});
                
                // c bits (alu)
                if (_cBits.ContainsKey(spited[cPos]))
                    bits += _cBits[spited[cPos]];
                else
                    bits += "0000000";
                
                // dest bits
                if (dest)
                {
                    bits += spited[0].Contains('A') ? '1' : '0';
                    bits += spited[0].Contains('D') ? '1' : '0';
                    bits += spited[0].Contains('M') ? '1' : '0';
                }
                else
                    bits += "000";
                
                // jump bits
                if (jump)
                {
                    if (_jump.ContainsKey(spited[^1]))
                        bits += _jump[spited[^1]];
                    else
                        bits += "000";
                }
                else
                    bits += "000";

                hackStringArray[i] = bits;
            }
        }
    }
}