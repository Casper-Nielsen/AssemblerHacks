using System.Collections.Generic;
using System.IO;
using System.Linq;
using VM.Model;
using VM.VMTranslator;

namespace VM
{
    public class VMConverter
    {
        public static void Convert(string vmFilePath, string asmName)
        {
            List<string> paths = Directory.EnumerateFiles(vmFilePath + @"\", "*.vm").ToList();

            var writer = new AssemblyWriter(vmFilePath + @"\" + asmName + ".asm");
            foreach (var path in paths)
            {
                var parser = new Parser(path);
            
                while (parser.HasMore)
                {
                    var command = parser.GetNext();
                    writer.WriteCommand(command);
                }
                parser.Close();
            }
            writer.Close();
        }
    }
}