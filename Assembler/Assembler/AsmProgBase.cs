using System;
using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;

namespace Assembler
{
    public class SourcePosition : IEquatable<SourcePosition>
    {
        public string FilePath { get; }

        public int Line { get; }

        public SourcePosition(string f, int l)
        {
            FilePath = f;
            Line = l;
        }

        public override bool Equals(object obj) => obj is SourcePosition && Equals((SourcePosition)obj);

        public override int GetHashCode()
        {
            unchecked
            {
                return ((FilePath?.GetHashCode() ?? 0) * 397) ^ Line;
            }
        }

        public bool Equals(SourcePosition other) => FilePath == other.FilePath && Line == other.Line;
    }

    public abstract class AsmProgBase
    {
        protected readonly List<IExecutableInstruction> Instructions;

        private readonly List<string> m_Filenames;

        protected readonly Dictionary<int, SourcePosition> Lines;

        protected readonly Dictionary<string, int> Symbols;

        public bool EnableExtension { get; set; }

        public bool EnableLongJump { get; set; }

        private int MaxLength => EnableLongJump ? 4096 : 256;

        protected int PCMask => MaxLength - 1;

        protected AsmProgBase()
        {
            EnableExtension = true;
            EnableLongJump = true;
            Instructions = new List<IExecutableInstruction>();
            m_Filenames = new List<string>();
            Lines = new Dictionary<int, SourcePosition>();
            Symbols = new Dictionary<string, int>();
        }

        public void Feed(string filename, bool halt)
        {
            var last = 0;
            using (var sin = new StreamReader(filename))
            {
                var lexer = new AsmELexer(new AntlrInputStream(sin));
                var parser = new AsmEParser(new CommonTokenStream(lexer));
                parser.AddErrorListener(new AssemblyHandler(filename));
                AsmEParser.ProgContext prog;
                try
                {
                    prog = parser.prog();
                }
                catch (AssemblyException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new AssemblyException(e.Message, e) { FilePath = filename };
                }
                foreach (var context in prog.line())
                    try
                    {
                        Parse(context, filename);
                        last = context.Start.Line;
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
                                      Line = context.start.Line
                                  };
                    }
            }

            if (halt)
            {
                var lexer = new AsmELexer(new AntlrInputStream("HALT" + Environment.NewLine));
                var parser = new AsmEParser(new CommonTokenStream(lexer));
                Parse(parser.line(), filename, last);
            }
        }

        public virtual void Done()
        {
            if (Instructions.Count > MaxLength)
                throw new ApplicationException($"程序过长（{Instructions.Count} / {MaxLength}）");
        }

        protected virtual void Parse(AsmEParser.LineContext context, string filename, int diff = 0)
        {
            if (context.label() != null)
            {
                var lbl = MakeUniqueSymbol(context.label().Name().Symbol.Text, filename);
                if (Symbols.ContainsKey(lbl))
                    throw new ApplicationException("duplicate labels");
                Symbols.Add(lbl, Instructions.Count);
            }

            if (context.instruction() != null)
            {
                Lines.Add(
                          Instructions.Count,
                          new SourcePosition(filename, diff + context.Start.Line));
                Instructions.Add(context.instruction());
                m_Filenames.Add(filename);
            }
            else if (context.macro() != null)
            {
                var instructions = context.macro().Flatten(ExpansionDebug);
                foreach (var inst in instructions)
                {
                    Lines.Add(
                              Instructions.Count,
                              new SourcePosition(filename, diff + context.Start.Line));
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
            if (!Symbols.TryGetValue(MakeUniqueSymbol(symbol, m_Filenames[now]), out pos))
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
