namespace Jack.SyntaxAnalyzer;

public static class ConstantLists
{
    public static readonly List<char> Symbols = new List<char>(){
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
    public static readonly List<string> Keywords = new List<string>(){
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