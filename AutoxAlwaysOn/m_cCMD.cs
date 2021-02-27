using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AutoxAlwaysOn
{
    public class m_cCMD
    {
        private static bool m_fUse(string m_sCmd, bool m_bIsWaitForExit = true, bool m_bLog = false)
        {
            var info = new ProcessStartInfo();
            info.FileName = string.Format("\"{0}\"", "cmd.exe");
            info.WorkingDirectory = m_cCMD.m_fWhere;
            info.Arguments = m_sCmd;

            info.RedirectStandardInput = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.UseShellExecute = false;
            info.CreateNoWindow = true;

            string m_sData = "\r\n";

            using (var proc = new Process())
            {
                proc.StartInfo = info;
                proc.Start();

                proc.OutputDataReceived += (a, b) =>
                {
                    if (b != null && b.Data != null)
                    {
                        Log.Instance.Debug($"OutputDataReceived:{b.Data}");
                    }
                };

                proc.ErrorDataReceived += (a, b) =>
                {
                    if (b != null && b.Data != null)
                    {
                        //m_sData += $"{b.Data}\r\n";
                        Log.Instance.Debug($"ErrorDataReceived:{b.Data}");
                    }
                };
                proc.BeginErrorReadLine();
                if (m_bIsWaitForExit)
                {
                    proc.WaitForExit(1000 * 15);
                }
                else
                {
                    proc.WaitForExit();
                }
            }

            return true;
        }

        private static string m_fWhere
        {
            get
            {
                return System.AppDomain.CurrentDomain.BaseDirectory;
            }
        }
    }
}
