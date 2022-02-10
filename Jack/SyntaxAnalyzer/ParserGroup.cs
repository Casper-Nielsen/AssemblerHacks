using Jack.SyntaxAnalyzer.Parsers;

namespace Jack.SyntaxAnalyzer;

/// <summary>
/// Its is a parser group class
/// It holds different parsers for each method
/// </summary>
public class ParserGroup
{
    public IParser SymbolParser { get; }
    public IParser ExpressionParser { get; }
    public IParser VarNameParser { get; }
    public IParser ArgParser { get; }
    public IParser VarParser { get; }
    public IParser ValueParser { get; }
    public IParser InlineDoParser { get; }
    public IParser ArrayParser { get; }

    public ParserGroup()
    {
        SymbolParser = new SymbolParser();
        ExpressionParser = new ExpressionParser();
        VarNameParser = new VarNameParser();
        ArgParser = new ArgParser();
        VarParser = new VarParser();
        ValueParser = new ValueParser();
        InlineDoParser = new InlineDoParser();
        ArrayParser = new ArrayParser();
    }
}