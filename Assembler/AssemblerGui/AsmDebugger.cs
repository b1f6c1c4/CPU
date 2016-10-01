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

    public class AsmDebugger : AsmProgBase
    {
        public event SimpleEventHandler OnPause;

        public Context CPU { get; }

        public SourcePosition Source => Lines[CPU.PC];

        private readonly HashSet<SourcePosition> m_BreakPoints;

        public AsmDebugger(HashSet<SourcePosition> breakPoints)
        {
            m_BreakPoints = breakPoints;
            CPU =
                new Context
                    {
                        PC = 0,
                        Registers = new byte[4],
                        Ram = new byte[256]
                    };
        }

        protected override bool ExpansionDebug => true;

        private bool Advance(ref int lease)
        {
            if (lease == 0)
                return false;
            if (lease > 0)
                lease--;

            var old = CPU.PC;
            var res = Instructions[CPU.PC].Execute(CPU);
            if (res == null)
                CPU.PC++;
            else if (res.IsSymbol)
                CPU.PC = GetSymbolPos(CPU.PC, res.Symbol);
            else if (res.IsAbs)
                CPU.PC = res.Position;
            else
                CPU.PC += res.Position + 1;

            CPU.PC &= Properties.Settings.Default.EnableLongJump ? 0xfff : 0xff;

            if (old == CPU.PC)
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
