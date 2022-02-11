using VM.Model;

namespace VM.VMTranslator.Converters
{
    /// <summary>
    /// Converts a Label command to assembly code
    /// </summary>
    internal class LabelConverter : IConverter<LabelCommand>
    {
        public string Convert(LabelCommand command, ref UniqueGen uniqueGen)
        {
            return $@"
({command.LabelName})";
        }
    }
}