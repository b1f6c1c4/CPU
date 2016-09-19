namespace Assembler
{
    public class Context
    {
        public byte[] Registers;

        public byte[] Ram;

        public bool ZeroFlag;

        public bool CFlag;
    }

    public interface IExecutableInstruction : IInstruction
    {
        string Execute(Context context);
    }
}
