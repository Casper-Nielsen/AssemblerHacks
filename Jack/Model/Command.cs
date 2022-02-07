namespace Jack.Model;

public abstract class Command
{
    private CommandType _type;

    protected Command(CommandType type)
    {
        _type = type;
    }
}

public enum CommandType
{
    IfStatement,
    WhileStatement,
    letStatement,
    Expression,
    Term,
    VarName,
    Constant,
    Operation
}