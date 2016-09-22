using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Antlr4.Runtime;
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
            List<string> fin;
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
                fin = opt.Parse(args);

                if (help)
                {
                    ShowHelp(opt);
                    return;
                }

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
                Environment.Exit(1);
                // ReSharper disable once HeuristicUnreachableCode
                return;
            }

            try
            {
                if (fin.Count > 0)
                    Do(
                       run,
                       debug,
                       fin.Select<string, Action<Action<TextReader>>>
                           (
                            f => a =>
                                 {
                                     using (var sin = new StreamReader(f))
                                         a(sin);
                                 }),
                       fout,
                       isHex);
                else
                {
                    if (!ConsoleEx.IsInputRedirected)
                        Console.WriteLine("Use Ctrl+Z to stop input and execute");
                    Do(run, debug, new Action<Action<TextReader>>[] { a => a(Console.In) }, fout, isHex);
                }
            }
            catch (Exception e)
            {
                Console.Error.Write("Assembler: ");
                Console.Error.WriteLine(e.ToString());
                Environment.Exit(1);
            }
        }

        private static void Do(bool run, bool debug, IEnumerable<Action<Action<TextReader>>> feeder, string fout,
                               bool isHex)
        {
            if (run)
                DoRun(debug, feeder);
            else if (!string.IsNullOrEmpty(fout))
                using (var sout = new StreamWriter(fout))
                    DoAssembly(isHex, false, feeder, sout);
            else
                DoAssembly(isHex, true, feeder, Console.Out);
        }

        private static void DoRun(bool debug, IEnumerable<Action<Action<TextReader>>> feeder)
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
            foreach (var action in feeder)
                action(sin => asm.Feed(Parse(sin).line()));
            asm.Done();
            PrintContext(asm.CPU);
        }

        private static void PrintContext(Context cpu)
        {
            Console.WriteLine($"PC: {cpu.PC} CFlag: {cpu.CFlag}  ZeroFlag: {cpu.ZeroFlag}");
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

        private static void DoAssembly(bool isHex, bool isConsole, IEnumerable<Action<Action<TextReader>>> feeder,
                                       TextWriter sout)
        {
            TextAssembler asm;
            if (isHex)
                asm = new HexAssembler(sout);
            else if (isConsole)
                asm = new BinAssembler(sout);
            else
                asm = new IntelAssembler(sout);

            foreach (var action in feeder)
                action(sin => asm.Feed(Parse(sin).line()));
            asm.Done();
        }

        private static AsmParser.ProgContext Parse(TextReader sin)
        {
            var lexer = new AsmLexer(new AntlrInputStream(sin));
            var parser = new AsmParser(new CommonTokenStream(lexer)); // { ErrorHandler = new BailErrorStrategy() };
            var prog = parser.prog();
            return prog;
        }
    }

    public static class ConsoleEx
    {
        public static bool IsOutputRedirected => FileType.Char != GetFileType(GetStdHandle(StdHandle.Stdout));

        public static bool IsInputRedirected => FileType.Char != GetFileType(GetStdHandle(StdHandle.Stdin));

        public static bool IsErrorRedirected => FileType.Char != GetFileType(GetStdHandle(StdHandle.Stderr));

        // P/Invoke:
        private enum FileType
        {
            Unknown,
            Disk,
            Char,
            Pipe
        };

        private enum StdHandle
        {
            Stdin = -10,
            Stdout = -11,
            Stderr = -12
        };

        [DllImport("kernel32.dll")]
        private static extern FileType GetFileType(IntPtr hdl);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(StdHandle std);
    }
}
