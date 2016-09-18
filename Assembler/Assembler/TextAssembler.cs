using System.Collections.Generic;
using System.IO;

namespace Assembler
{
    public class TextAssembler : AsmSerializer
    {
        private readonly TextWriter m_Writer;
        private readonly int m_Width;

        public TextAssembler(TextWriter writer, int width = 16)
        {
            m_Writer = writer;
            m_Width = width;
        }

        protected override void Put(List<int> res)
        {
            var fmt = "{0:X" + (m_Width / 4) + "}";
            foreach (var v in res)
                m_Writer.WriteLine(fmt, v);
        }
    }
}
