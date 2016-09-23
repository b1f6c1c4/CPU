using System;

namespace Assembler
{
    public class BinAssembler : TextAssembler
    {
        private readonly string m_D;

        public BinAssembler(string d = " ") { m_D = d; }

        protected override void Put(int v)
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
