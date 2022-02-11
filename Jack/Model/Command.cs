using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using System.Xml.Serialization;

namespace Jack.Model;

[XmlInclude(typeof(ArrayVarCommand))]
[XmlInclude(typeof(ClassCommand))]
[XmlInclude(typeof(Constant))]
[XmlInclude(typeof(DoCommand))]
[XmlInclude(typeof(ElseCommand))]
[XmlInclude(typeof(Expression))]
[XmlInclude(typeof(FunctionCommand))]
[XmlInclude(typeof(IfStatement))]
[XmlInclude(typeof(LetStatement))]
[XmlInclude(typeof(Operation))]
[XmlInclude(typeof(ReturnCommand))]
[XmlInclude(typeof(Statement))]
[XmlInclude(typeof(Token))]
[XmlInclude(typeof(ValueHolder))]
[XmlInclude(typeof(VarCommand))]
[XmlInclude(typeof(VarName))]
[XmlInclude(typeof(WhileStatement))]
public class Command
{
    private CommandType _type;

    public Command() { }
    public Command(CommandType type)
    {
        _type = type;
    }
}

public enum CommandType
{
    IF_STATEMENT,
    ELSE_STATEMENT,
    WHILE_STATEMENT,
    LET_STATEMENT,
    EXPRESSION,
    TERM,
    VAR_NAME,
    CONSTANT,
    OPERATION,
    RETURN,
    CLASS,
    DO,
    FUNCTION,
    VAR,
    ARRAY_VAR,
    STRING,
    CTOR,
    ARG
}