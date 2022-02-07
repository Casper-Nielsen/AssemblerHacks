using Jack.Model;
using Jack.SyntaxAnalyzer;

namespace Jack;

public class JackCompiler
{
    private Tokenizer _tokenizer;
    private Parser _parser;
    private XmlWriter _writer;

    public JackCompiler()
    {
        _tokenizer = new Tokenizer();
        _parser = new Parser();
        _writer = new XmlWriter();
    }

    public void run()
    {
        string jack = @"
class List{
    field Array array;
    field int arrayLenght;

    constructor List new(){
        let array = Array.new(1);
        let arrayLenght = 0;
        return this;
    }

    method void add(Player newPlayer){
        var Array tempArray;
        var int i;
        let i = 0;
        let tempArray = array;
        let array = Array.new(arrayLenght+1);
        while(i < arrayLenght){
            let array[i] = tempArray[i];
            let i = i + 1;
        }
        do tempArray.dispose();
        let array[arrayLenght] = newPlayer;
        let arrayLenght = arrayLenght + 1;
        return;
    }

    method void pulse(){
        var int i;
        var Player player;
        let i = 0;
        while(i < arrayLenght){
            let player = array[i];
            do player.move(array, arrayLenght);
            let array[i] = player;
            let i = i + 1;
        }
        return;
    }
}";

        var tokens = _tokenizer.Tokenize(jack);
        var commands = _parser.Parse(tokens);
        _writer.WriteGood(commands);
        
        return;
    }
}