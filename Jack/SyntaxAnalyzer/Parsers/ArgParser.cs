using Jack.Model;

namespace Jack.SyntaxAnalyzer.Parsers;

/// <summary>
/// Parses the Arguments
/// </summary>
public class ArgParser : IParser
{
    public Command? Parse(IReadOnlyCollection<Token> tokens, ParserGroup parserGroup, int startIndex, out int nextIndex)
    {
        nextIndex = startIndex;
        if (tokens.Count <= startIndex) return null;
        
        var arg = new ArgCommand();
        var currentToken = tokens.ElementAt(startIndex);
        nextIndex = startIndex+1;
        
        if (currentToken.Attribute != AttributeEnum.Identifier) return null;
        arg.Datatype = currentToken.Text;
        
        currentToken = tokens.ElementAt(nextIndex);
        nextIndex++;
        
        if (currentToken.Attribute != AttributeEnum.Identifier) return null;
        arg.Value = currentToken.Text;
        
        return arg;
    }
}