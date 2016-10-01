using System;

namespace Assembler.Frontend
{
    internal interface ITypeIContext
    {
        string Operator { get; }
        int RegisterS { get; }
        int RegisterT { get; }
        INumberContext ImmediateNumber { get; }
        IObjContext Target { get; }
    }

    internal static class TypeIHelper
    {
        public static int Serialize(this ITypeIContext inst, SymbolResolver symbols, bool enableLongJump)
        {
            var op = GetOpcode(inst.Operator);
            var rs = inst.RegisterS;
            var rt = inst.RegisterT;
            int imm;
            if (inst.ImmediateNumber != null)
                imm = inst.ImmediateNumber.GetValue();
            else if (inst.Target != null)
                imm = inst.Target.Serialize(symbols, false);
            else
                throw new InvalidOperationException();
            if (inst.Target != null &&
                enableLongJump &&
                (imm > 0x7f ||
                 imm < -0x80))
                throw new ApplicationException($"BEQ/BNE 指令无法跳转到相对偏移 {imm} 处；请使用 JMP 进行跳转");
            return (op << 12) | (rs << 10) | (rt << 8) | (imm & 0xff);
        }

        public static string Prettify(this ITypeIContext inst, SymbolResolver symbols, bool enableLongJump) =>
            inst.Target != null
                ? symbols == null
                      ? $"{inst.Operator.ToUpper().PadRight(4)} R{inst.RegisterS}, R{inst.RegisterT}, {inst.Target}"
                      : $"{inst.Operator.ToUpper().PadRight(4)} R{inst.RegisterS}, R{inst.RegisterT}, 0x{(inst.Target.Serialize(symbols, true) & 0xff):x2}"
                : $"{inst.Operator.ToUpper().PadRight(4)} R{inst.RegisterT}, R{inst.RegisterS}, {inst.ImmediateNumber}";

        private static int GetOpcode(string text)
        {
            switch (text.ToUpper())
            {
                case "ANDI":
                    return 0x8;
                case "ORI":
                    return 0x9;
                case "ADDI":
                    return 0xa;
                case "LW":
                    return 0xb;
                case "SW":
                    return 0xc;
                case "BEQ":
                    return 0xd;
                case "BNE":
                    return 0xe;
                default:
                    throw new ArgumentOutOfRangeException(nameof(text));
            }
        }

        public static PCTarget Execute(this ITypeIContext inst, Context context)
        {
            switch (inst.Operator.ToUpper())
            {
                case "ANDI":
                    context.Registers[inst.RegisterT] =
                        (byte)(context.Registers[inst.RegisterS] &
                               inst.ImmediateNumber.GetValue());
                    context.ZeroFlag = context.Registers[inst.RegisterT] == 0;
                    return null;
                case "ORI":
                    context.Registers[inst.RegisterT] =
                        (byte)(context.Registers[inst.RegisterS] |
                               inst.ImmediateNumber.GetValue());
                    context.ZeroFlag = context.Registers[inst.RegisterT] == 0;
                    return null;
                case "ADDI":
                    {
                        var s = context.Registers[inst.RegisterS] +
                                inst.ImmediateNumber.GetValue();
                        context.CFlag = (s & ~0xff) != 0;
                        context.Registers[inst.RegisterT] = (byte)s;
                        context.ZeroFlag = context.Registers[inst.RegisterT] == 0;
                        return null;
                    }
                case "LW":
                    {
                        var addr = (context.Registers[inst.RegisterS] + inst.ImmediateNumber.GetValue()) & 0xff;
                        context.Registers[inst.RegisterT] = context.Ram[addr];
                        return null;
                    }
                case "SW":
                    {
                        var addr = (context.Registers[inst.RegisterS] + inst.ImmediateNumber.GetValue()) & 0xff;
                        context.Ram[addr] = context.Registers[inst.RegisterT];
                        return null;
                    }
                case "BEQ":
                    if (context.Registers[inst.RegisterS] == context.Registers[inst.RegisterT])
                        return inst.Target.Label ?? (PCTarget)(sbyte)inst.Target.ImmediateNumber.GetValue();
                    return null;
                case "BNE":
                    if (context.Registers[inst.RegisterS] != context.Registers[inst.RegisterT])
                        return inst.Target.Label ?? (PCTarget)(sbyte)inst.Target.ImmediateNumber.GetValue();
                    return null;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
