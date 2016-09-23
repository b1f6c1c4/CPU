using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Assembler;

namespace AssemblerGui
{
    public partial class FrmMain : Form
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        public FrmMain()
        {
            SetProcessDPIAware();
            InitializeComponent();

            SetupEditor();
            SetupDebugger();

            UpdateTitle();
        }

        private void UpdateTitle() =>
            Text = (m_Debugger != null)
                       ? $"MIPS调试器 - [{m_FileName}]"
                       : $"MIPS编辑器 - [{m_FileName}]{(m_Edited ? "*" : "")}";

        private string PromptSaveDialog(string ext, string desc, string title)
        {
            var dialog =
                new SaveFileDialog
                    {
                        AddExtension = true,
                        AutoUpgradeEnabled = true,
                        CheckFileExists = false,
                        CheckPathExists = true,
                        CreatePrompt = false,
                        DefaultExt = ext,
                        Filter = $"{desc} (*.{ext})|*.{ext}|所有文件 (*)|*",
                        FileName = m_FileName,
                        Title = title
                    };
            var res = dialog.ShowDialog();
            return res == DialogResult.Cancel ? null : dialog.FileName;
        }

        private void ExportFile(Type t, Func<string> prompt)
        {
            try
            {
                var pre = new Preprocessor(new[] { m_FilePath });
                string fn;
                using (var mem = new MemoryStream())
                {
                    var asm = (TextAssembler)Activator.CreateInstance(t, new StreamWriter(mem), 16);
                    try
                    {
                        foreach (var p in pre)
                            asm.Feed(p, true);
                        asm.Done();
                    }
                    catch (AssemblyException e)
                    {
                        MessageBox.Show(e.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LoadDoc(e.FilePath, e.Line, e.CharPos);
                        return;
                    }
                    mem.Position = 0;
                    fn = prompt();
                    if (fn == null)
                        return;

                    using (var ou = File.OpenWrite(fn))
                        mem.CopyTo(ou);
                }
                var msg = MessageBox.Show(
                                          "导出成功，是否要用记事本打开？",
                                          "MIPS汇编器",
                                          MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Question,
                                          MessageBoxDefaultButton.Button2);
                if (msg != DialogResult.Yes)
                    return;

                Process.Start("notepad", fn);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!PromptForSave())
                e.Cancel = true;
        }

        private void intelHex文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PromptForSave(true))
                return;

            ExportFile(typeof(IntelAssembler), () => PromptSaveDialog("hex", "Intel Hex文件", "导出"));
        }

        private void 二进制机器码BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PromptForSave(true))
                return;

            ExportFile(typeof(BinAssembler), () => PromptSaveDialog("txt", "纯文本文件", "导出"));
        }

        private void 十六进制机器码HToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PromptForSave(true))
                return;

            ExportFile(typeof(HexAssembler), () => PromptSaveDialog("txt", "纯文本文件", "导出"));
        }
    }
}
