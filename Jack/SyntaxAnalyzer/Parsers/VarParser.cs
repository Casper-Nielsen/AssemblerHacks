using Jack.Model;

namespace Jack.SyntaxAnalyzer.Parsers;

/// <summary>
/// Parses a Var Command
/// </summary>
public class VarParser : IParser
{
    public Command? Parse(IReadOnlyCollection<Token> tokens, ParserGroup parserGroup, int startIndex, out int nextIndex)
    {
        nextIndex = startIndex;
        if (tokens.Count <= startIndex) return null;
        
        var varObj = new VarCommand();
        
        var currentToken = tokens.ElementAt(startIndex);
        nextIndex = startIndex+1;
        
        if (currentToken.Attribute != AttributeEnum.IDENTIFIER) return null;
        varObj.DataType = currentToken.Text;
        
        currentToken = tokens.ElementAt(nextIndex);
        nextIndex++;
        
        if (currentToken.Attribute != AttributeEnum.IDENTIFIER) return null;
        varObj.VarName = currentToken.Text;
        
        return varObj;
    }
}