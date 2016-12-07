using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Request
{
    public class ApiInvoke
    {
        /// <summary>
        /// 请求方法
        /// </summary>
        private static string Operation { get; set; }

        private static string Url
        {
            get { return "http://101.200.148.144:8000"; }
        }

        /// <summary>
        /// 公钥 
        /// </summary>
        public static string AppKey
        {
            get { return string.Empty; }
        }

        private static string AppSecret
        {
            get { return string.Empty; }
        }


        /// <summary>
        /// 执行Post接口方法
        /// </summary>
        /// <param name="operation">方法名</param>
        /// <param name="param">有序参数</param>
        /// <returns></returns>
        public static string BeginExec(string operation, SortedDictionary<string, string> param)
        {
            ApiInvoke.Operation = operation.Trim();
            StringBuilder builder = new StringBuilder();
            foreach (var p in param)
            {
                builder.Append(p.Key).Append("=").Append(p.Value).Append("&");
            }
            //return SendRequest.SendWebRequest(ApiInvoke.Url+ ApiInvoke.Operation, builder.ToString().Trim('&'), "post", Encoding.UTF8, Encoding.UTF8);
            return SendRequest.SendWebRequest(ApiInvoke.Url + ApiInvoke.Operation, builder.ToString().Trim('&'), "post");
        }

        /// <summary>
        /// 接口调用入口
        /// </summary>
        /// <param name="operation">接口操作方法名</param>
        /// <param name="param">调用传入的参数</param>
        /// <returns/>
        public static string BeginExec(string operation, Dictionary<string, string> param)
        {
            ApiInvoke.Operation = operation.Trim();
            StringBuilder builder = new StringBuilder();
            foreach (var p in param)
            {
                builder.Append(p.Key).Append("=").Append(p.Key).Append("&");
            }
            return SendRequest.SendWebRequest(ApiInvoke.Url, builder.ToString().Trim('&'), "post", Encoding.UTF8, Encoding.UTF8);
        }


    }
}
