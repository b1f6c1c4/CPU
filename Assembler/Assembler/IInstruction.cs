using System;
using System.Collections.Generic;

namespace Assembler
{
    public interface IInstruction
    {
        int Length { get; }

        List<int> Serialize(Func<IInstruction, string, bool, int> symbols);
    }
}
