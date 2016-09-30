namespace Assembler
{
    public abstract class AsmSerializer : AsmProgBase
    {
        public override void Done()
        {
            for (var i = 0; i < Instructions.Count; i++)
            {
                var inst = Instructions[i];
                var i1 = i;
                Put(inst.Serialize((s, a) => GetSymbol(i1, s, a)));
            }

            PutFinal();
        }

        protected abstract void Put(int res);

        protected virtual void PutFinal() { }
    }
}
