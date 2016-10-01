using System;

namespace Assembler
{
    partial class AsmEParser
    {
        public sealed partial class InstructionContext : IExecutableInstruction
        {
            public PCTarget Execute(Context context) => GetInst().Execute(context);

            public IExecutableInstruction GetInst()
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

        public sealed partial class TypeRContext : IExecutableInstruction
        {
            public PCTarget Execute(Context context) => TypeRHelper.Execute(this, context);
        }

        public sealed partial class TypeIContext : IExecutableInstruction
        {
            public PCTarget Execute(Context context) => TypeIHelper.Execute(this, context);
        }

        public sealed partial class TypeJContext : IExecutableInstruction
        {
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
        }
    }
}
