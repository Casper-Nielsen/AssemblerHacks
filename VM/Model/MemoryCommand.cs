namespace VM.Model
{
    class MemoryCommand : Command
    {
        public MemoryMethod Method { get; set; }
        public Location Location { get; set; }
        public int Address { get; set; }
    }

    enum MemoryMethod
    {
        POP,
        PUSH
    }

    enum Location
    {
        CONSTANT,
        LOCAL = 1,
        ARGUMENT = 2,
        THAT = 3,
        THIS = 4,
        STATIC,
        TEMP,
        POINTER
    }
}