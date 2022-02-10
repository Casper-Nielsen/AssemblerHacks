using System;
using System.IO;
using Assembler.HackAssembly;
using Jack;
using VM;

var test = new JackCompiler();

Console.WriteLine("From type (asm/vm/jack): ");
var type = Console.ReadLine();


if (type.ToLower() == "asm")
{
    Console.WriteLine("Path: ");
    var path = Console.ReadLine();
    string file = File.ReadAllText(path);
    Console.WriteLine("reading asm");
    var hack = HackAssemblyConverter.ConvertFromHackAssembly(file);
    path = path.Replace(".asm", ".hack");
    File.WriteAllText(path,hack);
}else if (type.ToLower() == "vm")
{
    Console.WriteLine("Path to files: ");
    var path = Console.ReadLine();
    Console.WriteLine("project Name: ");
    var name = Console.ReadLine();
    Console.WriteLine("reading files");
    VMConverter.Convert(path,name);
}else if (type.ToLower() == "jack")
{
    Console.WriteLine("Path to files: ");
    var path = Console.ReadLine();
    Console.WriteLine("reading files");
    var jack = new JackCompiler();
    jack.Compile(path);
}

Console.WriteLine("done");
Console.ReadLine();