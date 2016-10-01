using System;

namespace Assembler
{
    internal static class TypeRHelper
    {
        public static int Serialize(this ITypeRContext inst, SymbolResolver symbols, bool enableLongJump)
        {
            var op = GetOpcode(inst._Op);
            var rd = (inst._Rd);
            var rs = (inst._Rs);
            var rt = (inst._Rt);
            return (op << 12) | (rs << 10) | (rt << 8) | (rd << 6);
        }

        public static string Prettify(this ITypeRContext inst, SymbolResolver symbols, bool enableLongJump) =>
            $"{inst._Op.ToUpper().PadRight(4)} R{(inst._Rd)}, R{(inst._Rs)}, R{(inst._Rt)}";

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

        public static PCTarget Execute(this ITypeRContext inst,Context context)
        {
            switch (inst._Op.ToUpper())
            {
                case "AND":
                    context.Registers[inst._Rd] =
                        (byte)(context.Registers[inst._Rs] &
                               context.Registers[inst._Rt]);
                    context.ZeroFlag = context.Registers[inst._Rd] == 0;
                    return null;
                case "OR":
                    context.Registers[inst._Rd] =
                        (byte)(context.Registers[inst._Rs] |
                               context.Registers[inst._Rt]);
                    context.ZeroFlag = context.Registers[inst._Rd] == 0;
                    return null;
                case "ADD":
                    {
                        var s = context.Registers[inst._Rs] +
                                context.Registers[inst._Rt];
                        context.CFlag = (s & ~0xff) != 0;
                        context.Registers[inst._Rd] = (byte)s;
                        context.ZeroFlag = context.Registers[inst._Rd] == 0;
                        return null;
                    }
                case "SUB":
                    {
                        var s = context.Registers[inst._Rs] -
                                context.Registers[inst._Rt];
                        context.CFlag = (s & ~0xff) == 0;
                        context.Registers[inst._Rd] = (byte)s;
                        context.ZeroFlag = context.Registers[inst._Rd] == 0;
                        return null;
                    }
                case "ADDC":
                    {
                        var s = context.Registers[inst._Rs] +
                                context.Registers[inst._Rt] +
                                (context.CFlag ? 1 : 0);
                        context.CFlag = (s & ~0xff) != 0;
                        context.Registers[inst._Rd] = (byte)s;
                        context.ZeroFlag = context.Registers[inst._Rd] == 0;
                        return null;
                    }
                case "SUBC":
                    {
                        var s = context.Registers[inst._Rs] -
                                context.Registers[inst._Rt] -
                                (context.CFlag ? 0 : 1);
                        context.CFlag = (s & ~0xff) == 0;
                        context.Registers[inst._Rd] = (byte)s;
                        context.ZeroFlag = context.Registers[inst._Rd] == 0;
                        return null;
                    }
                case "SLT":
                    context.Registers[inst._Rd] =
                        (context.Registers[inst._Rs] <
                         context.Registers[inst._Rt])
                            ? (byte)1
                            : (byte)0;
                    context.ZeroFlag = context.Registers[inst._Rd] == 0;
                    return null;
                default:
                    throw new InvalidOperationException();
            }
        }

    }
}