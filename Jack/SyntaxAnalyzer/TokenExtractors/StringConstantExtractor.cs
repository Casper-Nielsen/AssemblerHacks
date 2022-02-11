using Jack.Model;

namespace Jack.SyntaxAnalyzer.TokenExtractors;

/// <summary>
/// Can extract strings from the file
/// </summary>
public class StringConstantExtractor : ITokenExtractor
{
    public bool TryExtract(ref string file, out Token token)
    {
        token = new Token();
        
        if (file[0] != '"') return false;
        var endIndex = file.IndexOf('"', 1);

        var sConstant = file[..endIndex];

        sConstant = sConstant.Replace("\"", "");

        token = new Token(AttributeEnum.STRING_CONSTANT, sConstant);
        file = file.Remove(0, sConstant.Length+2);
        return true;
    }
}