using Jack.Model;

namespace Jack.SyntaxAnalyzer.TokenExtractors;

/// <summary>
/// Can extract Identifiers from the file
/// This should be the last Extractor
/// </summary>
public class IdentifierExtractor : ITokenExtractor
{
    public bool TryExtract(ref string file, out Token token)
    {
        token = new Token();
        if (file.Length == 0) return false;
        
        var endIndexSpace = file.IndexOf(' ',0);
        var linePartSpace = endIndexSpace >= 0 ? file[..endIndexSpace]: file;
                
        var endIndex = linePartSpace.IndexOfAny(ConstantLists.Symbols.ToArray());
        var linePart = endIndex >= 0 ? linePartSpace[..endIndex] : linePartSpace;
                
        var word = new string(linePart.Where(l => char.IsLetter(l) || l == '.').ToArray());
                
        word = word.Trim();

        if (ConstantLists.Keywords.Contains(word)) return false;
        
        token = new Token( AttributeEnum.IDENTIFIER, word);
        file = file.Remove(0, word.Length);
        return true;
    }
}