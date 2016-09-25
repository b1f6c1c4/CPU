namespace AssemblerGui
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Addr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Data = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新建NToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开OToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.保存SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全部保存LToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.另存为AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.关闭CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全部关闭WToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.退出QToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.编辑EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.格式化代码FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.生成BToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.intelHex文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.二进制机器码BToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.十六进制机器码HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.原始汇编AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.下载DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.调试DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开始执行SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.暂停PToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.停止执行XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.逐指令IToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.逐语句SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.逐过程OToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.跳出JToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.切换断点BToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看帮助VToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(533, 24);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(190, 410);
            this.panel1.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Addr,
            this.Data});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.dataGridView1.Location = new System.Drawing.Point(0, 64);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.RowHeadersWidth = 16;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGridView1.RowTemplate.Height = 12;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(190, 346);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValidated);
            this.dataGridView1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView1_CellValidating);
            // 
            // Addr
            // 
            this.Addr.DataPropertyName = "Key";
            this.Addr.HeaderText = "Addr";
            this.Addr.Name = "Addr";
            this.Addr.ReadOnly = true;
            this.Addr.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Addr.Width = 70;
            // 
            // Data
            // 
            this.Data.DataPropertyName = "Value";
            this.Data.HeaderText = "Data";
            this.Data.Name = "Data";
            this.Data.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Data.Width = 70;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(190, 64);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件FToolStripMenuItem,
            this.编辑EToolStripMenuItem,
            this.生成BToolStripMenuItem,
            this.调试DToolStripMenuItem,
            this.帮助HToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(6, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(723, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件FToolStripMenuItem
            // 
            this.文件FToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建NToolStripMenuItem,
            this.打开OToolStripMenuItem,
            this.toolStripMenuItem1,
            this.保存SToolStripMenuItem,
            this.全部保存LToolStripMenuItem,
            this.另存为AToolStripMenuItem,
            this.toolStripMenuItem2,
            this.关闭CToolStripMenuItem,
            this.全部关闭WToolStripMenuItem,
            this.toolStripMenuItem7,
            this.退出QToolStripMenuItem});
            this.文件FToolStripMenuItem.Name = "文件FToolStripMenuItem";
            this.文件FToolStripMenuItem.Size = new System.Drawing.Size(58, 22);
            this.文件FToolStripMenuItem.Text = "文件(&F)";
            // 
            // 新建NToolStripMenuItem
            // 
            this.新建NToolStripMenuItem.Name = "新建NToolStripMenuItem";
            this.新建NToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.新建NToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.新建NToolStripMenuItem.Text = "新建(&N)";
            this.新建NToolStripMenuItem.Click += new System.EventHandler(this.新建NToolStripMenuItem_Click);
            // 
            // 打开OToolStripMenuItem
            // 
            this.打开OToolStripMenuItem.Name = "打开OToolStripMenuItem";
            this.打开OToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.打开OToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.打开OToolStripMenuItem.Text = "打开(&O)...";
            this.打开OToolStripMenuItem.Click += new System.EventHandler(this.打开OToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(213, 6);
            // 
            // 保存SToolStripMenuItem
            // 
            this.保存SToolStripMenuItem.Name = "保存SToolStripMenuItem";
            this.保存SToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.保存SToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.保存SToolStripMenuItem.Text = "保存(S)";
            this.保存SToolStripMenuItem.Click += new System.EventHandler(this.保存SToolStripMenuItem_Click);
            // 
            // 全部保存LToolStripMenuItem
            // 
            this.全部保存LToolStripMenuItem.Name = "全部保存LToolStripMenuItem";
            this.全部保存LToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.全部保存LToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.全部保存LToolStripMenuItem.Text = "全部保存(&L)";
            this.全部保存LToolStripMenuItem.Click += new System.EventHandler(this.全部保存LToolStripMenuItem_Click);
            // 
            // 另存为AToolStripMenuItem
            // 
            this.另存为AToolStripMenuItem.Name = "另存为AToolStripMenuItem";
            this.另存为AToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.另存为AToolStripMenuItem.Text = "另存为(A)...";
            this.另存为AToolStripMenuItem.Click += new System.EventHandler(this.另存为AToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(213, 6);
            // 
            // 关闭CToolStripMenuItem
            // 
            this.关闭CToolStripMenuItem.Name = "关闭CToolStripMenuItem";
            this.关闭CToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.关闭CToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.关闭CToolStripMenuItem.Text = "关闭(&C)";
            this.关闭CToolStripMenuItem.Click += new System.EventHandler(this.关闭CToolStripMenuItem_Click);
            // 
            // 全部关闭WToolStripMenuItem
            // 
            this.全部关闭WToolStripMenuItem.Name = "全部关闭WToolStripMenuItem";
            this.全部关闭WToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.全部关闭WToolStripMenuItem.Text = "全部关闭(&W)";
            this.全部关闭WToolStripMenuItem.Click += new System.EventHandler(this.全部关闭WToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(213, 6);
            // 
            // 退出QToolStripMenuItem
            // 
            this.退出QToolStripMenuItem.Name = "退出QToolStripMenuItem";
            this.退出QToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.退出QToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.退出QToolStripMenuItem.Text = "退出(&Q)";
            this.退出QToolStripMenuItem.Click += new System.EventHandler(this.退出QToolStripMenuItem_Click);
            // 
            // 编辑EToolStripMenuItem
            // 
            this.编辑EToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.格式化代码FToolStripMenuItem});
            this.编辑EToolStripMenuItem.Name = "编辑EToolStripMenuItem";
            this.编辑EToolStripMenuItem.Size = new System.Drawing.Size(59, 22);
            this.编辑EToolStripMenuItem.Text = "编辑(&E)";
            // 
            // 格式化代码FToolStripMenuItem
            // 
            this.格式化代码FToolStripMenuItem.Name = "格式化代码FToolStripMenuItem";
            this.格式化代码FToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.F)));
            this.格式化代码FToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.格式化代码FToolStripMenuItem.Text = "格式化代码(&F)";
            this.格式化代码FToolStripMenuItem.Click += new System.EventHandler(this.格式化代码FToolStripMenuItem_Click);
            // 
            // 生成BToolStripMenuItem
            // 
            this.生成BToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.intelHex文件ToolStripMenuItem,
            this.二进制机器码BToolStripMenuItem,
            this.十六进制机器码HToolStripMenuItem,
            this.toolStripMenuItem3,
            this.原始汇编AToolStripMenuItem,
            this.toolStripMenuItem6,
            this.下载DToolStripMenuItem});
            this.生成BToolStripMenuItem.Name = "生成BToolStripMenuItem";
            this.生成BToolStripMenuItem.Size = new System.Drawing.Size(60, 22);
            this.生成BToolStripMenuItem.Text = "生成(&B)";
            // 
            // intelHex文件ToolStripMenuItem
            // 
            this.intelHex文件ToolStripMenuItem.Name = "intelHex文件ToolStripMenuItem";
            this.intelHex文件ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.intelHex文件ToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.intelHex文件ToolStripMenuItem.Text = "&Intel Hex文件...";
            this.intelHex文件ToolStripMenuItem.Click += new System.EventHandler(this.intelHex文件ToolStripMenuItem_Click);
            // 
            // 二进制机器码BToolStripMenuItem
            // 
            this.二进制机器码BToolStripMenuItem.Name = "二进制机器码BToolStripMenuItem";
            this.二进制机器码BToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.二进制机器码BToolStripMenuItem.Text = "二进制机器码(&B)...";
            this.二进制机器码BToolStripMenuItem.Click += new System.EventHandler(this.二进制机器码BToolStripMenuItem_Click);
            // 
            // 十六进制机器码HToolStripMenuItem
            // 
            this.十六进制机器码HToolStripMenuItem.Name = "十六进制机器码HToolStripMenuItem";
            this.十六进制机器码HToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.十六进制机器码HToolStripMenuItem.Text = "十六进制机器码(&H)...";
            this.十六进制机器码HToolStripMenuItem.Click += new System.EventHandler(this.十六进制机器码HToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(201, 6);
            // 
            // 原始汇编AToolStripMenuItem
            // 
            this.原始汇编AToolStripMenuItem.Name = "原始汇编AToolStripMenuItem";
            this.原始汇编AToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.原始汇编AToolStripMenuItem.Text = "原始汇编(&A)...";
            this.原始汇编AToolStripMenuItem.Click += new System.EventHandler(this.原始汇编AToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(201, 6);
            // 
            // 下载DToolStripMenuItem
            // 
            this.下载DToolStripMenuItem.Name = "下载DToolStripMenuItem";
            this.下载DToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.下载DToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.下载DToolStripMenuItem.Text = "下载(&D)";
            this.下载DToolStripMenuItem.Click += new System.EventHandler(this.下载DToolStripMenuItem_Click);
            // 
            // 调试DToolStripMenuItem
            // 
            this.调试DToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.开始执行SToolStripMenuItem,
            this.暂停PToolStripMenuItem,
            this.停止执行XToolStripMenuItem,
            this.toolStripMenuItem4,
            this.逐指令IToolStripMenuItem,
            this.逐语句SToolStripMenuItem,
            this.逐过程OToolStripMenuItem,
            this.跳出JToolStripMenuItem,
            this.toolStripMenuItem5,
            this.切换断点BToolStripMenuItem});
            this.调试DToolStripMenuItem.Name = "调试DToolStripMenuItem";
            this.调试DToolStripMenuItem.Size = new System.Drawing.Size(61, 22);
            this.调试DToolStripMenuItem.Text = "调试(&D)";
            // 
            // 开始执行SToolStripMenuItem
            // 
            this.开始执行SToolStripMenuItem.Name = "开始执行SToolStripMenuItem";
            this.开始执行SToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.开始执行SToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.开始执行SToolStripMenuItem.Text = "开始执行(&R)";
            this.开始执行SToolStripMenuItem.Click += new System.EventHandler(this.开始执行SToolStripMenuItem_Click);
            // 
            // 暂停PToolStripMenuItem
            // 
            this.暂停PToolStripMenuItem.Name = "暂停PToolStripMenuItem";
            this.暂停PToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.暂停PToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.暂停PToolStripMenuItem.Text = "暂停(&P)";
            this.暂停PToolStripMenuItem.Click += new System.EventHandler(this.暂停PToolStripMenuItem_Click);
            // 
            // 停止执行XToolStripMenuItem
            // 
            this.停止执行XToolStripMenuItem.Name = "停止执行XToolStripMenuItem";
            this.停止执行XToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F5)));
            this.停止执行XToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.停止执行XToolStripMenuItem.Text = "停止执行(&X)";
            this.停止执行XToolStripMenuItem.Click += new System.EventHandler(this.停止执行XToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(192, 6);
            // 
            // 逐指令IToolStripMenuItem
            // 
            this.逐指令IToolStripMenuItem.Name = "逐指令IToolStripMenuItem";
            this.逐指令IToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F11)));
            this.逐指令IToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.逐指令IToolStripMenuItem.Text = "逐指令(&I)";
            this.逐指令IToolStripMenuItem.Click += new System.EventHandler(this.逐指令IToolStripMenuItem_Click);
            // 
            // 逐语句SToolStripMenuItem
            // 
            this.逐语句SToolStripMenuItem.Name = "逐语句SToolStripMenuItem";
            this.逐语句SToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.逐语句SToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.逐语句SToolStripMenuItem.Text = "逐语句(&S)";
            this.逐语句SToolStripMenuItem.Click += new System.EventHandler(this.逐语句SToolStripMenuItem_Click);
            // 
            // 逐过程OToolStripMenuItem
            // 
            this.逐过程OToolStripMenuItem.Name = "逐过程OToolStripMenuItem";
            this.逐过程OToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.逐过程OToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.逐过程OToolStripMenuItem.Text = "逐过程(&O)";
            this.逐过程OToolStripMenuItem.Click += new System.EventHandler(this.逐过程OToolStripMenuItem_Click);
            // 
            // 跳出JToolStripMenuItem
            // 
            this.跳出JToolStripMenuItem.Name = "跳出JToolStripMenuItem";
            this.跳出JToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F11)));
            this.跳出JToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.跳出JToolStripMenuItem.Text = "跳出(&J)";
            this.跳出JToolStripMenuItem.Click += new System.EventHandler(this.跳出JToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(192, 6);
            // 
            // 切换断点BToolStripMenuItem
            // 
            this.切换断点BToolStripMenuItem.Name = "切换断点BToolStripMenuItem";
            this.切换断点BToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.切换断点BToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.切换断点BToolStripMenuItem.Text = "切换断点(&B)";
            this.切换断点BToolStripMenuItem.Click += new System.EventHandler(this.切换断点BToolStripMenuItem_Click);
            // 
            // 帮助HToolStripMenuItem
            // 
            this.帮助HToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查看帮助VToolStripMenuItem});
            this.帮助HToolStripMenuItem.Name = "帮助HToolStripMenuItem";
            this.帮助HToolStripMenuItem.Size = new System.Drawing.Size(61, 22);
            this.帮助HToolStripMenuItem.Text = "帮助(&H)";
            // 
            // 查看帮助VToolStripMenuItem
            // 
            this.查看帮助VToolStripMenuItem.Name = "查看帮助VToolStripMenuItem";
            this.查看帮助VToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.查看帮助VToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.查看帮助VToolStripMenuItem.Text = "查看帮助(&V)...";
            this.查看帮助VToolStripMenuItem.Click += new System.EventHandler(this.查看帮助VToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(533, 410);
            this.tabControl1.TabIndex = 3;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            this.tabControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabControl1_MouseClick);
            // 
            // FrmMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 434);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.Name = "FrmMain";
            this.Text = "MIPS汇编器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FrmMain_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FrmMain_DragEnter);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新建NToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开OToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 保存SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 另存为AToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 退出QToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 调试DToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 开始执行SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 停止执行XToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem 逐指令IToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 逐语句SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 逐过程OToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 跳出JToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem 切换断点BToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Addr;
        private System.Windows.Forms.DataGridViewTextBoxColumn Data;
        private System.Windows.Forms.ToolStripMenuItem 编辑EToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 格式化代码FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 暂停PToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助HToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看帮助VToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 生成BToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem intelHex文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 二进制机器码BToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 十六进制机器码HToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem 原始汇编AToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem 下载DToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ToolStripMenuItem 关闭CToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全部保存LToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全部关闭WToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
    }
}

