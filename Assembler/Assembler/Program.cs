using System;

namespace Assembler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TextAssembler asm;
            if (args.Length >= 1 &&
                args[0] == "-h")
                asm = new HexAssembler(Console.Out);
            else
                asm = new BinAssembler(Console.Out);
            asm.Feed(Console.In);
        }
    }
}
