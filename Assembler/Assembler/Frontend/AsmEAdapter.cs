using System;

namespace Assembler.Frontend
{
    partial class AsmEParser
    {
        public sealed partial class LineContext : ILineContext
        {
            public int LineNo => Start.Line;
            string ILineContext.Label => label()?.Name()?.Symbol?.Text;
            IInstructionContext ILineContext.Instruction => instruction();
            IMacro ILineContext.Macro => macro();
            string ILineContext.comment => Comment()?.GetText();
        }

        public sealed partial class NumberContext : INumberContext
        {
            string INumberContext.DecimalForm => Decimal().Symbol.Text;
            string INumberContext.BinaryForm => Binary().Symbol.Text;
            string INumberContext.HexadecimalForm => Hexadecimal().Symbol.Text;
        }

        public sealed partial class ObjContext : IObjContext
        {
            INumberContext IObjContext.ImmediateNumber => number();
            public string Label => Name().Symbol.Text;
        }

        public sealed partial class InstructionContext : IInstructionContext, IExecutableInstruction
        {
            public int Serialize(SymbolResolver symbols, bool enableLongJump) =>
                Debug != null ? new int() : Inst.Serialize(symbols, enableLongJump);

            public string Prettify(SymbolResolver symbols, bool enableLongJump) =>
                (Debug != null && symbols == null ? "#   " : "    ") + Inst.Prettify(symbols, enableLongJump);

            public PCTarget Execute(Context context) => Inst.Execute(context);

            public IExecutableInstruction Inst
            {
                get
                {
                    if (typeI() != null)
                        return typeI();
                    if (typeR() != null)
                        return typeR();
                    if (typeJ() != null)
                        return typeJ();
                    if (typeP() != null)
                        return typeP();
                    throw new InvalidOperationException();
                }
            }
        }

        public sealed partial class TypeRContext : ITypeRContext, IExecutableInstruction
        {
            public string Operator => Op.Text;
            public int RegisterD => SemanticHelper.RegisterNumber(Rd);
            public int RegisterS => SemanticHelper.RegisterNumber(Rs);
            public int RegisterT => SemanticHelper.RegisterNumber(Rt);

            public int Serialize(SymbolResolver resolver, bool enableLongJump) =>
                TypeRHelper.Serialize(this, resolver, enableLongJump);

            public string Prettify(SymbolResolver symbols, bool enableLongJump) =>
                TypeRHelper.Prettify(this, symbols, enableLongJump);

            public PCTarget Execute(Context context) => TypeRHelper.Execute(this, context);
        }

        public sealed partial class TypeIContext : ITypeIContext, IExecutableInstruction
        {
            public string Operator => Op.Text;
            public int RegisterS => SemanticHelper.RegisterNumber(Rs);
            public int RegisterT => SemanticHelper.RegisterNumber(Rt);
            INumberContext ITypeIContext.ImmediateNumber => number();
            IObjContext ITypeIContext.Target => obj();

            public int Serialize(SymbolResolver resolver, bool enableLongJump) =>
                TypeIHelper.Serialize(this, resolver, enableLongJump);

            public string Prettify(SymbolResolver symbols, bool enableLongJump) =>
                TypeIHelper.Prettify(this, symbols, enableLongJump);

            public PCTarget Execute(Context context) => TypeIHelper.Execute(this, context);
        }

        public sealed partial class TypeJContext : ITypeJContext, IExecutableInstruction
        {
            public string Operator => Op.Text;
            IObjContext ITypeJContext.Target => obj();

            public int Serialize(SymbolResolver resolver, bool enableLongJump) =>
                TypeJHelper.Serialize(this, resolver, enableLongJump);

            public string Prettify(SymbolResolver symbols, bool enableLongJump) =>
                TypeJHelper.Prettify(this, symbols, enableLongJump);

            public PCTarget Execute(Context context) => TypeJHelper.Execute(this, context);
        }

        public sealed partial class TypePContext : IExecutableInstruction
        {
            public PCTarget Execute(Context context)
            {
                switch (Op.Text)
                {
                    case "LPCH":
                        context.Registers[SemanticHelper.RegisterNumber(Rt)] = (byte)((context.PC + 1) >> 8);
                        return null;
                    case "LPCL":
                        context.Registers[SemanticHelper.RegisterNumber(Rt)] = (byte)(context.PC + 1);
                        return null;
                    case "SPC":
                        return new PCTarget(
                            ((context.Registers[SemanticHelper.RegisterNumber(Rd)] << 8) |
                             context.Registers[SemanticHelper.RegisterNumber(Rt)]),
                            true
                            );
                    default:
                        throw new InvalidOperationException();
                }
            }

            public int Serialize(SymbolResolver symbols, bool enableLongJump)
            {
                switch (Op.Text.ToUpper())
                {
                    case "LPCH":
                        return 0xf000 | (SemanticHelper.RegisterNumber(Rt) << 8);
                    case "LPCL":
                        return 0xf400 | (SemanticHelper.RegisterNumber(Rt) << 8);
                    case "SPC":
                        return 0xf800 | (SemanticHelper.RegisterNumber(Rt) << 8) |
                               (SemanticHelper.RegisterNumber(Rd) << 6);
                    default:
                        throw new InvalidOperationException();
                }
            }

            public string Prettify(SymbolResolver symbols, bool enableLongJump)
            {
                switch (Op.Text.ToUpper())
                {
                    case "LPCH":
                        return $"{Op.Text.ToUpper().PadRight(4)} R{SemanticHelper.RegisterNumber(Rt)}";
                    case "LPCL":
                        return $"{Op.Text.ToUpper().PadRight(4)} R{SemanticHelper.RegisterNumber(Rt)}";
                    case "SPC":
                        return
                            $"{Op.Text.ToUpper().PadRight(4)} R{SemanticHelper.RegisterNumber(Rd)}, R{SemanticHelper.RegisterNumber(Rt)}";
                    default:
                        throw new InvalidOperationException();
                }
            }
        }
    }
}
