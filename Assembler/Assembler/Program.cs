using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

namespace Assembler
{
    class Program
    {
        static void Main(string[] args)
        {
            var lexer = new AsmLexer(new AntlrInputStream(Console.In));
            var parser = new AsmParser(new CommonTokenStream(lexer));
            var prog = parser.prog();
            // TODO: asm prog
        }
    }
}
