using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AutoxAlwaysOn
{
    /// <summary>
    /// 自动
    /// </summary>
    public class m_cAuto
    {
        private static Timer m_pTimer = null;

        #region ***开启
        public static void m_fStart()
        {
            try
            {
                if (m_cAuto.m_pTimer == null)
                    m_cAuto.m_pTimer = new Timer();

                ///定时器秒数
                int m_uSeconds = 0;
                int.TryParse(m_cSettings.m_dKeyValue["m_uSeconds"], out m_uSeconds);
                if (m_uSeconds <= 0) m_uSeconds = 15;

                m_cAuto.m_pTimer.Interval = 1000 * m_uSeconds;
                m_cAuto.m_pTimer.Elapsed += new ElapsedEventHandler(m_cAuto.m_fTimerElapsed);
                Log.Instance.Success($"[AutoxAlwaysOn][m_cAuto][m_fStart][auto timer start]");

                new System.Threading.Thread(() =>
                {
                    m_cAuto.m_fTimerElapsed(null, null);

                }).Start();
            }
            catch (Exception ex)
            {
                Log.Instance.Error($"[AutoxAlwaysOn][m_cAuto][m_fStart][Exception][{ex.Message}]");
            }
        }
        #endregion

        #region ***重启
        public static void m_fRestart()
        {
            m_cAuto.m_fStop();
            m_cAuto.m_fStart();
            Log.Instance.Success($"[AutoxAlwaysOn][m_cAuto][m_fRestart][auto timer restart]");
        }
        #endregion

        #region ***停止
        public static void m_fStop()
        {
            try
            {
                if (m_cAuto.m_pTimer != null)
                {
                    m_cAuto.m_pTimer.Stop();
                    m_cAuto.m_pTimer.Dispose();
                    m_cAuto.m_pTimer = null;
                }
                Log.Instance.Success($"[AutoxAlwaysOn][m_cAuto][m_fStop][auto timer stop]");
            }
            catch (Exception ex)
            {
                Log.Instance.Error($"[AutoxAlwaysOn][m_cAuto][m_fStop][Exception][{ex.Message}]");
            }
        }
        #endregion

        #region ***计时器动作
        /// <summary>
        /// 计时器动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void m_fTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                //先停止计时器
                m_cAuto.m_pTimer.Stop();
                //执行同步操作
                {
                    if (m_cSettings.m_lHTTP != null)
                    {
                        foreach (m_mHTTP item in m_cSettings.m_lHTTP)
                        {
                            try
                            {
                                ///是否到时间
                                if (DateTime.Compare(item.m_dtLastResq.AddSeconds(item.m_sSeconds), DateTime.Now) <= 0)
                                {
                                    string m_sRespStr = m_cHttp.HttpGet(item.m_sHttp);
                                    item.m_dtLastResq = DateTime.Now.AddSeconds(item.m_sSeconds);
                                    Log.Instance.Success($"[AutoxAlwaysOn][m_cAuto][m_fTimerElapsed][foreach.m_mHTTP][{item.m_sHttp}:{m_sRespStr}]");
                                }
                                else
                                {
                                    Log.Instance.Warn($"[AutoxAlwaysOn][m_cAuto][m_fTimerElapsed][foreach.m_mHTTP][not send:{item.m_sHttp},sec:{item.m_sSeconds},last:{item.m_dtLastResq.ToString("yyyy-MM-dd HH:mm:ss")}]");
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Instance.Error($"[AutoxAlwaysOn][m_cAuto][m_fTimerElapsed][foreach.m_mHTTP][Exception][{ex.Message}]");
                            }
                        }
                    }
                }
                //再启动
                m_cAuto.m_pTimer.Start();
            }
            catch (Exception ex)
            {
                Log.Instance.Error($"[AutoxAlwaysOn][m_cAuto][m_fTimerElapsed][Exception][{ex.Message}]");
                Log.Instance.Debug(ex);
                System.Threading.Thread.Sleep(1000 * 10);
                Log.Instance.Error($"[AutoxAlwaysOn][m_cAuto][m_fTimerElapsed][Exception][自动外呼同步计时器遇到错误10秒后重启]");
                m_cAuto.m_fRestart();
            }
        }
        #endregion
    }
}
