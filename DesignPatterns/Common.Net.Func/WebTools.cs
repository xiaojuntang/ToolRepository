/***************************************************************************** 
*        filename :WebTools 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.42000 
*        新建项输入的名称:   WebTools 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Common.Net.Func 
*        文件名:             WebTools 
*        创建系统时间:       2016/2/3 10:16:36 
*        创建年份:           2016 
/*****************************************************************************/
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Common.Net.Func
{
    public sealed class WebTools
    {

        public static HttpContext GetCurrHttpContext
        {
            get
            {
                return HttpContext.Current;
            }
        }

        /// <summary>
        /// 返回Session信息
        /// </summary>
        /// <param name="strName">Session名称</param>
        /// <returns></returns>
        public static T GetSession<T>(string strName) where T : class
        {
            if (string.IsNullOrEmpty(strName))
            {
                return null;
            }
            if (HttpContext.Current.Session[strName] == null)
            {
                return null;
            }
            return HttpContext.Current.Session[strName] as T;
        }
        /// <summary>
        /// 写Session信息
        /// </summary>
        /// <param name="strName">Session名称</param>
        /// <param name="strValue">值</param>
        public static void WriteSession<T>(string strName, T t) where T : class
        {
            if (string.IsNullOrEmpty(strName))
            {
                return;
            }
            if (t == null)
            {
                return;
            }
            HttpContext.Current.Session[strName] = t;
        }

        public static void RemoveSession(string strName)
        {
            if (string.IsNullOrEmpty(strName))
            {
                return;
            }
            HttpContext.Current.Session.Remove(strName);
        }
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            WriteCookie(strName, strValue, 0, string.Empty);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue, string domain)
        {
            WriteCookie(strName, strValue, 0, domain);
        }
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="strValue"></param>
        public static void WriteCookie(string strName, string strValue, int days)
        {
            WriteCookie(strName, strValue, days, string.Empty);
        }

        /// <summary>
        ///  写cookie 
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="strValue"></param>
        /// <param name="day"></param>
        public static void WriteCookie(string strName, string strValue, int day, string _doMain)
        {
            if (string.IsNullOrEmpty(strName))
            {
                return;
            }
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;

            if (!string.IsNullOrEmpty(_doMain))
            {
                HttpContext.Current.Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");
                cookie.Domain = _doMain;
            }
            if (day != 0)
            {
                cookie.Expires = DateTime.Now.AddDays(day);
            }
            HttpContext.Current.Response.AppendCookie(cookie);
        }


        public static void SetCookieDomain(string strName, string _domainValue)
        {
            if (string.IsNullOrEmpty(strName))
            {
                return;
            }
            if (string.IsNullOrEmpty(_domainValue))
            {
                return;
            }
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie != null)
            {
                cookie.Domain = _domainValue;
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }

        /// <summary>
        /// 删除cookie的方法
        /// </summary>
        /// <param name="strName"></param>
        public static void RemoveCookie(string strName, string _domainValue)
        {
            if (string.IsNullOrEmpty(strName))
            {
                return;
            }


            HttpCookie cookie = new HttpCookie(strName);
            if (!string.IsNullOrEmpty(_domainValue))
            {
                cookie.Domain = _domainValue;
            }
            // 删除Cookie
            cookie.Expires = new DateTime(1900, 1, 1);
            HttpContext.Current.Response.Cookies.Add(cookie);

        }
        /// <summary>
        /// 读取cookie 的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ReadCookie(string key)
        {
            string strValue = string.Empty;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie != null)
            {
                strValue = cookie.Value;
            }
            return strValue;
        }

        /// <summary>
        /// 将对象写入全局应用程序域
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        public static void WriteAppliction<T>(string key, T t) where T : class ,new()
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            if (t == null)
            {
                return;
            }
            HttpContext.Current.Application[key] = t;
        }

        //枷锁的方法
        public static void LockAppliction()
        {
            HttpContext.Current.Application.Lock();
        }

        //解锁的方法
        public static void UnLockAppliction()
        {
            HttpContext.Current.Application.UnLock();
        }

        /// <summary>
        /// 从appliction 获取对象的方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns> 
        public static T GetAppliction<T>(string key) where T : class ,new()
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            T t = HttpContext.Current.Application[key] as T;
            if (t == null)
            {
                LockAppliction();
                t = new T();
                HttpContext.Current.Application[key] = t;
                LockAppliction();
            }
            return t;
        }
        /// <summary>
        /// 删除 appliction的一个方法
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveAppliction(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            HttpContext.Current.Application.Lock();
            HttpContext.Current.Response.Cookies.Remove(key);
            HttpContext.Current.Application.UnLock();
        }

        /// <summary>
        /// 获取一个 登录 生成的惟一key 
        /// </summary>
        /// <returns></returns>
        public static string GetLoginKey()
        {
            return (HttpContext.Current.Session.SessionID + DateTime.Now.ToString("yyyyMMddHHss")).md5();
        }

        /// <summary>
        /// 发送post请求(utf8 编码)
        /// </summary>
        /// <param name="url">请求的url</param>
        /// <param name="postString">发送到数据 例如:"name=xhan&password=1231"</param>
        /// <returns>服务器响应字符串</returns>
        public static string SendPostRequest(string url, string postString)
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            byte[] postData = Encoding.UTF8.GetBytes(postString);
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("ContentLength", postData.Length.ToString());
            byte[] responseData = client.UploadData(url, "POST", postData);
            string result = Encoding.UTF8.GetString(responseData);
            return result;
        }
        /// <summary>获取用户的客户端来源 
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public static string GetClentSource(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
                return "unknown";
            string[] pc = { "Windows NT", "Macintosh" };
            if (pc.Any(source => userAgent.IndexOf(source, StringComparison.Ordinal) > -1))
            {
                return "PC";
            }
            string[] android = { "Android" };
            if (android.Any(source => userAgent.IndexOf(source, StringComparison.Ordinal) > -1))
            {
                return "Android";
            }
            string[] ios = { "iPhone", "iPod", "iPad" };
            if (ios.Any(source => userAgent.IndexOf(source, StringComparison.Ordinal) > -1))
            {
                return "IOS";
            }
            string[] wp = { "Windows Phone" };
            if (wp.Any(source => userAgent.IndexOf(source, StringComparison.Ordinal) > -1))
            {
                return "WP";
            }
            return "unknown";
        }
    }
}
