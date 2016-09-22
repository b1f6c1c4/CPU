using System;
using System.Collections.Generic;

namespace Assembler
{
    public abstract class AsmSerializer
    {
        private readonly List<IInstruction> m_Instructions;

        private readonly Dictionary<string, int> m_Symbols;

        protected AsmSerializer()
        {
            m_Instructions = new List<IInstruction>();
            m_Symbols = new Dictionary<string, int>();
        }

        public void Feed(IEnumerable<AsmParser.LineContext> prog)
        {
            foreach (var context in prog)
                Parse(context);
        }

        public void Done()
        {
            for (var i = 0; i < m_Instructions.Count; i++)
            {
                var inst = m_Instructions[i];
                var i1 = i;
                Put(inst.Serialize((s, a) => GetSymbol(i1, s, a)));
            }

            PutFinal();
        }

        private void Parse(AsmParser.LineContext context)
        {
            if (context.label() != null)
            {
                var lbl = context.label().Name().Symbol.Text;
                if (m_Symbols.ContainsKey(lbl))
                    throw new ApplicationException("duplicate labels");
                m_Symbols.Add(lbl, m_Instructions.Count);
            }

            if (context.instruction() != null)
                m_Instructions.Add(context.instruction());
            else if (context.macro() != null)
                m_Instructions.AddRange(context.macro().Flatten());
        }

        protected abstract void Put(int res);

        protected virtual void PutFinal() { }

        private int GetSymbol(int now, string symbol, bool isAbs)
        {
            int pos;
            if (!m_Symbols.TryGetValue(symbol, out pos))
                throw new KeyNotFoundException($"Symbol {symbol} not found.");

            if (isAbs)
                return pos;
            return pos - (now + 1);
        }
    }
}
