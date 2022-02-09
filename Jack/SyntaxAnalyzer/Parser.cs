using System.Diagnostics.SymbolStore;
using Jack.Model;

namespace Jack.SyntaxAnalyzer;

internal class Parser
{
    private List<Command?> _commands;

    public Parser()
    {
        _commands = new List<Command?>();
    }

    public IReadOnlyCollection<Command?> Parse(IReadOnlyCollection<Token> tokens)
    {
        if (tokens == null) throw new ArgumentNullException(nameof(tokens));
        var i = 0;
        while (i < tokens.Count)
        {
            _commands.Add(ParseCommand(tokens, i, out i));
        }

        return _commands;
    }

    private Command? ParseCommand(IReadOnlyCollection<Token> tokens, int startIndex, out int nextIndex)
    {
        bool neg = false;
        nextIndex = startIndex;
        while (nextIndex < tokens.Count)
        {
            var currentToken = tokens.ElementAt(nextIndex);
            nextIndex++;
            switch (currentToken.Attribute)
            {
                case AttributeEnum.Keyword:
                    Command? innerCommand;
                    switch (currentToken.Text)
                    {
                        case "let":
                            var letCommand = new LetStatement();

                            // Gets VarName
                            innerCommand = ParseCommand(tokens, nextIndex, out nextIndex);
                            if (innerCommand is VarName)
                            {
                                letCommand.VarName = (VarName) innerCommand;

                                // Sees if Next is symbol =
                                if (!CheckSymbol(tokens.ElementAt(nextIndex), "="))
                                    throw new Exception("Invalid let command");
                                nextIndex++;

                                // Gets the expression
                                innerCommand = ParseCommand(tokens, nextIndex, out nextIndex);
                                if (innerCommand is Expression or VarName)
                                    letCommand.Expression = innerCommand;
                                else
                                    throw new Exception("Invalid let command");
                            }
                            else if (innerCommand is Expression)
                            {
                                letCommand.VarName = ((Expression) innerCommand).Term;

                                // make a it to a do command 
                                if (CheckSymbol(tokens.ElementAt(nextIndex), "("))
                                {
                                    nextIndex++;

                                    var inlineDoCommand = new DoCommand
                                    {
                                        methodName = ((Expression) innerCommand).OpTerm
                                    };

                                    while (!CheckSymbol(tokens.ElementAt(nextIndex), ")"))
                                    {
                                        if (CheckSymbol(tokens.ElementAt(nextIndex), ","))
                                            nextIndex++;
                                        // Gets the expression
                                        innerCommand = ParseCommand(tokens, nextIndex, out nextIndex);
                                        if (innerCommand is VarName or Constant or Expression)
                                            inlineDoCommand.ValueHolders.Add(innerCommand);
                                        else
                                            throw new Exception("Invalid inline do command");
                                    }

                                    nextIndex++;
                                    letCommand.Expression = inlineDoCommand;
                                }
                                else
                                    letCommand.Expression = ((Expression) innerCommand).OpTerm;
                            }
                            else
                                throw new Exception("Invalid let command");

                            // Sees if the next symbol is ;
                            if (!CheckSymbol(tokens.ElementAt(nextIndex), ";"))
                                throw new Exception("Invalid let command");
                            nextIndex++;

                            return letCommand;

                        case "while":
                            var whileCommand = new WhileStatement();

                            // Sees if the next symbol is (
                            if (!CheckSymbol(tokens.ElementAt(nextIndex), "("))
                                throw new Exception("Invalid while command");
                            nextIndex++;

                            // Gets the expression
                            innerCommand = ParseCommand(tokens, nextIndex, out nextIndex);
                            if (innerCommand is Expression or VarName or Constant)
                                whileCommand.Expression = innerCommand;
                            else
                                throw new Exception("Invalid let command");

                            // Sees if the next symbol is )
                            if (!CheckSymbol(tokens.ElementAt(nextIndex), ")"))
                                throw new Exception("Invalid while command");
                            nextIndex++;

                            // Sees if the next symbol is {
                            if (!CheckSymbol(tokens.ElementAt(nextIndex), "{"))
                                throw new Exception("Invalid while command");
                            nextIndex++;

                            // Gets the next command while the next is not }
                            while (!CheckSymbol(tokens.ElementAt(nextIndex), "}"))
                            {
                                whileCommand.Statements.Add(ParseCommand(tokens, nextIndex, out nextIndex));
                            }

                            nextIndex++;
                            return whileCommand;

                        case "if":
                            var ifCommand = new IfStatement();

                            // Sees if the next symbol is (
                            if (!CheckSymbol(tokens.ElementAt(nextIndex), "("))
                                throw new Exception("Invalid if command");
                            nextIndex++;

                            // Gets the expression
                            innerCommand = ParseCommand(tokens, nextIndex, out nextIndex);
                            if (innerCommand is Expression or VarName)
                            {
                                // make a it to a do command 
                                if (CheckSymbol(tokens.ElementAt(nextIndex), "("))
                                {
                                    nextIndex++;

                                    var inlineDoCommand = new DoCommand
                                    {
                                        methodName = ((VarName) innerCommand)
                                    };

                                    while (!CheckSymbol(tokens.ElementAt(nextIndex), ")"))
                                    {
                                        if (CheckSymbol(tokens.ElementAt(nextIndex), ","))
                                            nextIndex++;
                                        // Gets the expression
                                        innerCommand = ParseCommand(tokens, nextIndex, out nextIndex);
                                        if (innerCommand is VarName or Constant or Expression)
                                            inlineDoCommand.ValueHolders.Add(innerCommand);
                                        else
                                            throw new Exception("Invalid inline do command");
                                    }

                                    nextIndex++;
                                    ifCommand.Expression = inlineDoCommand;
                                }

                                ifCommand.Expression = innerCommand;
                            }

                            else
                                throw new Exception("Invalid if command");
                            
                            // Sees if the next symbol is )
                            if (!CheckSymbol(tokens.ElementAt(nextIndex), ")"))
                                throw new Exception("Invalid if command");
                            nextIndex++;

                            // Sees if the next symbol is {
                            if (!CheckSymbol(tokens.ElementAt(nextIndex), "{"))
                                throw new Exception("Invalid if command");
                            nextIndex++;

                            // Gets the next command while the next is not }
                            while (!CheckSymbol(tokens.ElementAt(nextIndex), "}"))
                            {
                                ifCommand.Statements.Add(ParseCommand(tokens, nextIndex, out nextIndex));
                            }

                            nextIndex++;
                            return ifCommand;

                        case "return":
                            var returnCommand = new ReturnCommand();
                            if (!CheckSymbol(tokens.ElementAt(nextIndex), ";"))
                            {
                                innerCommand = ParseCommand(tokens, nextIndex, out nextIndex);
                                returnCommand.Value = innerCommand;
                            }
                            // Sees if the next symbol is ;
                            if (!CheckSymbol(tokens.ElementAt(nextIndex), ";"))
                                throw new Exception("Invalid let command");
                            nextIndex++;
                            return returnCommand;
                        
                        case "class":
                            var classCommand = new ClassCommand();
                            
                            // Gets the name
                            innerCommand = ParseCommand(tokens, nextIndex, out nextIndex);
                            if (innerCommand is VarName)
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
                        
                        case "constructor":
                            var ctorCommand = new CtorCommand();
                            
                            // Gets the type
                            innerCommand = ParseCommand(tokens, nextIndex, out nextIndex);
                            if (innerCommand is not VarName)
                                throw new Exception("Invalid function command");
                            
                            // Gets the name
                            innerCommand = ParseCommand(tokens, nextIndex, out nextIndex);
                            if (innerCommand is VarName)
                                ctorCommand.FunctionName = (VarName) innerCommand;
                            else
                                throw new Exception("Invalid function command");
                            
                            
                            // Sees if the next symbol is (
                            if (!CheckSymbol(tokens.ElementAt(nextIndex), "("))
                                throw new Exception("Invalid function command");
                            nextIndex++;


                            while (!CheckSymbol(tokens.ElementAt(nextIndex), ")"))
                            {
                                if(CheckSymbol(tokens.ElementAt(nextIndex),","))
                                    nextIndex++;
                                // Gets the expression
                                innerCommand = ParseCommand(tokens, nextIndex, out nextIndex);
                                if (innerCommand is ValueHolder)
                                    ctorCommand.ValueHolders.Add((ValueHolder)innerCommand);
                                else
                                    throw new Exception("Invalid do command");
                            }
                            nextIndex++;

                            // Sees if the next symbol is {
                            if (!CheckSymbol(tokens.ElementAt(nextIndex), "{"))
                                throw new Exception("Invalid function command");
                            nextIndex++;
                            
                            // Gets the next command while the next is not }
                            while (!CheckSymbol(tokens.ElementAt(nextIndex), "}"))
                            {
                                ctorCommand.Statements.Add(ParseCommand(tokens, nextIndex, out nextIndex));
                            }

                            nextIndex++;
                            return ctorCommand;
                        
                        case "method":
                        case "function":
                            var functionCommand = new FunctionCommand();
                            
                            // Gets the type
                            innerCommand = ParseCommand(tokens, nextIndex, out nextIndex);
                            if (innerCommand is not VarName)
                                throw new Exception("Invalid function command");
                            
                            // Gets the name
                            innerCommand = ParseCommand(tokens, nextIndex, out nextIndex);
                            if (innerCommand is VarName)
                                functionCommand.FunctionName = (VarName) innerCommand;
                            else
                                throw new Exception("Invalid function command");
                            
                            
                            // Sees if the next symbol is (
                            if (!CheckSymbol(tokens.ElementAt(nextIndex), "("))
                                throw new Exception("Invalid function command");
                            nextIndex++;


                            while (!CheckSymbol(tokens.ElementAt(nextIndex), ")"))
                            {
                                if(CheckSymbol(tokens.ElementAt(nextIndex),","))
                                    nextIndex++;
                                // Gets the expression
                                innerCommand = ParseCommand(tokens, nextIndex, out nextIndex);
                                if (innerCommand is ValueHolder)
                                    functionCommand.ValueHolders.Add((ValueHolder)innerCommand);
                                else
                                    throw new Exception("Invalid do command");
                            }
                            nextIndex++;

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
                        
                        case "static":
                        case "field":
                        case "var":
                            var varCommand = new VarCommand(currentToken.Text);
                            
                            // Gets the type
                            innerCommand = ParseCommand(tokens, nextIndex, out nextIndex);
                            if (innerCommand is not VarName)
                                throw new Exception("Invalid var command");
                            
                            // Gets the name
                            innerCommand = ParseCommand(tokens, nextIndex, out nextIndex);
                            if (innerCommand is VarName)
                                varCommand.VarName = (VarName) innerCommand;
                            else
                                throw new Exception("Invalid var command");
                            
                            // Sees if the next symbol is ;
                            if (!CheckSymbol(tokens.ElementAt(nextIndex), ";"))
                                throw new Exception("Invalid var command");
                            nextIndex++;
                            
                            return varCommand;
                        
                        case "true":
                            return new Constant(-1);
                        
                        case "false":
                            return new Constant(0);
                        
                        case "do":
                            var doCommand = new DoCommand();
                            
                            innerCommand = ParseCommand(tokens, nextIndex, out nextIndex);
                            if (innerCommand is VarName)
                                doCommand.methodName = (VarName) innerCommand;
                            else
                                throw new Exception("Invalid do command");
                            
                            // Sees if the next symbol is (
                            if (!CheckSymbol(tokens.ElementAt(nextIndex), "("))
                                throw new Exception("Invalid do command");
                            nextIndex++;

                            while (!CheckSymbol(tokens.ElementAt(nextIndex), ")"))
                            {
                                if(CheckSymbol(tokens.ElementAt(nextIndex),","))
                                    nextIndex++;
                                // Gets the expression
                                innerCommand = ParseCommand(tokens, nextIndex, out nextIndex);
                                if (innerCommand is VarName or Constant or ThisHolder)
                                    doCommand.ValueHolders.Add(innerCommand);
                                else
                                    throw new Exception("Invalid do command");
                            }
                            nextIndex++;
                            
                            // Sees if the next symbol is ;
                            if (!CheckSymbol(tokens.ElementAt(nextIndex), ";"))
                                throw new Exception("Invalid do command");
                            nextIndex++;
                            
                            return doCommand;
                        
                        case "else":
                            var elseCommand = new ElseCommand();
                            
                            // Sees if the next symbol is {
                            if (!CheckSymbol(tokens.ElementAt(nextIndex), "{"))
                                throw new Exception("Invalid else command");
                            nextIndex++;

                            // Gets the next command while the next is not }
                            while (!CheckSymbol(tokens.ElementAt(nextIndex), "}"))
                            {
                                elseCommand.Statements.Add(ParseCommand(tokens, nextIndex, out nextIndex));
                            }

                            nextIndex++;
                            
                            return elseCommand;

                        case "this":
                            return new ThisHolder();

                        default:
                            throw new Exception("unknown Keyword");
                    }

                    break;
                
                case AttributeEnum.Symbol:
                    if(!CheckSymbol(tokens.ElementAt(nextIndex-2)))
                        return currentToken.Text is "+" or "-" or "=" or ">" or "<"
                            ? new Operation(currentToken.Text)
                            : null;
                    neg = true;
                    break;
                
                case AttributeEnum.IntegerConstant:
                case AttributeEnum.Identifier:
                    var expressionCommand = new Expression();
                    
                    // Gets the first term
                    if (currentToken.Attribute == AttributeEnum.IntegerConstant)
                        expressionCommand.Term = new Constant(neg ? -int.Parse(currentToken.Text) : int.Parse(currentToken.Text));
                    else if (CheckSymbol(tokens.ElementAt(nextIndex), "["))
                    {
                        var arrayVar = new ArrayVarCommand();
                        nextIndex++;
                        
                        arrayVar.Value = currentToken.Text;
                        
                        innerCommand = ParseCommand(tokens, nextIndex, out nextIndex);
                        if (innerCommand is VarName or Constant or Expression)
                            arrayVar.index = innerCommand;
                        else
                            throw new Exception("Invalid array index");
                        
                        nextIndex++;

                        expressionCommand.Term = arrayVar;
                    }
                    else
                        expressionCommand.Term = new VarName(currentToken.Text);
                    
                    // Get if there is a symbol and another term
                    if (tokens.ElementAt(nextIndex).Attribute != AttributeEnum.Symbol) return expressionCommand.Term;
                    if (CheckSymbol(tokens.ElementAt(nextIndex), ";")) return expressionCommand.Term;
                    
                    var next = ParseCommand(tokens, nextIndex, out nextIndex);
                    if (next == null)
                    {
                        nextIndex--;
                        return expressionCommand.Term;
                    }
                    
                    expressionCommand.Operation = next;
                    
                    // Gets the next part of the expression
                    expressionCommand.OpTerm = ParseCommand(tokens, nextIndex, out nextIndex);
                    return expressionCommand;
                
                case AttributeEnum.StringConstant:
                    return new StringConstant(currentToken.Text);
                
                default:
                    throw new Exception("Unknown Command");
            }
        }

        throw new Exception("No More Commands");
    }

    private bool CheckSymbol(Token token, string validSymbol)
    {
        if (token.Attribute != AttributeEnum.Symbol) return false;
        return token.Text == validSymbol;
    }   
    private bool CheckSymbol(Token token)
    {
        if (token.Attribute != AttributeEnum.Symbol) return false;
        return true;
    }   
}