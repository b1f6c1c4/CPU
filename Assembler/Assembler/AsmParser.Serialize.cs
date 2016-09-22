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
            public int Serialize(IInstruction inst, SymbolResolver symbols, bool absolute)
            {
                if (number() != null)
                    return number();
                if (Name() != null)
                    return symbols(Name().Symbol.Text, absolute);
                throw new InvalidOperationException();
            }
        }

        public sealed partial class InstructionContext : IInstruction
        {
            public int Serialize(SymbolResolver symbols) =>
                Debug != null ? new int() : GetInst().Serialize(symbols);
        }

        public sealed partial class TypeRContext : IInstruction
        {
            public int Serialize(SymbolResolver symbols)
            {
                var op = GetOpcode(TypeR().Symbol.Text);
                var rd = RegisterNumber(Rd);
                var rs = RegisterNumber(Rs);
                var rt = RegisterNumber(Rt);
                return (op << 12) | (rs << 10) | (rt << 8) | (rd << 6);
            }

            private static int GetOpcode(string text)
            {
                switch (text)
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
                bool br;
                var op = GetOpcode((TypeI() ?? TypeIJ()).Symbol.Text, out br);
                var rs = RegisterNumber(Rs);
                var rt = RegisterNumber(Rt);
                int imm;
                if (TypeI() != null)
                    imm = number();
                else if (TypeIJ() != null)
                    imm = obj().Serialize(this, symbols, false);
                else
                    throw new InvalidOperationException();
                if (br && (imm > 0x7f || imm < -0x80))
                    throw new ApplicationException($"BEQ/BNE at line {TypeI().Symbol.Line} jump too long; use JMP");
                return (op << 12) | (rs << 10) | (rt << 8) | (imm & 0xff);
            }

            private static int GetOpcode(string text, out bool br)
            {
                br = false;
                switch (text)
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
                        br = true;
                        return 0xd;
                    case "BNE":
                        br = true;
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
                var op = GetOpcode(TypeJ().Symbol.Text);
                var imm = obj().Serialize(this, symbols, true);
                return (op << 12) | (imm & 0xfff);
            }

            private static int GetOpcode(string text)
            {
                switch (text)
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
                switch (Op.Text)
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
        }
    }
}
