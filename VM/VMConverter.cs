using VM.VMTranslator;

namespace VM
{
    /// <summary>
    /// Converts vm code to Assembly code 
    /// </summary>
    public static class VmConverter
    {
        /// <summary>
        /// Converts the vm files from the path to the assembly file with the given name
        /// </summary>
        /// <param name="vmFilePath">The path to the vm files</param>
        /// <param name="asmName">The name of the new assembly file</param>
        public static void Convert(string vmFilePath, string asmName)
        {
            var paths = Directory.EnumerateFiles(vmFilePath + @"\", "*.vm").ToList();

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