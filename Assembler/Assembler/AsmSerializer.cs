using System;

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
                try
                {
                    Put(inst.Serialize((s, a) => GetSymbol(i1, s, a)));
                }
                catch (AssemblyException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new AssemblyException(e.Message, e)
                              {
                                  FilePath = Lines[i].FilePath,
                                  Line = Lines[i].Line
                              };
                }
            }

            PutFinal();
        }

        protected abstract void Put(int res);

        protected virtual void PutFinal() { }
    }
}
