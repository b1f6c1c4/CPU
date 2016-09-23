using System.Runtime.InteropServices;
using System.Windows.Forms;
using Assembler;

namespace AssemblerGui
{
    public partial class FrmMain : Form
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        private Context m_CPU;

        public FrmMain()
        {
            SetProcessDPIAware();
            InitializeComponent();

            SetupEditor();
            SetupDebugger();

            UpdateTitle();
        }

        private void UpdateTitle() =>
            Text = m_Debugging
                       ? $"MIPS调试器 - [{m_FileName}]"
                       : $"MIPS编辑器 - [{m_FileName}]{(m_Edited ? "*" : "")}";

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!PromptForSave())
                e.Cancel = true;
        }
    }
}
