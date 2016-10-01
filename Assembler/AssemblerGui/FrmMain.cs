using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Assembler;
using AssemblerGui.Properties;

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

            启用长跳转LToolStripMenuItem.Checked = Settings.Default.EnableLongJump;

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
            asm.EnableLongJump = Settings.Default.EnableLongJump;
            try
            {
                var pre = SaveDependency(TheEditor.FilePath);
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
                        MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        OpenFile(e.FilePath, e.Line, e.CharPos);
                        return false;
                    }
                    sw.Flush();

                    mem.Position = 0;
                    fn = prompt();
                    if (fn == null)
                        return false;

                    using (var ou = File.Open(fn, FileMode.Create, FileAccess.Write))
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
                MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private Preprocessor SaveDependency(string initial)
        {
            var pre = new Preprocessor { initial };
            var flag = true;
            while (flag)
            {
                flag = false;
                var lst = new List<string>();
                foreach (var fn in pre)
                {
                    var e = Editors.FirstOrDefault(ed => ed.FilePath == fn);
                    if (e == null ||
                        !e.Edited)
                        continue;
                    e.PerformSave();
                    lst.Add(fn);
                    flag = true;
                }
                pre.AddRange(lst);
            }
            return pre;
        }

        private void Cycle<T>(T asm)
            where T : AsmProgBase, IWriter
        {
            asm.EnableLongJump = Settings.Default.EnableLongJump;
            var tmp = Path.GetTempFileName();
            try
            {
                File.WriteAllText(tmp, TheEditor.Value);
                using (var mem = new MemoryStream())
                using (var sw = new StreamWriter(mem))
                {
                    asm.SetWriter(sw);
                    try
                    {
                        asm.Feed(tmp, false);
                        asm.Done();
                    }
                    catch (AssemblyException e)
                    {
                        MessageBox.Show(
                                        e.Message.Replace(tmp, TheEditor.FilePath),
                                        "错误",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        OpenFile(TheEditor.FilePath, e.Line, e.CharPos);
                        return;
                    }
                    sw.Flush();

                    mem.Position = 0;

                    using (var sr = new StreamReader(mem))
                        TheEditor.Value = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                File.Delete(tmp);
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
                启用长跳转LToolStripMenuItem.Enabled = false;
            }
            else
            {
                intelHex文件ToolStripMenuItem.Enabled = TheEditor != null;
                二进制机器码BToolStripMenuItem.Enabled = TheEditor != null;
                十六进制机器码HToolStripMenuItem.Enabled = TheEditor != null;
                原始汇编AToolStripMenuItem.Enabled = TheEditor != null;

                下载DToolStripMenuItem.Enabled = TheEditor != null;

                格式化代码FToolStripMenuItem.Enabled = TheEditor != null;
                启用长跳转LToolStripMenuItem.Enabled = true;
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!SaveAll())
                e.Cancel = true;
        }

        private void intelHex文件ToolStripMenuItem_Click(object sender, EventArgs e) =>
            ExportFile(new IntelAssembler(), () => PromptSaveDialog("hex", "Intel Hex文件", "导出", TheEditor.FileName));

        private void 二进制机器码BToolStripMenuItem_Click(object sender, EventArgs e) =>
            ExportFile(new BinAssembler(), () => PromptSaveDialog("txt", "纯文本文件", "导出", TheEditor.FileName));

        private void 十六进制机器码HToolStripMenuItem_Click(object sender, EventArgs e) =>
            ExportFile(new HexAssembler(), () => PromptSaveDialog("txt", "纯文本文件", "导出", TheEditor.FileName));

        private void 原始汇编AToolStripMenuItem_Click(object sender, EventArgs e) =>
            ExportFile(new AsmFinalPrettifier(), () => PromptSaveDialog("mips", "MIPS文件", "导出", TheEditor.FileName));

        private void 格式化代码FToolStripMenuItem_Click(object sender, EventArgs e) => Cycle(new AsmPrettifier());

        private void 查看帮助VToolStripMenuItem_Click(object sender, EventArgs e) => FrmHelp.ShowHelp(this);

        private void 下载DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Downloader.CheckStpExistance(this))
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) => OnStateChanged?.Invoke();

        private void 全部关闭WToolStripMenuItem_Click(object sender, EventArgs e)
        {
            while (TheEditor != null)
                if (!TheEditor.SaveClose())
                    break;
        }

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

        private void 启用长跳转LToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.EnableLongJump = 启用长跳转LToolStripMenuItem.Checked;
            Settings.Default.Save();
        }
    }
}
