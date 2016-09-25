using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Assembler;

namespace AssemblerGui
{
    public partial class FrmMain : Form
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        private event SimpleEventHandler OnStateChanged;

        private bool m_Downloading;

        public FrmMain()
        {
            SetProcessDPIAware();
            InitializeComponent();

            OnStateChanged += UpdateTitle;
            OnStateChanged += ToggleEditorMenus;
            OnStateChanged += ToggleAssemblerMenus;
            OnStateChanged += ToggleDebuggerMenus;
            OnStateChanged += () =>
                              {
                                  foreach (var ed in Editors)
                                      ed.ReadOnly = m_Downloading || m_Debugger != null;
                              };

            SetupDebugger();

            NewFile();

            ActiveControl = TheEditor?.ActiveControl;
        }

        private void UpdateTitle()
        {
            var sb = new StringBuilder();

            sb.Append(m_Debugger != null ? "MIPS调试器" : "MIPS编辑器");

            if (TheEditor != null)
            {
                sb.Append($"- [{TheEditor.FileName}]");
                if (TheEditor.Edited)
                    sb.Append("*");
            }

            if (m_Debugger != null && m_IsRunning)
                sb.Append(" - Running");

            if (m_Downloading)
                sb.Append(" - Downloading");

            Text = sb.ToString();
        }

        public static string PromptSaveDialog(string ext, string desc, string title, string fileName)
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
                        FileName = fileName,
                        Title = title
                    };
            var res = dialog.ShowDialog();
            return res == DialogResult.Cancel ? null : dialog.FileName;
        }

        private bool ExportFile<T>(T asm, Func<string> prompt, bool open = true)
            where T : AsmProgBase, IWriter
        {
            try
            {
                var pre = new Preprocessor(new[] { TheEditor.FilePath });
                string fn;
                using (var mem = new MemoryStream())
                using (var sw = new StreamWriter(mem))
                {
                    asm.SetWriter(sw);
                    try
                    {
                        foreach (var p in pre)
                            asm.Feed(p, true);
                        asm.Done();
                    }
                    catch (AssemblyException e)
                    {
                        MessageBox.Show(e.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        OpenFile(e.FilePath, e.Line, e.CharPos);
                        return false;
                    }
                    sw.Flush();

                    mem.Position = 0;
                    fn = prompt();
                    if (fn == null)
                        return false;

                    using (var ou = File.OpenWrite(fn))
                        mem.CopyTo(ou);
                }
                if (!open)
                    return true;

                var msg = MessageBox.Show(
                                          "导出成功，是否要用记事本打开？",
                                          "MIPS汇编器",
                                          MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Question,
                                          MessageBoxDefaultButton.Button2);
                if (msg != DialogResult.Yes)
                    return true;

                Process.Start("notepad", fn);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void Cycle<T>(T asm)
            where T : AsmProgBase, IWriter
        {
            try
            {
                using (var mem = new MemoryStream())
                using (var sw = new StreamWriter(mem))
                {
                    asm.SetWriter(sw);
                    try
                    {
                        asm.Feed(TheEditor.FilePath, false);
                        asm.Done();
                    }
                    catch (AssemblyException e)
                    {
                        MessageBox.Show(e.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        OpenFile(e.FilePath, e.Line, e.CharPos);
                        return;
                    }
                    sw.Flush();

                    mem.Position = 0;

                    using (var ou = File.OpenWrite(TheEditor.FilePath))
                        mem.CopyTo(ou);
                }
                OpenFile(TheEditor.FilePath, force : true);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToggleAssemblerMenus()
        {
            if (m_Downloading || m_Debugger != null)
            {
                intelHex文件ToolStripMenuItem.Enabled = false;
                二进制机器码BToolStripMenuItem.Enabled = false;
                十六进制机器码HToolStripMenuItem.Enabled = false;
                原始汇编AToolStripMenuItem.Enabled = false;

                下载DToolStripMenuItem.Enabled = false;

                格式化代码FToolStripMenuItem.Enabled = false;
            }
            else
            {
                intelHex文件ToolStripMenuItem.Enabled = TheEditor != null;
                二进制机器码BToolStripMenuItem.Enabled = TheEditor != null;
                十六进制机器码HToolStripMenuItem.Enabled = TheEditor != null;
                原始汇编AToolStripMenuItem.Enabled = TheEditor != null;

                下载DToolStripMenuItem.Enabled = TheEditor != null;

                格式化代码FToolStripMenuItem.Enabled = TheEditor != null;
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!SaveAll())
                e.Cancel = true;
        }

        private void intelHex文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SaveAll(true))
                return;

            ExportFile(new IntelAssembler(), () => PromptSaveDialog("hex", "Intel Hex文件", "导出", TheEditor.FileName));
        }

        private void 二进制机器码BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SaveAll(true))
                return;

            ExportFile(new BinAssembler(), () => PromptSaveDialog("txt", "纯文本文件", "导出", TheEditor.FileName));
        }

        private void 十六进制机器码HToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SaveAll(true))
                return;

            ExportFile(new HexAssembler(), () => PromptSaveDialog("txt", "纯文本文件", "导出", TheEditor.FileName));
        }

        private void 原始汇编AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SaveAll(true))
                return;

            ExportFile(new AsmPrettifier(true), () => PromptSaveDialog("mips", "MIPS文件", "导出", TheEditor.FileName));
        }

        private void 格式化代码FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!TheEditor.PromptForSave(true))
                return;

            Cycle(new AsmPrettifier());
        }

        private void 查看帮助VToolStripMenuItem_Click(object sender, EventArgs e) => FrmHelp.ShowHelp(this);

        private void 下载DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SaveAll(true))
                return;

            var path = Path.GetTempFileName();
            File.Move(path, path + ".hex");
            var hexPath = path + ".hex";

            if (!ExportFile(new IntelAssembler(), () => hexPath, false))
                return;

            m_Downloading = true;
            OnStateChanged?.Invoke();

            var downloader = new Downloader(hexPath);
            downloader.OnExited +=
                msg =>
                {
                    if (msg == null)
                        MessageBox.Show("下载成功！", "MIPS汇编器", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("下载失败，错误信息：" + msg, "MIPS汇编器", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    File.Delete(hexPath);
                    m_Downloading = false;
                    InvokeOnMainThread(OnStateChanged)();
                };
            downloader.Start();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e) => OnStateChanged?.Invoke();

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) => TheEditor?.Focus();

        private void 全部关闭WToolStripMenuItem_Click(object sender, EventArgs e)
        {
            while (TheEditor != null)
            {
                if (TheEditor.Edited)
                    if (!TheEditor.PromptForSave())
                        break;

                tabControl1.TabPages.RemoveAt(0);
            }
        }

        private void tabControl1_MouseClick(object sender, MouseEventArgs e) => TheEditor?.Focus();

        private void FrmMain_DragDrop(object sender, DragEventArgs e)
        {
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files == null)
                return;
            foreach (var file in files)
                OpenFile(file);
        }

        private void FrmMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }
    }
}
