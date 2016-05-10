using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Climb.Utility.SystemExt;

namespace Climb.WebUtility
{
    /// <summary>
    /// 请求类
    /// </summary>
    public sealed class HttpWebHelper
    {
        private HttpWebRequest _request;
        private HttpWebResponse _response;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        public HttpWebHelper(HttpRequestItem items)
        {
            SetRequest(items);//填充对象
        }

        /// <summary>
        /// 获取请求内容
        /// </summary>
        /// <returns></returns>
        public HttpResult RequestGetReult()
        {
            HttpResult httpReult = new HttpResult();
            //开始请求
            try
            {
                _response = (HttpWebResponse)_request.GetResponse();
                using (_response)
                {
                    CookieCollection cookieColl = _response.Cookies;
                    if (cookieColl != null && cookieColl.Count > 0)
                    {
                        httpReult.CookieContainer.Add(cookieColl);
                    }
                    httpReult.ReturnState = _response.StatusCode.ToString();
                    if (httpReult.ReturnState == HttpStatusCode.OK.ToString())//成功
                    {
                        string resposeEncoding = _response.ContentEncoding;
                        StreamReader streamReader = resposeEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase) ? new StreamReader(new GZipStream(_response.GetResponseStream(), CompressionMode.Decompress)) : new StreamReader(_response.GetResponseStream());
                        string lineStr;
                        StringBuilder readerContent = new StringBuilder();
                        while ((lineStr = streamReader.ReadLine()) != null)
                        {
                            readerContent.AppendLine(lineStr);
                        }
                        httpReult.ReHtml = readerContent.ToString();
                    }

                }

            }
            catch (WebException ex)
            {
                httpReult.ErrorMsg = ex.Message;
                httpReult.ReturnState = "RequestError";
            }
            return httpReult;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filePath"></param>
        /// <param name="actionErr"></param>
        public static void HttpDownFile(string url, string filePath, Action<string> actionErr)
        {
            if (url.Length == 0) return;
            using (WebClient wc = new WebClient())
            {
                try
                {
                    wc.DownloadFile(url, filePath);
                }
                catch (Exception ex)
                {
                    actionErr(ex.Message);
                }
            }
        }

        /// <summary>
        /// 设置 request对象
        /// </summary>
        /// <param name="items"></param>
        public void SetRequest(HttpRequestItem items)
        {
            _request = WebRequest.Create(new Uri(items.Url)) as HttpWebRequest;
            if (_request == null) return;
            _request.Method = items.Method;
            _request.Accept = items.Accept;
            _request.UserAgent = items.UserAgent;
            _request.KeepAlive = items.KeepAlive;
            _request.ContentType = items.ContentType;
            _request.Timeout = items.ReTimeOut;
            _request.Referer = items.Referer;
            _request.ContentLength = items.ContentLength;
            if (items.ReWebProxy != null)//设置代理
            {
                _request.Proxy = items.ReWebProxy;
            }
            if (!items.PostBytes.IsNullEmpty())
            {
                _request.GetRequestStream().Write(items.PostBytes, 0, items.ContentLength);
            }
        }
    }

    /// <summary>
    /// 请求结果类
    /// </summary>
    public class HttpResult
    {
        public CookieContainer CookieContainer { get; set; }//cookie容器
        public string ReturnState { get; set; } //返回状态
        public string ErrorMsg { get; set; }//错误信息
        public string ReHtml { get; set; }
    }

    /// <summary>
    /// 请求参数
    /// </summary>
    public class HttpRequestItem
    {
        public string Url { get; set; }
        public string Method { get; set; }
        public string UserAgent { get; set; }
        public bool KeepAlive { get; set; }
        public string ContentType { get; set; }
        public int ReTimeOut { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Accept { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Referer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PostData { get; set; }
        public int ContentLength { get; set; }
        public byte[] PostBytes { get; set; }
        public WebProxy ReWebProxy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CookieContainer ReCookieContainer { get; set; }
        /// <summary>
        /// 设置509证书集合
        /// </summary>
        public X509CertificateCollection ClentCertificates { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url"></param>
        public HttpRequestItem(string url)
        {
            if (!url.IsNullOrEmpty())
            {
                Url = url;
                UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
                KeepAlive = true;
                ContentType = "text/html";
                ReTimeOut = 12000;
                Accept = "text/html, application/xhtml+xml, */*";
                ReCookieContainer = new CookieContainer();
            }
            else
            {
                throw new Exception("url 无效");
            }
        }

        /// <summary>
        /// 获取Get对象
        /// </summary>
        public void GetItems()
        {
            Method = "GET";
        }

        /// <summary>
        /// 获取Post对象
        /// </summary>
        /// <param name="postData"></param>
        /// <param name="postencoding"></param>
        public void PostItems(string postData, Encoding postencoding)
        {
            if (!postData.IsNullOrEmpty())
            {
                Method = "POST";
                PostBytes = postencoding.GetBytes(postData);
                ContentLength = PostBytes.Length;
            }
            else
            {
                throw new ArgumentNullException("postData");
            }

        }

        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="point"></param>
        public void SetWebProxy(string ip, int point)
        {
            ReWebProxy = new WebProxy(ip, point);
        }
    }

    /// <summary>
    /// C#模拟Http提交 Get和Post方法
    /// </summary>
    public class HttpReuqest
    {

        public string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }


        public string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
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
    }
}
