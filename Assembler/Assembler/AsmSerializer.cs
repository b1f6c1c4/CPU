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

        protected AsmSerializer(int initial = 0)
        {
            m_Position = initial;
            m_Symbols = new Dictionary<string, int>();
        }

        public void Feed(TextReader reader)
        {
            var lexer = new AsmLexer(new AntlrInputStream(reader));
            var parser = new AsmParser(new CommonTokenStream(lexer)) { ErrorHandler = new BailErrorStrategy() };
            var prog = parser.prog();

            var ini = m_Position;

            foreach (var context in prog.line())
                Parse(context);

            m_Position = ini;

            foreach (var context in prog.line())
                Serialize(context);

            PutFinal();
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
                m_Position += context.instruction().Length;
        }

        private void Serialize(AsmParser.LineContext context)
        {
            if (context.instruction() != null)
            {
                var res = context.instruction().Serialize(GetSymbol);
                Put(res);
                m_Position += res.Count;
            }
        }

        protected abstract void Put(List<int> res);

        protected virtual void PutFinal() { }

        private int GetSymbol(IInstruction inst, string symbol, bool absolute)
        {
            int pos;
            if (!m_Symbols.TryGetValue(symbol, out pos))
                throw new KeyNotFoundException($"Symbol {symbol} not found.");

            if (absolute)
                return pos;
            return pos - (m_Position + inst.Length);
        }
    }
}
