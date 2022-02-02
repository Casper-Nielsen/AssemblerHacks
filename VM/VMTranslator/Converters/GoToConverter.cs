using VM.Model;

namespace VM.VMTranslator.Converters
{
    internal class GoToConverter : IConverter<GoToCommand>
    {
        public string Convert(GoToCommand command, ref UniqueGen uniqueGen)
        {
            return command.JumpIf ? $@"
@SP
A=M-1
D=M+1
@{command.LabelName}
D;JEQ" 
                : 
                $@"
@{command.LabelName}
0;JMP";
        }
    }
}