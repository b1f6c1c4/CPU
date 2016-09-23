using System.IO;

namespace Assembler
{
    public interface IWriter
    {
        void SetWriter(TextWriter writer);
    }

    public abstract class TextAssembler : AsmSerializer, IWriter
    {
        protected TextWriter Writer;
        protected readonly int Width;

        protected TextAssembler() { Width = 16; }

        public void SetWriter(TextWriter writer) => Writer = writer;

        protected override bool ExpansionDebug => false;
    }
}
