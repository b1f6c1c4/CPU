using System;

namespace Assembler.Frontend
{
    public class SourcePosition : IEquatable<SourcePosition>
    {
        public string FilePath { get; }

        public int Line { get; }

        public SourcePosition(string f, int l)
        {
            FilePath = f;
            Line = l;
        }

        public override bool Equals(object obj) => obj is SourcePosition && Equals((SourcePosition)obj);

        public override int GetHashCode()
        {
            unchecked
            {
                return ((FilePath?.GetHashCode() ?? 0) * 397) ^ Line;
            }
        }

        public bool Equals(SourcePosition other) => FilePath == other.FilePath && Line == other.Line;
    }
}
