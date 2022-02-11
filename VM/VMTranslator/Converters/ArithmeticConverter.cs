using VM.Model;

namespace VM.VMTranslator.Converters
{
    /// <summary>
    /// Converts the arithmetic command to assembly
    /// </summary>
    internal class ArithmeticConverter : IConverter<ArithmeticCommand>
    {
        public string Convert(ArithmeticCommand command, ref UniqueGen uniqueGen)
        {
            var stringCommand = string.Empty;
            string? label;
            switch (command.Method)
            {
                case Method.ADD:
                    stringCommand = @"
@SP
AM=M-1
D=M
@SP
AM=M-1
M=M+D
@SP
AM=M+1";
                    break;
                case Method.SUB:
                    stringCommand = @"
@SP
AM=M-1
D=M
@SP
AM=M-1
M=M-D
@SP
AM=M+1";
                    break;
                case Method.NEG:
                    stringCommand = @"
@SP
AM=M-1
M=-M
@SP
AM=M+1";
                    break;
                case Method.EQ:
                    label = uniqueGen.GenUniqueLabel("CONTINUE");
                    stringCommand = @$"
@{label}
D=A
@R14
M=D
@EQ
0;JMP
({label})";
                    break;
                case Method.GT:
                    label = uniqueGen.GenUniqueLabel("CONTINUE");
                    stringCommand = @$"
@{label}
D=A
@R14
M=D
@GT
0;JMP
({label})";
                    break;
                case Method.LT:
                    label = uniqueGen.GenUniqueLabel("CONTINUE");
                    stringCommand = @$"
@{label}
D=A
@R14
M=D
@LT
0;JMP
({label})";
                    break;
                case Method.AND:
                    stringCommand = @"
@SP
AM=M-1
D=M
@SP
AM=M-1
M=M&D
@SP
AM=M+1";
                    break;
                case Method.OR:
                    stringCommand = @"
@SP
AM=M-1
D=M
@SP
AM=M-1
M=M|D
@SP
AM=M+1";
                    break;
                case Method.NOT:
                    stringCommand = @"
@SP
AM=M-1
M=!M
@SP
AM=M+1";
                    break;
            }

            return stringCommand;
        }
    }
}