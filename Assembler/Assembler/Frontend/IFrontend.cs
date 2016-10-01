using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;

namespace Assembler.Frontend
{
    public interface IFrontend
    {
        IEnumerable<ILineContext> Parse(string filename, TextReader sin);
    }

    public sealed class AntlrStandardFrontend : IFrontend
    {
        public IEnumerable<ILineContext> Parse(string filename, TextReader sin)
        {
            var lexer = new AsmLexer(new AntlrInputStream(sin));
            var parser = new AsmParser(new CommonTokenStream(lexer));
            parser.AddErrorListener(new AssemblyHandler(filename));
            var prog = parser.prog();
            return prog.line();
        }
    }

    public sealed class AntlrExtendedFrontend : IFrontend
    {
        public IEnumerable<ILineContext> Parse(string filename, TextReader sin)
        {
            var lexer = new AsmELexer(new AntlrInputStream(sin));
            var parser = new AsmEParser(new CommonTokenStream(lexer));
            parser.AddErrorListener(new AssemblyHandler(filename));
            var prog = parser.prog();
            return prog.line();
        }
    }
}
