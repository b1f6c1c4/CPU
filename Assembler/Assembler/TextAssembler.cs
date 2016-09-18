using System.Collections.Generic;
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

        protected override void Put(List<int> res)
        {
            foreach (var v in res)
                PutValue(v & ((1 << Width) - 1));
        }

        protected abstract void PutValue(int v);
    }
}
