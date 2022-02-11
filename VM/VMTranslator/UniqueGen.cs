namespace VM.VMTranslator
{
    /// <summary>
    /// A class to Generate unique label names
    /// </summary>
    internal class UniqueGen
    {
        private int _uniqueNumber;
        
        /// <summary>
        /// Generates a unique label name containing the given name
        /// </summary>
        /// <param name="name">A name that will be added to the unique label</param>
        /// <returns>A Unique label</returns>
        internal string GenUniqueLabel(string name)
        {
            var unique = name + Convert.ToString(_uniqueNumber, 16);
            _uniqueNumber++;
            return unique;
        }
    }
}