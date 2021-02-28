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

        #region ***BAT执行列表
        private static List<m_mBAT> _m_lBAT;
        public static List<m_mBAT> m_lBAT
        {
            get
            {
                if (_m_lBAT == null)
                {
                    List<m_mBAT> m_deflBAT = new List<m_mBAT>();
                    ///得到要激活的网站
                    Dictionary<string, string> m_dDic = m_cSettings.m_dKeyValue;
                    foreach (KeyValuePair<string, string> item in m_dDic)
                    {
                        if (item.Key.StartsWith("BAT"))
                        {
                            ///说明是命令行
                            if (item.Key.Contains("-"))
                            {
                                string[] m_lIndex = item.Key.Split('-');
                                m_mBAT m_defpBAT = m_deflBAT.Find(x => x.m_sName.Equals(m_lIndex[0]));
                                ///如果不存在直接创建
                                if (m_defpBAT == null)
                                {
                                    m_defpBAT = new m_mBAT();
                                    m_defpBAT.m_sName = m_lIndex[0];
                                    m_defpBAT.m_lBAT = new List<string>();
                                    m_deflBAT.Add(m_defpBAT);
                                }
                                ///顺序放入命令
                                int m_uBATIndex = int.Parse(m_lIndex[1]) - 1;
                                ///容错
                                if (m_uBATIndex > m_defpBAT.m_lBAT.Count) m_uBATIndex = m_defpBAT.m_lBAT.Count;
                                m_defpBAT.m_lBAT.Insert(m_uBATIndex, item.Value);
                            }
                            ///说明是执行模式
                            else if (item.Key.Contains("MODE"))
                            {
                                string m_sName = item.Key.Replace("MODE", "");
                                m_mBAT m_defpBAT = m_deflBAT.Find(x => x.m_sName.Equals(m_sName));
                                ///如果不存在直接创建
                                if (m_defpBAT == null)
                                {
                                    m_defpBAT = new m_mBAT();
                                    m_defpBAT.m_sName = m_sName;
                                    m_defpBAT.m_lBAT = new List<string>();
                                    m_deflBAT.Add(m_defpBAT);
                                }
                                ///直接赋值
                                m_defpBAT.m_sDoWay = item.Value;
                            }
                        }
                    }
                    ///赋值返回
                    _m_lBAT = m_deflBAT;
                }
                return _m_lBAT;
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
