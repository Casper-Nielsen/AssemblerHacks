namespace Jack.Model.VMModels;

public enum DataLocation
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