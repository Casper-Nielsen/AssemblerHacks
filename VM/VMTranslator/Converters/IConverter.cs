using VM.Model;

namespace VM.VMTranslator.Converters
{
    internal interface IConverter<T>
    {
        public string Convert(T command, ref UniqueGen uniqueGen);
    }
}