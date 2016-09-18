using System.Collections.Generic;

namespace Assembler
{
    internal interface IInstruction
    {
        int Length { get; }

        List<byte> Serialize(IDictionary<string, int> symbols);
    }
}
