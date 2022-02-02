using VM.Model;

namespace VM.VMTranslator.CommandFinder
{
    public interface ICommandFinder
    {
        void Search(string line, ref Command command, ref bool found);
    }
}