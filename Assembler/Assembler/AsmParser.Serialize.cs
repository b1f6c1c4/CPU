using System;
using Antlr4.Runtime;

namespace Assembler
{
    partial class AsmParser
    {
        private static int RegisterNumber(IToken reg) => Convert.ToInt32(reg.Text.Substring(1));

        public sealed partial class NumberContext
        {
            public static implicit operator int(NumberContext num)
            {
                if (num.Decimal() != null)
                    return Convert.ToInt32(num.Decimal().Symbol.Text);
                if (num.Binary() != null)
                    return Convert.ToInt32(num.Binary().Symbol.Text.Substring(2), 2);
                if (num.Hexadecimal() != null)
                    return Convert.ToInt32(num.Hexadecimal().Symbol.Text.Substring(2), 16);
                throw new InvalidOperationException();
            }
        }

        public sealed partial class ObjContext
        {
            public int Serialize(SymbolResolver symbols, bool absolute)
            {
                if (number() != null)
                    return (sbyte)(int)number();
                if (Name() != null)
                    return symbols(Name().Symbol.Text, absolute);
                throw new InvalidOperationException();
            }
        }

        public sealed partial class InstructionContext : IInstruction
        {
            public int Serialize(SymbolResolver symbols) =>
                Debug != null ? new int() : GetInst().Serialize(symbols);

            public string Prettify(SymbolResolver symbols) =>
                (Debug != null && symbols == null ? "#   " : "    ") + GetInst().Prettify(symbols);
        }

        public sealed partial class TypeRContext : IInstruction
        {
            public int Serialize(SymbolResolver symbols)
            {
                var op = GetOpcode(Op.Text);
                var rd = RegisterNumber(Rd);
                var rs = RegisterNumber(Rs);
                var rt = RegisterNumber(Rt);
                return (op << 12) | (rs << 10) | (rt << 8) | (rd << 6);
            }

            public string Prettify(SymbolResolver symbols) =>
                $"{Op.Text.ToUpper().PadRight(4)} R{RegisterNumber(Rd)}, R{RegisterNumber(Rs)}, R{RegisterNumber(Rt)}";

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
        }

        public sealed partial class TypeIContext : IInstruction
        {
            public int Serialize(SymbolResolver symbols)
            {
                var op = GetOpcode(Op.Text);
                var rs = RegisterNumber(Rs);
                var rt = RegisterNumber(Rt);
                int imm;
                if (number() != null)
                    imm = number();
                else if (obj() != null)
                    imm = obj().Serialize(symbols, false);
                else
                    throw new InvalidOperationException();
                if (obj() != null &&
                    (imm > 0x7f || imm < -0x80))
                    throw new ApplicationException($"BEQ/BNE at line {Op.Line} jump too long ({imm}); use JMP");
                return (op << 12) | (rs << 10) | (rt << 8) | (imm & 0xff);
            }

            public string Prettify(SymbolResolver symbols) =>
                obj() != null
                    ? symbols == null
                          ? $"{Op.Text.ToUpper().PadRight(4)} R{RegisterNumber(Rs)}, R{RegisterNumber(Rt)}, {obj().GetText()}"
                          : $"{Op.Text.ToUpper().PadRight(4)} R{RegisterNumber(Rs)}, R{RegisterNumber(Rt)}, 0x{(obj().Serialize(symbols, true) & 0xff):x2}"
                    : $"{Op.Text.ToUpper().PadRight(4)} R{RegisterNumber(Rt)}, R{RegisterNumber(Rs)}, {number().GetText()}";

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
        }

        public sealed partial class TypeJContext : IInstruction
        {
            public int Serialize(SymbolResolver symbols)
            {
                var op = GetOpcode(Op.Text);
                var imm = obj().Serialize(symbols, true);
                return (op << 12) | (imm & 0xfff);
            }

            public string Prettify(SymbolResolver symbols) =>
                symbols == null
                    ? $"{Op.Text.ToUpper().PadRight(4)} {obj().GetText()}"
                    : $"{Op.Text.ToUpper().PadRight(4)} 0x{(obj().Serialize(symbols, true) & 0xfff):x3}";

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
        }

        public sealed partial class TypePContext : IInstruction
        {
            public int Serialize(SymbolResolver symbols)
            {
                switch (Op.Text.ToUpper())
                {
                    case "LPCH":
                        return 0xf000 | (RegisterNumber(Rt) << 8);
                    case "LPCL":
                        return 0xf400 | (RegisterNumber(Rt) << 8);
                    case "SPC":
                        return 0xf800 | (RegisterNumber(Rt) << 8) | (RegisterNumber(Rd) << 6);
                    default:
                        throw new InvalidOperationException();
                }
            }

            public string Prettify(SymbolResolver symbols)
            {
                switch (Op.Text.ToUpper())
                {
                    case "LPCH":
                        return $"{Op.Text.ToUpper().PadRight(4)} R{RegisterNumber(Rt)}";
                    case "LPCL":
                        return $"{Op.Text.ToUpper().PadRight(4)} R{RegisterNumber(Rt)}";
                    case "SPC":
                        return $"{Op.Text.ToUpper().PadRight(4)} R{RegisterNumber(Rd)}, R{RegisterNumber(Rt)}";
                    default:
                        throw new InvalidOperationException();
                }
            }
        }
    }
}
