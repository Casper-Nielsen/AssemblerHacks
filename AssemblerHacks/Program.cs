using System;
using System.IO;
using Assembler.HackAssembly;
using Jack;
using VM;

var test = new JackCompiler();

Console.WriteLine("From type (asm/vm/jack): ");
var type = Console.ReadLine();

// Looks on what type the user want to convert
switch (type?.ToLower())
{
    case "asm":
    {
        Console.WriteLine("Path: ");
        var path = Console.ReadLine();
        if (path != null)
        {
            string file = File.ReadAllText(path);
            Console.WriteLine("reading asm");
            var hack = HackAssemblyConverter.ConvertFromHackAssembly(file);
            path = path.Replace(".asm", ".hack");
            File.WriteAllText(path,hack);
        }

        break;
    }
    case "vm":
    {
        Console.WriteLine("Path to files: ");
        var path = Console.ReadLine();
        Console.WriteLine("project Name: ");
        var name = Console.ReadLine();
        Console.WriteLine("reading files");
        if (path != null)
            if (name != null)
                VmConverter.Convert(path, name);
        break;
    }
    case "jack":
    {
        Console.WriteLine("Path to files: ");
        var path = Console.ReadLine();
        Console.WriteLine("reading files");
        var jack = new JackCompiler();
        if (path != null) jack.Compile(path);
        break;
    }
}

Console.WriteLine("done");
Console.ReadLine();