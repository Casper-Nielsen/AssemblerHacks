namespace Jack.SyntaxAnalyzer;

public class ConstantLists
{
    public static List<char> Symbols = new List<char>(){
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
    public static List<string> Keywords = new List<string>(){
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