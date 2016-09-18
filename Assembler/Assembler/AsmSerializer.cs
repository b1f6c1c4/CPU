using System;
using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;

namespace Assembler
{
    public abstract class AsmSerializer
    {
        private int m_Position;

        private readonly Dictionary<string, int> m_Symbols;

        protected AsmSerializer()
        {
            m_Position = 0;
            m_Symbols = new Dictionary<string, int>();
        }

        public void Feed(TextReader reader)
        {
            var lexer = new AsmLexer(new AntlrInputStream(reader));
            var parser = new AsmParser(new CommonTokenStream(lexer));
            var prog = parser.prog();

            foreach (var context in prog.line())
                Parse(context);
        }

        private void Parse(AsmParser.LineContext context)
        {
            if (context.label() != null)
            {
                var lbl = context.label().Name().Symbol.Text;
                if (m_Symbols.ContainsKey(lbl))
                    throw new ApplicationException("duplicate labels");
                m_Symbols.Add(lbl, m_Position);
            }

            if (context.instruction() != null)
            {
                var res = context.instruction().Serialize(GetSymbol);
                Put(res);
                m_Position += res.Count;
            }
        }

        protected abstract void Put(List<int> res);

        private int GetSymbol(IInstruction inst, string symbol)
        {
            int pos;
            if (!m_Symbols.TryGetValue(symbol, out pos))
                throw new KeyNotFoundException($"Symbol {symbol} not found.");

            return pos - (m_Position + inst.Length);
        }
    }
}
