using VM.Model;

namespace VM.VMTranslator.CommandFinder
{
    /// <summary>
    /// A interface for Finding commands from a string
    /// </summary>
    public interface ICommandFinder
    {
        /// <summary>
        /// Searches the line for a Command
        /// </summary>
        /// <param name="line">The line that will be search through</param>
        /// <param name="command">The command will be set with the command if found</param>
        /// <param name="found">Will be set to true if found</param>
        void Search(string line, ref Command command, ref bool found);
    }
}