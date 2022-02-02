using VM.Model;

namespace VM.VMTranslator.Converters
{
    internal class FunctionConverter : IConverter<FunctionCommand>
    {
        public string Convert(FunctionCommand command, ref UniqueGen uniqueGen)
        {
            string stringCommand;
            stringCommand = $@"
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