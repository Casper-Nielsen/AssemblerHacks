using System.Diagnostics.SymbolStore;
using Jack.Model;

namespace Jack.SyntaxAnalyzer;

/// <summary>
/// This Can Parse tokens in to different commands and statements
/// </summary>
internal class Parser
{
    private List<Command?> _commands;
    private readonly ParserGroup _parserGroup;
    public Parser()
    {
        _commands = new List<Command?>();
        _parserGroup = new ParserGroup();
    }

    /// <summary>
    /// Parses all the tokens to useful commands 
    /// </summary>
    /// <param name="tokens">The tokens that should be parsed</param>
    /// <returns>A array of commands that was parsed</returns>
    /// <exception>throws a exception if the tokens dont aline with jack grammar</exception>
    public IReadOnlyCollection<Command?> Parse(IReadOnlyCollection<Token> tokens)
    {
        _commands = new List<Command?>();
        if (tokens == null) throw new ArgumentNullException(nameof(tokens));
        var i = 0;
        while (i < tokens.Count)
        {
            _commands.Add(ParseCommand(tokens, i, out i));
        }

        return _commands;
    }

    /// <summary>
    /// Parses the tokens to more usefull data
    /// </summary>
    /// <param name="tokens">The tokens to parse</param>
    /// <param name="startIndex">The index it will start at</param>
    /// <param name="nextIndex">The next index that was not parsed</param>
    /// <returns>A command for the tokens it parsed</returns>
    /// <exception cref="Exception">Throws exception If the Tokens does not aline with the jack grammar</exception>
    private Command? ParseCommand(IReadOnlyCollection<Token> tokens, int startIndex, out int nextIndex)
    {
        nextIndex = startIndex;
        while (nextIndex < tokens.Count)
        {
            var currentToken = tokens.ElementAt(nextIndex);
            nextIndex++;
            
            switch (currentToken.Attribute)
            {
                case AttributeEnum.Keyword:
                    switch (currentToken.Text)
                    {
                        case "let":
                            return ParseLet(tokens, startIndex, out nextIndex);
                        case "while":
                            return ParseWhile(tokens, startIndex, out nextIndex);
                        case "if":
                            return ParseIf(tokens, startIndex, out nextIndex);
                        case "return":
                            return ParseReturn(tokens, startIndex, out nextIndex);
                        case "class":
                            return ParseClass(tokens, startIndex, out nextIndex);
                        case "constructor":
                            return ParseCtor(tokens, startIndex, out nextIndex);
;                        case "method":
                        case "function":
                            return ParseFunction(tokens, startIndex, out nextIndex);
                        case "static":
                        case "field":
                        case "var":
                            return ParseVar(tokens, startIndex, out nextIndex);
                        case "true":
                            return new Constant(-1);
                        case "false":
                            return new Constant(0);
                        case "do":
                            return ParseDo(tokens, startIndex, out nextIndex);
                        case "else":
                            return ParseElse(tokens, startIndex, out nextIndex);
                        case "this":
                            return new ThisHolder();
                        default:
                            throw new Exception("unknown Keyword");
                    }
                default:
                    throw new Exception("Unknown Command");
            }
        }

        throw new Exception("No More Commands");
    }

#region KeywordParser

    /// <summary>
    /// Parses the tokens to a let statement
    /// </summary>
    /// <param name="tokens">Tokens to parse</param>
    /// <param name="startIndex">The index the else command starts</param>
    /// <param name="nextIndex">The next index that was not parsed</param>
    /// <returns>A let statement</returns>
    /// <exception cref="Exception">Throws exception If the Tokens does not aline with the jack grammar</exception>
    private LetStatement ParseLet(IReadOnlyCollection<Token> tokens, int startIndex, out int nextIndex)
    {
        nextIndex = startIndex+1;
        var letCommand = new LetStatement();

        var innerCommand =
            _parserGroup.VarNameParser.Parse(tokens, _parserGroup, nextIndex, out nextIndex);
        if (innerCommand == null) throw new Exception("Invalid let command");
                            
        letCommand.VarName = innerCommand;
                            
        if(!CheckSymbol(tokens.ElementAt(nextIndex), "="))
            throw new Exception("Invalid let command");
        nextIndex++;
                            
                           
        innerCommand =
            _parserGroup.ExpressionParser.Parse(tokens, _parserGroup, nextIndex, out nextIndex);
        if (innerCommand == null) throw new Exception("Invalid let command");

        letCommand.Expression = innerCommand;
                            
                            
        // Sees if the next symbol is ;
        if (!CheckSymbol(tokens.ElementAt(nextIndex), ";"))
            throw new Exception("Invalid let command");
        nextIndex++;

        return letCommand;
    }
    
    /// <summary>
    /// Parses the tokens to a while statement
    /// </summary>
    /// <param name="tokens">Tokens to parse</param>
    /// <param name="startIndex">The index the else command starts</param>
    /// <param name="nextIndex">The next index that was not parsed</param>
    /// <returns>A while statement</returns>
    /// <exception cref="Exception">Throws exception If the Tokens does not aline with the jack grammar</exception>
    private WhileStatement ParseWhile(IReadOnlyCollection<Token> tokens, int startIndex, out int nextIndex)
    {
        nextIndex = startIndex+1;
        var whileCommand = new WhileStatement();

        CheckSymbol(tokens.ElementAt(nextIndex), "(", "Invalid while command", ref nextIndex);
                            
         var innerCommand =
            _parserGroup.ExpressionParser.Parse(tokens, _parserGroup, nextIndex, out nextIndex);
                            
        whileCommand.Expression = innerCommand;

        CheckSymbol(tokens.ElementAt(nextIndex), ")", "Invalid while command", ref nextIndex);
        CheckSymbol(tokens.ElementAt(nextIndex), "{", "Invalid while command", ref nextIndex);

        // Gets the next command while the next is not }
        while (!CheckSymbol(tokens.ElementAt(nextIndex), "}"))
        {
            whileCommand.Statements.Add(ParseCommand(tokens, nextIndex, out nextIndex));
        }

        nextIndex++;
        return whileCommand;
    }
    
    /// <summary>
    /// Parses the tokens to a if statement
    /// </summary>
    /// <param name="tokens">Tokens to parse</param>
    /// <param name="startIndex">The index the else command starts</param>
    /// <param name="nextIndex">The next index that was not parsed</param>
    /// <returns>A if statement</returns>
    /// <exception cref="Exception">Throws exception If the Tokens does not aline with the jack grammar</exception>
    private IfStatement ParseIf(IReadOnlyCollection<Token> tokens, int startIndex, out int nextIndex)
    {
        nextIndex = startIndex+1;
        var ifCommand = new IfStatement();

        CheckSymbol(tokens.ElementAt(nextIndex), "(", "Invalid if command", ref nextIndex);
                            
        // Gets the expression
        var innerCommand =
            _parserGroup.ExpressionParser.Parse(tokens, _parserGroup, nextIndex, out nextIndex);
                            
        ifCommand.Expression = innerCommand;
                            
        CheckSymbol(tokens.ElementAt(nextIndex), ")", "Invalid if command", ref nextIndex);
        CheckSymbol(tokens.ElementAt(nextIndex), "{", "Invalid if command", ref nextIndex);

        // Gets the next command while the next is not }
        while (!CheckSymbol(tokens.ElementAt(nextIndex), "}"))
        {
            ifCommand.Statements.Add(ParseCommand(tokens, nextIndex, out nextIndex));
        }

        nextIndex++;
        return ifCommand;
    }
    
    /// <summary>
    /// Parses the tokens to a return command
    /// </summary>
    /// <param name="tokens">Tokens to parse</param>
    /// <param name="startIndex">The index the else command starts</param>
    /// <param name="nextIndex">The next index that was not parsed</param>
    /// <returns>A return command</returns>
    /// <exception cref="Exception">Throws exception If the Tokens does not aline with the jack grammar</exception>
    private ReturnCommand ParseReturn(IReadOnlyCollection<Token> tokens, int startIndex, out int nextIndex)
    {
        nextIndex = startIndex+1;
        var returnCommand = new ReturnCommand();
        if (!CheckSymbol(tokens.ElementAt(nextIndex), ";"))
        {
            var innerCommand = _parserGroup.ValueParser.Parse(tokens, _parserGroup, nextIndex, out nextIndex);
            if (innerCommand == null) throw new Exception("Invalid return command");
                                
            returnCommand.Value = innerCommand;
        }
        // Sees if the next symbol is ;
        if (!CheckSymbol(tokens.ElementAt(nextIndex), ";"))
            throw new Exception("Invalid return command");
        nextIndex++;
        return returnCommand;
    }
    
    /// <summary>
    /// Parses the tokens to a class command
    /// </summary>
    /// <param name="tokens">Tokens to parse</param>
    /// <param name="startIndex">The index the else command starts</param>
    /// <param name="nextIndex">The next index that was not parsed</param>
    /// <returns>A class command</returns>
    /// <exception cref="Exception">Throws exception If the Tokens does not aline with the jack grammar</exception>
    private ClassCommand ParseClass(IReadOnlyCollection<Token> tokens, int startIndex, out int nextIndex)
    {
        nextIndex = startIndex+1;
        var classCommand = new ClassCommand();
                            
        // Gets the name
        var innerCommand = _parserGroup.VarNameParser.Parse(tokens, _parserGroup, nextIndex, out nextIndex);
        if (innerCommand != null)
            classCommand.ClassName = (VarName) innerCommand;
        else
            throw new Exception("Invalid class command");
                        
        // Sees if the next symbol is {
        if (!CheckSymbol(tokens.ElementAt(nextIndex), "{"))
            throw new Exception("Invalid class command");
        nextIndex++;
                            
                            
        // Gets the next command while the next is not }
        while (!CheckSymbol(tokens.ElementAt(nextIndex), "}"))
        {
            classCommand.ClassBody.Add(ParseCommand(tokens, nextIndex, out nextIndex));
        }
                            
        nextIndex++;
        return classCommand;
    }
    
    /// <summary>
    /// Parses the tokens to a ctor command
    /// </summary>
    /// <param name="tokens">Tokens to parse</param>
    /// <param name="startIndex">The index the else command starts</param>
    /// <param name="nextIndex">The next index that was not parsed</param>
    /// <returns>A ctor command</returns>
    /// <exception cref="Exception">Throws exception If the Tokens does not aline with the jack grammar</exception>
    private CtorCommand ParseCtor(IReadOnlyCollection<Token> tokens, int startIndex, out int nextIndex)
    {
        nextIndex = startIndex+1;

        var ctorCommand = new CtorCommand();

        // Gets the type
        var innerCommand =
            _parserGroup.VarNameParser.Parse(tokens, _parserGroup, nextIndex, out nextIndex);
        if (innerCommand == null) throw new Exception("Invalid constructor command");

        // Gets the name
        innerCommand =
            _parserGroup.VarNameParser.Parse(tokens, _parserGroup, nextIndex, out nextIndex);
        if (innerCommand == null) throw new Exception("Invalid constructor command");

        ctorCommand.FunctionName = ((VarName) innerCommand).Value;


        // Sees if the next symbol is (
        if (!CheckSymbol(tokens.ElementAt(nextIndex), "("))
            throw new Exception("Invalid constructor command");
        nextIndex++;

        ctorCommand.ValueHolders.AddRange(ParseArg(tokens, nextIndex, out nextIndex));

        // Sees if the next symbol is {
        if (!CheckSymbol(tokens.ElementAt(nextIndex), "{"))
            throw new Exception("Invalid constructor command");
        nextIndex++;

        // Gets the next command while the next is not }
        while (!CheckSymbol(tokens.ElementAt(nextIndex), "}"))
        {
            ctorCommand.Statements.Add(ParseCommand(tokens, nextIndex, out nextIndex));
        }

        nextIndex++;
        return ctorCommand;
    }

    /// <summary>
    /// Parses the tokens to a function command
    /// </summary>
    /// <param name="tokens">Tokens to parse</param>
    /// <param name="startIndex">The index the else command starts</param>
    /// <param name="nextIndex">The next index that was not parsed</param>
    /// <returns>A function command</returns>
    /// <exception cref="Exception">Throws exception If the Tokens does not aline with the jack grammar</exception>
    private FunctionCommand ParseFunction(IReadOnlyCollection<Token> tokens, int startIndex, out int nextIndex)
    {
        nextIndex = startIndex + 1;
        var currentToken = tokens.ElementAt(startIndex);
        var functionCommand = new FunctionCommand(currentToken.Text);

        // Gets the type
        var innerCommand =
            _parserGroup.VarNameParser.Parse(tokens, _parserGroup, nextIndex, out nextIndex);
        if (innerCommand == null) throw new Exception("Invalid function command");

        // Gets the name
        innerCommand =
            _parserGroup.VarNameParser.Parse(tokens, _parserGroup, nextIndex, out nextIndex);
        if (innerCommand == null) throw new Exception("Invalid function command");

        functionCommand.FunctionName = ((VarName) innerCommand).Value;


        // Sees if the next symbol is (
        if (!CheckSymbol(tokens.ElementAt(nextIndex), "("))
            throw new Exception("Invalid function command");
        nextIndex++;

        functionCommand.ValueHolders.AddRange(ParseArg(tokens, nextIndex, out nextIndex));

        // Sees if the next symbol is {
        if (!CheckSymbol(tokens.ElementAt(nextIndex), "{"))
            throw new Exception("Invalid function command");
        nextIndex++;

        // Gets the next command while the next is not }
        while (!CheckSymbol(tokens.ElementAt(nextIndex), "}"))
        {
            functionCommand.Statements.Add(ParseCommand(tokens, nextIndex, out nextIndex));
        }

        nextIndex++;
        return functionCommand;
    }

    /// <summary>
    /// Parses the tokens to a var command
    /// </summary>
    /// <param name="tokens">Tokens to parse</param>
    /// <param name="startIndex">The index the else command starts</param>
    /// <param name="nextIndex">The next index that was not parsed</param>
    /// <returns>A var command</returns>
    /// <exception cref="Exception">Throws exception If the Tokens does not aline with the jack grammar</exception>
    private VarCommand ParseVar(IReadOnlyCollection<Token> tokens, int startIndex, out int nextIndex)
    {
        nextIndex = startIndex+1;
        var currentToken = tokens.ElementAt(startIndex);
        var varCommand = new VarCommand(currentToken.Text);
                            
        // Gets the type
        var innerCommand = _parserGroup.VarParser.Parse(tokens, _parserGroup, nextIndex, out nextIndex);
                            
        if (innerCommand == null) throw new Exception("Invalid var command");
        varCommand.DataType = ((VarCommand) innerCommand).DataType;
        varCommand.VarName = ((VarCommand) innerCommand).VarName;
                            
        // Sees if the next symbol is ;
        if (!CheckSymbol(tokens.ElementAt(nextIndex), ";")) 
            throw new Exception("Invalid var command");
                                
        nextIndex++;
                            
        return varCommand;
    }

    /// <summary>
    /// Parses the tokens to a do command
    /// </summary>
    /// <param name="tokens">Tokens to parse</param>
    /// <param name="startIndex">The index the else command starts</param>
    /// <param name="nextIndex">The next index that was not parsed</param>
    /// <returns>A do command</returns>
    /// <exception cref="Exception">Throws exception If the Tokens does not aline with the jack grammar</exception>
    private DoCommand ParseDo(IReadOnlyCollection<Token> tokens, int startIndex, out int nextIndex)
    {
        nextIndex = startIndex+1;
        var doCommand =
            (DoCommand) _parserGroup.InlineDoParser.Parse(tokens, _parserGroup, nextIndex, out nextIndex);
        if(doCommand == null) throw new Exception("Invalid do command");

        doCommand.NoReturn = true;
                            
        // Sees if the next symbol is ;
        if (!CheckSymbol(tokens.ElementAt(nextIndex), ";"))
            throw new Exception("Invalid do command");
        nextIndex++;
                            
        return doCommand;
    }
    
    /// <summary>
    /// Parses the tokens to a Else Command
    /// </summary>
    /// <param name="tokens">Tokens to parse</param>
    /// <param name="startIndex">The index the else command starts</param>
    /// <param name="nextIndex">The next index that was not parsed</param>
    /// <returns>A else Command</returns>
    /// <exception cref="Exception">Throws exception If the Tokens does not aline with the jack grammar</exception>
    private ElseCommand ParseElse(IReadOnlyCollection<Token> tokens, int startIndex, out int nextIndex)
    {
        nextIndex = startIndex+1;
        var command = new ElseCommand();
                            
        // Sees if the next symbol is {
        if (!CheckSymbol(tokens.ElementAt(nextIndex), "{"))
            throw new Exception("Invalid else command");
        nextIndex++;

        // Gets the next command while the next is not }
        while (!CheckSymbol(tokens.ElementAt(nextIndex), "}"))
        {
            command.Statements.Add(ParseCommand(tokens, nextIndex, out nextIndex));
        }

        nextIndex++;
                            
        return command;
    }

    /// <summary>
    /// Parses the Tokens to Arg commands
    /// </summary>
    /// <param name="tokens">Tokens to parse</param>
    /// <param name="startIndex">The index the else command starts</param>
    /// <param name="nextIndex">The next index that was not parsed</param>
    /// <returns>A list of arg commands</returns>
    /// <exception cref="Exception">Throws exception If the Tokens does not aline with the jack grammar</exception>
    private IEnumerable<ArgCommand> ParseArg(IReadOnlyCollection<Token> tokens, int startIndex, out int nextIndex)
    {
        var parsedCommands = new List<ArgCommand>();
        nextIndex = startIndex;
        
        while (!CheckSymbol(tokens.ElementAt(nextIndex), ")"))
        {
            if (CheckSymbol(tokens.ElementAt(nextIndex), ","))
                nextIndex++;

            // Gets the expression
            var arg = _parserGroup.ArgParser.Parse(tokens, _parserGroup, nextIndex, out nextIndex);

            if (arg == null) throw new Exception("Invalid arg command");
            parsedCommands.Add((ArgCommand) arg);
        }
        nextIndex++;
        
        return parsedCommands;
    }
    
#endregion
    
    /// <summary>
    /// Checks if the token is that symbol will throw the error if not
    /// and return true if it is the symbol
    /// </summary>
    /// <param name="token">The token to check</param>
    /// <param name="validSymbol">The Valid symbol</param>
    /// <param name="error">The error message it will throw if it is not the symbol</param>
    /// <param name="nextIndex">The next index that has not been parsed</param>
    /// <returns>true if it don't throw</returns>
    /// <exception cref="Exception">Throws the error message if the token isn't the valid symbol</exception>
    internal static bool CheckSymbol(Token token, string validSymbol, string error, ref int nextIndex)
    {
        if (token.Attribute != AttributeEnum.Symbol) throw new Exception(error);
        if (token.Text != validSymbol) throw new Exception(error);
        nextIndex++;
        return true;
    }   
    
    /// <summary>
    /// Checks if the token is the valid symbol
    /// </summary>
    /// <param name="token">The token that will be checked</param>
    /// <param name="validSymbol">The valid symbol</param>
    /// <returns>If the token is the valid symbol</returns>
    internal static bool CheckSymbol(Token token, string validSymbol)
    {
        if (token.Attribute != AttributeEnum.Symbol) return false;
        return token.Text == validSymbol;
    }    
}