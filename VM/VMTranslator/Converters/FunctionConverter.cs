using VM.Model;

namespace VM.VMTranslator.Converters
{
    /// <summary>
    /// Converts the function command to assembly
    /// </summary>
    internal class FunctionConverter : IConverter<FunctionCommand>
    {
        public string Convert(FunctionCommand command, ref UniqueGen uniqueGen)
        {
            var stringCommand = $@"
({command.Name})
@SP 
D=M
@LCL
M=D
@{command.ArgNumber}
D=A
@SP
M=M+D
";
            return stringCommand;
        }
    }
}