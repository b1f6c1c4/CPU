using System;

namespace Assembler.Frontend
{
    internal interface ITypeJContext
    {
        string Operator { get; }
        IObjContext Target { get; }
    }

    internal static class TypeJHelper
    {
        public static int Serialize(this ITypeJContext inst, SymbolResolver symbols, bool enableLongJump)
        {
            var op = GetOpcode(inst.Operator);
            var imm = inst.Target.Serialize(symbols, true);
            return (op << 12) | (imm & 0xfff);
        }

        public static string Prettify(this ITypeJContext inst, SymbolResolver symbols, bool enableLongJump) =>
            symbols == null
                ? $"{inst.Operator.ToUpper().PadRight(4)} {inst.Target.Prettify()}"
                : enableLongJump
                      ? $"{inst.Operator.ToUpper().PadRight(4)} 0x{(inst.Target.Serialize(symbols, true) & 0xfff):x3}"
                      : $"{inst.Operator.ToUpper().PadRight(4)} 0x{(inst.Target.Serialize(symbols, true) & 0xff):x2}";

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

        public static PCTarget Execute(this ITypeJContext inst, Context context)
        {
            switch (inst.Operator.ToUpper())
            {
                case "JMP":
                    return inst.Target.Label ?? new PCTarget(inst.Target.ImmediateNumber.GetValue(), true);
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
