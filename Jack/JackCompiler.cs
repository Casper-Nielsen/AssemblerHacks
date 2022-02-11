using Jack.Converter;
using Jack.Model;
using Jack.SyntaxAnalyzer;

namespace Jack;

/// <summary>
/// This can Compile Jack code.
/// </summary>
public class JackCompiler
{
    private readonly Tokenizer _tokenizer;
    private readonly Parser _parser;
    private readonly ConvertToVmModels _converter;

    public JackCompiler()
    {
        _tokenizer = new Tokenizer();
        _parser = new Parser();
        _converter = new ConvertToVmModels();
    }

    /// <summary>
    /// Compiles the jack files from the file path to vm files 
    /// </summary>
    /// <param name="filepath">The path to the folder where the jack code is</param>
    public void Compile(string filepath)
    {
        var paths = Directory.EnumerateFiles(filepath + @"\", "*.jack").ToList();
        var commands = new Dictionary<string, IReadOnlyCollection<Command>>();
        
        // Reads all the files
        foreach (var path in paths)
        {
            using var stream = new FileStream(path, FileMode.Open);
            using var reader = new StreamReader(stream);
            var jack = new List<string>();
            while (!reader.EndOfStream)
            {
                jack.Add(reader.ReadLine() ?? string.Empty);
            }
            // Tokenizes the jack file
            var tokens = _tokenizer.Tokenize(jack);
            // Parses the Tokens
            commands.Add(path,_parser.Parse(tokens)!);
        }

        /* // used for debugging
        * var xmlWriter = new XmlWriter();
        * xmlWriter.WriteGood(commands.First(c => true).Value);
        */
        
        // Gets the Classes name from all the files 
        foreach (var (_, value) in commands)
        {
            _converter.AddClasses(value);
        }
        
        //Compiles the Parsed tokens to vm code
        foreach (var (path, value) in commands)
        {
            var converted = _converter.Convert(value);

            using var stream = new FileStream(path.Replace(".jack", ".vm"), FileMode.OpenOrCreate);
            using var writer = new StreamWriter(stream);
            foreach (var line in converted)
            {
                writer.WriteLine(line);
            }
        }
    }
}