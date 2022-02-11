using Jack.Model;

namespace Jack.SyntaxAnalyzer;

/// <summary>
/// This Can Tokenize Jack Code
/// </summary>
internal class Tokenizer
{
    private readonly ExtractorGroup _extractorGroup;
    public Tokenizer()
    {
        _extractorGroup = new ExtractorGroup();
    }
    
    /// <summary>
    /// Tokenizes the list of lines
    /// </summary>
    /// <param name="lines">A list of lines</param>
    /// <returns>A list of tokens</returns>
    public IReadOnlyCollection<Token> Tokenize(List<string> lines)
    {
        var tokens = new List<Token>();
        foreach (var line in lines)
        {
            var lineStr = line;
            var index = line.IndexOf("//", StringComparison.Ordinal);

            if (index > 0)
                lineStr = line.Remove(index);

            while (lineStr.Length > 0)
            {
                lineStr = lineStr.Trim();

                if (_extractorGroup.IntegerConstantExtractor.TryExtract(ref lineStr, out var token)) {}
                else if (_extractorGroup.StringConstantExtractor.TryExtract(ref lineStr, out token)){}
                else if (_extractorGroup.SymbolExtractor.TryExtract(ref lineStr, out token)){}
                else if(_extractorGroup.KeywordExtractor.TryExtract(ref lineStr, out token)){}
                else if(_extractorGroup.IdentifierExtractor.TryExtract(ref lineStr, out token)){}
                else
                    continue;
                
                tokens.Add(token);
            }
        }

        return tokens;
    }
}