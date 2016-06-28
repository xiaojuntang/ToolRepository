using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace Common.Net.Core
{
    public class Http2Helper
    {
        #region 回调验证证书问题

        //文档http://www.cnblogs.com/txw1958/archive/2013/01/11/csharp-HttpWebRequest-HttpWebResponse.html
        //回调验证证书问题
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            // 总是接受    
            return true;
        }

        ///<summary>
        ///采用https协议访问网络
        ///</summary>
        ///<param name="URL">url地址</param>
        ///<param name="strPostdata">发送的数据</param>
        ///<returns></returns>
        public static string OpenReadWithHttps(string URL, string strPostdata, string strEncoding)
        {
            // 这一句一定要写在创建连接的前面。使用回调的方法进行证书验证。
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            //创建证书文件
            X509Certificate objx509 = new X509Certificate("\\123.cer");//Application.StartupPath
            //加载Cookie
            request.CookieContainer = new CookieContainer();
            //添加到请求里
            request.ClientCertificates.Add(objx509);
            request.Method = "post";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] buffer = encoding.GetBytes(strPostdata);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding(strEncoding)))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// 传入URL返回网页的html代码
        /// </summary>
        /// <param name="Url">URL</param>
        /// <returns></returns>
        public static string GetUrltoHtml(string Url)
        {
            StringBuilder content = new StringBuilder();
            try
            {
                //这一句一定要写在创建连接的前面。使用回调的方法进行证书验证。
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                // 与指定URL创建HTTP请求
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                //创建证书文件
                X509Certificate objx509 = new X509Certificate("\\123.cer");//Application.StartupPath
                //添加到请求里
                request.ClientCertificates.Add(objx509);
                // 获取对应HTTP请求的响应
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // 获取响应流
                Stream responseStream = response.GetResponseStream();
                // 对接响应流(以"GBK"字符集)
                StreamReader sReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                // 开始读取数据
                Char[] sReaderBuffer = new Char[256];
                int count = sReader.Read(sReaderBuffer, 0, 256);
                while (count > 0)
                {
                    String tempStr = new String(sReaderBuffer, 0, count);
                    content.Append(tempStr);
                    count = sReader.Read(sReaderBuffer, 0, 256);
                }
                // 读取结束
                sReader.Close();
            }
            catch (Exception)
            {
                content = new StringBuilder("Runtime Error");
            }
            return content.ToString();
        }

        /// <summary>
        /// 传入URL返回网页的html代码带有证书的方法
        /// </summary>
        /// <param name="Url">URL</param>
        /// <returns></returns>
        public static string GetUrltoHtml2(string Url)
        {
            StringBuilder content = new StringBuilder();
            try
            {
                // 与指定URL创建HTTP请求
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0; BOIE9;ZHCN)";
                request.Method = "GET";
                request.Accept = "*/*";
                //如果方法验证网页来源就加上这一句如果不验证那就可以不写了
                request.Referer = "http://txw1958.cnblogs.com";
                CookieContainer objcok = new CookieContainer();
                objcok.Add(new Uri("http://txw1958.cnblogs.com"), new Cookie("键", "值"));
                objcok.Add(new Uri("http://txw1958.cnblogs.com"), new Cookie("键", "值"));
                objcok.Add(new Uri("http://txw1958.cnblogs.com"), new Cookie("sidi_sessionid", "360A748941D055BEE8C960168C3D4233"));
                request.CookieContainer = objcok;
                //不保持连接
                request.KeepAlive = true;
                // 获取对应HTTP请求的响应
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // 获取响应流
                Stream responseStream = response.GetResponseStream();
                // 对接响应流(以"GBK"字符集)
                StreamReader sReader = new StreamReader(responseStream, Encoding.GetEncoding("gb2312"));
                // 开始读取数据
                Char[] sReaderBuffer = new Char[256];
                int count = sReader.Read(sReaderBuffer, 0, 256);
                while (count > 0)
                {
                    String tempStr = new String(sReaderBuffer, 0, count);
                    content.Append(tempStr);
                    count = sReader.Read(sReaderBuffer, 0, 256);
                }
                // 读取结束
                sReader.Close();
            }
            catch (Exception)
            {
                content = new StringBuilder("Runtime Error");
            }
            return content.ToString();
        }

        ///<summary>
        ///采用https协议访问网络
        ///</summary>
        ///<param name="URL">url地址</param>
        ///<param name="strPostdata">发送的数据</param>
        ///<returns></returns>
        public string OpenReadWithHttps(string URL, string strPostdata)
        {
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "post";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/x-www-form-urlencoded";
            CookieContainer objcok = new CookieContainer();
            objcok.Add(new Uri("http://txw1958.cnblogs.com"), new Cookie("键", "值"));
            objcok.Add(new Uri("http://txw1958.cnblogs.com"), new Cookie("键", "值"));
            objcok.Add(new Uri("http://txw1958.cnblogs.com"), new Cookie("sidi_sessionid", "360A748941D055BEE8C960168C3D4233"));
            request.CookieContainer = objcok;
            byte[] buffer = encoding.GetBytes(strPostdata);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8"));
            return reader.ReadToEnd();
        }

        #endregion

        #region 测试方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body">是要传递的参数,格式"roleId=1&uid=2"</param>
        /// <param name="contentType">post的cotentType填写:"application/x-www-form-urlencoded" soap填写:"text/xml; charset=utf-8"</param>
        /// <returns></returns>
        public static string PostHttp(string url, string body, string contentType)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.ContentType = contentType;
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 20000;

            byte[] btBodys = Encoding.UTF8.GetBytes(body);
            httpWebRequest.ContentLength = btBodys.Length;
            httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();

            httpWebResponse.Close();
            streamReader.Close();
            httpWebRequest.Abort();
            httpWebResponse.Close();

            return responseContent;
        }


        /// <summary>
        /// 通过WebClient类Post数据到远程地址，需要Basic认证；
        /// 调用端自己处理异常
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="paramStr">name=张三&age=20</param>
        /// <param name="encoding">请先确认目标网页的编码方式</param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Request_WebClient(string uri, string paramStr, Encoding encoding, string username, string password)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            string result = string.Empty;

            WebClient wc = new WebClient();

            // 采取POST方式必须加的Header
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            byte[] postData = encoding.GetBytes(paramStr);

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                wc.Credentials = GetCredentialCache(uri, username, password);
                wc.Headers.Add("Authorization", GetAuthorization(username, password));
            }

            byte[] responseData = wc.UploadData(uri, "POST", postData); // 得到返回字符流
            return encoding.GetString(responseData);// 解码                  
        }

        public static string GetHttp(string url, Dictionary<string, object> param)
        {
            string queryString = "?";

            foreach (var item in param)
            {
                queryString += item.Key + "=" + item.Value.ToString() + "&";
            }

            queryString = queryString.Substring(0, queryString.Length - 1);

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url + queryString);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = 20000;

            //byte[] btBodys = Encoding.UTF8.GetBytes(body);
            //httpWebRequest.ContentLength = btBodys.Length;
            //httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();

            httpWebResponse.Close();
            streamReader.Close();

            return responseContent;
        }

        #region 生成 Http Basic 访问凭证

        private static CredentialCache GetCredentialCache(string uri, string username, string password)
        {
            string authorization = string.Format("{0}:{1}", username, password);
            CredentialCache credCache = new CredentialCache();
            credCache.Add(new Uri(uri), "Basic", new NetworkCredential(username, password));
            return credCache;
        }

        private static string GetAuthorization(string username, string password)
        {
            string authorization = string.Format("{0}:{1}", username, password);
            return "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(authorization));
        }

        #endregion

        #endregion
    }
}
