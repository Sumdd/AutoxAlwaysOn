using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AutoxAlwaysOn
{
    public class m_cBAT
    {
        public static bool m_fUse(string m_sBatAndArguments, bool m_bIsWaitForExit = true, bool m_bLog = false)
        {
            var m_bOK = true;

            var info = new ProcessStartInfo();

            #region ***组织参数
            string[] m_lBatAndArguments = m_sBatAndArguments.Split('|');
            info.FileName = Path.Combine(m_lBatAndArguments[0]);
            if (m_lBatAndArguments.Count() > 1)
            {
                for (int i = 1; i < m_lBatAndArguments.Length; i++)
                {
                    string argument = m_lBatAndArguments[i];
                    if (argument.StartsWith("Code:"))
                    {
                        argument = m_cCode.m_fGetString(argument);
                    }
                    if (!string.IsNullOrWhiteSpace(info.Arguments))
                    {
                        info.Arguments = $"{info.Arguments} {argument.Replace("Code:", "")}";
                    }
                    else info.Arguments = argument;
                }
            }
            #endregion
            info.RedirectStandardInput = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.UseShellExecute = false;
            info.CreateNoWindow = true;

            ///string m_sOutputDataReceivedData = "\r\n";
            string m_sErrorDataReceivedData = "\r\n";

            using (var proc = new Process())
            {
                proc.StartInfo = info;
                proc.Start();

                ///基本输出
                proc.OutputDataReceived += (a, b) =>
                {
                    //if (b != null && b.Data != null)
                    //{
                    //    m_sOutputDataReceivedData += "\r\n" + b.Data;
                    //}
                };
                proc.BeginOutputReadLine();
                ///错误输出
                proc.ErrorDataReceived += (a, b) =>
                {
                    if (b != null && b.Data != null)
                    {
                        m_sErrorDataReceivedData += "\r\n" + b.Data;
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

                ///是否成功
                m_bOK = proc.ExitCode == 0;
                if (!m_bOK) Log.Instance.Error(m_sErrorDataReceivedData);
            }

            return m_bOK;
        }
    }
}
