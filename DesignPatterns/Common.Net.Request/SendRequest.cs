using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Request
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
            webRequest.Method = String.Compare(method, "GET", StringComparison.OrdinalIgnoreCase) == 0 ? "GET" : "POST";
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
                webRequest.Abort();
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

        /// <summary>
        /// 发送到Http请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="method"></param>
        /// <param name="contenttype"></param>
        /// <returns></returns>
        internal static string SendWebRequest(string url, string data, string method, string contenttype = "application/x-www-form-urlencoded")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = String.Compare(method, "GET", StringComparison.OrdinalIgnoreCase) == 0 ? "GET" : "POST";
            request.ContentType = contenttype;//application/json
            request.Credentials = CredentialCache.DefaultCredentials;//new NetworkCredential(userName, password);
            request.Timeout = 180000;
            request.ReadWriteTimeout = 180000;
            request.KeepAlive = false;
            byte[] datas = Encoding.UTF8.GetBytes(data);
            request.ContentLength = datas.Length;
            try
            {
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(datas, 0, datas.Length);
                requestStream.Close();
            }
            catch (System.Net.ProtocolViolationException ex)
            {
                request.Abort();
            }
            catch (System.Net.WebException ex)
            {
                request.Abort();
            }
            catch (System.ObjectDisposedException ex)
            {
                request.Abort();
            }
            catch (System.InvalidOperationException ex)
            {
                request.Abort();
            }
            catch (System.NotSupportedException ex)
            {
                request.Abort();
            }
            HttpWebResponse response = null;
            string responseDatas = string.Empty;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                Stream streamResponse = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(streamResponse))
                {
                    responseDatas = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                request.Abort();
            }
            finally
            {
                if (response != null)
                {
                    try
                    {
                        response.Close();
                    }
                    catch
                    {
                        request.Abort();
                    }
                }
            }
            return responseDatas;
        }
    }
}
