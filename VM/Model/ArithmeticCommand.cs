namespace VM.Model
{
    class ArithmeticCommand : Command
    {
        public Method Method { get; set; }
    }

    enum Method
    {
        ADD,
        SUB,
        NEG,
        EQ,
        GT,
        LT,
        AND,
        OR,
        NOT
    }
}