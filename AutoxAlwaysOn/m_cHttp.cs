using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutoxAlwaysOn
{
    public class m_cHttp
    {
        /// <summary>
        /// http Get提交
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public static string HttpGet(string Url, string postDataStr = "")
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (string.IsNullOrWhiteSpace(postDataStr) ? "" : "?") + postDataStr);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return retString;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Stream HttpGetStream(string Url, string postDataStr = "")
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();

                ///将流转换成内存流
                var memoryStream = new MemoryStream();
                //将基础流写入内存流
                const int bufferLength = 1024;
                byte[] buffer = new byte[bufferLength];
                int actual = myResponseStream.Read(buffer, 0, bufferLength);
                while (actual > 0)
                {
                    memoryStream.Write(buffer, 0, actual);
                    actual = myResponseStream.Read(buffer, 0, bufferLength);
                }
                memoryStream.Position = 0;

                myResponseStream.Close();

                return memoryStream;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
