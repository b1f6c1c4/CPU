using System;
using System.IO;

namespace Assembler
{
    public class BinAssembler : TextAssembler
    {
        private readonly string m_D;

        public BinAssembler(TextWriter writer, int width = 16, string d = " ") : base(writer, width) { m_D = d; }

        protected override void PutValue(int v)
        {
            var s = Convert.ToString(v, 2);
            s = s.PadLeft(Width, '0');
            while (s.Length > 0)
            {
                Writer.Write(s.Substring(0, 4));
                s = s.Substring(4);
                if (s.Length > 0)
                    Writer.Write(m_D);
            }
            Writer.WriteLine();
        }
    }
}
