namespace Jack.Model;

public class Token
{
    public AttributeEnum Attribute { get; set; }
    public string Text { get; set; }

    public Token() { }
    public Token(AttributeEnum attribute, string text)
    {
        Attribute = attribute;
        Text = text;
    }
}

public enum AttributeEnum
{
    Keyword,
    Symbol,
    IntegerConstant,
    StringConstant,
    Identifier
}