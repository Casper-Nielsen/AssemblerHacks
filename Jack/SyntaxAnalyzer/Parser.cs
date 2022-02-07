﻿using System.Diagnostics.SymbolStore;
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
                            var letCommand = new LetStatement();

                            // Gets VarName
                            var innerCommand = ParseCommand(tokens, nextIndex, out nextIndex);
                            if (innerCommand is VarName)
                                letCommand.VarName = (VarName) innerCommand;
                            else if(innerCommand is Expression)
                                letCommand.VarName = ((Expression) innerCommand).Term;
                            else
                                throw new Exception("Invalid let command");

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
                            return letCommand;

                        case "while":
                            var whileCommand = new WhileStatement();

                            // Sees if the next symbol is (
                            if (!CheckSymbol(tokens.ElementAt(nextIndex), "("))
                                throw new Exception("Invalid while command");
                            nextIndex++;

                            // Gets the expression
                            innerCommand = ParseCommand(tokens, nextIndex, out nextIndex);
                            if (innerCommand is Expression or VarName)
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
                                throw new Exception("Invalid while command");
                            nextIndex++;

                            // Gets the expression
                            innerCommand = ParseCommand(tokens, nextIndex, out nextIndex);
                            if (innerCommand is Expression or VarName)
                                ifCommand.Expression = innerCommand;
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
                                ifCommand.Statements.Add(ParseCommand(tokens, nextIndex, out nextIndex));
                            }

                            nextIndex++;
                            return ifCommand;

                        default:
                            throw new Exception("unknown Keyword");
                    }

                    break;
                case AttributeEnum.Symbol:
                    return currentToken.Text is "+" or "-" or "=" or ">" or "<"
                        ? new Operation(currentToken.Text)
                        : null;

                case AttributeEnum.StringConstant:
                    break;

                case AttributeEnum.IntegerConstant:
                case AttributeEnum.Identifier:
                    var expressionCommand = new Expression();
                    
                    // Gets the first term
                    if (currentToken.Attribute == AttributeEnum.IntegerConstant)
                        expressionCommand.Term = new Constant(int.Parse(currentToken.Text));
                    else
                        expressionCommand.Term = new VarName(currentToken.Text);
                    
                    // Get if there is a symbol and another term
                    if (tokens.ElementAt(nextIndex).Attribute != AttributeEnum.Symbol) return expressionCommand.Term;

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
}