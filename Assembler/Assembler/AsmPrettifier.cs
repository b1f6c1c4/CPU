using System.IO;

namespace Assembler
{
    public class AsmPrettifier : AsmProgBase, IWriter
    {
        private TextWriter m_Writer;
        private readonly bool m_ExpandMacro;

        public AsmPrettifier(bool expandMacro = false) { m_ExpandMacro = expandMacro; }
        public void SetWriter(TextWriter writer) => m_Writer = writer;

        protected override void Parse(AsmParser.LineContext context, string filename, int diff = 0)
        {
            if (context.label() != null)
                m_Writer.WriteLine($"{context.label().Name().Symbol.Text}:");

            string str = null;
            if (context.instruction() != null)
                str = context.instruction().Prettify();
            else if (context.macro() != null)
                str = (m_ExpandMacro ? ";" : "") + context.macro().Prettify();

            if (context.Comment() != null)
                m_Writer.WriteLine((str?.PadRight(24) ?? "    ") + context.Comment().Symbol.Text);
            else if (str != null)
                m_Writer.WriteLine(str);

            if (!m_ExpandMacro ||
                context.macro() == null)
                return;

            foreach (var inst in context.macro().Flatten(ExpansionDebug))
                m_Writer.WriteLine(inst.Prettify());
        }

        public override void Done() { }

        protected override bool ExpansionDebug => true;
    }
}
