using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Zxxk.OpenApiClient
{
    /// <summary>
    /// 发送请求
    /// </summary>
    internal static class SendRequest
    {
        /// <summary>
        /// 发送到Http请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="data">请求参数</param>
        /// <param name="method">请求方法</param>
        /// <param name="wEnc">Encoding.UTF8</param>
        /// <param name="rEnc">Encoding.UTF8</param>
        /// <returns></returns>
        internal static string SendWebRequest(string url, string data, string method, Encoding wEnc, Encoding rEnc)
        {
            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Method = string.Compare(method, "GET", true) == 0 ? "GET" : "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Credentials = CredentialCache.DefaultCredentials;
            webRequest.Timeout = 20000;
            if (!string.IsNullOrEmpty(data))
            {
                byte[] bytes = wEnc.GetBytes(data);
                webRequest.ContentLength = (long)bytes.Length;
                Stream requestStream = webRequest.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
            }
            WebResponse response;
            try
            {
                response = webRequest.GetResponse();
            }
            catch (WebException ex)
            {
                response = ex.Response;
            }
            string str = string.Empty;
            if (response != null)
            {
                StreamReader streamReader = new StreamReader(response.GetResponseStream(), rEnc);
                str = streamReader.ReadToEnd();
                response.Close();
                streamReader.Dispose();
            }
            return str;
        }
    }
}
