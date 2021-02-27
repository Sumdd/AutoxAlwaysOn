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
        private static Timer m_pHTTPTimer = null;
        private static Timer m_pCMDTimer = null;
        private static Timer m_pBATTimer = null;

        #region ***HTTP已完成无需改动

        #region ***HTTP开启
        public static void m_fHTTPStart()
        {
            try
            {
                ///实例化
                if (m_cAuto.m_pHTTPTimer == null) m_cAuto.m_pHTTPTimer = new Timer();

                ///定时器秒数
                int m_uSeconds = 0;
                int.TryParse(m_cSettings.m_dKeyValue["m_uSeconds"], out m_uSeconds);
                if (m_uSeconds <= 0) m_uSeconds = 15;

                m_cAuto.m_pHTTPTimer.Interval = 1000 * m_uSeconds;
                m_cAuto.m_pHTTPTimer.Elapsed += new ElapsedEventHandler(m_cAuto.m_fHTTPTimerElapsed);
                Log.Instance.Success($"[AutoxAlwaysOn][m_cAuto][m_fHTTPStart][auto timer start]");

                new System.Threading.Thread(() =>
                {
                    m_cAuto.m_fHTTPTimerElapsed(null, null);

                }).Start();
            }
            catch (Exception ex)
            {
                Log.Instance.Error($"[AutoxAlwaysOn][m_cAuto][m_fHTTPStart][Exception][{ex.Message}]");
            }
        }
        #endregion

        #region ***HTTP停止
        public static void m_fHTTPStop()
        {
            try
            {
                if (m_cAuto.m_pHTTPTimer != null)
                {
                    m_cAuto.m_pHTTPTimer.Stop();
                    m_cAuto.m_pHTTPTimer.Dispose();
                    m_cAuto.m_pHTTPTimer = null;
                }
                Log.Instance.Success($"[AutoxAlwaysOn][m_cAuto][m_fHTTPStop][auto timer stop]");
            }
            catch (Exception ex)
            {
                Log.Instance.Error($"[AutoxAlwaysOn][m_cAuto][m_fHTTPStop][Exception][{ex.Message}]");
            }
        }
        #endregion

        #region ***HTTP重启
        public static void m_fHTTPRestart()
        {
            m_cAuto.m_fHTTPStop();
            m_cAuto.m_fHTTPStart();
            Log.Instance.Success($"[AutoxAlwaysOn][m_cAuto][m_fHTTPRestart][auto timer restart]");
        }
        #endregion

        #region ***HTTP计时器动作
        /// <summary>
        /// 计时器动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void m_fHTTPTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                //先停止计时器
                m_cAuto.m_pHTTPTimer.Stop();
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
                                    string m_sRespStr = m_cHttp.HttpGet(item.m_sHttp, item.m_sArgs);
                                    item.m_dtLastResq = DateTime.Now.AddSeconds(item.m_sSeconds);
                                    Log.Instance.Success($"[AutoxAlwaysOn][m_cAuto][m_fHTTPTimerElapsed][foreach.m_mHTTP][{item.m_sHttp}:{m_sRespStr}]");
                                }
                                else
                                {
                                    Log.Instance.Warn($"[AutoxAlwaysOn][m_cAuto][m_fHTTPTimerElapsed][foreach.m_mHTTP][not send:{item.m_sHttp},sec:{item.m_sSeconds},last:{item.m_dtLastResq.ToString("yyyy-MM-dd HH:mm:ss")}]");
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Instance.Error($"[AutoxAlwaysOn][m_cAuto][m_fHTTPTimerElapsed][foreach.m_mHTTP][Exception][{ex.Message}]");
                            }
                        }
                    }
                }
                //再启动
                m_cAuto.m_pHTTPTimer.Start();
            }
            catch (Exception ex)
            {
                Log.Instance.Error($"[AutoxAlwaysOn][m_cAuto][m_fHTTPTimerElapsed][Exception][{ex.Message}]");
                Log.Instance.Debug(ex);
                System.Threading.Thread.Sleep(1000 * 10);
                Log.Instance.Error($"[AutoxAlwaysOn][m_cAuto][m_fHTTPTimerElapsed][Exception][自动外呼同步计时器遇到错误10秒后重启]");
                m_cAuto.m_fHTTPRestart();
            }
        }
        #endregion

        #endregion

        #region ***CMD正在做

        #region ***CMD开启
        public static void m_fCMDStart()
        {
            try
            {
                ///实例化
                if (m_cAuto.m_pCMDTimer == null) m_cAuto.m_pCMDTimer = new Timer();

                ///定时器秒数
                int m_uSeconds = 0;
                int.TryParse(m_cSettings.m_dKeyValue["m_uSeconds"], out m_uSeconds);
                if (m_uSeconds <= 0) m_uSeconds = 15;

                m_cAuto.m_pCMDTimer.Interval = 1000 * m_uSeconds;
                m_cAuto.m_pCMDTimer.Elapsed += new ElapsedEventHandler(m_cAuto.m_fCMDTimerElapsed);
                Log.Instance.Success($"[AutoxAlwaysOn][m_cAuto][m_fCMDStart][auto timer start]");

                new System.Threading.Thread(() =>
                {
                    m_cAuto.m_fCMDTimerElapsed(null, null);

                }).Start();
            }
            catch (Exception ex)
            {
                Log.Instance.Error($"[AutoxAlwaysOn][m_cAuto][m_fCMDStart][Exception][{ex.Message}]");
            }
        }
        #endregion

        #region ***CMD停止
        public static void m_fCMDStop()
        {
            try
            {
                if (m_cAuto.m_pCMDTimer != null)
                {
                    m_cAuto.m_pCMDTimer.Stop();
                    m_cAuto.m_pCMDTimer.Dispose();
                    m_cAuto.m_pCMDTimer = null;
                }
                Log.Instance.Success($"[AutoxAlwaysOn][m_cAuto][m_fCMDStop][auto timer stop]");
            }
            catch (Exception ex)
            {
                Log.Instance.Error($"[AutoxAlwaysOn][m_cAuto][m_fCMDStop][Exception][{ex.Message}]");
            }
        }
        #endregion

        #region ***CMD重启
        public static void m_fCMDRestart()
        {
            m_cAuto.m_fCMDStop();
            m_cAuto.m_fCMDStart();
            Log.Instance.Success($"[AutoxAlwaysOn][m_cAuto][m_fCMDRestart][auto timer restart]");
        }
        #endregion

        #region ***CMD计时器动作
        /// <summary>
        /// 计时器动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void m_fCMDTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                //先停止计时器
                m_cAuto.m_pCMDTimer.Stop();
                //执行同步操作
                {
                }
                //再启动
                m_cAuto.m_pCMDTimer.Start();
            }
            catch (Exception ex)
            {
                Log.Instance.Error($"[AutoxAlwaysOn][m_cAuto][m_fCMDTimerElapsed][Exception][{ex.Message}]");
                Log.Instance.Debug(ex);
                System.Threading.Thread.Sleep(1000 * 10);
                Log.Instance.Error($"[AutoxAlwaysOn][m_cAuto][m_fCMDTimerElapsed][Exception][自动外呼同步计时器遇到错误10秒后重启]");
                m_cAuto.m_fCMDRestart();
            }
        }
        #endregion

        #endregion

        #region ***BAT无正在做

        #region ***BAT开启
        public static void m_fBATStart()
        {
            try
            {
                ///实例化
                if (m_cAuto.m_pBATTimer == null) m_cAuto.m_pBATTimer = new Timer();

                ///定时器秒数
                int m_uSeconds = 0;
                int.TryParse(m_cSettings.m_dKeyValue["m_uSeconds"], out m_uSeconds);
                if (m_uSeconds <= 0) m_uSeconds = 15;

                m_cAuto.m_pBATTimer.Interval = 1000 * m_uSeconds;
                m_cAuto.m_pBATTimer.Elapsed += new ElapsedEventHandler(m_cAuto.m_fBATTimerElapsed);
                Log.Instance.Success($"[AutoxAlwaysOn][m_cAuto][m_fBATStart][auto timer start]");

                new System.Threading.Thread(() =>
                {
                    m_cAuto.m_fBATTimerElapsed(null, null);

                }).Start();
            }
            catch (Exception ex)
            {
                Log.Instance.Error($"[AutoxAlwaysOn][m_cAuto][m_fBATStart][Exception][{ex.Message}]");
            }
        }
        #endregion

        #region ***BAT停止
        public static void m_fBATStop()
        {
            try
            {
                if (m_cAuto.m_pBATTimer != null)
                {
                    m_cAuto.m_pBATTimer.Stop();
                    m_cAuto.m_pBATTimer.Dispose();
                    m_cAuto.m_pBATTimer = null;
                }
                Log.Instance.Success($"[AutoxAlwaysOn][m_cAuto][m_fBATStop][auto timer stop]");
            }
            catch (Exception ex)
            {
                Log.Instance.Error($"[AutoxAlwaysOn][m_cAuto][m_fBATStop][Exception][{ex.Message}]");
            }
        }
        #endregion

        #region ***BAT重启
        public static void m_fBATRestart()
        {
            m_cAuto.m_fBATStop();
            m_cAuto.m_fBATStart();
            Log.Instance.Success($"[AutoxAlwaysOn][m_cAuto][m_fBATRestart][auto timer restart]");
        }
        #endregion

        #region ***BAT计时器动作
        /// <summary>
        /// 计时器动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void m_fBATTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                //先停止计时器
                m_cAuto.m_pBATTimer.Stop();
                //执行同步操作
                {

                }
                //再启动
                m_cAuto.m_pBATTimer.Start();
            }
            catch (Exception ex)
            {
                Log.Instance.Error($"[AutoxAlwaysOn][m_cAuto][m_fBATTimerElapsed][Exception][{ex.Message}]");
                Log.Instance.Debug(ex);
                System.Threading.Thread.Sleep(1000 * 10);
                Log.Instance.Error($"[AutoxAlwaysOn][m_cAuto][m_fBATTimerElapsed][Exception][自动外呼同步计时器遇到错误10秒后重启]");
                m_cAuto.m_fBATRestart();
            }
        }
        #endregion

        #endregion
    }
}
