namespace Assembler
{
    public class AsmExecuter : AsmProgBase
    {
        public delegate void OnBreakPointEventHandler(SourcePosition pos);

        public event OnBreakPointEventHandler OnBreakPoint;

        public Context CPU { get; }

        public AsmExecuter()
        {
            CPU = new Context
                      {
                          PC = 0,
                          Registers = new byte[4],
                          Ram = new byte[256]
                      };
        }

        public override void Done()
        {
            while (CPU.PC < Instructions.Count)
            {
                if (Symbols.ContainsValue(CPU.PC))
                    OnBreakPoint?.Invoke(Lines[CPU.PC]);

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
                if (oldPC == CPU.PC)
                    break;
            }
        }
    }
}
