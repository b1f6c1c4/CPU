using Antlr4.Runtime;

namespace Assembler.Frontend
{
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
