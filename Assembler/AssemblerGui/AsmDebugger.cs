using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using Assembler;

namespace AssemblerGui
{
    public class HaltException : ApplicationException
    {
        public HaltException() : base("HALT") { }
    }

    public class AsmDebugger : AsmProgBase
    {
        public delegate void UpdatedEventHandler();

        public event UpdatedEventHandler OnPause;

        public Context CPU { get; }

        public SourcePosition Source => Lines[CPU.PC];

        private readonly HashSet<SourcePosition> m_BreakPoints;

        public AsmDebugger()
        {
            m_BreakPoints = new HashSet<SourcePosition>();

            CPU =
                new Context
                    {
                        PC = 0,
                        Registers = new byte[4],
                        Ram = new byte[256]
                    };
        }

        public override void Done() { }

        protected override bool ExpansionDebug => true;

        public void ForceUpdate() => OnPause?.Invoke();

        public void AddBreakPoint(string filename, int id) =>
            m_BreakPoints.Add(new SourcePosition(filename, id));

        public void RemoveBreakPoint(string filename, int id) =>
            m_BreakPoints.Remove(new SourcePosition(filename, id));

        private void Advance()
        {
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
            if (old == CPU.PC)
                throw new HaltException();
        }

        private bool AdvanceStatement()
        {
            var pos = Source;
            while (pos == Source)
                Advance();
            return m_BreakPoints.Contains(Source);
        }

        public void NextInstruction()
        {
            Advance();
            OnPause?.Invoke();
        }

        public void NextStatement()
        {
            AdvanceStatement();
            OnPause?.Invoke();
        }

        public void NextProcedure(CancellationToken cancel)
        {
            var pos = Lines[CPU.PC];
            var lines = File.ReadAllLines(pos.FilePath);
            if (pos.Line > lines.Length ||
                !lines[pos.Line - 1].TrimStart().StartsWith("CALL", true, CultureInfo.InvariantCulture))
                AdvanceStatement();
            else
            {
                var l = pos.Line + 1;
                while (lines[pos.Line - 1].TrimStart().StartsWith(";", true, CultureInfo.InvariantCulture))
                    l++;

                var pox = new SourcePosition(pos.FilePath, l);

                while (!cancel.IsCancellationRequested &&
                       pox != Source)
                    if (AdvanceStatement())
                        break;
            }
            OnPause?.Invoke();
        }

        public void JumpOut(CancellationToken cancel)
        {
            var bp = CPU.Ram[CPU.Ram[0xfe]];
            while (!cancel.IsCancellationRequested &&
                   bp != CPU.Ram[0xfe])
                if (AdvanceStatement())
                    break;
            OnPause?.Invoke();
        }

        public void Run(CancellationToken cancel)
        {
            while (!cancel.IsCancellationRequested)
                if (AdvanceStatement())
                    break;
            OnPause?.Invoke();
        }
    }
}
