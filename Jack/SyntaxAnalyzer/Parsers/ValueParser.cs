using Jack.Model;

namespace Jack.SyntaxAnalyzer.Parsers;

/// <summary>
/// Parses a value to the right type
/// </summary>
public class ValueParser : IParser
{
    public Command? Parse(IReadOnlyCollection<Token> tokens, ParserGroup parserGroup, int startIndex, out int nextIndex)
    {
        nextIndex = startIndex;
        if (tokens.Count <= startIndex) return null;
        
        var currentToken = tokens.ElementAt(startIndex);
        nextIndex = startIndex+1;
        return currentToken.Attribute switch
        {
            AttributeEnum.KEYWORD when currentToken.Text == "this" => new ThisHolder(),
            AttributeEnum.KEYWORD when currentToken.Text == "false" => new Constant(0),
            AttributeEnum.KEYWORD when currentToken.Text == "true" => new Constant(-1),
            AttributeEnum.KEYWORD => null,
            AttributeEnum.INTEGER_CONSTANT => new Constant(int.Parse(currentToken.Text)),
            AttributeEnum.IDENTIFIER => new VarName(currentToken.Text),
            AttributeEnum.STRING_CONSTANT => new StringConstant(currentToken.Text),
            _ => null
        };
    }
}