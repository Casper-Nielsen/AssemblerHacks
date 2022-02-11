using VM.Model;

namespace VM.VMTranslator.Converters
{
    /// <summary>
    /// Converts a memory command to assembly
    /// Takes both push and pop
    /// </summary>
    internal class MemoryConverter : IConverter<MemoryCommand>
    {
        public string Convert(MemoryCommand command, ref UniqueGen uniqueGen)
        {
            var stringCommand = string.Empty;
            if (command.Method == MemoryMethod.PUSH)
            {
                switch (command.Location)
                {
                    case Location.CONSTANT:
                        stringCommand = @$"
@{command.Address}
D=A
@SP
A=M
M=D
@SP
M=M+1";
                        break;
                    case Location.LOCAL:
                        stringCommand = @$"
@{command.Address}
D=A
@LCL
A=M+D
D=M
@SP
A=M
M=D
@SP
M=M+1";
                        break;
                    case Location.ARGUMENT:
                        stringCommand = @$"
@{command.Address}
D=A
@ARG
A=M+D
D=M
@SP
A=M
M=D
@SP
M=M+1";

                        break;
                    case Location.STATIC:
                        stringCommand = @$"
@{command.Address}
D=A
@16
A=A+D
D=M
@SP
A=M
M=D
@SP
M=M+1";
                        break;
                    case Location.TEMP:
                        stringCommand = @$"
@{command.Address}
D=A
@5
A=A+D
D=M
@SP
A=M
M=D
@SP
M=M+1";
                        break;
                    case Location.POINTER:
                        stringCommand = command.Address == 1 ? @$"
@THAT
D=M
@SP
M=D+1" 
                            : 
                            @$"
@THIS
D=M
@SP
M=D+1";
                        break;
                    case Location.THAT:
                        stringCommand = @$"
@{command.Address}
D=A
@THAT
A=M+D
D=M
@SP
A=M
M=D
@SP
M=M+1";
                        break;
                    case Location.THIS:
                        stringCommand = @$"
@{command.Address}
D=A
@THIS
A=M+D
D=M
@SP
A=M
M=D
@SP
M=M+1";
                        break;
                }
            }
            else
            {
                switch (command.Location)
                {
                    case Location.CONSTANT:
                        break;
                    case Location.LOCAL:
                        stringCommand = $@"
@{command.Address}
D=A
@LCL
D=M+D
@R15
M=D
@SP
A=M-1
D=M
@R15
A=M
M=D
@SP
M=M-1";
                        break;
                    case Location.ARGUMENT:
                        stringCommand = $@"
@{command.Address}
D=A
@ARG
D=M+D
@R15
M=D
@SP
A=M-1
D=M
@R15
A=M
M=D
@SP
M=M-1";
                        break;
                    case Location.STATIC:
                        stringCommand = $@"
@{command.Address}
D=A
@16
D=A+D
@R15
M=D
@SP
A=M-1
D=M
@R15
A=M
M=D
@SP
M=M-1";
                        break;
                    case Location.TEMP:
                        stringCommand = $@"
@{command.Address}
D=A
@5
D=A+D
@R15
M=D
@SP
A=M-1
D=M
@R15
A=M
M=D
@SP
M=M-1";
                        break;
                    case Location.POINTER:
                        stringCommand = command.Address == 1 ? @$"
@SP
AM=M-1
D=M
@THAT
M=D
@SP
AM=M+1
@SP
M=M-1" 
                            : 
                            @$"
@SP
AM=M-1
D=M
@THIS
M=D
@SP
AM=M+1
@SP
M=M-1";
                        break;
                    case Location.THAT:
                        stringCommand = $@"
@{command.Address}
D=A
@THAT
D=M+D
@R15
M=D
@SP
A=M-1
D=M
@R15
A=M
M=D
@SP
M=M-1";
                        break;
                    case Location.THIS:
                        stringCommand = $@"
@{command.Address}
D=A
@THIS
D=M+D
@R15
M=D
@SP
A=M-1
D=M
@R15
A=M
M=D
@SP
M=M-1";
                        break;
                }
            }

            return stringCommand;
        }
    }
}