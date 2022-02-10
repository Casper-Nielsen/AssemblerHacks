using Jack.Model;

namespace Jack.SyntaxAnalyzer.Parsers;

/// <summary>
/// Parses a symbol
/// </summary>
public class SymbolParser : IParser
{
    public Command? Parse(IReadOnlyCollection<Token> tokens, ParserGroup parserGroup, int startIndex, out int nextIndex)
    {
        nextIndex = startIndex;
        if (tokens.Count <= startIndex) return null;
        
        var currentToken = tokens.ElementAt(startIndex);
        nextIndex = startIndex+1;
        return currentToken.Text is "+" or "-" or "=" or ">" or "<" or "|" or "&"
            ? new Operation(currentToken.Text)
            : null;
    }
}