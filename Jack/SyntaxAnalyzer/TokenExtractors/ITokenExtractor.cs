using Jack.Model;

namespace Jack.SyntaxAnalyzer.TokenExtractors;

/// <summary>
/// A interface That Can Extract tokens from jack code
/// </summary>
public interface ITokenExtractor
{
    /// <summary>
    /// This trys to extract a token from the given string
    /// If it extracts a token it will remove where the token was in the given string
    /// </summary>
    /// <param name="file">A line or the full file of jack code</param>
    /// <param name="token">Gives the token if extracted else it will be a empty token</param>
    /// <returns>If it was able to extract a token</returns>
    bool TryExtract(ref string file, out Token token);
}