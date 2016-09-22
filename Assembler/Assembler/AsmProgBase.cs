using System;
using System.Collections.Generic;
using Antlr4.Runtime;

namespace Assembler
{
    public abstract class AsmProgBase
    {
        protected readonly List<IExecutableInstruction> Instructions;

        private readonly List<string> m_Filenames;

        protected readonly Dictionary<int, string> Lines;

        protected readonly Dictionary<string, int> Symbols;

        protected AsmProgBase()
        {
            Instructions = new List<IExecutableInstruction>();
            m_Filenames = new List<string>();
            Lines = new Dictionary<int, string>();
            Symbols = new Dictionary<string, int>();
        }

        public void Feed(IEnumerable<AsmParser.LineContext> prog, string filename = "")
        {
            if (Instructions.Count != 0)
            {
                var lexer = new AsmLexer(new AntlrInputStream("HALT" + Environment.NewLine));
                var parser = new AsmParser(new CommonTokenStream(lexer));
                Parse(parser.line(), "Before " + filename);
            }

            foreach (var context in prog)
                Parse(context, filename);
        }

        public abstract void Done();

        private void Parse(AsmParser.LineContext context, string filename)
        {
            if (context.label() != null)
            {
                var lbl = MakeUniqueSymbol(context.label().Name().Symbol.Text, filename);
                if (Symbols.ContainsKey(lbl))
                    throw new ApplicationException("duplicate labels");
                Symbols.Add(lbl, Instructions.Count);
            }

            if (context.instruction() != null)
            {
                Lines.Add(
                          Instructions.Count,
                          $"{filename}:{context.Start.Line},{context.Start.Column} {context.GetText()}");
                Instructions.Add(context.instruction());
                m_Filenames.Add(filename);
            }
            else if (context.macro() != null)
            {
                Lines.Add(
                          Instructions.Count,
                          $"{filename}:{context.Start.Line},{context.Start.Column} {context.GetText()}");
                var instructions = context.macro().Flatten();
                Instructions.AddRange(instructions);
                for (var i = 0; i < instructions.Count; i++)
                    m_Filenames.Add(filename);
            }
        }

        private static string MakeUniqueSymbol(string lbl, string filename) =>
            !char.IsUpper(lbl, 0) ? lbl + "@" + filename : lbl;

        protected int GetSymbolPos(int now, string symbol)
        {
            int pos;
            if (!Symbols.TryGetValue(MakeUniqueSymbol(symbol, m_Filenames[now]), out pos))
                throw new KeyNotFoundException($"Symbol {symbol} not found.");
            return pos;
        }
    }
}
