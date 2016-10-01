using System;

namespace Assembler
{
    partial class AsmEParser
    {
        public sealed partial class NumberContext : INumberContext { }

        public sealed partial class ObjContext : IObjContext
        {
            INumberContext IObjContext.number() => number();
        }

        public sealed partial class InstructionContext : IInstructionContext, IInstruction
        {
            public int Serialize(SymbolResolver symbols, bool enableLongJump) =>
                Debug != null ? new int() : GetInst().Serialize(symbols, enableLongJump);

            public string Prettify(SymbolResolver symbols, bool enableLongJump) =>
                (Debug != null && symbols == null ? "#   " : "    ") + GetInst().Prettify(symbols, enableLongJump);
        }

        public sealed partial class TypeRContext : ITypeRContext, IInstruction
        {
            public string _Op => Op.Text;
            public int _Rd => SemanticHelper.RegisterNumber(Rd);
            public int _Rs => SemanticHelper.RegisterNumber(Rs);
            public int _Rt => SemanticHelper.RegisterNumber(Rt);

            public int Serialize(SymbolResolver resolver, bool enableLongJump) =>
                TypeRHelper.Serialize(this, resolver, enableLongJump);

            public string Prettify(SymbolResolver symbols, bool enableLongJump) =>
                TypeRHelper.Prettify(this, symbols, enableLongJump);
        }

        public sealed partial class TypeIContext : ITypeIContext, IInstruction
        {
            public string _Op => Op.Text;
            public int _Rs => SemanticHelper.RegisterNumber(Rs);
            public int _Rt => SemanticHelper.RegisterNumber(Rt);
            INumberContext ITypeIContext.number() => number();
            IObjContext ITypeIContext.obj() => obj();

            public int Serialize(SymbolResolver resolver, bool enableLongJump) =>
                TypeIHelper.Serialize(this, resolver, enableLongJump);

            public string Prettify(SymbolResolver symbols, bool enableLongJump) =>
                TypeIHelper.Prettify(this, symbols, enableLongJump);
        }

        public sealed partial class TypeJContext : ITypeJContext, IInstruction
        {
            public string _Op => Op.Text;
            IObjContext ITypeJContext.obj() => obj();

            public int Serialize(SymbolResolver resolver, bool enableLongJump) =>
                TypeJHelper.Serialize(this, resolver, enableLongJump);

            public string Prettify(SymbolResolver symbols, bool enableLongJump) =>
                TypeJHelper.Prettify(this, symbols, enableLongJump);
        }

        public sealed partial class TypePContext : IInstruction
        {
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
