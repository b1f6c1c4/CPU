﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Assembler;

namespace AssemblerGui
{
    public partial class FrmMain
    {
        private AsmDebugger m_Debugger;

        private event AsmDebugger.UpdatedEventHandler OnUpdated;

        private void SetupDebugger()
        {
            StopDebugger();

            AddReg(tableLayoutPanel1, "PC", () => m_Debugger.CPU.PC, v => m_Debugger.CPU.PC = v, newRow: false);
            AddReg(tableLayoutPanel1, "R0", () => m_Debugger.CPU.Registers[0], v => m_Debugger.CPU.Registers[0] = (byte)v);
            AddReg(tableLayoutPanel1, "R1", () => m_Debugger.CPU.Registers[1], v => m_Debugger.CPU.Registers[1] = (byte)v);
            AddReg(tableLayoutPanel1, "R2", () => m_Debugger.CPU.Registers[2], v => m_Debugger.CPU.Registers[2] = (byte)v);
            AddReg(tableLayoutPanel1, "R3", () => m_Debugger.CPU.Registers[3], v => m_Debugger.CPU.Registers[3] = (byte)v);
            AddReg(
                   tableLayoutPanel1,
                   "Flag",
                   () => (m_Debugger.CPU.CFlag ? 0x10 : 0x00) | (m_Debugger.CPU.ZeroFlag ? 0x01 : 0x00),
                   v =>
                   {
                       m_Debugger.CPU.CFlag = (v & 0xf0) != 0;
                       m_Debugger.CPU.ZeroFlag = (v & 0x0f) != 0;
                   });
        }

        private void StartDebugger()
        {
            m_Debugger = new AsmDebugger();

            var pre = new Preprocessor(new[] { m_FilePath });
            try
            {
                foreach (var p in pre)
                    m_Debugger.Feed(p);
                m_Debugger.Done();
            }
            catch (AssemblyException e)
            {
                m_Debugger = null;
                MessageBox.Show(e.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadDoc(e.FilePath, e.Line, e.CharPos);
                return;
            }

            m_Debugger.OnUpdated += () => OnUpdated?.Invoke();
            m_Debugger.OnUpdated += () => LoadDoc(m_Debugger.Source.FilePath, m_Debugger.Source.Line, null);

            foreach (var line in scintilla.Lines)
                if ((line.MarkerGet() & 1) != 0)
                    m_Debugger.AddBreakPoint(m_FilePath, line.Index + 1);

            panel1.Show();

            开始执行SToolStripMenuItem.Visible = false;
            停止执行XToolStripMenuItem.Visible = true;
            跳出JToolStripMenuItem.Enabled = true;

            scintilla.ReadOnly = true;
            scintilla.CaretLineVisible = true;

            UpdateTitle();
            m_Debugger.ForceUpdate();
        }

        private void StopDebugger()
        {
            m_Debugger = null;
            panel1.Hide();

            开始执行SToolStripMenuItem.Visible = true;
            停止执行XToolStripMenuItem.Visible = false;
            跳出JToolStripMenuItem.Enabled = false;

            scintilla.ReadOnly = false;
            scintilla.CaretLineVisible = false;

            UpdateTitle();
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
            if (updated != null)
                OnUpdated += () => txt.Text = $"0x{updated():x2}";
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
                                     m_Debugger.ForceUpdate();
                                 };
            table.Controls.Add(lbl, 0, row);
            table.Controls.Add(txt, 1, row);
            table.RowStyles[row].Height = h;
            table.Height = h * table.RowCount;
        }

        private void ToggleBreakPoint(int id)
        {
            var line = scintilla.Lines[id];

            if ((line.MarkerGet() & 1) != 0)
            {
                line.MarkerDelete(0);
                m_Debugger?.RemoveBreakPoint(m_FilePath, id);
            }
            else
            {
                line.MarkerAdd(0);
                m_Debugger?.AddBreakPoint(m_FilePath, id);
            }
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

        private void 开始执行SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PromptForSave(true))
                return;

            StartDebugger();
        }

        private void 停止执行XToolStripMenuItem_Click(object sender, EventArgs e) { StopDebugger(); }

        private void 逐指令IToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_Debugger == null)
            {
                if (!PromptForSave(true))
                    return;

                StartDebugger();
            }

            // ReSharper disable once PossibleNullReferenceException
            m_Debugger.NextInstruction();
        }

        private void 逐语句SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_Debugger == null)
            {
                if (!PromptForSave(true))
                    return;

                StartDebugger();
            }

            // ReSharper disable once PossibleNullReferenceException
            m_Debugger.NextStatement();
        }

        private void 逐过程OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_Debugger == null)
            {
                if (!PromptForSave(true))
                    return;

                StartDebugger();
            }

            // ReSharper disable once PossibleNullReferenceException
            m_Debugger.NextProcedure();
        }

        private void 跳出JToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_Debugger.JumpOut();
        }

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
            m_Debugger.CPU.Ram[e.RowIndex] = (byte)v.Value;
        }
    }
}
