using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Assembler;
using WeifenLuo.WinFormsUI.Docking;

namespace AssemblerGui
{
    public partial class FrmMain
    {
        private bool m_IsRunning;

        private AsmDebugger m_RawDebugger;

        private AsmAsyncDebugger m_Debugger;

        private event SimpleEventHandler OnPause;

        private event SimpleEventHandler OnStarted;

        private event SimpleEventHandler OnExited;

        private HashSet<SourcePosition> m_BreakPoints;

        // ReSharper disable InconsistentNaming
        private DebuggerPanel panel1;
        // ReSharper restore InconsistentNaming

        private void SetupDebugger()
        {
            m_BreakPoints = new HashSet<SourcePosition>();
            panel1 = new DebuggerPanel();
            panel1.Show(tabControl1, DockState.DockRight);

            StopDebugger();

            OnStarted +=
                () =>
                {
                    if (m_Debugger == null)
                        return;
                    m_IsRunning = true;
                    panel1.Start(m_RawDebugger);
                    foreach (var ed in Editors)
                        ed.ClearCurrentPositon();
                    OnStateChanged?.Invoke();
                };
            OnPause +=
                () =>
                {
                    if (m_Debugger == null)
                        return;
                    m_IsRunning = false;
                    OpenFile(m_RawDebugger.Source.FilePath, m_RawDebugger.Source.Line, null, true);
                    panel1.Pause(m_RawDebugger);
                };
            OnExited += StopDebugger;
        }

        private SimpleEventHandler InvokeOnMainThread(SimpleEventHandler handler) =>
            () =>
            {
                if (InvokeRequired)
                    Invoke(handler);
                else
                    handler?.Invoke();
            };

        private void StartDebugger()
        {
            m_RawDebugger = new AsmDebugger(m_BreakPoints);

            var pre = SaveDependency(TheEditor.FilePath);
            try
            {
                foreach (var p in pre)
                    m_RawDebugger.Feed(p, true);
                m_RawDebugger.Done();
            }
            catch (AssemblyException e)
            {
                MessageBox.Show(e.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                OpenFile(e.FilePath, e.Line, e.CharPos);
                return;
            }

            m_Debugger = new AsmAsyncDebugger(m_RawDebugger);

            m_Debugger.OnPause += InvokeOnMainThread(OnPause);
            m_Debugger.OnStarted += InvokeOnMainThread(OnStarted);
            m_Debugger.OnExited += InvokeOnMainThread(OnExited);

            panel1.Show();
            ToggleDebuggerMenus();

            m_IsRunning = false;

            OnPause?.Invoke();
        }

        private void StopDebugger()
        {
            m_Debugger?.Stop();
            m_Debugger?.Dispose();
            m_Debugger = null;
            m_RawDebugger = null;
            panel1.Hide();

            foreach (var ed in Editors)
                ed.ClearCurrentPositon();
            m_IsRunning = false;

            OnStateChanged?.Invoke();
        }

        private void ToggleDebuggerMenus()
        {
            if (m_Downloading)
            {
                开始执行SToolStripMenuItem.Enabled = false;
                暂停PToolStripMenuItem.Enabled = false;
                停止执行XToolStripMenuItem.Enabled = false;

                跳出JToolStripMenuItem.Enabled = false;
                逐指令IToolStripMenuItem.Enabled = false;
                逐语句SToolStripMenuItem.Enabled = false;
                逐过程OToolStripMenuItem.Enabled = false;
                跳出JToolStripMenuItem.Enabled = false;
            }
            else if (m_Debugger == null)
            {
                开始执行SToolStripMenuItem.Enabled = TheEditor != null;
                暂停PToolStripMenuItem.Enabled = false;
                停止执行XToolStripMenuItem.Enabled = false;

                逐指令IToolStripMenuItem.Enabled = TheEditor != null;
                逐语句SToolStripMenuItem.Enabled = TheEditor != null;
                逐过程OToolStripMenuItem.Enabled = TheEditor != null;
                跳出JToolStripMenuItem.Enabled = false;
                切换断点BToolStripMenuItem.Enabled = TheEditor != null;
            }
            else if (m_IsRunning)
            {
                开始执行SToolStripMenuItem.Enabled = false;
                暂停PToolStripMenuItem.Enabled = true;
                停止执行XToolStripMenuItem.Enabled = true;

                跳出JToolStripMenuItem.Enabled = false;
                逐指令IToolStripMenuItem.Enabled = false;
                逐语句SToolStripMenuItem.Enabled = false;
                逐过程OToolStripMenuItem.Enabled = false;
                跳出JToolStripMenuItem.Enabled = false;
            }
            else
            {
                开始执行SToolStripMenuItem.Enabled = true;
                暂停PToolStripMenuItem.Enabled = false;
                停止执行XToolStripMenuItem.Enabled = true;

                跳出JToolStripMenuItem.Enabled = true;
                逐指令IToolStripMenuItem.Enabled = true;
                逐语句SToolStripMenuItem.Enabled = true;
                逐过程OToolStripMenuItem.Enabled = true;
                跳出JToolStripMenuItem.Enabled = true;
            }
        }

        private void RunDebugger(Action<AsmAsyncDebugger> f, bool force = false)
        {
            if (m_Debugger == null)
            {
                SaveDependency(TheEditor.FilePath);

                StartDebugger();

                if (m_Debugger == null)
                    return;

                if (!force)
                    return;
            }

            try
            {
                f(m_Debugger);
            }
            catch (HaltException)
            {
                StopDebugger();
            }
        }

        private void ToggledBreakPoint(string file, int line, bool isAdd)
        {
            if (isAdd)
                m_BreakPoints.Add(new SourcePosition(file, line));
            else
                m_BreakPoints.Remove(new SourcePosition(file, line));
        }

        private void 开始执行SToolStripMenuItem_Click(object sender, EventArgs e) =>
            RunDebugger(d => d.Run(), true);

        private void 停止执行XToolStripMenuItem_Click(object sender, EventArgs e) =>
            StopDebugger();

        private void 逐指令IToolStripMenuItem_Click(object sender, EventArgs e) =>
            RunDebugger(d => d.NextInstruction());

        private void 逐语句SToolStripMenuItem_Click(object sender, EventArgs e) =>
            RunDebugger(d => d.NextStatement());

        private void 逐过程OToolStripMenuItem_Click(object sender, EventArgs e) =>
            RunDebugger(d => d.NextProcedure());

        private void 跳出JToolStripMenuItem_Click(object sender, EventArgs e) =>
            RunDebugger(d => d.JumpOut());

        private void 暂停PToolStripMenuItem_Click(object sender, EventArgs e) =>
            m_Debugger.Pause();
    }
}
