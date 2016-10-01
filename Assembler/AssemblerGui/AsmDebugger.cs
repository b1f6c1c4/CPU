using System;
using System.Collections.Generic;
using Assembler;

namespace AssemblerGui
{
    public delegate void SimpleEventHandler();

    public class HaltException : ApplicationException
    {
        public HaltException() : base("HALT") { }
    }

    public class AsmDebugger : AsmExecuterBase
    {
        public event SimpleEventHandler OnPause;

        public SourcePosition Source => Lines[CPU.PC];

        private readonly HashSet<SourcePosition> m_BreakPoints;

        public AsmDebugger(HashSet<SourcePosition> breakPoints) { m_BreakPoints = breakPoints; }

        private bool Advance(ref int lease)
        {
            if (lease == 0)
                return false;
            if (lease > 0)
                lease--;

            if (Advance())
                throw new HaltException();

            return true;
        }

        public void Next(PauseCriterion criterion)
        {
            var lease = -1;
            Next(ref lease, criterion);
        }

        public void Next(ref int lease, PauseCriterion criterion)
        {
            while (true)
            {
                var old = Source;
                if (Advance(ref lease))
                    criterion.NotifyInstruction();
                if (!Source.Equals(old))
                    if (m_BreakPoints.Contains(Source))
                        break;
                if (criterion.ShouldPause(CPU, Source))
                    break;
                if (lease == 0)
                    return;
            }
            OnPause?.Invoke();
        }
    }
}
