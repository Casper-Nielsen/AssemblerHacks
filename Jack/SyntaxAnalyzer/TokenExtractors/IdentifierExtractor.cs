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
        
        var endIndexSpace = file.IndexOf(' ',0);
        var linePartSpace = file[..endIndexSpace];
                
        var endIndex = linePartSpace.IndexOfAny(ConstantLists.Symbols.ToArray());
        var linePart = endIndex >= 0 ? linePartSpace[..endIndex] : linePartSpace;
                
        var word = new string(linePart.Where(l => char.IsLetter(l) || l == '.').ToArray());
                
        word = word.Trim();

        if (ConstantLists.Keywords.Contains(word)) return false;
        
        token = new Token( AttributeEnum.Identifier, word);
        file = file.Remove(0, word.Length);
        return true;
    }
}