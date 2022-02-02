using VM.Model;

namespace VM.VMTranslator.Converters
{
    internal class CallConverter : IConverter<CallCommand>
    {
        public string Convert(CallCommand command, ref UniqueGen uniqueGen)
        {
            string returnLabel = uniqueGen.GenUniqueLabel("RETURN");
            string stringCommand = "";
            if (command.Amount == 0)
            {
                stringCommand += $@"
@SP
M=M+1";
            }
            stringCommand += $@"
@{returnLabel}
D=A
@SP
A=M
M=D
@LCL
D=M
@SP
AM=M+1
M=D
@ARG
D=M
@SP
AM=M+1
M=D
@THIS
D=M
@SP
AM=M+1
M=D
@THAT
D=M
@SP
AM=M+1
M=D
@{(command.Amount == 0 ? 5 : 4+command.Amount)}
D=A
@SP
A=M-D
D=A
@ARG
M=D
@SP
AM=M+1
@{command.MethodName}
0;JMP
({returnLabel})
";
            return stringCommand;
        }
    }
}