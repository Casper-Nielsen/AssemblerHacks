using Jack.Model;

namespace Jack.SyntaxAnalyzer.Parsers;

/// <summary>
/// Parses a inline do Command
/// also used for parsing do commands but it has not been given the "do" token
/// </summary>
public class InlineDoParser : IParser
{
    public Command? Parse(IReadOnlyCollection<Token> tokens, ParserGroup parserGroup, int startIndex, out int nextIndex)
    {
        nextIndex = startIndex;
        if (tokens.Count <= startIndex) return null;
        
        var currentToken = tokens.ElementAt(startIndex);
        var doCommand = new DoCommand();
        nextIndex = startIndex+1;
        
            
        doCommand.methodName = currentToken.Text;
        
        if (!Parser.CheckSymbol(tokens.ElementAt(nextIndex), "("))
            throw new Exception("Invalid inline do command");
        nextIndex++;

        while (!Parser.CheckSymbol(tokens.ElementAt(nextIndex), ")"))
        {
            if(Parser.CheckSymbol(tokens.ElementAt(nextIndex),","))
                nextIndex++;
                                
            // Gets the expression
            var innerCommand =
                parserGroup.ExpressionParser.Parse(tokens, parserGroup, nextIndex, out nextIndex);
            if (innerCommand == null) throw new Exception("Invalid inline do command");
                                
            doCommand.ValueHolders.Add(innerCommand);
        }
        nextIndex++;
        
        return doCommand;
    }
}