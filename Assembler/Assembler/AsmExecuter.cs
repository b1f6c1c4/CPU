using System;
using System.Collections.Generic;

namespace Assembler
{
    public class AsmExecuter
    {
        public delegate void OnBreakPointEventHandler(string line);

        public event OnBreakPointEventHandler OnBreakPoint;

        private readonly List<IExecutableInstruction> m_Instructions;

        private readonly Dictionary<int, string> m_Lines;

        private readonly Dictionary<string, int> m_Symbols;

        public Context CPU { get; }

        public AsmExecuter()
        {
            m_Instructions = new List<IExecutableInstruction>();
            m_Lines = new Dictionary<int, string>();
            m_Symbols = new Dictionary<string, int>();

            CPU = new Context
                      {
                          PC = 0,
                          Registers = new byte[4],
                          Ram = new byte[256]
                      };
        }

        public void Feed(IEnumerable<AsmParser.LineContext> prog, string filename = "")
        {
            foreach (var context in prog)
                Parse(context, filename);
        }

        public void Done()
        {
            while (CPU.PC < m_Instructions.Count)
            {
                if (m_Symbols.ContainsValue(CPU.PC))
                    OnBreakPoint?.Invoke(m_Lines[CPU.PC]);

                var res = m_Instructions[CPU.PC].Execute(CPU);
                if (res == null)
                {
                    CPU.PC++;
                    continue;
                }

                if (res.IsSymbol)
                {
                    if (!m_Symbols.TryGetValue(res.Symbol, out CPU.PC))
                        throw new KeyNotFoundException($"Symbol {res.Symbol} not found.");
                    continue;
                }
                if (res.IsAbs)
                    CPU.PC = res.Position;
                else
                    CPU.PC += res.Position + 1;
            }
        }

        private void Parse(AsmParser.LineContext context, string filename)
        {
            if (context.label() != null)
            {
                var lbl = context.label().Name().Symbol.Text;
                if (m_Symbols.ContainsKey(lbl))
                    throw new ApplicationException("duplicate labels");
                m_Symbols.Add(lbl, m_Instructions.Count);
            }

            if (context.instruction() != null)
            {
                m_Lines.Add(
                            m_Instructions.Count,
                            $"{filename}:{context.Start.Line},{context.Start.Column} {context.GetText()}");
                m_Instructions.Add(context.instruction());
            }
            else if (context.macro() != null)
            {
                m_Lines.Add(
                            m_Instructions.Count,
                            $"{filename}:{context.Start.Line},{context.Start.Column} {context.GetText()}");
                m_Instructions.AddRange(context.macro().Flatten());
            }
        }
    }
}
