namespace Jack.Model;

internal class Token
{
    public AttributeEnum Attribute { get; set; }
    public string Text { get; set; }

    public Token(AttributeEnum attribute, string text)
    {
        Attribute = attribute;
        Text = text;
    }
}

internal enum AttributeEnum
{
    Keyword,
    Symbol,
    IntegerConstant,
    StringConstant,
    Identifier
}