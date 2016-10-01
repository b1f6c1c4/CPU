using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Assembler;
using NDesk.Options;

namespace AssemblerCli
{
    internal static class Program
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
            var prettify = false;
            var final = false;
            var expand = false;
            var noComment = false;
            var debug = false;
            var isHex = false;
            var isBin = false;
            var help = false;
            var noHalt = false;
            var shortCode = false;

            var opt =
                new OptionSet
                    {
                        { "r|run", "don't assembly, just run", v => run = v != null },
                        { "s|short-code", "don't expand code space from 256 to 4096", v => shortCode = v != null },
                        { "n|no-halt", "don't append HALT after each file", v => noHalt = v != null },
                        { "p|prettify", "don't assembly, prettify asm", v => prettify = v != null },
                        { "no-comment", "when prettify, remove comments", v => noComment = v != null },
                        {
                            "f|final", "when prettify, remove labels, remove comments, expand macros",
                            v => final = v != null
                        },
                        { "e|expand", "when prettify, expand macros", v => expand = v != null },
                        { "d|debug", "pause at every symbol to debug", v => debug = v != null },
                        { "o|output=", "output {FILE}", v => fout = v },
                        { "H|hex", "output pure hex", v => isHex = v != null },
                        { "B|binary", "output pure binary", v => isBin = v != null },
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

                if (run && prettify)
                    throw new ApplicationException("cann't run with --prettify");

                if (prettify && isHex)
                    throw new ApplicationException("cann't prettify with --hex");

                if (prettify && isBin)
                    throw new ApplicationException("cann't prettify with --bin");

                if (isBin && isHex)
                    throw new ApplicationException("cann't --bin and --hex together");

                if (expand && !prettify)
                    throw new ApplicationException("cann't expand macros without --prettify");

                if (noComment && !prettify)
                    throw new ApplicationException("cann't remove comments without --prettify");

                if (final && !prettify)
                    throw new ApplicationException("cann't final prettify without --prettify");

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

            var isTmp = (fin.Count == 0);
            try
            {
                if (isTmp)
                {
                    if (!ConsoleEx.IsInputRedirected)
                        Console.WriteLine("Use Ctrl+Z to stop input and execute");

                    var ftmp = Path.GetTempFileName();
                    using (var tmpOut = new StreamWriter(ftmp))
                    {
                        string s;
                        while ((s = Console.In.ReadLine()) != null)
                            tmpOut.WriteLine(s);
                    }
                    fin.Add(ftmp);
                }

                var pre = new Preprocessor();
                pre.AddRange(fin);
                Action<AsmProgBase> action =
                    apb =>
                    {
                        foreach (var f in pre)
                            apb.Feed(f, !noHalt);
                        apb.Done();
                    };

                if (run)
                {
                    var asm = new AsmExecuter();
                    if (debug)
                        asm.OnBreakPoint +=
                            s =>
                            {
                                Console.WriteLine($"BreakPoint: {s}");
                                PrintContext(asm.CPU);
                                Console.Read();
                                Console.Read();
                            };

                    action(asm);
                    PrintContext(asm.CPU);
                }
                else
                {
                    var sout = string.IsNullOrEmpty(fout) ? Console.Out : new StreamWriter(fout);
                    try
                    {
                        AsmProgBase asm;
                        if (prettify)
                            if (final)
                                asm = new AsmFinalPrettifier();
                            else
                                asm = new AsmPrettifier(!noComment, expand);
                        else if (isHex)
                            asm = new HexAssembler();
                        else if (isBin)
                            asm = new BinAssembler();
                        else
                            asm = new IntelAssembler();

                        asm.EnableLongJump = !shortCode;

                        ((IWriter)asm).SetWriter(sout);

                        action(asm);
                    }
                    finally
                    {
                        if (!sout.Equals(Console.Out))
                            sout.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.Write("Assembler: ");
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }
            finally
            {
                if (isTmp && fin.Count > 0)
                    File.Delete(fin[0]);
            }
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
