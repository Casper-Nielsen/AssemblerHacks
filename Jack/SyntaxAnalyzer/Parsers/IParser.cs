using Jack.Model;

namespace Jack.SyntaxAnalyzer.Parsers;

/// <summary>
/// Interface for Parsing tokens to commands and statements
/// </summary>
public interface IParser
{
    /// <summary>
    /// Parses Tokens to a command or statement
    /// </summary>
    /// <param name="tokens">Tokens</param>
    /// <param name="parserGroup">The ParserGroup</param>
    /// <param name="startIndex">The index The command or statement starts</param>
    /// <param name="nextIndex">Outputs the first index that haven't been used</param>
    /// <returns>A Command or statement</returns>
    Command? Parse(IReadOnlyCollection<Token> tokens, ParserGroup parserGroup, int startIndex,  out int nextIndex);
}