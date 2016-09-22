using System.IO;

namespace Assembler
{
    public class IntelAssembler : TextAssembler
    {
        private int m_Address;

        public IntelAssembler(TextWriter writer, int width = 16) : base(writer, width) { m_Address = 0; }

        protected override void Put(int v)
        {
            Writer.Write($":{Width / 8:X2}{m_Address:X4}00");
            var fmt = "{0:X" + (Width / 4) + "}";
            Writer.Write(fmt, v);

            var ch = (byte)0;
            ch += (byte)(Width / 8);
            ch += (byte)((m_Address) >> 8);
            ch += (byte)(m_Address);
            {
                var t = v;
                for (var i = 0; i < Width / 4; i++)
                {
                    ch += (byte)t;
                    t >>= 8;
                }
            }
            ch = (byte)-ch;

            Writer.WriteLine($"{ch:X2}");

            m_Address ++; // += Width / 8;
        }

        protected override void PutFinal() => Writer.WriteLine($":00000001FF");
    }
}
