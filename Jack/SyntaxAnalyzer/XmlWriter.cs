using System.Xml;
using Jack.Model;

namespace Jack.SyntaxAnalyzer;

internal class XmlWriter
{
    private XmlDocument _doc;
    private XmlNode _node;
    
    public XmlWriter()
    {
        _doc = new XmlDocument();
        _node = _doc.CreateElement("Jack");
        _doc.AppendChild(_node);
    }

    public void Write(Token token)
    {
        var rootNode = _doc.CreateElement(token.Attribute.ToString());
        rootNode.InnerText = token.Text;
        _node.AppendChild(rootNode);
    }

    public void Close()
    {
        _doc.Save(@"C:\Users\caspe\Desktop\test\temp.xml");
    }
}