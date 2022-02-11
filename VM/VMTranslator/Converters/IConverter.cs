namespace VM.VMTranslator.Converters
{
    /// <summary>
    /// A interface for converting different commands to assembly 
    /// </summary>
    /// <typeparam name="T">the command type</typeparam>
    internal interface IConverter<in T>
    {
        /// <summary>
        /// Converts the Command to Assembly
        /// </summary>
        /// <param name="command">The command that will be converted</param>
        /// <param name="uniqueGen">A Unique generator</param>
        /// <returns>The assembly code in string format</returns>
        public string Convert(T command, ref UniqueGen uniqueGen);
    }
}