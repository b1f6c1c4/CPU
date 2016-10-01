using System;

namespace Assembler.Frontend
{
    partial class AsmParser
    {
        public sealed partial class LineContext : ILineContext
        {
            public int LineNo => Start.Line;
            string ILineContext.Label => label()?.Name()?.Symbol?.Text;
            IInstructionContext ILineContext.Instruction => instruction();
            IMacro ILineContext.Macro => null;
            string ILineContext.TheComment => Comment()?.GetText();
        }

        public sealed partial class NumberContext : INumberContext
        {
            string INumberContext.DecimalForm => Decimal()?.Symbol?.Text;
            string INumberContext.BinaryForm => Binary()?.Symbol?.Text;
            string INumberContext.HexadecimalForm => Hexadecimal()?.Symbol?.Text;
        }

        public sealed partial class ObjContext : IObjContext
        {
            INumberContext IObjContext.ImmediateNumber => number();
            public string Label => Name()?.Symbol?.Text;
        }

        public sealed partial class InstructionContext : IInstructionContext, IExecutableInstruction
        {
            public int Serialize(SymbolResolver symbols, bool enableLongJump) =>
                Inst.Serialize(symbols, enableLongJump);

            public string Prettify(SymbolResolver symbols, bool enableLongJump) =>
                "    " + Inst.Prettify(symbols, enableLongJump);

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
    }
}
