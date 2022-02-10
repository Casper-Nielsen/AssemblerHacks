using Jack.Model;

namespace Jack.SyntaxAnalyzer.Parsers;

/// <summary>
/// Parses a Expression
/// </summary>
public class ExpressionParser: IParser
{
    public Command? Parse(IReadOnlyCollection<Token> tokens, ParserGroup parserGroup, int startIndex, out int nextIndex)
    {
        return ParseCommand(tokens, parserGroup, startIndex, out nextIndex);
    }

    private Command? ParseCommand(IReadOnlyCollection<Token> tokens, ParserGroup parserGroup, int startIndex, out int nextIndex, bool imp = false)    
    {
        nextIndex = startIndex;
        if (tokens.Count <= startIndex) return null;
        
        var currentToken = tokens.ElementAt(startIndex);
        nextIndex = startIndex+1;
        var neg = false;
        var expressionCommand = new Expression()
        {
            First = imp
        };

        if (Parser.CheckSymbol(currentToken, "("))
        {
            imp = true;
            expressionCommand.Term = ParseCommand(tokens, parserGroup, nextIndex, out nextIndex, imp);
        }
        else
        {

            if (Parser.CheckSymbol(currentToken, "-"))
            {
                neg = true;
                currentToken = tokens.ElementAt(nextIndex);
                nextIndex++;
            }

            // Gets the first term
            if (currentToken.Attribute == AttributeEnum.IntegerConstant)
                expressionCommand.Term =
                    new Constant(neg ? -int.Parse(currentToken.Text) : int.Parse(currentToken.Text));
            else if (currentToken.Attribute == AttributeEnum.StringConstant)
            {
                expressionCommand.Term = new StringConstant(currentToken.Text);
            }
            else if (Parser.CheckSymbol(tokens.ElementAt(nextIndex), "["))
                expressionCommand.Term = parserGroup.ArrayParser.Parse(tokens, parserGroup, startIndex, out nextIndex);

            else if (Parser.CheckSymbol(tokens.ElementAt(nextIndex), "("))
                expressionCommand.Term =
                    parserGroup.InlineDoParser.Parse(tokens, parserGroup, startIndex, out nextIndex);
            else
            {
                var value =
                    parserGroup.ValueParser.Parse(tokens, parserGroup, nextIndex - 1, out nextIndex);
                if (value is VarName varName)
                    varName.Neg = neg;
                expressionCommand.Term = value;
            }
        }


        if(imp)
            if (Parser.CheckSymbol(tokens.ElementAt(nextIndex), ")"))
                nextIndex++;
        
        // Get if there is a symbol and another term
        if (tokens.ElementAt(nextIndex).Attribute != AttributeEnum.Symbol) return expressionCommand.Term;
        if (Parser.CheckSymbol(tokens.ElementAt(nextIndex), ";")) return expressionCommand.Term;
        if (Parser.CheckSymbol(tokens.ElementAt(nextIndex), "(")) return expressionCommand.Term;
        
        var next = parserGroup.SymbolParser.Parse(tokens, parserGroup, nextIndex, out nextIndex);
        if (next == null)
        {
            nextIndex--;
            return expressionCommand.Term;
        }
        
        expressionCommand.Operation = next;
        
        // Gets the next part of the expression
        expressionCommand.OpTerm = Parse(tokens, parserGroup, nextIndex, out nextIndex);
        return expressionCommand;
    }
}