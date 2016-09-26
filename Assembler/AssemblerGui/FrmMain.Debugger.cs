using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Assembler;

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

        private void SetupDebugger()
        {
            m_BreakPoints = new HashSet<SourcePosition>();

            for (var i = 0; i <= 0xff; i++)
                dataGridView1
                    .Rows
                    .Add(
                         new DataGridViewRow
                             {
                                 Cells =
                                     {
                                         new DataGridViewTextBoxCell { Value = $"0x{i:x2}" },
                                         new DataGridViewTextBoxCell()
                                     }
                             });
            StopDebugger();

            AddReg(tableLayoutPanel1, "PC", () => m_RawDebugger.CPU.PC, v => m_RawDebugger.CPU.PC = v, newRow: false);
            AddReg(
                   tableLayoutPanel1,
                   "R0",
                   () => m_RawDebugger.CPU.Registers[0],
                   v => m_RawDebugger.CPU.Registers[0] = (byte)v);
            AddReg(
                   tableLayoutPanel1,
                   "R1",
                   () => m_RawDebugger.CPU.Registers[1],
                   v => m_RawDebugger.CPU.Registers[1] = (byte)v);
            AddReg(
                   tableLayoutPanel1,
                   "R2",
                   () => m_RawDebugger.CPU.Registers[2],
                   v => m_RawDebugger.CPU.Registers[2] = (byte)v);
            AddReg(
                   tableLayoutPanel1,
                   "R3",
                   () => m_RawDebugger.CPU.Registers[3],
                   v => m_RawDebugger.CPU.Registers[3] = (byte)v);
            AddReg(
                   tableLayoutPanel1,
                   "Flag",
                   () => (m_RawDebugger.CPU.CFlag ? 0x10 : 0x00) | (m_RawDebugger.CPU.ZeroFlag ? 0x01 : 0x00),
                   v =>
                   {
                       m_RawDebugger.CPU.CFlag = (v & 0xf0) != 0;
                       m_RawDebugger.CPU.ZeroFlag = (v & 0x0f) != 0;
                   });

            OnPause += () =>
                       {
                           for (var i = 0; i < m_RawDebugger.CPU.Ram.Length; i++)
                               dataGridView1.Rows[i].Cells[1].Value = $"0x{m_RawDebugger.CPU.Ram[i]:x2}";
                       };
            OnStarted +=
                () =>
                {
                    m_IsRunning = true;
                    foreach (var ed in Editors)
                        ed.ClearCurrentPositon();
                    dataGridView1.Enabled = false;
                    OnStateChanged?.Invoke();
                };
            OnPause +=
                () =>
                {
                    m_IsRunning = false;
                    dataGridView1.Enabled = true;
                    OpenFile(m_RawDebugger.Source.FilePath, m_RawDebugger.Source.Line, null, true);
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

            var pre = new Preprocessor(new[] { TheEditor.FilePath });
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

        private void AddReg(TableLayoutPanel table, string name,
                            Func<int> updated = null,
                            Action<int> validated = null,
                            bool compact = false, bool newRow = true)
        {
            var h = compact ? 30 : 35;
            if (newRow)
            {
                table.RowCount++;
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, h));
            }

            var row = table.RowCount - 1;
            var lbl =
                new Label
                    {
                        Text = name,
                        Dock = DockStyle.Fill,
                        Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point, 134),
                        TextAlign = ContentAlignment.MiddleCenter
                    };
            var txt =
                new TextBox
                    {
                        Dock = DockStyle.Fill,
                        Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point, 134)
                    };
            OnStarted += () => txt.Enabled = false;
            OnPause += () => txt.Enabled = true;
            if (updated != null)
                OnPause += () => txt.Text = $"0x{updated():x2}";
            txt.Validating +=
                (s, e) =>
                {
                    if (!TryParse(txt.Text).HasValue)
                        e.Cancel = true;
                };
            if (validated != null)
                txt.Validated += (s, e) =>
                                 {
                                     // ReSharper disable once PossibleInvalidOperationException
                                     validated(TryParse(txt.Text).Value);
                                     if (updated != null)
                                         txt.Text = $"0x{updated():x2}";
                                 };
            table.Controls.Add(lbl, 0, row);
            table.Controls.Add(txt, 1, row);
            table.RowStyles[row].Height = h;
            table.Height = h * table.RowCount;
        }

        private static int? TryParse(string str)
        {
            try
            {
                // ReSharper disable once PossibleNullReferenceException
                // ReSharper disable once UnusedVariable
                return (int)new Int32Converter().ConvertFromString(str);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void RunDebugger(Action<AsmAsyncDebugger> f, bool force = false)
        {
            if (m_Debugger == null)
            {
                if (!SaveAll(true))
                    return;

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

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex != 1)
                return;

            var v = TryParse((string)dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            if (!v.HasValue)
                e.Cancel = true;
        }

        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 1)
                return;

            var v = TryParse((string)dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            // ReSharper disable once PossibleInvalidOperationException
            m_RawDebugger.CPU.Ram[e.RowIndex] = (byte)v.Value;
        }
    }
}
