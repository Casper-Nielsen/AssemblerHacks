using VM.Model;

namespace VM.VMTranslator.Converters
{
    /// <summary>
    /// Converts Goto Commands to assembly
    /// Takes both goto and if-goto commands
    /// </summary>
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