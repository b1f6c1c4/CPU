using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AssemblerGui
{
    public partial class FrmMain
    {
        private bool m_IsInitial = true;

        private Editor TheEditor => tabControl1.SelectedTab as Editor;

        private IEnumerable<Editor> Editors => tabControl1.TabPages.Cast<Editor>();

        private static string PromptOpen()
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
                return null;

            return dialog.FileName;
        }

        private Editor MakeNewEditor()
        {
            var the = new Editor();
            the.OnStateChanged += () => OnStateChanged?.Invoke();
            the.OnToggleBreakPoint += ToggledBreakPoint;
            tabControl1.TabPages.Add(the);
            return the;
        }

        private void NewFile()
        {
            var the = MakeNewEditor();
            tabControl1.SelectedTab = the;
            the.Focus();
            OnStateChanged?.Invoke();
        }

        private void OpenFile(string str, int? line = null, int? charPos = null, bool debugging = false)
        {
            if (m_IsInitial &&
                tabControl1.TabCount == 1 &&
                !TheEditor.Edited)
                tabControl1.TabPages.RemoveAt(0);

            m_IsInitial = false;

            var the = Editors.FirstOrDefault(ed => ed.FilePath == str) ?? MakeNewEditor();
            tabControl1.SelectedTab = the;
            the.Focus();
            the.LoadDoc(str, line, charPos, debugging);
            OnStateChanged?.Invoke();
        }

        private bool SaveAll(bool forbidNo = false) =>
            Editors.All(ed => ed.PromptForSave(forbidNo));

        private void ToggleEditorMenus()
        {
            if (m_Debugger == null)
            {
                新建NToolStripMenuItem.Enabled = true;
                打开OToolStripMenuItem.Enabled = true;

                保存SToolStripMenuItem.Enabled = TheEditor != null;
                另存为AToolStripMenuItem.Enabled = TheEditor != null;

                关闭CToolStripMenuItem.Enabled = TheEditor != null;
            }
            else
            {
                新建NToolStripMenuItem.Enabled = false;
                打开OToolStripMenuItem.Enabled = true;

                保存SToolStripMenuItem.Enabled = false;
                另存为AToolStripMenuItem.Enabled = false;

                关闭CToolStripMenuItem.Enabled = TheEditor != null;
            }
        }

        private void 新建NToolStripMenuItem_Click(object sender, EventArgs e) => NewFile();

        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e) => TheEditor.PerformSave();

        private void 另存为AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!TheEditor.PromptSaveAs())
                return;

            TheEditor.PerformSave();
        }

        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var str = PromptOpen();
            if (str == null)
                return;

            OpenFile(str);
        }

        private void 退出QToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SaveAll())
                return;

            Environment.Exit(0);
        }

        private void 切换断点BToolStripMenuItem_Click(object sender, EventArgs e) =>
            TheEditor.ToggleBreakPoint();

        private void 关闭CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_IsInitial = false;
            if (!TheEditor.PromptForSave())
                return;

            var id = tabControl1.SelectedIndex;
            tabControl1.TabPages.RemoveAt(id);
            tabControl1.SelectedIndex = id >= tabControl1.TabCount ? tabControl1.TabCount - 1 : id;
            OnStateChanged?.Invoke();
        }
    }
}
