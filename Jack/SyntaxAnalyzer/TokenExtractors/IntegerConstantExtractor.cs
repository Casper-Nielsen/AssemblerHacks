using System.Text.RegularExpressions;
using Jack.Model;

namespace Jack.SyntaxAnalyzer.TokenExtractors;

/// <summary>
/// Can extract integers from the file
/// </summary>
public class IntegerConstantExtractor : ITokenExtractor
{
    public bool TryExtract(ref string file, out Token token)
    {
        token = new Token();
        if (file.Length == 0) return false;
        if (!int.TryParse(file[0].ToString(), out _)) return false;
        
        var endIndex = file.IndexOf(' ', 0);
        var linePart = endIndex >= 0 ? file[..endIndex] : file;

        var intConstant = Regex.Match(linePart, @"\d+").Value;

        if (!int.TryParse(intConstant, out _)) return false;
            
        token = new Token(AttributeEnum.INTEGER_CONSTANT, intConstant);
        file = file.Remove(0, intConstant.Length);
        return true;

    }
}