using System;
using System.IO;
using Assembler.HackAssembly;
using Jack;
using VM;

var test = new JackCompiler();
test.run();
Console.WriteLine("From type (asm/vm): ");
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
}

Console.WriteLine("done");
Console.ReadLine();