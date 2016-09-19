using System;
using System.IO;
using NDesk.Options;

namespace Assembler
{
    internal class Program
    {
        private static void ShowHelp(OptionSet opt)
        {
            Console.WriteLine("Usage: Assembler [OPTIONS]+");
            Console.WriteLine("Assembly or run MIPS program");
            Console.WriteLine();
            Console.WriteLine("Options:");
            opt.WriteOptionDescriptions(Console.Out);
        }

        private static void Main(string[] args)
        {
            string fin = null;
            string fout = null;
            var run = false;
            var isHex = false;
            var help = false;

            var opt =
                new OptionSet
                    {
                        { "r|run", "don't assembly, just run", v => run = v != null },
                        { "o|output=", "output {FILE}", v => fout = v },
                        { "H|hex", "output hex instead of binary", v => isHex = v != null },
                        { "h|?|help", "show this message and exit", v => help = v != null }
                    };

            try
            {
                var extra = opt.Parse(args);

                if (help)
                {
                    ShowHelp(opt);
                    return;
                }

                if (extra.Count == 0)
                    throw new ApplicationException("must specify --input");

                fin = extra[0];

                if (extra.Count > 1)
                    throw new ApplicationException("extra command line argument(s):" + string.Join(" ", extra));

                if (run && isHex)
                    throw new ApplicationException("cann't run with --hex");

                if (run && !string.IsNullOrEmpty(fout))
                    throw new ApplicationException("cann't run with --output");
            }
            catch (Exception e)
            {
                Console.Error.Write("Assembler: ");
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine("Try `Assembler --help' for more information.");
                return;
            }

            try
            {
                using (var sin = new StreamReader(fin))
                    if (run)
                    {
                        var asm = new AsmExecuter();
                        asm.Feed(sin);
                        Console.WriteLine($"CFlag: {asm.CPU.CFlag}  ZeroFlag: {asm.CPU.ZeroFlag}");
                        for (var i = 0; i < asm.CPU.Registers.Length; i++)
                            Console.WriteLine($"R{i} = 0x{asm.CPU.Registers[i]:x2} ({asm.CPU.Registers[i]})");
                        for (var i = 0; i < asm.CPU.Ram.Length; i += 8)
                        {
                            Console.Write($"0x{i:x2} | ");
                            for (var j = i; j < i + 8 && j < asm.CPU.Ram.Length; j++)
                                Console.Write($"0x{asm.CPU.Ram[j]:x2} ({asm.CPU.Ram[j]})".PadRight(11));
                            Console.WriteLine();
                        }
                    }
                    else if (!string.IsNullOrEmpty(fout))
                        using (var sout = new StreamWriter(fout))
                            DoAssembly(isHex, false, sin, sout);
                    else
                        DoAssembly(isHex, false, sin, Console.Out);
            }
            catch (Exception e)
            {
                Console.Error.Write("Assembler: ");
                Console.Error.WriteLine(e.Message);
            }
        }

        private static void DoAssembly(bool isHex, bool isConsole, TextReader sin, TextWriter sout)
        {
            TextAssembler asm;
            if (isHex)
                asm = new HexAssembler(sout);
            else if (isConsole)
                asm = new BinAssembler(sout);
            else
                asm = new BinAssembler(sout, d: "");
            asm.Feed(sin);
        }
    }
}
