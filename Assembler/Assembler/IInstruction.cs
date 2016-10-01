using System.Collections.Generic;

namespace Assembler
{
    public delegate int SymbolResolver(string symbol, bool isAbs);

    public interface IFlattenable<out TOut>
    {
        IReadOnlyList<TOut> Flatten(bool debug);
    }

    public interface IInstruction
    {
        int Serialize(SymbolResolver resolver, bool enableLongJump);

        string Prettify(SymbolResolver symbols, bool enableLongJump);
    }
}
