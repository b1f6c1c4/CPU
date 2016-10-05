using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace AssemblerGui
{
    internal static class Program
    {
        /// <summary>
        ///     应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new FrmMain());
            }
            catch (Exception e)
            {
                MessageBox.Show(
                                @"发生严重错误：" +
                                e +
                                Environment.NewLine +
                                @"请将错误反馈给程序作者，谢谢！",
                                "严重错误",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                var sb = new StringBuilder();
                var sb1 = new StringBuilder();
                sb.Append("mailto:b1f6c1c4@gmail.com");
                sb.Append("?subject=MIPS%20Assembler%20Bug%20Report");
                sb.Append("&body=");
                sb1.Append("Version ");
                sb1.AppendLine(Application.ProductVersion);
                sb1.AppendLine("Error:");
                sb1.AppendLine(e.ToString());
                sb.Append(WebUtility.UrlEncode(sb1.ToString()));
                Process.Start(sb.ToString());
            }
        }
    }
}
