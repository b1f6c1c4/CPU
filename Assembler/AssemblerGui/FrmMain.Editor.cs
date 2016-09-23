using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ScintillaNET;

namespace AssemblerGui
{
    public partial class FrmMain
    {
        private string m_FileName;

        private string m_FilePath;

        private bool m_Edited;

        private int m_LineNumberLength;

        private void SetupEditor()
        {
            SetupScintilla();

            LoadEmptyDoc();
        }

        private void SetupScintilla()
        {
            scintilla.StyleResetDefault();
            scintilla.Styles[Style.Default].Font = "Microsoft YaHei Mono";
            scintilla.Styles[Style.Default].SizeF = 10F;
            scintilla.StyleClearAll();

            scintilla.Styles[Style.Asm.Default].ForeColor = Color.Silver;
            scintilla.Styles[Style.Asm.CpuInstruction].ForeColor = Color.Blue;
            scintilla.Styles[Style.Asm.MathInstruction].ForeColor = Color.DarkBlue;
            scintilla.Styles[Style.Asm.Comment].ForeColor = Color.FromArgb(0, 139, 139);
            scintilla.Styles[Style.Asm.Register].ForeColor = Color.Magenta;
            scintilla.Styles[Style.Asm.Number].ForeColor = Color.Black;
            scintilla.Styles[Style.Asm.Identifier].ForeColor = Color.FromArgb(128, 0, 128);

            scintilla.SetKeywords(
                                  0,
                                  "AND ANDI OR ORI ADD ADDI ADDC SUB SUBC LW SW JMP BEQ BNE LPCL LPCH SPC".ToLower());
            scintilla.SetKeywords(1, "INIT PUSH POP CALL RET HALT".ToLower());
            scintilla.SetKeywords(2, "R0 R1 R2 R3 BP".ToLower());

            scintilla.Margins[1].Type = MarginType.Number;
            scintilla.Margins[1].Width = 16;
            scintilla.Margins[1].Mask = 0;

            scintilla.Margins[0].Type = MarginType.Symbol;
            scintilla.Margins[0].Sensitive = true;
            scintilla.Margins[0].Mask = 1;
            scintilla.Margins[0].Cursor = MarginCursor.Arrow;
            scintilla.Margins[0].Width = 16;

            scintilla.Markers[0].Symbol = MarkerSymbol.Circle;
            scintilla.Markers[0].SetBackColor(Color.FromArgb(229, 20, 0));
            scintilla.Markers[0].SetForeColor(Color.White);

            scintilla.TextChanged += scintilla_TextChanged;
            scintilla.MarginClick += scintilla_MarginClick;
        }

        private void scintilla_TextChanged(object s, EventArgs e)
        {
            m_Edited = true;
            UpdateTitle();

            var length = scintilla.Lines.Count.ToString().Length;
            if (length == m_LineNumberLength)
                return;

            m_LineNumberLength = length;

            const int padding = 2;
            scintilla.Margins[1].Width = scintilla.TextWidth(Style.LineNumber, new string('9', length + 1)) + padding;
        }

        private void scintilla_MarginClick(object sender, MarginClickEventArgs e)
        {
            if (e.Margin == 0)
                ToggleBreakPoint(scintilla.LineFromPosition(e.Position));
        }

        private void SetFile(string filePath)
        {
            m_FilePath = filePath;
            m_FileName = Path.GetFileNameWithoutExtension(m_FilePath);
        }

        private void LoadEmptyDoc()
        {
            m_FileName = @"新建文件";
            m_FilePath = null;

            var b = scintilla.ReadOnly;
            scintilla.ReadOnly = false;
            scintilla.Text = "";
            scintilla.ReadOnly = b;

            m_Edited = false;
            UpdateTitle();
        }

        private void LoadDoc()
        {
            var b = scintilla.ReadOnly;
            scintilla.ReadOnly = false;
            scintilla.Text = File.ReadAllText(m_FilePath);
            scintilla.ReadOnly = b;

            m_Edited = false;
            UpdateTitle();
        }

        private bool PromptForSave(bool forbidNo = false)
        {
            if ((!forbidNo || m_FilePath != null) &&
                !m_Edited)
                return true;

            var res = MessageBox.Show(
                                      $"{m_FileName} 尚未保存，是否要保存？",
                                      "MIPS编辑器",
                                      forbidNo ? MessageBoxButtons.OKCancel : MessageBoxButtons.YesNoCancel,
                                      MessageBoxIcon.Exclamation);
            if (res == DialogResult.Cancel)
                return false;

            if (res == DialogResult.No)
                return true;

            return PerformSave();
        }

        private bool PerformSave()
        {
            if (m_FilePath == null)
                if (!PromptSaveAs())
                    return false;

            // ReSharper disable once AssignNullToNotNullAttribute
            File.WriteAllText(m_FilePath, scintilla.Text);
            m_Edited = false;
            UpdateTitle();

            return true;
        }

        private bool PromptSaveAs()
        {
            var res = PromptSaveDialog("mips", "MIPS文件", "另存为");
            if (res == null)
                return false;

            SetFile(res);
            return true;
        }

        private bool PromptOpen()
        {
            var dialog =
                new OpenFileDialog
                    {
                        AddExtension = true,
                        AutoUpgradeEnabled = true,
                        CheckFileExists = true,
                        CheckPathExists = true,
                        DefaultExt = "mips",
                        Filter = "MIPS文件 (*.mips)|*.mips|所有文件 (*)|*",
                        Title = "打开"
                    };
            var res = dialog.ShowDialog();
            if (res == DialogResult.Cancel)
                return false;

            SetFile(dialog.FileName);

            return true;
        }

        private void 新建NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PromptForSave())
                return;

            LoadEmptyDoc();
        }

        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e) =>
            PerformSave();

        private void 另存为AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PromptSaveAs())
                return;

            PerformSave();
        }

        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PromptForSave())
                return;

            if (!PromptOpen())
                return;

            LoadDoc();
        }

        private void 退出QToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PromptForSave())
                return;

            Environment.Exit(0);
        }

        private void 切换断点BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (scintilla.Focused)
                ToggleBreakPoint(scintilla.CurrentLine);
        }

        private void LoadDoc(string filePath, int? line = null, int? charPos = null)
        {
            if (filePath == null)
                return;

            if (filePath != m_FilePath)
            {
                if (!PromptForSave())
                    return;

                SetFile(filePath);
                LoadDoc();
            }

            if (!line.HasValue)
                return;

            scintilla.GotoPosition(scintilla.Lines[line.Value - 1].Position + (charPos ?? 0));
        }
    }
}
