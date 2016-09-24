using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AssemblerGui
{
    public class Downloader
    {
        public delegate void ExitedEventHandler(string message);

        public event ExitedEventHandler OnExited;

        private const string TclScript = @"
set hws [get_hardware_names]
set hw  [lindex $hws 0]
set dvs [get_device_names -hardware_name $hw]
set dv  [lindex $dvs 0]
begin_memory_edit -hardware_name $hw -device_name $dv
update_content_to_memory_from_file -instance_index 0 -mem_file_path ""{0}"" -mem_file_type hex
end_memory_edit
";

        private readonly string m_HexPath;

        private string m_ScriptPath;

        private StringBuilder m_Result;

        private StringBuilder m_ResultErr;

        private Process m_Process;

        private int m_Retry;

        public Downloader(string hexPath) { m_HexPath = hexPath; }

        public void Start()
        {
            m_Retry = 1;
            m_ScriptPath = Path.GetTempFileName();
            File.WriteAllText(m_ScriptPath, string.Format(TclScript, m_HexPath.Replace(@"\", @"\\")));
            StartDownload();
        }

        private void StartDownload()
        {
            m_Result = new StringBuilder();
            m_ResultErr = new StringBuilder();
            m_Process =
                new Process
                    {
                        StartInfo =
                            {
                                FileName = @"C:\altera\13.0sp1\quartus\bin64\quartus_stp.exe",
                                Arguments = $"-t {m_ScriptPath}",
                                UseShellExecute = false,
                                RedirectStandardError = true,
                                RedirectStandardOutput = true,
                                CreateNoWindow = true
                            },
                        EnableRaisingEvents = true
                    };
            m_Process.OutputDataReceived += DataReceived;
            m_Process.ErrorDataReceived += ErrorReceived;
            m_Process.Exited += Exited;
            m_Process.Start();
            m_Process.BeginOutputReadLine();
            m_Process.BeginErrorReadLine();
        }

        private void Exited(object sender, EventArgs e)
        {
            if (m_Process.ExitCode != 0)
            {
                var message = m_Result.ToString() + m_ResultErr;
                if (message.Contains("The modifiable node at index"))
                    if (m_Retry-- > 0)
                    {
                        StartDownload();
                        return;
                    }
                OnExited?.Invoke(message);
            }
            else
                OnExited?.Invoke(null);

            m_Result = null;
            m_ResultErr = null;
            m_Process = null;
            File.Delete(m_ScriptPath);
        }

        private void DataReceived(object sender, DataReceivedEventArgs e) => m_Result.AppendLine(e.Data);

        private void ErrorReceived(object sender, DataReceivedEventArgs e) => m_ResultErr.AppendLine(e.Data);
    }
}
