namespace AssemblerGui
{
    partial class frmMain
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
            this.scintilla = new ScintillaNET.Scintilla();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.调试DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新建NToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开OToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.另存为AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.导出EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.intelHex文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.二进制机器码BToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.十六进制机器码HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.退出QToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开始执行SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.停止执行XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.逐语句SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.逐过程OToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.跳出JToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.逐指令IToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.切换断点BToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // scintilla
            // 
            this.scintilla.CaretPeriod = 200;
            this.scintilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla.EndAtLastLine = false;
            this.scintilla.Lexer = ScintillaNET.Lexer.Asm;
            this.scintilla.Location = new System.Drawing.Point(0, 25);
            this.scintilla.Margin = new System.Windows.Forms.Padding(2);
            this.scintilla.Name = "scintilla";
            this.scintilla.Size = new System.Drawing.Size(743, 560);
            this.scintilla.TabIndex = 0;
            this.scintilla.TabStop = false;
            this.scintilla.WrapMode = ScintillaNET.WrapMode.Char;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(743, 25);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(190, 560);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.tableLayoutPanel2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 64);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(190, 496);
            this.panel2.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(190, 64);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(190, 64);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件FToolStripMenuItem,
            this.调试DToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(933, 25);
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
            this.另存为AToolStripMenuItem,
            this.toolStripMenuItem2,
            this.导出EToolStripMenuItem,
            this.toolStripMenuItem3,
            this.退出QToolStripMenuItem});
            this.文件FToolStripMenuItem.Name = "文件FToolStripMenuItem";
            this.文件FToolStripMenuItem.Size = new System.Drawing.Size(58, 21);
            this.文件FToolStripMenuItem.Text = "文件(&F)";
            // 
            // 调试DToolStripMenuItem
            // 
            this.调试DToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.开始执行SToolStripMenuItem,
            this.停止执行XToolStripMenuItem,
            this.toolStripMenuItem4,
            this.逐指令IToolStripMenuItem,
            this.逐语句SToolStripMenuItem,
            this.逐过程OToolStripMenuItem,
            this.跳出JToolStripMenuItem,
            this.toolStripMenuItem5,
            this.切换断点BToolStripMenuItem});
            this.调试DToolStripMenuItem.Name = "调试DToolStripMenuItem";
            this.调试DToolStripMenuItem.Size = new System.Drawing.Size(61, 21);
            this.调试DToolStripMenuItem.Text = "调试(&D)";
            // 
            // 新建NToolStripMenuItem
            // 
            this.新建NToolStripMenuItem.Name = "新建NToolStripMenuItem";
            this.新建NToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.新建NToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.新建NToolStripMenuItem.Text = "新建(&N)";
            // 
            // 打开OToolStripMenuItem
            // 
            this.打开OToolStripMenuItem.Name = "打开OToolStripMenuItem";
            this.打开OToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.打开OToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.打开OToolStripMenuItem.Text = "打开(&O)...";
            // 
            // 保存SToolStripMenuItem
            // 
            this.保存SToolStripMenuItem.Name = "保存SToolStripMenuItem";
            this.保存SToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.保存SToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.保存SToolStripMenuItem.Text = "保存(S)";
            // 
            // 另存为AToolStripMenuItem
            // 
            this.另存为AToolStripMenuItem.Name = "另存为AToolStripMenuItem";
            this.另存为AToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.另存为AToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.另存为AToolStripMenuItem.Text = "另存为(A)...";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(149, 6);
            // 
            // 导出EToolStripMenuItem
            // 
            this.导出EToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.intelHex文件ToolStripMenuItem,
            this.二进制机器码BToolStripMenuItem,
            this.十六进制机器码HToolStripMenuItem});
            this.导出EToolStripMenuItem.Name = "导出EToolStripMenuItem";
            this.导出EToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.导出EToolStripMenuItem.Text = "导出(&E)";
            // 
            // intelHex文件ToolStripMenuItem
            // 
            this.intelHex文件ToolStripMenuItem.Name = "intelHex文件ToolStripMenuItem";
            this.intelHex文件ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.intelHex文件ToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.intelHex文件ToolStripMenuItem.Text = "&Intel Hex文件...";
            // 
            // 二进制机器码BToolStripMenuItem
            // 
            this.二进制机器码BToolStripMenuItem.Name = "二进制机器码BToolStripMenuItem";
            this.二进制机器码BToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.二进制机器码BToolStripMenuItem.Text = "二进制机器码(&B)...";
            // 
            // 十六进制机器码HToolStripMenuItem
            // 
            this.十六进制机器码HToolStripMenuItem.Name = "十六进制机器码HToolStripMenuItem";
            this.十六进制机器码HToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.十六进制机器码HToolStripMenuItem.Text = "十六进制机器码(&H)...";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(149, 6);
            // 
            // 退出QToolStripMenuItem
            // 
            this.退出QToolStripMenuItem.Name = "退出QToolStripMenuItem";
            this.退出QToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.退出QToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.退出QToolStripMenuItem.Text = "退出(&Q)";
            // 
            // 开始执行SToolStripMenuItem
            // 
            this.开始执行SToolStripMenuItem.Name = "开始执行SToolStripMenuItem";
            this.开始执行SToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.开始执行SToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.开始执行SToolStripMenuItem.Text = "开始执行(&R)";
            // 
            // 停止执行XToolStripMenuItem
            // 
            this.停止执行XToolStripMenuItem.Name = "停止执行XToolStripMenuItem";
            this.停止执行XToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F5)));
            this.停止执行XToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.停止执行XToolStripMenuItem.Text = "停止执行(&X)";
            // 
            // 逐语句SToolStripMenuItem
            // 
            this.逐语句SToolStripMenuItem.Name = "逐语句SToolStripMenuItem";
            this.逐语句SToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.逐语句SToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.逐语句SToolStripMenuItem.Text = "逐语句(&S)";
            // 
            // 逐过程OToolStripMenuItem
            // 
            this.逐过程OToolStripMenuItem.Name = "逐过程OToolStripMenuItem";
            this.逐过程OToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.逐过程OToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.逐过程OToolStripMenuItem.Text = "逐过程(&O)";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(158, 6);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(158, 6);
            // 
            // 跳出JToolStripMenuItem
            // 
            this.跳出JToolStripMenuItem.Name = "跳出JToolStripMenuItem";
            this.跳出JToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F11)));
            this.跳出JToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.跳出JToolStripMenuItem.Text = "跳出(&J)";
            // 
            // 逐指令IToolStripMenuItem
            // 
            this.逐指令IToolStripMenuItem.Name = "逐指令IToolStripMenuItem";
            this.逐指令IToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F11)));
            this.逐指令IToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.逐指令IToolStripMenuItem.Text = "逐指令(&I)";
            // 
            // 切换断点BToolStripMenuItem
            // 
            this.切换断点BToolStripMenuItem.Name = "切换断点BToolStripMenuItem";
            this.切换断点BToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.切换断点BToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.切换断点BToolStripMenuItem.Text = "切换断点(&B)";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 585);
            this.Controls.Add(this.scintilla);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmMain";
            this.Text = "MIPS汇编器";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ScintillaNET.Scintilla scintilla;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新建NToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开OToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 保存SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 另存为AToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 导出EToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem intelHex文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 二进制机器码BToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 十六进制机器码HToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
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
    }
}

