using System.IO;
using System.Reflection;
using System.Windows.Forms;
using HeyRed.MarkdownSharp;

namespace AssemblerGui
{
    public partial class FrmHelp : Form
    {
        private static FrmHelp m_Instance;

        public static void ShowHelp(IWin32Window owner)
        {
            if (m_Instance == null ||
                m_Instance.IsDisposed)
                m_Instance = new FrmHelp();

            if (!m_Instance.Visible)
                m_Instance.Show(owner);
            else
                m_Instance.Focus();
        }

        private FrmHelp()
        {
            InitializeComponent();

            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "AssemblerGui.Resources.AssemblerGui.md";

            string result;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
                // ReSharper disable once AssignNullToNotNullAttribute
            using (var reader = new StreamReader(stream))
                result = reader.ReadToEnd();

            var md = new Markdown();
            webBrowser1.DocumentText = md.Transform(result);
        }
    }
}
