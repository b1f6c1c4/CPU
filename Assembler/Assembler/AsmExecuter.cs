namespace Assembler
{
    public abstract class AsmExecuterBase : AsmProgBase
    {
        protected AsmExecuterBase()
        {
            CPU = new Context
                      {
                          PC = 0,
                          Registers = new byte[4],
                          Ram = new byte[256]
                      };
        }

        public Context CPU { get; }

        protected override bool ExpansionDebug => true;

        protected bool Advance()
        {
            var oldPC = CPU.PC;
            var res = Instructions[CPU.PC].Execute(CPU);
            if (res == null)
                CPU.PC++;
            else if (res.IsSymbol)
                CPU.PC = GetSymbolPos(CPU.PC, res.Symbol);
            else if (res.IsAbs)
                CPU.PC = res.Position;
            else
                CPU.PC += res.Position + 1;

            CPU.PC &= PCMask;

            return oldPC == CPU.PC;
        }
    }

    public class AsmExecuter : AsmExecuterBase
    {
        public override void Done()
        {
            base.Done();

            while (CPU.PC < Instructions.Count)
                if (Advance())
                    break;
        }
    }
}
