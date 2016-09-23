using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Assembler;

namespace AssemblerGui
{
    public partial class FrmMain
    {
        private bool m_Debugging;

        private delegate void CpuUpdateEventHandler();

        private event CpuUpdateEventHandler OnCpuUpdate;

        private void MakeCPU()
        {
            m_CPU = new Context
                        {
                            PC = 345,
                            Registers = new byte[4],
                            Ram = new byte[256]
                        };
            OnCpuUpdate?.Invoke();
        }

        private void SetupDebugger()
        {
            m_Debugging = false;
            panel1.Hide();

            AddReg(tableLayoutPanel1, "PC", () => m_CPU.PC, v => m_CPU.PC = v, newRow: false);
            AddReg(tableLayoutPanel1, "R0", () => m_CPU.Registers[0], v => m_CPU.Registers[0] = (byte)v);
            AddReg(tableLayoutPanel1, "R1", () => m_CPU.Registers[1], v => m_CPU.Registers[1] = (byte)v);
            AddReg(tableLayoutPanel1, "R2", () => m_CPU.Registers[2], v => m_CPU.Registers[2] = (byte)v);
            AddReg(tableLayoutPanel1, "R3", () => m_CPU.Registers[3], v => m_CPU.Registers[3] = (byte)v);
            AddReg(
                   tableLayoutPanel1,
                   "Flag",
                   () => (m_CPU.CFlag ? 0x10 : 0x00) | (m_CPU.ZeroFlag ? 0x01 : 0x00),
                   v =>
                   {
                       m_CPU.CFlag = (v & 0xf0) != 0;
                       m_CPU.ZeroFlag = (v & 0x0f) != 0;
                   });
            for (var i = 0; i <= 0xff; i++)
            {
                var i1 = i;
                AddReg(
                       tableLayoutPanel2,
                       $"MEM[0x{i:x2}]",
                       () => m_CPU.Ram[i1],
                       v => m_CPU.Ram[i1] = (byte)v,
                       true,
                       i > 0);
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
            if (updated != null)
                OnCpuUpdate += () => txt.Text = $"0x{updated():x2}";
            txt.Validating +=
                (s, e) =>
                {
                    try
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        // ReSharper disable once UnusedVariable
                        var res = (int)new Int32Converter().ConvertFromString(txt.Text);
                    }
                    catch (Exception)
                    {
                        e.Cancel = true;
                    }
                };
            if (validated != null)
                // ReSharper disable once PossibleNullReferenceException
                txt.Validated += (s, e) =>
                                 {
                                     validated((int)new Int32Converter().ConvertFromString(txt.Text));
                                     OnCpuUpdate?.Invoke();
                                 };
            table.Controls.Add(lbl, 0, row);
            table.Controls.Add(txt, 1, row);
            table.RowStyles[row].Height = h;
            table.Height = h * table.RowCount;
        }
    }
}
