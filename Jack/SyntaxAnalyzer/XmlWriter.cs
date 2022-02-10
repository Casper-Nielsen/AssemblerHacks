using System.Xml;
using Jack.Model;

namespace Jack.SyntaxAnalyzer;

/// <summary>
/// This is a class used for debugging
/// It can write object to a xml file 
/// </summary>
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

    /// <summary>
    /// Writes the token in the xml
    /// needs to use close to save it
    /// </summary>
    /// <param name="token">The token that will be added to the xml</param>
    public void Write(Token token)
    {
        var rootNode = _doc.CreateElement(token.Attribute.ToString());
        rootNode.InnerText = token.Text;
        _node.AppendChild(rootNode);
    }

    /// <summary>
    /// Writes the object to the drive in the same method
    /// </summary>
    /// <param name="obj">The object The will be written in xml</param>
    public void WriteGood(object obj)
    {
        var x = new System.Xml.Serialization.XmlSerializer(obj.GetType());
        var stream = new FileStream(@".\temp.xml", FileMode.OpenOrCreate);
        x.Serialize(stream,obj);
        stream.Close();
    }
    
    /// <summary>
    /// Saves the file to the drive
    /// </summary>
    public void Close()
    {
        _doc.Save(@".\temp.xml");
    }
}