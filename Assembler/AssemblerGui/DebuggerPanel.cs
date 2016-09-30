using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace AssemblerGui
{
    public class DebuggerPanel : DockContent
    {
        private event SimpleEventHandler OnPause;

        private event SimpleEventHandler OnStarted;

        // ReSharper disable InconsistentNaming
        private DataGridView dataGridView1;
        private TableLayoutPanel tableLayoutPanel1;
        // ReSharper restore InconsistentNaming

        private AsmDebugger m_RawDebugger;

        public void Start(AsmDebugger debugger)
        {
            m_RawDebugger = debugger;
            OnStarted?.Invoke();
        }

        public void Pause(AsmDebugger debugger)
        {
            m_RawDebugger = debugger;
            OnPause?.Invoke();
        }

        public DebuggerPanel()
        {
            CloseButton = false;
            CloseButtonVisible = false;
            DockAreas = DockAreas.DockRight;
            InitializeComponent();
            foreach (DataGridViewColumn c in dataGridView1.Columns)
                c.DefaultCellStyle.Font = new Font("Consolas", 10F, GraphicsUnit.Point);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Consolas", 10F, GraphicsUnit.Point);
            dataGridView1.CellValidating += dataGridView1_CellValidating;
            dataGridView1.CellValidated += dataGridView1_CellValidated;

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
                                     },
                                 Height = 20
                             });
            AddReg(
                   tableLayoutPanel1,
                   "PC",
                   () => m_RawDebugger.CPU.PC,
                   v => m_RawDebugger.CPU.PC = v);
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
            OnStarted += () => dataGridView1.Enabled = false;
            OnPause += () => dataGridView1.Enabled = true;
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            var addr =
                new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Key",
                        HeaderText = "Addr",
                        Name = "Addr",
                        ReadOnly = true,
                        SortMode = DataGridViewColumnSortMode.NotSortable,
                        FillWeight = 3,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    };
            var data =
                new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Value",
                        HeaderText = "Data",
                        Name = "Data",
                        SortMode = DataGridViewColumnSortMode.NotSortable,
                        FillWeight = 7,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    };
            dataGridView1 =
                new DataGridView
                    {
                        AllowUserToAddRows = false,
                        AllowUserToDeleteRows = false,
                        AllowUserToResizeRows = false,
                        ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                        Dock = DockStyle.Fill,
                        ImeMode = ImeMode.Off,
                        Location = new Point(0, 89),
                        Margin = new Padding(4),
                        MultiSelect = false,
                        Name = "dataGridView1",
                        RowHeadersWidth = 16,
                        RowTemplate = { Height = 6 },
                        SelectionMode = DataGridViewSelectionMode.CellSelect,
                        Size = new Size(244, 562),
                        TabIndex = 3,
                        Columns =
                            {
                                addr,
                                data
                            }
                    };
            tableLayoutPanel1 =
                new TableLayoutPanel
                    {
                        ColumnCount = 2,
                        ColumnStyles =
                            {
                                new ColumnStyle(SizeType.Percent, 40F),
                                new ColumnStyle(SizeType.Percent, 60F)
                            },
                        Dock = DockStyle.Top,
                        Location = new Point(0, 0),
                        Margin = new Padding(3, 1, 3, 1),
                        Name = "tableLayoutPanel1",
                        RowCount = 0,
                        Size = new Size(244, 89),
                        TabIndex = 4
                    };
            ClientSize = new Size(244, 651);
            Controls.Add(dataGridView1);
            Controls.Add(tableLayoutPanel1);
            AutoScaleDimensions = new SizeF(6F, 13F);
            ResumeLayout(false);
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

        private static void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex != 1)
                return;

            var v = TryParse((string)e.FormattedValue);
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
            dataGridView1.Rows[e.RowIndex].Cells[1].Value = $"0x{(byte)v.Value:x2}";
        }
    }
}
