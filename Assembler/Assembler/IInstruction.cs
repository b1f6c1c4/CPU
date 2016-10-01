using System.Collections.Generic;

namespace Assembler
{
    public delegate int SymbolResolver(string symbol, bool isAbs);

    public interface IFlattenable<out TOut>
    {
        IReadOnlyList<TOut> Flatten(bool debug);
    }

    public interface IPrettifyable
    {
        string Prettify(SymbolResolver symbols, bool enableLongJump);
    }

    public interface IInstruction : IPrettifyable
    {
        int Serialize(SymbolResolver resolver, bool enableLongJump);
    }

    public interface IMacro : IFlattenable<IExecutableInstruction>, IPrettifyable { }
}
