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
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.启用长跳转LToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.启用扩展指令EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.生成BToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.文件末尾自动停机HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
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
            this.tabControl1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(19, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(1008, 26);
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
            this.文件FToolStripMenuItem.Size = new System.Drawing.Size(69, 24);
            this.文件FToolStripMenuItem.Text = "文件(&F)";
            // 
            // 新建NToolStripMenuItem
            // 
            this.新建NToolStripMenuItem.Name = "新建NToolStripMenuItem";
            this.新建NToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.新建NToolStripMenuItem.Size = new System.Drawing.Size(261, 26);
            this.新建NToolStripMenuItem.Text = "新建(&N)";
            this.新建NToolStripMenuItem.Click += new System.EventHandler(this.新建NToolStripMenuItem_Click);
            // 
            // 打开OToolStripMenuItem
            // 
            this.打开OToolStripMenuItem.Name = "打开OToolStripMenuItem";
            this.打开OToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.打开OToolStripMenuItem.Size = new System.Drawing.Size(261, 26);
            this.打开OToolStripMenuItem.Text = "打开(&O)...";
            this.打开OToolStripMenuItem.Click += new System.EventHandler(this.打开OToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(258, 6);
            // 
            // 保存SToolStripMenuItem
            // 
            this.保存SToolStripMenuItem.Name = "保存SToolStripMenuItem";
            this.保存SToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.保存SToolStripMenuItem.Size = new System.Drawing.Size(261, 26);
            this.保存SToolStripMenuItem.Text = "保存(S)";
            this.保存SToolStripMenuItem.Click += new System.EventHandler(this.保存SToolStripMenuItem_Click);
            // 
            // 全部保存LToolStripMenuItem
            // 
            this.全部保存LToolStripMenuItem.Name = "全部保存LToolStripMenuItem";
            this.全部保存LToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.全部保存LToolStripMenuItem.Size = new System.Drawing.Size(261, 26);
            this.全部保存LToolStripMenuItem.Text = "全部保存(&L)";
            this.全部保存LToolStripMenuItem.Click += new System.EventHandler(this.全部保存LToolStripMenuItem_Click);
            // 
            // 另存为AToolStripMenuItem
            // 
            this.另存为AToolStripMenuItem.Name = "另存为AToolStripMenuItem";
            this.另存为AToolStripMenuItem.Size = new System.Drawing.Size(261, 26);
            this.另存为AToolStripMenuItem.Text = "另存为(A)...";
            this.另存为AToolStripMenuItem.Click += new System.EventHandler(this.另存为AToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(258, 6);
            // 
            // 关闭CToolStripMenuItem
            // 
            this.关闭CToolStripMenuItem.Name = "关闭CToolStripMenuItem";
            this.关闭CToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.关闭CToolStripMenuItem.Size = new System.Drawing.Size(261, 26);
            this.关闭CToolStripMenuItem.Text = "关闭(&C)";
            this.关闭CToolStripMenuItem.Click += new System.EventHandler(this.关闭CToolStripMenuItem_Click);
            // 
            // 全部关闭WToolStripMenuItem
            // 
            this.全部关闭WToolStripMenuItem.Name = "全部关闭WToolStripMenuItem";
            this.全部关闭WToolStripMenuItem.Size = new System.Drawing.Size(261, 26);
            this.全部关闭WToolStripMenuItem.Text = "全部关闭(&W)";
            this.全部关闭WToolStripMenuItem.Click += new System.EventHandler(this.全部关闭WToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(258, 6);
            // 
            // 退出QToolStripMenuItem
            // 
            this.退出QToolStripMenuItem.Name = "退出QToolStripMenuItem";
            this.退出QToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.退出QToolStripMenuItem.Size = new System.Drawing.Size(261, 26);
            this.退出QToolStripMenuItem.Text = "退出(&Q)";
            this.退出QToolStripMenuItem.Click += new System.EventHandler(this.退出QToolStripMenuItem_Click);
            // 
            // 编辑EToolStripMenuItem
            // 
            this.编辑EToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.格式化代码FToolStripMenuItem,
            this.toolStripMenuItem8,
            this.启用长跳转LToolStripMenuItem,
            this.启用扩展指令EToolStripMenuItem});
            this.编辑EToolStripMenuItem.Name = "编辑EToolStripMenuItem";
            this.编辑EToolStripMenuItem.Size = new System.Drawing.Size(69, 24);
            this.编辑EToolStripMenuItem.Text = "编辑(&E)";
            // 
            // 格式化代码FToolStripMenuItem
            // 
            this.格式化代码FToolStripMenuItem.Name = "格式化代码FToolStripMenuItem";
            this.格式化代码FToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.F)));
            this.格式化代码FToolStripMenuItem.Size = new System.Drawing.Size(263, 26);
            this.格式化代码FToolStripMenuItem.Text = "格式化代码(&F)";
            this.格式化代码FToolStripMenuItem.Click += new System.EventHandler(this.格式化代码FToolStripMenuItem_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(260, 6);
            // 
            // 启用长跳转LToolStripMenuItem
            // 
            this.启用长跳转LToolStripMenuItem.CheckOnClick = true;
            this.启用长跳转LToolStripMenuItem.Name = "启用长跳转LToolStripMenuItem";
            this.启用长跳转LToolStripMenuItem.Size = new System.Drawing.Size(263, 26);
            this.启用长跳转LToolStripMenuItem.Text = "启用长跳转(&L)";
            this.启用长跳转LToolStripMenuItem.CheckedChanged += new System.EventHandler(this.启用长跳转LToolStripMenuItem_CheckedChanged);
            // 
            // 启用扩展指令EToolStripMenuItem
            // 
            this.启用扩展指令EToolStripMenuItem.CheckOnClick = true;
            this.启用扩展指令EToolStripMenuItem.Name = "启用扩展指令EToolStripMenuItem";
            this.启用扩展指令EToolStripMenuItem.Size = new System.Drawing.Size(263, 26);
            this.启用扩展指令EToolStripMenuItem.Text = "启用扩展指令(&E)";
            this.启用扩展指令EToolStripMenuItem.CheckedChanged += new System.EventHandler(this.启用扩展指令EToolStripMenuItem_CheckedChanged);
            // 
            // 生成BToolStripMenuItem
            // 
            this.生成BToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件末尾自动停机HToolStripMenuItem,
            this.toolStripMenuItem9,
            this.intelHex文件ToolStripMenuItem,
            this.二进制机器码BToolStripMenuItem,
            this.十六进制机器码HToolStripMenuItem,
            this.toolStripMenuItem3,
            this.原始汇编AToolStripMenuItem,
            this.toolStripMenuItem6,
            this.下载DToolStripMenuItem});
            this.生成BToolStripMenuItem.Name = "生成BToolStripMenuItem";
            this.生成BToolStripMenuItem.Size = new System.Drawing.Size(70, 24);
            this.生成BToolStripMenuItem.Text = "生成(&B)";
            // 
            // 文件末尾自动停机HToolStripMenuItem
            // 
            this.文件末尾自动停机HToolStripMenuItem.CheckOnClick = true;
            this.文件末尾自动停机HToolStripMenuItem.Name = "文件末尾自动停机HToolStripMenuItem";
            this.文件末尾自动停机HToolStripMenuItem.Size = new System.Drawing.Size(245, 26);
            this.文件末尾自动停机HToolStripMenuItem.Text = "文件末尾自动停机(&H)";
            this.文件末尾自动停机HToolStripMenuItem.Click += new System.EventHandler(this.文件末尾自动停机HToolStripMenuItem_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(242, 6);
            // 
            // intelHex文件ToolStripMenuItem
            // 
            this.intelHex文件ToolStripMenuItem.Name = "intelHex文件ToolStripMenuItem";
            this.intelHex文件ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.intelHex文件ToolStripMenuItem.Size = new System.Drawing.Size(245, 26);
            this.intelHex文件ToolStripMenuItem.Text = "&Intel Hex文件...";
            this.intelHex文件ToolStripMenuItem.Click += new System.EventHandler(this.intelHex文件ToolStripMenuItem_Click);
            // 
            // 二进制机器码BToolStripMenuItem
            // 
            this.二进制机器码BToolStripMenuItem.Name = "二进制机器码BToolStripMenuItem";
            this.二进制机器码BToolStripMenuItem.Size = new System.Drawing.Size(245, 26);
            this.二进制机器码BToolStripMenuItem.Text = "二进制机器码(&B)...";
            this.二进制机器码BToolStripMenuItem.Click += new System.EventHandler(this.二进制机器码BToolStripMenuItem_Click);
            // 
            // 十六进制机器码HToolStripMenuItem
            // 
            this.十六进制机器码HToolStripMenuItem.Name = "十六进制机器码HToolStripMenuItem";
            this.十六进制机器码HToolStripMenuItem.Size = new System.Drawing.Size(245, 26);
            this.十六进制机器码HToolStripMenuItem.Text = "十六进制机器码(&H)...";
            this.十六进制机器码HToolStripMenuItem.Click += new System.EventHandler(this.十六进制机器码HToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(242, 6);
            // 
            // 原始汇编AToolStripMenuItem
            // 
            this.原始汇编AToolStripMenuItem.Name = "原始汇编AToolStripMenuItem";
            this.原始汇编AToolStripMenuItem.Size = new System.Drawing.Size(245, 26);
            this.原始汇编AToolStripMenuItem.Text = "原始汇编(&A)...";
            this.原始汇编AToolStripMenuItem.Click += new System.EventHandler(this.原始汇编AToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(242, 6);
            // 
            // 下载DToolStripMenuItem
            // 
            this.下载DToolStripMenuItem.Name = "下载DToolStripMenuItem";
            this.下载DToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.下载DToolStripMenuItem.Size = new System.Drawing.Size(245, 26);
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
            this.调试DToolStripMenuItem.Size = new System.Drawing.Size(72, 24);
            this.调试DToolStripMenuItem.Text = "调试(&D)";
            // 
            // 开始执行SToolStripMenuItem
            // 
            this.开始执行SToolStripMenuItem.Name = "开始执行SToolStripMenuItem";
            this.开始执行SToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.开始执行SToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.开始执行SToolStripMenuItem.Text = "开始执行(&R)";
            this.开始执行SToolStripMenuItem.Click += new System.EventHandler(this.开始执行SToolStripMenuItem_Click);
            // 
            // 暂停PToolStripMenuItem
            // 
            this.暂停PToolStripMenuItem.Name = "暂停PToolStripMenuItem";
            this.暂停PToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.暂停PToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.暂停PToolStripMenuItem.Text = "暂停(&P)";
            this.暂停PToolStripMenuItem.Click += new System.EventHandler(this.暂停PToolStripMenuItem_Click);
            // 
            // 停止执行XToolStripMenuItem
            // 
            this.停止执行XToolStripMenuItem.Name = "停止执行XToolStripMenuItem";
            this.停止执行XToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F5)));
            this.停止执行XToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.停止执行XToolStripMenuItem.Text = "停止执行(&X)";
            this.停止执行XToolStripMenuItem.Click += new System.EventHandler(this.停止执行XToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(231, 6);
            // 
            // 逐指令IToolStripMenuItem
            // 
            this.逐指令IToolStripMenuItem.Name = "逐指令IToolStripMenuItem";
            this.逐指令IToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F11)));
            this.逐指令IToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.逐指令IToolStripMenuItem.Text = "逐指令(&I)";
            this.逐指令IToolStripMenuItem.Click += new System.EventHandler(this.逐指令IToolStripMenuItem_Click);
            // 
            // 逐语句SToolStripMenuItem
            // 
            this.逐语句SToolStripMenuItem.Name = "逐语句SToolStripMenuItem";
            this.逐语句SToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.逐语句SToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.逐语句SToolStripMenuItem.Text = "逐语句(&S)";
            this.逐语句SToolStripMenuItem.Click += new System.EventHandler(this.逐语句SToolStripMenuItem_Click);
            // 
            // 逐过程OToolStripMenuItem
            // 
            this.逐过程OToolStripMenuItem.Name = "逐过程OToolStripMenuItem";
            this.逐过程OToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.逐过程OToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.逐过程OToolStripMenuItem.Text = "逐过程(&O)";
            this.逐过程OToolStripMenuItem.Click += new System.EventHandler(this.逐过程OToolStripMenuItem_Click);
            // 
            // 跳出JToolStripMenuItem
            // 
            this.跳出JToolStripMenuItem.Name = "跳出JToolStripMenuItem";
            this.跳出JToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F11)));
            this.跳出JToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.跳出JToolStripMenuItem.Text = "跳出(&J)";
            this.跳出JToolStripMenuItem.Click += new System.EventHandler(this.跳出JToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(231, 6);
            // 
            // 切换断点BToolStripMenuItem
            // 
            this.切换断点BToolStripMenuItem.Name = "切换断点BToolStripMenuItem";
            this.切换断点BToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.切换断点BToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.切换断点BToolStripMenuItem.Text = "切换断点(&B)";
            this.切换断点BToolStripMenuItem.Click += new System.EventHandler(this.切换断点BToolStripMenuItem_Click);
            // 
            // 帮助HToolStripMenuItem
            // 
            this.帮助HToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查看帮助VToolStripMenuItem});
            this.帮助HToolStripMenuItem.Name = "帮助HToolStripMenuItem";
            this.帮助HToolStripMenuItem.Size = new System.Drawing.Size(73, 24);
            this.帮助HToolStripMenuItem.Text = "帮助(&H)";
            // 
            // 查看帮助VToolStripMenuItem
            // 
            this.查看帮助VToolStripMenuItem.Name = "查看帮助VToolStripMenuItem";
            this.查看帮助VToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.查看帮助VToolStripMenuItem.Size = new System.Drawing.Size(202, 26);
            this.查看帮助VToolStripMenuItem.Text = "查看帮助(&V)...";
            this.查看帮助VToolStripMenuItem.Click += new System.EventHandler(this.查看帮助VToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.AllowDrop = true;
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.tabControl1.Location = new System.Drawing.Point(0, 26);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Size = new System.Drawing.Size(1008, 544);
            this.tabControl1.TabIndex = 4;
            this.tabControl1.ActiveDocumentChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabControl1.DragDrop += new System.Windows.Forms.DragEventHandler(this.FrmMain_DragDrop);
            this.tabControl1.DragEnter += new System.Windows.Forms.DragEventHandler(this.FrmMain_DragEnter);
            // 
            // FrmMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 570);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.Name = "FrmMain";
            this.Text = "MIPS汇编器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FrmMain_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FrmMain_DragEnter);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
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
        private System.Windows.Forms.ToolStripMenuItem 关闭CToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全部保存LToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全部关闭WToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private WeifenLuo.WinFormsUI.Docking.DockPanel tabControl1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem 启用长跳转LToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 启用扩展指令EToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 文件末尾自动停机HToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
    }
}

