using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Jack.Model;

namespace Jack.SyntaxAnalyzer;

internal class Tokenizer
{
    private XmlWriter _xmlWriter;
    private List<char> _symbols;
    private List<string> _keywords;

    public Tokenizer()
    {
        _xmlWriter = new XmlWriter();
        _symbols = new List<char>(){
            '{',
            '}',
            '(',
            ')',
            '[',
            ']',
            ',',
            ';',
            '+',
            '-',
            '*',
            '/',
            '&',
            '|',
            '<',
            '>',
            '=',
            '~'
        };
        _keywords = new List<string>(){
            "class",
            "constructor",
            "function",
            "method",
            "field",
            "static",
            "var",
            "true",
            "false",
            "null",
            "this",
            "let",
            "do",
            "if",
            "else",
            "while",
            "return"
        };
    }
    
    public IReadOnlyCollection<Token> Tokenize(string line)
    {
        var tokens = new List<Token>();
        var index = line.IndexOf("//", StringComparison.Ordinal);

        if (index > 0)
            line = line.Remove(index);
        

        while (line.Length > 0)
        {
            line = line.Trim();
            if (line[0] == '"')
            {
                var endIndex = line.IndexOf('"', 1);

                var sConstant = line.Substring(0, endIndex);

                sConstant = sConstant.Replace("\"", "");

                tokens.Add(
                    new Token(AttributeEnum.StringConstant,
                        sConstant)
                );
                line = line.Remove(0, sConstant.Length+2);
            }
            else if (int.TryParse(line[0].ToString(), out _))
            {
                var endIndex = line.IndexOf(' ', 0);
                var linePart = endIndex >= 0 ? line.Substring(0, endIndex) : line;
                
                var intConstant = Regex.Match(linePart, @"\d+").Value;

                if (int.TryParse(intConstant, out _))
                    tokens.Add(
                        new Token(AttributeEnum.IntegerConstant,
                            intConstant)
                    );
                line = line.Remove(0, intConstant.Length);
            }
            else if (_symbols.Contains(line[0]))
            {
                tokens.Add(
                    new Token(AttributeEnum.Symbol,
                        line[..1])
                );
                line = line.Remove(0, 1);
            }
            else
            {
                var endIndexSpace = line.IndexOf(' ',0);
                var linePartSpace = line[..endIndexSpace];
                
                var endIndex = linePartSpace.IndexOfAny(_symbols.ToArray());
                var linePart = endIndex >= 0 ? linePartSpace[..endIndex] : linePartSpace;
                
                var word = new string(linePart.Where(l => char.IsLetter(l) || l == '.').ToArray());
                
                word = word.Trim();

                if (_keywords.Contains(word))
                {
                    tokens.Add(new Token(
                        AttributeEnum.Keyword,
                        word));
                }
                else
                {
                    tokens.Add(new Token(
                        AttributeEnum.Identifier,
                        word));
                }
                line = line.Remove(0, word.Length);
            }
        }

        return tokens;
    }
}