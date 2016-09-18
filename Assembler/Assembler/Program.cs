using System;

namespace Assembler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var asm = new TextAssembler(Console.Out);
            asm.Feed(Console.In);
        }
    }
}
