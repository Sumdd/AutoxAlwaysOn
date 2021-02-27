using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoxAlwaysOn
{
    public class m_mHTTP
    {
        public string m_sName { get; set; }
        public string m_sHttp { get; set; }
        public int m_sSeconds { get; set; }
        public string m_sArgs { get; set; }
        public DateTime m_dtLastResq { get; set; }
    }

    public class m_mCMD
    {
        public string m_sName { get; set; }
        public List<string> m_lCMD { get; set; }
        public string m_sDoWay { get; set; }
    }
}
