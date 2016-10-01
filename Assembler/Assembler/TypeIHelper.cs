using System;

namespace Assembler
{
    internal static class TypeIHelper
    {
        public static int Serialize(this ITypeIContext inst, SymbolResolver symbols, bool enableLongJump)
        {
            var op = GetOpcode(inst._Op);
            var rs = inst._Rs;
            var rt = inst._Rt;
            int imm;
            if (inst.number() != null)
                imm = inst.number().GetValue();
            else if (inst.obj() != null)
                imm = inst.obj().Serialize(symbols, false);
            else
                throw new InvalidOperationException();
            if (inst.obj() != null &&
                enableLongJump &&
                (imm > 0x7f ||
                 imm < -0x80))
                throw new ApplicationException($"BEQ/BNE 指令无法跳转到相对偏移 {imm} 处；请使用 JMP 进行跳转");
            return (op << 12) | (rs << 10) | (rt << 8) | (imm & 0xff);
        }

        public static string Prettify(this ITypeIContext inst, SymbolResolver symbols, bool enableLongJump) =>
            inst.obj() != null
                ? symbols == null
                      ? $"{inst._Op.ToUpper().PadRight(4)} R{inst._Rs}, R{inst._Rt}, {inst.obj()}"
                      : $"{inst._Op.ToUpper().PadRight(4)} R{inst._Rs}, R{inst._Rt}, 0x{(inst.obj().Serialize(symbols, true) & 0xff):x2}"
                : $"{inst._Op.ToUpper().PadRight(4)} R{inst._Rt}, R{inst._Rs}, {inst.number()}";

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

        public static PCTarget Execute(this ITypeIContext inst,Context context)
        {
            switch (inst._Op.ToUpper())
            {
                case "ANDI":
                    context.Registers[inst._Rt] =
                        (byte)(context.Registers[inst._Rs] &
                               inst.number().GetValue());
                    context.ZeroFlag = context.Registers[inst._Rt] == 0;
                    return null;
                case "ORI":
                    context.Registers[inst._Rt] =
                        (byte)(context.Registers[inst._Rs] |
                               inst.number().GetValue());
                    context.ZeroFlag = context.Registers[inst._Rt] == 0;
                    return null;
                case "ADDI":
                    {
                        var s = context.Registers[inst._Rs] +
                                inst.number().GetValue();
                        context.CFlag = (s & ~0xff) != 0;
                        context.Registers[inst._Rt] = (byte)s;
                        context.ZeroFlag = context.Registers[inst._Rt] == 0;
                        return null;
                    }
                case "LW":
                    {
                        var addr = (context.Registers[inst._Rs] + inst.number().GetValue()) & 0xff;
                        context.Registers[inst._Rt] = context.Ram[addr];
                        return null;
                    }
                case "SW":
                    {
                        var addr = (context.Registers[inst._Rs] + inst.number().GetValue()) & 0xff;
                        context.Ram[addr] = context.Registers[inst._Rt];
                        return null;
                    }
                case "BEQ":
                    if (context.Registers[inst._Rs] == context.Registers[inst._Rt])
                        return inst.obj().Name() == null
                                   ? (PCTarget)(sbyte)inst.obj().number().GetValue()
                                   : inst.obj().Name().Symbol.Text;
                    return null;
                case "BNE":
                    if (context.Registers[inst._Rs] != context.Registers[inst._Rt])
                        return inst.obj().Name() == null
                                   ? (PCTarget)(sbyte)inst.obj().number().GetValue()
                                   : inst.obj().Name().Symbol.Text;
                    return null;
                default:
                    throw new InvalidOperationException();
            }
        }

    }
}