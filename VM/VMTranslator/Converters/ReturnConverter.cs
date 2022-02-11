using VM.Model;

namespace VM.VMTranslator.Converters
{
    /// <summary>
    /// Converts a return command to assembly
    /// </summary>
    internal class ReturnConverter : IConverter<ReturnCommand>
    {
        public string Convert(ReturnCommand command, ref UniqueGen uniqueGen)
        {
            return $@"
@SP
A=M-1
D=M
@ARG
A=M
M=D
@ARG
D=M+1
@SP
M=D
@LCL
D=M
@R13
AM=D-1
D=M
@THIS
M=D
@R13
AM=M-1
D=M
@THAT
M=D
@R13
AM=M-1
D=M
@ARG
M=D
@R13
AM=M-1
D=M
@LCL
M=D
@R13
AM=M-1
A=M
0;JMP
";
        }
    }
}