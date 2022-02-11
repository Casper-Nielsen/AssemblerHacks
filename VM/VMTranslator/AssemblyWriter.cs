using VM.Model;
using VM.VMTranslator.Converters;

namespace VM.VMTranslator
{
    /// <summary>
    /// Writes commands to assembly
    /// </summary>
    public class AssemblyWriter
    {
        private readonly FileStream _file;
        private readonly StreamWriter _sw;
        private UniqueGen _uniqueGen;

        private readonly ConverterGroup _converterGroup;

        public AssemblyWriter(string path)
        {
            _file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            _sw = new StreamWriter(_file);
            _uniqueGen = new UniqueGen();
            _converterGroup = new ConverterGroup();
            StartUp();
        }
        
        /// <summary>
        /// Adds The start up values
        /// </summary>
        private void StartUp()
        {
            string preSet = @"
@256
D=A
@SP
M=D
@2048
D=A
@ARG
M=D
@4096
D=A
@LCL
M=D
@sys.init
0;JMP
(FALSE)
@SP
A=M
M=0
@SP
M=M+1
@R14
A=M
0;JMP
(TRUE)
@SP
A=M
M=-1
@SP
M=M+1
@R14
A=M
0;JMP
(EQ)
@SP
AM=M-1
D=M
@SP
AM=M-1
D=M-D
@TRUE
D;JEQ
@FALSE
0;JMP
(GT)
@SP
AM=M-1
D=M
@SP
AM=M-1
D=M-D
@TRUE
D;JGT
@FALSE
0;JMP
(LT)
@SP
AM=M-1
D=M
@SP
AM=M-1
D=M-D
@TRUE
D;JLT
@FALSE
0;JMP";
            _sw.Write(preSet);
        }

        /// <summary>
        /// Closes the stream
        /// </summary>
        public void Close()
        {
            _sw.Close();
            _file.Close();
        }
        
        /// <summary>
        /// Writes the command in Assembly
        /// </summary>
        /// <param name="command">The command that will be converted to assembly</param>
        public void WriteCommand(Command command)
        {
            var stringCommand = command switch
            {
                ArithmeticCommand arithmeticCommand => _converterGroup.ArithmeticConverter.Convert(arithmeticCommand, ref _uniqueGen),
                MemoryCommand memoryCommand => _converterGroup.MemoryConverter.Convert(memoryCommand, ref _uniqueGen),
                LabelCommand labelCommand => _converterGroup.LabelConverter.Convert(labelCommand, ref _uniqueGen),
                GoToCommand goToCommand => _converterGroup.GoToConverter.Convert(goToCommand, ref _uniqueGen),
                FunctionCommand functionCommand => _converterGroup.FunctionConverter.Convert(functionCommand, ref _uniqueGen),
                CallCommand callCommand => _converterGroup.CallConverter.Convert(callCommand, ref _uniqueGen),
                ReturnCommand returnCommand => _converterGroup.ReturnConverter.Convert(returnCommand, ref _uniqueGen),
                _ => string.Empty
            };
            _sw.Write(stringCommand);
        }
        

    }
}