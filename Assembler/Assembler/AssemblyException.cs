using System;
using System.Text;

namespace Assembler
{
    public class AssemblyException : ApplicationException
    {
        public string FilePath { get; set; }

        public int? Line { get; set; }

        public int? CharPos { get; set; }

        public AssemblyException(string message) : base(message) { }

        public AssemblyException(string message, Exception e) : base(message, e) { }

        public override string Message => base.Message + Description;

        private string Description
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendLine();

                if (FilePath != null)
                    sb.AppendLine(FilePath);

                if (Line.HasValue)
                    sb.Append($"第 {Line} 行");

                if (CharPos.HasValue)
                    sb.Append($" 第 {CharPos} 字符");
                return sb.ToString();
            }
        }

        public override string ToString() => Description + base.ToString();
    }
}
