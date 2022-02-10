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
            AttributeEnum.Keyword when currentToken.Text == "this" => new ThisHolder(),
            AttributeEnum.Keyword when currentToken.Text == "false" => new Constant(0),
            AttributeEnum.Keyword when currentToken.Text == "true" => new Constant(-1),
            AttributeEnum.Keyword => null,
            AttributeEnum.IntegerConstant => new Constant(int.Parse(currentToken.Text)),
            AttributeEnum.Identifier => new VarName(currentToken.Text),
            AttributeEnum.StringConstant => new StringConstant(currentToken.Text),
            _ => null
        };
    }
}