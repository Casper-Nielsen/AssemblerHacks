using Jack.SyntaxAnalyzer.TokenExtractors;

namespace Jack.SyntaxAnalyzer;

/// <summary>
/// Its is a extractor group class
/// It holds different extractor for each method
/// </summary>
public class ExtractorGroup
{
    public ITokenExtractor IdentifierExtractor { get; }
    public ITokenExtractor IntegerConstantExtractor { get; }
    public ITokenExtractor KeywordExtractor { get; }
    public ITokenExtractor StringConstantExtractor { get; }
    public ITokenExtractor SymbolExtractor { get; }

    public ExtractorGroup()
    {
        IdentifierExtractor = new IdentifierExtractor();
        IntegerConstantExtractor = new IntegerConstantExtractor();
        KeywordExtractor = new KeywordExtractor();
        StringConstantExtractor = new StringConstantExtractor();
        SymbolExtractor = new SymbolExtractor();
    }
}