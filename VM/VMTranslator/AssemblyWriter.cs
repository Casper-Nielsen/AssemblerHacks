using System;
using System.IO;
using VM.Model;
using VM.VMTranslator.Converters;

namespace VM.VMTranslator
{
    public class AssemblyWriter
    {
        private FileStream _file;
        private StreamWriter _sw;
        private UniqueGen _uniqueGen;
        private readonly IConverter<ArithmeticCommand> _arithmeticConverter;
        private readonly IConverter<MemoryCommand> _memoryConverter;
        private readonly IConverter<LabelCommand> _labelConverter;
        private readonly IConverter<GoToCommand> _goToConverter;
        private readonly IConverter<FunctionCommand> _functionConverter;
        private readonly IConverter<CallCommand> _callConverter;
        private readonly IConverter<ReturnCommand> _returnConverter;

        public AssemblyWriter(string path)
        {
            _file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            _sw = new StreamWriter(_file);
            _uniqueGen = new UniqueGen();
            _arithmeticConverter = new ArithmeticConverter();
            _memoryConverter = new MemoryConverter();
            _labelConverter = new LabelConverter();
            _goToConverter = new GoToConverter();
            _functionConverter = new FunctionConverter();
            _callConverter = new CallConverter();
            _returnConverter = new ReturnConverter();
            StartUp();
        }
        
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
@JUMPOVERMETHODS
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
0;JMP
(JUMPOVERMETHODS)";
            _sw.Write(preSet);
        }

        public void Close()
        {
            _sw.Close();
            _file.Close();
        }
        public void WriteCommand(Command command)
        {
            var stringCommand = command switch
            {
                ArithmeticCommand arithmeticCommand => _arithmeticConverter.Convert(arithmeticCommand, ref _uniqueGen),
                MemoryCommand memoryCommand => _memoryConverter.Convert(memoryCommand, ref _uniqueGen),
                LabelCommand labelCommand => _labelConverter.Convert(labelCommand, ref _uniqueGen),
                GoToCommand goToCommand => _goToConverter.Convert(goToCommand, ref _uniqueGen),
                FunctionCommand functionCommand => _functionConverter.Convert(functionCommand, ref _uniqueGen),
                CallCommand callCommand => _callConverter.Convert(callCommand, ref _uniqueGen),
                ReturnCommand returnCommand => _returnConverter.Convert(returnCommand, ref _uniqueGen),
                _ => string.Empty
            };
            _sw.Write(stringCommand);
        }
        

    }
}