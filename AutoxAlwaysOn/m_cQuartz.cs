using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;
using System.Data;

namespace AutoxAlwaysOn
{
    #region ***模型
    public class m_cQuartzJobModel
    {
        /// <summary>
        /// 默认BAT组
        /// </summary>
        public const string GROUP_BAT = "BAT";
        /// <summary>
        /// 计划：通用全任务规划
        /// </summary>
        public const string JOB_ALL = "JOB_ALL";

    }
    #endregion

    #region ***请求用户令牌
    [DisallowConcurrentExecution]
    public class m_cQuartzJobAll : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                switch (context.Trigger.Key.Group)
                {
                    case m_cQuartzJobModel.GROUP_BAT:
                        {
                            m_mBAT _m_mBAT = m_cSettings.m_lBAT?.Find(x => x.m_sName.Equals(context.Trigger.Key.Name));
                            if (_m_mBAT != null)
                            {
                                #region ***任务内容
                                int j = 0;
                                for (int i = 1; i <= _m_mBAT.m_lBAT.Count; i++)
                                {
                                    string m_sBAT = _m_mBAT.m_lBAT[i - 1];
                                    if (m_cBAT.m_fUse(m_sBAT, false, true))
                                    {
                                        Log.Instance.Success($"[AutoxAlwaysOn][m_cQuartzJobAll][Execute][+OK:{m_cQuartzJobModel.GROUP_BAT} > {_m_mBAT.m_sName} > 步骤{i} > 成功]");
                                        j++;
                                    }
                                    else
                                    {
                                        Log.Instance.Fail($"[AutoxAlwaysOn][m_cQuartzJobAll][Execute][-Err:{m_cQuartzJobModel.GROUP_BAT} > {_m_mBAT.m_sName} > 步骤{i} > 失败]");
                                        break;
                                    }
                                }
                                if (j == _m_mBAT.m_lBAT.Count)
                                {
                                    Log.Instance.Success($"[AutoxAlwaysOn][m_cQuartzJobAll][Execute][+OK:{m_cQuartzJobModel.GROUP_BAT} > {_m_mBAT.m_sName} > 任务成功]");
                                }
                                else
                                {
                                    Log.Instance.Warn($"[AutoxAlwaysOn][m_cQuartzJobAll][Execute][-Err:{m_cQuartzJobModel.GROUP_BAT} > {_m_mBAT.m_sName} > 执行步骤{(j + 1)}时失败,任务终止]");
                                }
                                #endregion
                            }
                            else
                            {
                                Log.Instance.Error($"[AutoxAlwaysOn][m_cQuartzJobAll][Execute][-Err:{m_cQuartzJobModel.GROUP_BAT} > {context.Trigger.Key.Name} > 任务内容丢失]");
                            }
                        }
                        break;
                    default:
                        Log.Instance.Debug($"[AutoxAlwaysOn][m_cQuartzJobAll][Execute][未知任务组]");
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Instance.Debug(ex);
            }
        }
    }
    #endregion
}