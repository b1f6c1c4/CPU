using System;

namespace Assembler
{
    partial class AsmParser
    {
        public sealed partial class InstructionContext : IExecutableInstruction
        {
            public PCTarget Execute(Context context) => GetInst().Execute(context);

            private IExecutableInstruction GetInst()
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
            public PCTarget Execute(Context context)
            {
                switch (TypeR().Symbol.Text)
                {
                    case "AND":
                        context.Registers[RegisterNumber(Rd)] =
                            (byte)(context.Registers[RegisterNumber(Rs)] &
                                   context.Registers[RegisterNumber(Rt)]);
                        return null;
                    case "OR":
                        context.Registers[RegisterNumber(Rd)] =
                            (byte)(context.Registers[RegisterNumber(Rs)] |
                                   context.Registers[RegisterNumber(Rt)]);
                        return null;
                    case "ADD":
                        {
                            var s = context.Registers[RegisterNumber(Rs)] +
                                    context.Registers[RegisterNumber(Rt)];
                            context.CFlag = (s & ~0xff) != 0;
                            context.Registers[RegisterNumber(Rd)] = (byte)s;
                            return null;
                        }
                    case "SUB":
                        {
                            var s = context.Registers[RegisterNumber(Rs)] -
                                    context.Registers[RegisterNumber(Rt)];
                            context.CFlag = (s & ~0xff) == 0;
                            context.Registers[RegisterNumber(Rd)] = (byte)s;
                            return null;
                        }
                    case "ADDC":
                        {
                            var s = context.Registers[RegisterNumber(Rs)] +
                                    context.Registers[RegisterNumber(Rt)] +
                                    (context.CFlag ? 1 : 0);
                            context.CFlag = (s & ~0xff) != 0;
                            context.Registers[RegisterNumber(Rd)] = (byte)s;
                            return null;
                        }
                    case "SUBC":
                        {
                            var s = context.Registers[RegisterNumber(Rs)] -
                                    context.Registers[RegisterNumber(Rt)] -
                                    (context.CFlag ? 0 : 1);
                            context.CFlag = (s & ~0xff) == 0;
                            context.Registers[RegisterNumber(Rd)] = (byte)s;
                            return null;
                        }
                    case "SLT":
                        context.Registers[RegisterNumber(Rd)] =
                            (context.Registers[RegisterNumber(Rs)] <
                             context.Registers[RegisterNumber(Rt)])
                                ? (byte)1
                                : (byte)0;
                        return null;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public sealed partial class TypeIContext : IExecutableInstruction
        {
            public PCTarget Execute(Context context)
            {
                switch ((TypeI() ?? TypeIJ()).Symbol.Text)
                {
                    case "ANDI":
                        context.Registers[RegisterNumber(Rt)] =
                            (byte)(context.Registers[RegisterNumber(Rs)] &
                                   number());
                        return null;
                    case "ORI":
                        context.Registers[RegisterNumber(Rt)] =
                            (byte)(context.Registers[RegisterNumber(Rs)] |
                                   number());
                        return null;
                    case "ADDI":
                        {
                            var s = context.Registers[RegisterNumber(Rs)] +
                                    number();
                            context.CFlag = (s & ~0xff) != 0;
                            context.Registers[RegisterNumber(Rt)] = (byte)s;
                            return null;
                        }
                    case "LW":
                        {
                            var addr = (context.Registers[RegisterNumber(Rs)] + number()) & 0xff;
                            context.Registers[RegisterNumber(Rt)] = context.Ram[addr];
                            return null;
                        }
                    case "SW":
                        {
                            var addr = (context.Registers[RegisterNumber(Rs)] + number()) & 0xff;
                            context.Ram[addr] = context.Registers[RegisterNumber(Rt)];
                            return null;
                        }
                    case "BEQ":
                        if (context.Registers[RegisterNumber(Rs)] == context.Registers[RegisterNumber(Rt)])
                            return obj().Name() == null ? (PCTarget)(int)obj().number() : obj().Name().Symbol.Text;
                        return null;
                    case "BNE":
                        if (context.Registers[RegisterNumber(Rs)] != context.Registers[RegisterNumber(Rt)])
                            return obj().Name() == null ? (PCTarget)(int)obj().number() : obj().Name().Symbol.Text;
                        return null;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public sealed partial class TypeJContext : IExecutableInstruction
        {
            public PCTarget Execute(Context context)
            {
                switch (TypeJ().Symbol.Text)
                {
                    case "JMP":
                        return obj().Name() == null ? new PCTarget(obj().number(), true) : obj().Name().Symbol.Text;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public sealed partial class TypePContext : IExecutableInstruction
        {
            public PCTarget Execute(Context context)
            {
                switch (Op.Text)
                {
                    case "LPCH":
                        context.Registers[RegisterNumber(Rt)] = (byte)((context.PC + 1) >> 8);
                        return null;
                    case "LPCL":
                        context.Registers[RegisterNumber(Rt)] = (byte)(context.PC + 1);
                        return null;
                    case "SPC":
                        return new PCTarget(
                            (context.Registers[RegisterNumber(Rd)] << 8) |
                            context.Registers[RegisterNumber(Rt)],
                            true
                            );
                    default:
                        throw new InvalidOperationException();
                }
            }
        }
    }
}
