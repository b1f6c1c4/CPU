using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ScintillaNET;

namespace AssemblerGui
{
    public class Editor : TabPage
    {
        private static int m_ID = 1;

        public delegate void ToggleBreakPointEventHandler(string file, int line, bool isAdd);

        public event SimpleEventHandler OnStateChanged;

        public event ToggleBreakPointEventHandler OnToggleBreakPoint;

        public string FileName
        {
            get { return m_FileName; }
            private set
            {
                m_FileName = value;
                OnStateChanged?.Invoke();
            }
        }

        public string FilePath { get; private set; }

        public bool Edited { get; private set; }

        public bool ReadOnly { set { m_Scintilla.ReadOnly = value; } }

        public Control ActiveControl => m_Scintilla;

        private int m_LineNumberLength;

        private Scintilla m_Scintilla;
        private string m_FileName;

        public new void Focus() => m_Scintilla.Focus();

        public Editor()
        {
            OnStateChanged += () => Text = m_FileName + (Edited ? "*" : "");

            InitializeComponent();

            LoadEmptyDoc();
        }

        private void InitializeComponent()
        {
            m_Scintilla = new Scintilla();
            SuspendLayout();
            // 
            // scintilla
            // 
            m_Scintilla.CaretPeriod = 200;
            m_Scintilla.Dock = DockStyle.Fill;
            m_Scintilla.EndAtLastLine = false;
            m_Scintilla.Lexer = Lexer.Asm;
            m_Scintilla.Location = new Point(0, 0);
            m_Scintilla.Margin = new Padding(3, 2, 3, 2);
            m_Scintilla.Margins.Left = 16;
            m_Scintilla.Name = "m_Scintilla";
            m_Scintilla.Size = new Size(450, 344);
            m_Scintilla.TabIndex = 0;
            m_Scintilla.WrapMode = WrapMode.Char;
            // 
            // Editor
            // 
            Controls.Add(m_Scintilla);

            m_Scintilla.StyleResetDefault();
            m_Scintilla.Styles[Style.Default].Font = "Microsoft YaHei Mono";
            m_Scintilla.Styles[Style.Default].SizeF = 10F;
            m_Scintilla.StyleClearAll();

            m_Scintilla.Styles[Style.Asm.Default].ForeColor = Color.Silver;
            m_Scintilla.Styles[Style.Asm.CpuInstruction].ForeColor = Color.Blue;
            m_Scintilla.Styles[Style.Asm.MathInstruction].ForeColor = Color.DarkBlue;
            m_Scintilla.Styles[Style.Asm.Comment].ForeColor = Color.FromArgb(0, 139, 139);
            m_Scintilla.Styles[Style.Asm.Register].ForeColor = Color.Magenta;
            m_Scintilla.Styles[Style.Asm.Number].ForeColor = Color.Black;
            m_Scintilla.Styles[Style.Asm.Identifier].ForeColor = Color.FromArgb(128, 0, 128);

            m_Scintilla.SetKeywords(
                                    0,
                                    "AND ANDI OR ORI ADD ADDI ADDC SUB SUBC LW SW JMP BEQ BNE LPCL LPCH SPC".ToLower());
            m_Scintilla.SetKeywords(1, "INIT PUSH POP CALL RET HALT".ToLower());
            m_Scintilla.SetKeywords(2, "R0 R1 R2 R3 BP".ToLower());

            m_Scintilla.Margins[1].Type = MarginType.Number;
            m_Scintilla.Margins[1].Width = 16;
            m_Scintilla.Margins[1].Mask = 0;

            m_Scintilla.Margins[0].Type = MarginType.Symbol;
            m_Scintilla.Margins[0].Sensitive = true;
            m_Scintilla.Margins[0].Mask = 1 | 2;
            m_Scintilla.Margins[0].Cursor = MarginCursor.Arrow;
            m_Scintilla.Margins[0].Width = 16;

            m_Scintilla.Markers[0].Symbol = MarkerSymbol.Circle;
            m_Scintilla.Markers[0].SetBackColor(Color.FromArgb(229, 20, 0));
            m_Scintilla.Markers[0].SetForeColor(Color.White);

            m_Scintilla.Markers[1].Symbol = MarkerSymbol.Arrow;
            m_Scintilla.Markers[1].SetBackColor(Color.Yellow);
            m_Scintilla.Markers[1].SetForeColor(Color.Black);

            m_Scintilla.TextChanged += scintilla_TextChanged;
            m_Scintilla.MarginClick += scintilla_MarginClick;

            ResumeLayout(false);
        }

        private void scintilla_TextChanged(object s, EventArgs e)
        {
            if (!Edited)
            {
                Edited = true;
                OnStateChanged?.Invoke();
            }

            var length = m_Scintilla.Lines.Count.ToString().Length;
            if (length == m_LineNumberLength)
                return;

            m_LineNumberLength = length;

            const int padding = 2;
            m_Scintilla.Margins[1].Width = m_Scintilla.TextWidth(Style.LineNumber, new string('9', length + 1)) +
                                           padding;
        }

        private void scintilla_MarginClick(object sender, MarginClickEventArgs e)
        {
            if (e.Margin == 0)
                ToggleBreakPoint(m_Scintilla.LineFromPosition(e.Position));
        }

        private void SetFile(string filePath)
        {
            FilePath = filePath;
            FileName = Path.GetFileNameWithoutExtension(FilePath);
        }

        private void LoadEmptyDoc()
        {
            FileName = $"Untitled {m_ID++}";
            FilePath = null;

            var b = m_Scintilla.ReadOnly;
            m_Scintilla.ReadOnly = false;
            m_Scintilla.Text = "";
            m_Scintilla.ReadOnly = b;

            Edited = false;
            OnStateChanged?.Invoke();
        }

        private void LoadDoc()
        {
            var b = m_Scintilla.ReadOnly;
            m_Scintilla.ReadOnly = false;
            m_Scintilla.Text = File.ReadAllText(FilePath);
            m_Scintilla.ReadOnly = b;

            Edited = false;
            OnStateChanged?.Invoke();
        }

        public void LoadDoc(string filePath, int? line = null, int? charPos = null, bool debugging = false)
        {
            if (filePath == null)
                return;

            if (filePath != FilePath)
            {
                if (!PromptForSave())
                    return;
                SetFile(filePath);
                LoadDoc();
            }
            else if(!debugging)　LoadDoc();

            if (!line.HasValue)
                return;

            m_Scintilla.GotoPosition(m_Scintilla.Lines[line.Value - 1].Position + (charPos ?? 0));

            if (!debugging)
                return;

            m_Scintilla.MarkerDeleteAll(1);
            m_Scintilla.Lines[line.Value - 1].MarkerAdd(1);
        }

        public bool PromptForSave(bool forbidNo = false)
        {
            if ((!forbidNo || FilePath != null) &&
                !Edited)
                return true;

            var res = MessageBox.Show(
                                      $"{FileName} 尚未保存，是否要保存？",
                                      "MIPS编辑器",
                                      forbidNo ? MessageBoxButtons.OKCancel : MessageBoxButtons.YesNoCancel,
                                      MessageBoxIcon.Exclamation);
            if (res == DialogResult.Cancel)
                return false;

            if (res == DialogResult.No)
                return true;

            return PerformSave();
        }

        public bool PerformSave()
        {
            if (FilePath == null)
                if (!PromptSaveAs())
                    return false;

            // ReSharper disable once AssignNullToNotNullAttribute
            File.WriteAllText(FilePath, m_Scintilla.Text);
            Edited = false;
            OnStateChanged?.Invoke();

            return true;
        }

        public bool PromptSaveAs()
        {
            var res = FrmMain.PromptSaveDialog("mips", "MIPS文件", "另存为", FileName);
            if (res == null)
                return false;

            SetFile(res);
            return true;
        }

        public void ToggleBreakPoint() => ToggleBreakPoint(m_Scintilla.CurrentLine + 1);

        public void ToggleBreakPoint(int id)
        {
            var line = m_Scintilla.Lines[id - 1];

            var had = (line.MarkerGet() & 1) != 0;
            if (had)
                line.MarkerDelete(0);
            else
                line.MarkerAdd(0);

            OnToggleBreakPoint?.Invoke(FilePath, id, !had);
        }

        public void ClearCurrentPositon() => m_Scintilla.MarkerDeleteAll(1);
    }
}
