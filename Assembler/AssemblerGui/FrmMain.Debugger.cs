using System;
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

        private void SetupDebugger()
        {
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

            OnPause += () => OpenFile(m_RawDebugger.Source.FilePath, m_RawDebugger.Source.Line);
            OnPause += () =>
                       {
                           for (var i = 0; i < m_RawDebugger.CPU.Ram.Length; i++)
                               dataGridView1.Rows[i].Cells[1].Value = $"0x{m_RawDebugger.CPU.Ram[i]:x2}";
                       };
            OnStarted +=
                () =>
                {
                    m_IsRunning = true;
                    // TODO: scintilla.MarkerDeleteAll(1);
                    dataGridView1.Enabled = false;

                    UpdateTitle();
                };
            OnPause +=
                () =>
                {
                    m_IsRunning = false;
                    dataGridView1.Enabled = true;

                    UpdateTitle();
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
            m_RawDebugger = new AsmDebugger();

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

           /* TODO: foreach (var line in scintilla.Lines)
                if ((line.MarkerGet() & 1) != 0)
                    m_RawDebugger.AddBreakPoint(m_FilePath, line.Index + 1); */

            m_Debugger = new AsmAsyncDebugger(m_RawDebugger);

            m_Debugger.OnPause += InvokeOnMainThread(OnPause);
            m_Debugger.OnStarted += InvokeOnMainThread(OnStarted);
            m_Debugger.OnExited += InvokeOnMainThread(OnExited);

            panel1.Show();
            ToggleDebuggerMenus();

            // TODO: scintilla.ReadOnly = true;
            m_IsRunning = false;

            UpdateTitle();
            m_RawDebugger.ForceUpdate();
        }

        private void StopDebugger()
        {
            m_Debugger?.Dispose();
            m_Debugger = null;
            panel1.Hide();

            // TODO: scintilla.MarkerDeleteAll(1);
            // TODO: scintilla.ReadOnly = false;
            m_IsRunning = false;

            UpdateTitle();
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
                        Font = new Font("Microsoft YaHei Mono", 10F, FontStyle.Regular, GraphicsUnit.Point, 134),
                        TextAlign = ContentAlignment.MiddleCenter
                    };
            var txt =
                new TextBox
                    {
                        Dock = DockStyle.Fill,
                        Font = new Font("Microsoft YaHei Mono", 10F, FontStyle.Regular, GraphicsUnit.Point, 134)
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
                                     m_RawDebugger.ForceUpdate();
                                 };
            table.Controls.Add(lbl, 0, row);
            table.Controls.Add(txt, 1, row);
            table.RowStyles[row].Height = h;
            table.Height = h * table.RowCount;
        }

        private void ToggleBreakPoint(int id)
        {
            /*var line = scintilla.Lines[id];

            if ((line.MarkerGet() & 1) != 0)
            {
                line.MarkerDelete(0);
                m_RawDebugger?.RemoveBreakPoint(m_FilePath, id);
            }
            else
            {
                line.MarkerAdd(0);
                m_RawDebugger?.AddBreakPoint(m_FilePath, id);
            }*/
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
