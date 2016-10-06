using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Assembler;
using Assembler.Frontend;

namespace AssemblerGui
{
    public abstract class PauseCriterion
    {
        public virtual void NotifyInstruction() { }

        public abstract bool ShouldPause(Context cpu, SourcePosition pos);
    }

    public class NextInstruction : PauseCriterion
    {
        private bool m_Executed;

        public override void NotifyInstruction() => m_Executed = true;

        public override bool ShouldPause(Context cpu, SourcePosition pos) => m_Executed;
    }

    public class NextStatement : PauseCriterion
    {
        private readonly SourcePosition m_Pos;

        public NextStatement(SourcePosition pos) { m_Pos = pos; }

        public override bool ShouldPause(Context cpu, SourcePosition pos) => !m_Pos.Equals(pos);
    }

    public class NextProcedure : PauseCriterion
    {
        private readonly SourcePosition m_Pos;

        private readonly bool m_IsCall;

        public NextProcedure(SourcePosition pos)
        {
            var lines = File.ReadAllLines(pos.FilePath);
            if (pos.Line > lines.Length ||
                !lines[pos.Line - 1].TrimStart().StartsWith("CALL", true, CultureInfo.InvariantCulture))
            {
                m_Pos = pos;
                m_IsCall = false;
                return;
            }

            var l = pos.Line + 1;
            while (lines[l - 1].TrimStart().StartsWith(";", true, CultureInfo.InvariantCulture))
                l++;

            m_Pos = new SourcePosition(pos.FilePath, l);
            m_IsCall = true;
        }

        public override bool ShouldPause(Context cpu, SourcePosition pos) => !m_IsCall ^ m_Pos.Equals(pos);
    }

    public class JumpOut : PauseCriterion
    {
        private readonly int m_Bp;

        private readonly SourcePosition m_Pos;

        private SourcePosition m_Ret;

        public JumpOut(Context cpu, SourcePosition pos)
        {
            m_Ret = null;

            var regex = new Regex(@"\s*PUSH\s*BP\s*", RegexOptions.IgnoreCase);

            var lines = File.ReadAllLines(pos.FilePath);
            if (pos.Line > lines.Length ||
                !regex.IsMatch(lines[pos.Line - 1]))
            {
                var bp = cpu.Ram[0xfe];
                m_Bp = cpu.Ram[bp];
                m_Pos = null;
                return;
            }

            m_Bp = cpu.Ram[0xfe];
            m_Pos = pos;
        }

        public override bool ShouldPause(Context cpu, SourcePosition pos)
        {
            if (m_Ret != null)
                return !m_Ret.Equals(pos);

            if ((!(m_Pos?.Equals(pos) ?? false)) &&
                cpu.Ram[0xfe] == m_Bp)
                m_Ret = pos;
            return false;
        }
    }

    public class Cancellable : PauseCriterion
    {
        private readonly PauseCriterion m_Inner;
        private readonly CancellationToken m_Token;

        public Cancellable(PauseCriterion inner, CancellationToken token)
        {
            m_Inner = inner;
            m_Token = token;
        }

        public override bool ShouldPause(Context cpu, SourcePosition pos) =>
            m_Token.IsCancellationRequested || (m_Inner?.ShouldPause(cpu, pos) ?? false);
    }
}
