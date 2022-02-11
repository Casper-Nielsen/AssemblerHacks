using VM.Model;

namespace VM.VMTranslator.Converters;

/// <summary>
/// A Class that holds the different converters
/// </summary>
internal class ConverterGroup
{
    public IConverter<ArithmeticCommand> ArithmeticConverter { get; }
    public IConverter<MemoryCommand> MemoryConverter { get; }
    public IConverter<LabelCommand> LabelConverter { get; }
    public IConverter<GoToCommand> GoToConverter { get; }
    public IConverter<FunctionCommand> FunctionConverter { get; }
    public IConverter<CallCommand> CallConverter { get; }
    public IConverter<ReturnCommand> ReturnConverter { get; }

    public ConverterGroup()
    {
        ArithmeticConverter = new ArithmeticConverter();
        MemoryConverter = new MemoryConverter();
        LabelConverter = new LabelConverter();
        GoToConverter = new GoToConverter();
        FunctionConverter = new FunctionConverter();
        CallConverter = new CallConverter();
        ReturnConverter = new ReturnConverter();
    }
}