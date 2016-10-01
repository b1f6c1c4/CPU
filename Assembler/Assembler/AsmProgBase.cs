using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assembler.Frontend;

namespace Assembler
{
    public abstract class AsmProgBase
    {
        protected readonly List<IExecutableInstruction> Instructions;

        private readonly List<string> m_Filenames;

        protected readonly Dictionary<int, SourcePosition> Lines;

        private readonly Dictionary<string, int> m_Symbols;

        public IFrontend Frontend { get; set; }

        public bool EnableLongJump { get; set; }

        private int MaxLength => EnableLongJump ? 4096 : 256;

        protected int PCMask => MaxLength - 1;

        protected AsmProgBase()
        {
            EnableLongJump = true;
            Instructions = new List<IExecutableInstruction>();
            m_Filenames = new List<string>();
            Lines = new Dictionary<int, SourcePosition>();
            m_Symbols = new Dictionary<string, int>();
        }

        public void Feed(string filename, bool halt)
        {
            var last = 0;
            using (var sin = new StreamReader(filename))
            {
                IEnumerable<ILineContext> lines;
                try
                {
                    lines = Frontend.Parse(filename, sin);
                }
                catch (AssemblyException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new AssemblyException(e.Message, e) { FilePath = filename };
                }
                foreach (var context in lines)
                    try
                    {
                        Parse(context, filename);
                        last = context.LineNo;
                    }
                    catch (AssemblyException)
                    {
                        throw;
                    }
                    catch (Exception e)
                    {
                        throw new AssemblyException(e.Message, e)
                                  {
                                      FilePath = filename,
                                      Line = context.LineNo
                                  };
                    }
            }

            if (halt)
                try
                {
                    var str = new StringReader("BEQ R1, R1, 0xff" + Environment.NewLine);
                    Parse(Frontend.Parse(filename, str).Single(), filename, last);
                }
                catch (AssemblyException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new AssemblyException(e.Message, e)
                              {
                                  FilePath = filename,
                                  Line = File.ReadAllLines(filename).Length + 1
                              };
                }
        }

        public virtual void Done()
        {
            if (Instructions.Count > MaxLength)
                throw new ApplicationException($"程序过长（{Instructions.Count} / {MaxLength}）");
        }

        protected virtual void Parse(ILineContext context, string filename, int diff = 0)
        {
            if (context.Label != null)
            {
                var lbl = MakeUniqueSymbol(context.Label, filename);
                if (m_Symbols.ContainsKey(lbl))
                    throw new ApplicationException("duplicate labels");
                m_Symbols.Add(lbl, Instructions.Count);
            }

            if (context.Instruction != null)
            {
                Lines.Add(
                          Instructions.Count,
                          new SourcePosition(filename, diff + context.LineNo));
                Instructions.Add(context.Instruction);
                m_Filenames.Add(filename);
            }
            else if (context.Macro != null)
            {
                var instructions = context.Macro.Flatten(ExpansionDebug);
                foreach (var inst in instructions)
                {
                    Lines.Add(
                              Instructions.Count,
                              new SourcePosition(filename, diff + context.LineNo));
                    Instructions.Add(inst);
                    m_Filenames.Add(filename);
                }
            }
        }

        protected abstract bool ExpansionDebug { get; }

        private static string MakeUniqueSymbol(string lbl, string filename) =>
            !char.IsUpper(lbl, 0) ? lbl + "@" + filename : lbl;

        protected int GetSymbolPos(int now, string symbol)
        {
            int pos;
            if (!m_Symbols.TryGetValue(MakeUniqueSymbol(symbol, m_Filenames[now]), out pos))
                throw new AssemblyException($"找不到符号“{symbol}”")
                          {
                              FilePath = Lines[now].FilePath,
                              Line = Lines[now].Line
                          };
            return pos;
        }

        protected int GetSymbol(int now, string symbol, bool isAbs)
        {
            var pos = GetSymbolPos(now, symbol);

            if (isAbs)
                return pos;
            return pos - (now + 1);
        }
    }
}
