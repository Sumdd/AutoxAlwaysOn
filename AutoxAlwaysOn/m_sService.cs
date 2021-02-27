using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AutoxAlwaysOn
{
    partial class m_sService : ServiceBase
    {
        public static bool m_bStop = false;
        public static bool m_bLoad = true;
        public m_sService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            m_cAuto.m_fHTTPStart();
            m_cAuto.m_fCMDStart();
            m_cAuto.m_fBATStart();
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            m_sService.m_bStop = true;
            m_cAuto.m_fHTTPStop();
            m_cAuto.m_fCMDStop();
            m_cAuto.m_fBATStop();
        }
    }
}
