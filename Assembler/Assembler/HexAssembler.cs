using System.IO;

namespace Assembler
{
    public class HexAssembler : TextAssembler
    {
        public HexAssembler(TextWriter writer, int width = 16) : base(writer, width) { }

        protected override void Put(int v)
        {
            var fmt = "{0:X" + (Width / 4) + "}";
            Writer.WriteLine(fmt, v);
        }
    }
}
