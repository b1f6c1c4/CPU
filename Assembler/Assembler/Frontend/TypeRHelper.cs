using System;

namespace Assembler.Frontend
{
    internal interface ITypeRContext
    {
        string Operator { get; }
        int RegisterD { get; }
        int RegisterS { get; }
        int RegisterT { get; }
    }

    internal static class TypeRHelper
    {
        public static int Serialize(this ITypeRContext inst, SymbolResolver symbols, bool enableLongJump)
        {
            var op = GetOpcode(inst.Operator);
            var rd = (inst.RegisterD);
            var rs = (inst.RegisterS);
            var rt = (inst.RegisterT);
            return (op << 12) | (rs << 10) | (rt << 8) | (rd << 6);
        }

        public static string Prettify(this ITypeRContext inst, SymbolResolver symbols, bool enableLongJump) =>
            $"{inst.Operator.ToUpper().PadRight(4)} R{(inst.RegisterD)}, R{(inst.RegisterS)}, R{(inst.RegisterT)}";

        private static int GetOpcode(string text)
        {
            switch (text.ToUpper())
            {
                case "AND":
                    return 0x0;
                case "OR":
                    return 0x1;
                case "ADD":
                    return 0x2;
                case "SUB":
                    return 0x3;
                case "ADDC":
                    return 0x6;
                case "SUBC":
                    return 0x5;
                case "SLT":
                    return 0x4;
                default:
                    throw new ArgumentOutOfRangeException(nameof(text));
            }
        }

        public static PCTarget Execute(this ITypeRContext inst, Context context)
        {
            switch (inst.Operator.ToUpper())
            {
                case "AND":
                    context.Registers[inst.RegisterD] =
                        (byte)(context.Registers[inst.RegisterS] &
                               context.Registers[inst.RegisterT]);
                    context.ZeroFlag = context.Registers[inst.RegisterD] == 0;
                    return null;
                case "OR":
                    context.Registers[inst.RegisterD] =
                        (byte)(context.Registers[inst.RegisterS] |
                               context.Registers[inst.RegisterT]);
                    context.ZeroFlag = context.Registers[inst.RegisterD] == 0;
                    return null;
                case "ADD":
                    {
                        var s = context.Registers[inst.RegisterS] +
                                context.Registers[inst.RegisterT];
                        context.CFlag = (s & ~0xff) != 0;
                        context.Registers[inst.RegisterD] = (byte)s;
                        context.ZeroFlag = context.Registers[inst.RegisterD] == 0;
                        return null;
                    }
                case "SUB":
                    {
                        var s = context.Registers[inst.RegisterS] -
                                context.Registers[inst.RegisterT];
                        context.CFlag = (s & ~0xff) == 0;
                        context.Registers[inst.RegisterD] = (byte)s;
                        context.ZeroFlag = context.Registers[inst.RegisterD] == 0;
                        return null;
                    }
                case "ADDC":
                    {
                        var s = context.Registers[inst.RegisterS] +
                                context.Registers[inst.RegisterT] +
                                (context.CFlag ? 1 : 0);
                        context.CFlag = (s & ~0xff) != 0;
                        context.Registers[inst.RegisterD] = (byte)s;
                        context.ZeroFlag = context.Registers[inst.RegisterD] == 0;
                        return null;
                    }
                case "SUBC":
                    {
                        var s = context.Registers[inst.RegisterS] -
                                context.Registers[inst.RegisterT] -
                                (context.CFlag ? 0 : 1);
                        context.CFlag = (s & ~0xff) == 0;
                        context.Registers[inst.RegisterD] = (byte)s;
                        context.ZeroFlag = context.Registers[inst.RegisterD] == 0;
                        return null;
                    }
                case "SLT":
                    context.Registers[inst.RegisterD] =
                        (context.Registers[inst.RegisterS] <
                         context.Registers[inst.RegisterT])
                            ? (byte)1
                            : (byte)0;
                    context.ZeroFlag = context.Registers[inst.RegisterD] == 0;
                    return null;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
