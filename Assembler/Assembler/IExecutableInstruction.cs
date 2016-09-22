namespace Assembler
{
    public class Context
    {
        public int PC;

        public byte[] Registers;

        public byte[] Ram;

        public bool ZeroFlag;

        public bool CFlag;
    }

    public interface IExecutableInstruction : IInstruction
    {
        PCTarget Execute(Context context);
    }

    public class PCTarget
    {
        public PCTarget()
        {
            IsSymbol = false;
            IsAbs = false;
            Position = 0;
            Symbol = string.Empty;
        }

        public PCTarget(int position, bool isAbs = false)
        {
            IsSymbol = false;
            IsAbs = isAbs;
            Position = position;
            Symbol = string.Empty;
        }

        private PCTarget(string symbol)
        {
            IsSymbol = true;
            IsAbs = true;
            Position = 0x7fffffff;
            Symbol = symbol;
        }

        public static implicit operator PCTarget(string symbol) => new PCTarget(symbol);

        public static implicit operator PCTarget(int position) => new PCTarget(position);

        public bool IsSymbol;
        public bool IsAbs;
        public int Position;
        public string Symbol;
    }
}
