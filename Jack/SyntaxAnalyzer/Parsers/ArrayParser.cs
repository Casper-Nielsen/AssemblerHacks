using Jack.Model;

namespace Jack.SyntaxAnalyzer.Parsers;

/// <summary>
/// Parses a array
/// </summary>
public class ArrayParser : IParser
{
    public Command? Parse(IReadOnlyCollection<Token> tokens, ParserGroup parserGroup, int startIndex, out int nextIndex)
    {
        nextIndex = startIndex;
        if (tokens.Count <= startIndex) return null;
        
        var currentToken = tokens.ElementAt(startIndex);
        var arrayVar = new ArrayVarCommand();
        nextIndex = startIndex+2;
            
        arrayVar.Value = currentToken.Text;
            
        var innerCommand = parserGroup.ExpressionParser.Parse(tokens, parserGroup, nextIndex, out nextIndex);
        if (innerCommand is VarName or Constant or Expression)
            arrayVar.Index = innerCommand;
        else
            throw new Exception("Invalid array index");
            
        nextIndex++;

        return arrayVar;
    }
}