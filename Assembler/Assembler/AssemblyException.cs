using System;
using System.Text;
using Antlr4.Runtime;

namespace Assembler
{
    public class AssemblyException : ApplicationException
    {
        public string FilePath { get; set; }

        public int? Line { get; set; }

        public int? CharPos { get; set; }

        public AssemblyException(string message) : base(message) { }

        public AssemblyException(string message, Exception e) : base(message, e) { }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (FilePath != null)
                sb.Append($"在 {FilePath} ");

            if (Line.HasValue)
                sb.Append($"第 {Line} 行");

            if (CharPos.HasValue)
                sb.Append($"第 {CharPos} 字符");

            sb.Append("处发生错误：");
            sb.Append(base.ToString());

            return sb.ToString();
        }
    }

    internal class AssemblyHandler : IAntlrErrorListener<IToken>
    {
        private readonly string m_Filename;

        public AssemblyHandler(string filename) { m_Filename = filename; }

        public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine,
                                string msg,
                                RecognitionException e)
        {
            throw new AssemblyException(msg, e)
                      {
                          FilePath = m_Filename,
                          Line = line,
                          CharPos = charPositionInLine
                      };
        }
    }
}
