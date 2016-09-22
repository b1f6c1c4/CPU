using System.IO;

namespace Assembler
{
    public abstract class TextAssembler : AsmSerializer
    {
        protected readonly TextWriter Writer;
        protected readonly int Width;

        protected TextAssembler(TextWriter writer, int width = 16)
        {
            Writer = writer;
            Width = width;
        }
    }
}
