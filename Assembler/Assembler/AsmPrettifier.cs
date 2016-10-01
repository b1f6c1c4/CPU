using System.IO;

namespace Assembler
{
    public class AsmPrettifier : AsmProgBase, IWriter
    {
        private TextWriter m_Writer;
        private readonly bool m_ExpandMacro;
        private readonly bool m_IncludeComment;

        public AsmPrettifier(bool includeComment = true, bool expandMacro = false)
        {
            m_IncludeComment = includeComment;
            m_ExpandMacro = expandMacro;
        }

        public void SetWriter(TextWriter writer) => m_Writer = writer;

        protected override void Parse(AsmEParser.LineContext context, string filename, int diff = 0)
        {
            if (context.label() != null)
                m_Writer.WriteLine($"{context.label().Name().Symbol.Text}:");

            string str = null;
            if (context.instruction() != null)
                str = context.instruction().Prettify(null, EnableLongJump);
            else if (context.macro() != null &&
                     (!m_ExpandMacro || m_IncludeComment))
                str = (m_ExpandMacro ? ";" : "") + context.macro().Prettify();

            if (m_IncludeComment && context.Comment() != null)
                m_Writer.WriteLine((str?.PadRight(24) ?? "    ") + context.Comment().Symbol.Text);
            else if (str != null)
                m_Writer.WriteLine(str);

            if (!m_ExpandMacro ||
                context.macro() == null)
                return;

            foreach (var inst in context.macro().Flatten(ExpansionDebug))
                m_Writer.WriteLine(inst.Prettify(null, EnableLongJump));
        }

        protected override bool ExpansionDebug => true;
    }

    public class AsmFinalPrettifier : AsmProgBase, IWriter
    {
        private TextWriter m_Writer;

        public void SetWriter(TextWriter writer) => m_Writer = writer;

        public override void Done()
        {
            base.Done();

            for (var i = 0; i < Instructions.Count; i++)
            {
                var inst = Instructions[i];
                var i1 = i;
                m_Writer.WriteLine(inst.Prettify((s, a) => GetSymbol(i1, s, a), EnableLongJump));
            }
        }

        protected override bool ExpansionDebug => true;
    }
}
