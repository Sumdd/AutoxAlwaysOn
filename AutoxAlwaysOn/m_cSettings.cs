using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoxAlwaysOn
{
    public class m_cSettings
    {
        #region ***HTTP站点激活列表
        private static List<m_mHTTP> _m_lHTTP;
        public static List<m_mHTTP> m_lHTTP
        {
            get
            {
                if (_m_lHTTP == null)
                {
                    List<m_mHTTP> m_deflHTTP = new List<m_mHTTP>();
                    ///得到要激活的网站
                    Dictionary<string, string> m_dDic = m_cSettings.m_dKeyValue;
                    foreach (KeyValuePair<string, string> item in m_dDic)
                    {
                        if (item.Key.StartsWith("HTTP"))
                        {
                            m_mHTTP _m_mHTTP = new m_mHTTP();
                            _m_mHTTP.m_sName = item.Key;
                            _m_mHTTP.m_sHttp = item.Value;
                            _m_mHTTP.m_sSeconds = 60;
                            _m_mHTTP.m_dtLastResq = DateTime.MinValue;
                            m_deflHTTP.Add(_m_mHTTP);
                        }
                    }

                    foreach (m_mHTTP item in m_deflHTTP)
                    {
                        ///得到对应的激活频率秒数
                        string m_sRecName = $"RATESEC{item.m_sName}";
                        if (m_dDic.ContainsKey(m_sRecName))
                        {
                            item.m_sSeconds = int.Parse(m_dDic[m_sRecName]);
                        }
                        ///得到对应的激活所需参数
                        string m_sArgsName = $"ARGS{item.m_sName}";
                        if (m_dDic.ContainsKey(m_sArgsName))
                        {
                            item.m_sArgs = m_dDic[m_sArgsName];
                        }
                    }
                    ///赋值返回
                    _m_lHTTP = m_deflHTTP;
                }
                return _m_lHTTP;
            }
        }
        #endregion

        #region ***通用加载参数
        private static Dictionary<string, string> _m_dKeyValue = null;
        public static Dictionary<string, string> m_dKeyValue
        {
            get
            {
                if (_m_dKeyValue == null || (_m_dKeyValue != null && _m_dKeyValue.Count <= 0))
                {
                    _m_dKeyValue = new Dictionary<string, string>();
                    foreach (string item in System.Configuration.ConfigurationSettings.AppSettings.AllKeys)
                    {
                        _m_dKeyValue.Add(item, System.Configuration.ConfigurationSettings.AppSettings[item].Replace("\\r\\n", "\r\n"));
                    }
                }
                return _m_dKeyValue;
            }
        }
        #endregion
    }
}
