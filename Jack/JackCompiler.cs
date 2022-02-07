using Jack.Model;
using Jack.SyntaxAnalyzer;

namespace Jack;

public class JackCompiler
{
    private Tokenizer _tokenizer;
    private Parser _parser;

    public JackCompiler()
    {
        _tokenizer = new Tokenizer();
        _parser = new Parser();
    }

    public void run()
    {
        string jack = @"
if (x < 0) {
    let sign = "+
                      " \"negative\""+
                      @";
            }
";

        var tokens = _tokenizer.Tokenize(jack);
        var commands = _parser.Parse(tokens);
        return;
    }
}