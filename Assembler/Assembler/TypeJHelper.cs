using System;

namespace Assembler
{
    internal static class TypeJHelper
    {
        public static int Serialize(this ITypeJContext inst, SymbolResolver symbols, bool enableLongJump)
        {
            var op = GetOpcode(inst._Op);
            var imm = inst.obj().Serialize(symbols, true);
            return (op << 12) | (imm & 0xfff);
        }

        public static string Prettify(this ITypeJContext inst, SymbolResolver symbols, bool enableLongJump) =>
            symbols == null
                ? $"{inst._Op.ToUpper().PadRight(4)} {inst.obj()}"
                : enableLongJump
                      ? $"{inst._Op.ToUpper().PadRight(4)} 0x{(inst.obj().Serialize(symbols, true) & 0xfff):x3}"
                      : $"{inst._Op.ToUpper().PadRight(4)} 0x{(inst.obj().Serialize(symbols, true) & 0xff):x2}";

        private static int GetOpcode(string text)
        {
            switch (text.ToUpper())
            {
                case "JMP":
                    return 0x7;
                default:
                    throw new ArgumentOutOfRangeException(nameof(text));
            }
        }

        public static PCTarget Execute(this ITypeJContext inst,Context context)
        {
            switch (inst._Op.ToUpper())
            {
                case "JMP":
                    return inst.obj().Name() == null ? new PCTarget(inst.obj().number().GetValue(), true) : inst.obj().Name().Symbol.Text;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}