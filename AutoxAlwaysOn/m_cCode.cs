using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoxAlwaysOn
{
    public class m_cCode
    {
        public static string m_fGetString(string m_sCode)
        {
            string m_sReturnStr = string.Empty;
            string strCode = $@"
                    using System;
                    namespace ParseEx
                    {{
                        public class ParseExC
                        {{
                            public static string GetValue()
                            {{
                                return {m_sCode.Replace("Code:", "")};
                            }}
                        }}
                    }}";

            CodeDomProvider comp = new CSharpCodeProvider();
            CompilerParameters cp = new CompilerParameters();

            StringBuilder codeBuilder = new StringBuilder();
            codeBuilder.AppendLine(strCode);

            cp.ReferencedAssemblies.Add("System.dll");
            cp.GenerateExecutable = false;
            cp.GenerateInMemory = true;

            CompilerResults cr = comp.CompileAssemblyFromSource(cp, codeBuilder.ToString());
            if (cr.Errors.HasErrors)
            {
                Log.Instance.Error($"[AutoxAlwaysOn][m_cCode][m_fGetString][{m_sCode},动态解析时错误,{strCode}]");
            }
            else
            {
                Assembly a = cr.CompiledAssembly;
                if (a != null)
                {
                    Type t = a.GetType("ParseEx.ParseExC");
                    if (t != null)
                    {
                        MethodInfo mi = t.GetMethod("GetValue", BindingFlags.Static | BindingFlags.Public);
                        if (mi != null)
                        {
                            m_sReturnStr = (string)mi.Invoke(null, null);
                        }
                    }
                }
            }
            return m_sReturnStr;
        }
    }
}
