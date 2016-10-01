using System;
using Antlr4.Runtime;

namespace Assembler.Frontend
{
    public interface INumberContext
    {
        string DecimalForm { get; }

        string BinaryForm { get; }

        string HexadecimalForm { get; }
    }

    public interface IObjContext
    {
        INumberContext ImmediateNumber { get; }

        string Label { get; }
    }

    public interface IInstructionContext : IExecutableInstruction
    {
        IExecutableInstruction Inst { get; }
    }

    public interface ILineContext
    {
        int LineNo { get; }

        string Label { get; }

        IInstructionContext Instruction { get; }

        IMacro Macro { get; }

        string comment { get; }
    }

    public static class SemanticHelper
    {
        public static int RegisterNumber(IToken reg) => Convert.ToInt32(reg.Text.Substring(1));

        public static int GetValue(this INumberContext num)
        {
            if (num.DecimalForm != null)
                return Convert.ToInt32(num.DecimalForm);
            if (num.BinaryForm != null)
                return Convert.ToInt32(num.BinaryForm.Substring(2), 2);
            if (num.HexadecimalForm != null)
                return Convert.ToInt32(num.HexadecimalForm.Substring(2), 16);
            throw new InvalidOperationException();
        }

        public static int Serialize(this IObjContext obj, SymbolResolver symbols, bool absolute)
        {
            if (obj.ImmediateNumber != null)
                return (sbyte)obj.ImmediateNumber.GetValue();
            if (obj.Label != null)
                return symbols(obj.Label, absolute);
            throw new InvalidOperationException();
        }
    }
}
