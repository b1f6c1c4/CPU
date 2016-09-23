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
            scintilla.SetKeywords(1, "INIT PUSH POP CALL RET".ToLower());
            scintilla.SetKeywords(2, "R0 R1 R2 R3 BP".ToLower());

            scintilla.TextChanged +=
                (s, e) =>
                {
                    m_Edited = true;
                    UpdateTitle();
                };
        }

        private void LoadEmptyDoc()
        {
            m_FileName = @"新建文件";
            m_FilePath = null;

            scintilla.Text = "";
            m_Edited = false;
            UpdateTitle();
        }

        private void LoadDoc()
        {
            scintilla.Text = File.ReadAllText(m_FilePath);
            m_Edited = false;
            UpdateTitle();
        }

        private bool PromptForSave()
        {
            var res = MessageBox.Show($"{m_FileName} 尚未保存，是否要保存？", "MIPS编辑器", MessageBoxButtons.YesNoCancel);
            if (res == DialogResult.Cancel)
                return false;

            if (res == DialogResult.No)
                return true;

            return PerformSave();
        }

        private bool PerformSave(bool force = false)
        {
            if (!m_Edited &&
                !force)
                return true;

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
            var dialog =
                new SaveFileDialog
                    {
                        AddExtension = true,
                        AutoUpgradeEnabled = true,
                        CheckFileExists = false,
                        CheckPathExists = true,
                        CreatePrompt = false,
                        DefaultExt = "mips",
                        Filter = "MIPS文件 (*.mips)|*.mips|所有文件 (*)|*",
                        FileName = m_FileName,
                        Title = "另存为"
                    };
            var res = dialog.ShowDialog();
            if (res == DialogResult.Cancel)
                return false;

            m_FilePath = dialog.FileName;
            m_FileName = Path.GetFileNameWithoutExtension(m_FilePath);

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

            m_FilePath = dialog.FileName;
            m_FileName = Path.GetFileNameWithoutExtension(m_FilePath);

            return true;
        }

        private void 新建NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PromptForSave())
                return;

            LoadEmptyDoc();
        }

        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e) =>
            PerformSave(true);

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
    }
}
