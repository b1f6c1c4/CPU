using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Assembler
{
    internal interface INumberContext
    {
        ITerminalNode Decimal();

        ITerminalNode Binary();

        ITerminalNode Hexadecimal();
    }

    internal interface IObjContext
    {
        INumberContext number();

        ITerminalNode Name();
    }

    internal interface IInstructionContext
    {
        IExecutableInstruction GetInst();
    }

    internal interface ITypeRContext
    {
        string _Op { get; }
        int _Rd { get; }
        int _Rs { get; }
        int _Rt { get; }
    }

    internal interface ITypeIContext
    {
        string _Op { get; }
        int _Rs { get; }
        int _Rt { get; }
        INumberContext number();
        IObjContext obj();
    }

    internal interface ITypeJContext
    {
        string _Op { get; }
        IObjContext obj();
    }

    internal static class SemanticHelper
    {
        public static int RegisterNumber(IToken reg) => Convert.ToInt32(reg.Text.Substring(1));

        public static int GetValue(this INumberContext num)
        {
            if (num.Decimal() != null)
                return Convert.ToInt32(num.Decimal().Symbol.Text);
            if (num.Binary() != null)
                return Convert.ToInt32(num.Binary().Symbol.Text.Substring(2), 2);
            if (num.Hexadecimal() != null)
                return Convert.ToInt32(num.Hexadecimal().Symbol.Text.Substring(2), 16);
            throw new InvalidOperationException();
        }

        public static int Serialize(this IObjContext obj, SymbolResolver symbols, bool absolute)
        {
            if (obj.number() != null)
                return (sbyte)obj.number().GetValue();
            if (obj.Name() != null)
                return symbols(obj.Name().Symbol.Text, absolute);
            throw new InvalidOperationException();
        }
    }
}
