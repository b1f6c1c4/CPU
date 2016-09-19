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
            string fin;
            string fout = null;
            var run = false;
            var debug = false;
            var isHex = false;
            var help = false;

            var opt =
                new OptionSet
                    {
                        { "r|run", "don't assembly, just run", v => run = v != null },
                        { "d|debug", "pause at every symbol to debug", v => debug = v != null },
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

                if (!run && debug)
                    throw new ApplicationException("cann't assembly with --debug");

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
                        DoRun(debug, sin);
                    else if (!string.IsNullOrEmpty(fout))
                        using (var sout = new StreamWriter(fout))
                            DoAssembly(isHex, false, sin, sout);
                    else
                        DoAssembly(isHex, true, sin, Console.Out);
            }
            catch (Exception e)
            {
                Console.Error.Write("Assembler: ");
                Console.Error.WriteLine(e.Message);
            }
        }

        private static void DoRun(bool debug, TextReader sin)
        {
            var asm = new AsmExecuter();
            if (debug)
                asm.OnBreakPoint += s =>
                                    {
                                        Console.WriteLine($"BreakPoint: {s}");
                                        PrintContext(asm.CPU);
                                        Console.Read();
                                        Console.Read();
                                    };
            asm.Feed(sin);
            PrintContext(asm.CPU);
        }

        private static void PrintContext(Context cpu)
        {
            Console.WriteLine($"CFlag: {cpu.CFlag}  ZeroFlag: {cpu.ZeroFlag}");
            for (var i = 0; i < cpu.Registers.Length; i++)
                Console.WriteLine($"R{i} = 0x{cpu.Registers[i]:x2} ({cpu.Registers[i]})");
            for (var i = 0; i < cpu.Ram.Length; i += 8)
            {
                Console.Write($"0x{i:x2} | ");
                for (var j = i; j < i + 8 && j < cpu.Ram.Length; j++)
                    Console.Write($"0x{cpu.Ram[j]:x2} ({cpu.Ram[j]})".PadRight(11));
                Console.WriteLine();
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
                asm = new IntelAssembler(sout);
            asm.Feed(sin);
        }
    }
}
