using VM.Model;

namespace VM.VMTranslator.Converters
{
    internal class LabelConverter : IConverter<LabelCommand>
    {
        public string Convert(LabelCommand command, ref UniqueGen uniqueGen)
        {
            return $@"
({command.LabelName})";
        }
    }
}