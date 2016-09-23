namespace Assembler
{
    public class HexAssembler : TextAssembler
    {
        protected override void Put(int v)
        {
            var fmt = "{0:X" + (Width / 4) + "}";
            Writer.WriteLine(fmt, v);
        }
    }
}
