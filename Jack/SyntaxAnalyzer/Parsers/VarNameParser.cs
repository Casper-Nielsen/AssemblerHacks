using Jack.Model;

namespace Jack.SyntaxAnalyzer.Parsers;

/// <summary>
/// Parses a Var Name / Just a name
/// </summary>
public class VarNameParser : IParser
{
    public Command? Parse(IReadOnlyCollection<Token> tokens, ParserGroup parserGroup, int startIndex, out int nextIndex)
    {
        nextIndex = startIndex;
        if (tokens.Count <= startIndex) return null;
        
        var currentToken = tokens.ElementAt(startIndex);
        nextIndex = startIndex+1;

        if (Parser.CheckSymbol(tokens.ElementAt(nextIndex), "["))
            return parserGroup.ArrayParser.Parse(tokens, parserGroup, startIndex, out nextIndex);
        else
            return currentToken.Attribute == AttributeEnum.IDENTIFIER ? new VarName(currentToken.Text) : null;
    }
}