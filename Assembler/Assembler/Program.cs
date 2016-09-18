using System;

namespace Assembler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                TextAssembler asm;
                if (args.Length < 1)
                    asm = new HexAssembler(Console.Out);
                else
                    switch (args[0])
                    {
                        case "-b":
                            asm = new BinAssembler(Console.Out);
                            break;
                        case "-l":
                            asm = new BinAssembler(Console.Out, d: "");
                            break;
                        default:
                            throw new ArgumentException();
                    }
                asm.Feed(Console.In);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }
    }
}
