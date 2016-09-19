using System;
using System.Collections.Generic;
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
            public int Serialize(IInstruction inst, Func<IInstruction, string, bool, int> symbols, bool absolute)
            {
                if (number() != null)
                    return number();
                if (Name() != null)
                    return symbols(inst, Name().Symbol.Text, absolute);
                throw new InvalidOperationException();
            }
        }

        public sealed partial class InstructionContext : IInstruction
        {
            public int Length => GetInst().Length;

            public List<int> Serialize(Func<IInstruction, string, bool, int> symbols) => GetInst().Serialize(symbols);
        }

        public sealed partial class TypeRContext : IInstruction
        {
            public int Length => 1;

            public List<int> Serialize(Func<IInstruction, string, bool, int> symbols)
            {
                var op = GetOpcode(TypeR().Symbol.Text);
                var rd = RegisterNumber(Rd);
                var rs = RegisterNumber(Rs);
                var rt = RegisterNumber(Rt);
                return new List<int>
                           {
                               (op << 12) | (rs << 10) | (rt << 8) | (rd << 6)
                           };
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
            public int Length => 1;

            public List<int> Serialize(Func<IInstruction, string, bool, int> symbols)
            {
                var op = GetOpcode((TypeI() ?? TypeIJ()).Symbol.Text);
                var rs = RegisterNumber(Rs);
                var rt = RegisterNumber(Rt);
                int imm;
                if (TypeI() != null)
                    imm = number();
                else if (TypeIJ() != null)
                    imm = obj().Serialize(this, symbols, false);
                else
                    throw new InvalidOperationException();
                if (imm > 0x7f ||
                    imm < -0x80)
                    throw new ApplicationException("BEQ/BNE jump too long; use JMP");
                return new List<int>
                           {
                               (op << 12) | (rs << 10) | (rt << 8) | (imm & 0xff)
                           };
            }

            private static int GetOpcode(string text)
            {
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
            public int Length => 1;

            public List<int> Serialize(Func<IInstruction, string, bool, int> symbols)
            {
                var op = GetOpcode(TypeJ().Symbol.Text);
                var imm = obj().Serialize(this, symbols, true);
                return new List<int>
                           {
                               (op << 12) | (imm & 0xfff)
                           };
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
    }
}
