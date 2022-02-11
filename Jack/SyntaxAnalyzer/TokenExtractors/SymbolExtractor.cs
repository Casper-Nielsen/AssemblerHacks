using Jack.Model;

namespace Jack.SyntaxAnalyzer.TokenExtractors;

/// <summary>
/// Can extract Symbols from the file
/// the symbols are defined in the constant list
/// </summary>
public class SymbolExtractor : ITokenExtractor
{
    public bool TryExtract(ref string file, out Token token)
    {
        token = new Token();
        if (!ConstantLists.Symbols.Contains(file[0])) return false;
        token = new Token(AttributeEnum.SYMBOL,file[..1]);
        file = file.Remove(0, 1);
        return true;
    }
}