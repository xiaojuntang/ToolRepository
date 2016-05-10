
/**************************************************
* 文 件 名：MFGRequest.cs
* 文件版本：1.0
* 创 建 人：ssh
* 联系方式： Email:shaoshouhe@mofangge.com  
* 创建日期：2015/6/18  
* 文件说明：表单请求操作类
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Climb.WebUtility
{
    /// <summary>
    /// WEB表单请求类
    /// </summary>
    public class Request
    {
        [DllImport("Iphlpapi.dll")]
        private static extern int SendARP(Int32 dest, Int32 host, ref   Int64 mac, ref   Int32 length);
        [DllImport("Ws2_32.dll")]
        private static extern Int32 inet_addr(string ip);

        /// <summary>
        /// 获取当前主机全名[地址+端口号]
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFullHost()
        {
            HttpRequest request = HttpContext.Current.Request;
            if (!request.Url.IsDefaultPort)
            {
                return string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());
            }
            return request.Url.Host;
        }

        #region 获取Form表单数据

        /// <summary>
        /// 获取表单中的Float数据类型
        /// </summary>
        /// <param name="strName">表单名称</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static float GetFormFloat(string strName, float defValue)
        {
            return StrToFloat(HttpContext.Current.Request.Form[strName], defValue);
        }

        /// <summary>
        /// 获取表单中的Int数据类型
        /// </summary>
        /// <param name="strName">表单名称</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static int GetFormInt(string strName, int defValue)
        {
            return StrToInt(HttpContext.Current.Request.Form[strName], defValue);
        }
        private static int StrToInt(string str, int defValue)
        {
            int num;
            if (!((!string.IsNullOrEmpty(str) && (str.Trim().Length < 11)) && Regex.IsMatch(str.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$")))
            {
                return defValue;
            }
            if (int.TryParse(str, out num))
            {
                return num;
            }
            return Convert.ToInt32(StrToFloat(str, (float)defValue));
        }
        /// <summary>
        /// 将string转换成Float
        /// </summary>
        /// <param name="strValue">需要转换的字符串</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        private static float StrToFloat(string strValue, float defValue)
        {
            if ((strValue == null) || (strValue.Length > 10))
            {
                return defValue;
            }
            float result = defValue;
            if ((strValue != null) && Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
            {
                float.TryParse(strValue, out result);
            }
            return result;
        }
        /// <summary>
        /// 获取表单中的字符串，为验证SQL语句的安全性
        /// </summary>
        /// <param name="strName">表单名称</param>
        /// <returns></returns>
        public static string GetFormString(string strName)
        {
            return GetFormString(strName, false);
        }

        /// <summary>
        /// 获取表单数据并且验证是否SQL安全性
        /// </summary>
        /// <param name="strName">表单名称</param>
        /// <param name="sqlSafeCheck">是否验证SQL的安全性</param>
        /// <returns></returns>
        public static string GetFormString(string strName, bool sqlSafeCheck)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
            {
                return "";
            }
            if (!(!sqlSafeCheck || IsSafeSqlString(HttpContext.Current.Request.Form[strName])))
            {
                return "unsafe string";
            }
            return HttpContext.Current.Request.Form[strName];
        }
        /// <summary>
        /// 检测SQL语句是否安全
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
        #endregion

        #region 获取Querying信息

        /// <summary>
        /// 获取Request 参数个数
        /// </summary>
        /// <returns></returns>
        public static int GetParamCount()
        {
            return (HttpContext.Current.Request.Form.Count + HttpContext.Current.Request.QueryString.Count);
        }

        /// <summary>
        /// 获取浮点型 参数
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static float GetQueryFloat(string strName, float defValue)
        {
            return StrToFloat(HttpContext.Current.Request.QueryString[strName], defValue);
        }

        /// <summary>
        /// 获取字符型 参数
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public static int GetQueryInt(string strName)
        {
            return StrToInt(HttpContext.Current.Request.QueryString[strName], 0);
        }

        /// <summary>
        /// 获取字符型 参数
        /// </summary>
        /// <param name="strName">参数名称</param>
        /// <param name="defValue">参数的默认值</param>
        /// <returns></returns>
        public static int GetQueryInt(string strName, int defValue)
        {
            return StrToInt(HttpContext.Current.Request.QueryString[strName], defValue);
        }

        /// <summary>
        /// 获取字符串型 参数
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public static string GetQueryString(string strName)
        {
            return GetQueryString(strName, false);
        }

        /// <summary>
        /// 获取URL中的参数的值，返回stirng类型
        /// </summary>
        /// <param name="strName">URL参数名称</param>
        /// <param name="sqlSafeCheck">是否验证SQL安全性</param>
        /// <returns></returns>
        public static string GetQueryString(string strName, bool sqlSafeCheck)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return "";
            }
            if (!(!sqlSafeCheck || IsSafeSqlString(HttpContext.Current.Request.QueryString[strName])))
            {
                return "unsafe string";
            }
            return HttpContext.Current.Request.QueryString[strName];
        }

        /// <summary>
        /// 获取原始请求的URL地址
        /// </summary>
        /// <returns></returns>
        public static string GetRawUrl()
        {
            return HttpContext.Current.Request.RawUrl;
        }

        /// <summary>
        /// 判断param不存在且不为空
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool QueryStringParamIsTrue(string param)
        {
            if (System.Web.HttpContext.Current.Request.QueryString[param] != null && System.Web.HttpContext.Current.Request.QueryString[param].ToString().Trim() != "") return true;
            return false;
        }

        #endregion

        /// <summary>
        /// 获取主机名称
        /// </summary>
        /// <returns></returns>
        public static string GetHost()
        {
            return HttpContext.Current.Request.Url.Host;
        }

        /// <summary>
        /// 获取Int型URL参数
        /// </summary>
        /// <param name="strName">URL参数名称</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static int GetInt(string strName, int defValue)
        {
            if (GetQueryInt(strName, defValue) == defValue)
            {
                return defValue;
            }
            return GetQueryInt(strName, defValue);
        }

        /// <summary>
        /// 获取IP
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            if (HttpContext.Current == null)
                return "";

            string userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.UserHostAddress;
            }
            if (!(!string.IsNullOrEmpty(userHostAddress) && IsIpStr(userHostAddress)))
            {
                return "127.0.0.1";
            }
            return userHostAddress;
        }
        /// <summary>
        /// 判读是否是IP地址
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        private static bool IsIpStr(string inputStr)
        {
            IPAddress ip;
            return IPAddress.TryParse(inputStr, out ip);
        }
        ///<summary>
        /// 利用客户端ip得到客户端mac
        ///</summary>
        ///<param name="remoteip">客户端ip</param>
        ///<returns>int16类型的mac</returns>
        private static Int64 getremotemac(string remoteip)
        {
            Int32 ldest = inet_addr(remoteip);
            try
            {
                Int64 macinfo = new Int64();
                Int32 len = 6;
                int res = SendARP(ldest, 0, ref   macinfo, ref   len);
                return macinfo;
            }
            catch (Exception err)
            {
                Console.WriteLine("error:{0}", err.Message);
            }
            return 0;
        }

        ///<summary>
        /// int64类型的mac转换成正确的客户端mac
        ///</summary>
        ///<returns>mac</returns>
        public static string GetMAC()
        {
            Int64 macid = getremotemac(GetIP());
            if (macid == 0)
                return "0";
            string beforeMacAddr = Convert.ToString(macid, 16);
            string endMacAddr = "";
            string[] macArray = new string[6];
            for (int i = 0; i < 6; i++)
            {
                macArray[i] = beforeMacAddr.Substring(i * 2, 2);
            }
            for (int i = 0; i < 6; i++)
            {
                endMacAddr += macArray[5 - i] + "-";
            }
            endMacAddr = endMacAddr.Substring(0, endMacAddr.Length - 1);
            endMacAddr = endMacAddr.ToUpper();
            return endMacAddr;
        }

        /// <summary>
        /// 获取Page名称
        /// </summary>
        /// <returns></returns>
        public static string GetPageName()
        {
            string[] strArray = HttpContext.Current.Request.Url.AbsolutePath.Split(new char[] { '/' });
            return strArray[strArray.Length - 1].ToLower();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public static string GetServerString(string strName)
        {
            if (HttpContext.Current.Request.ServerVariables[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.ServerVariables[strName].ToString();
        }

        /// <summary>
        /// 获取当前页面的URL地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <returns></returns>
        public static string GetAuthority()
        {
            return HttpContext.Current.Request.Url.Authority;
        }

        /// <summary>
        /// 获取上次请求的URL不存在则返回空值
        /// </summary>
        /// <returns></returns>
        public static string GetUrlReferrer()
        {
            string str = null;
            try
            {
                str = HttpContext.Current.Request.UrlReferrer.ToString();
            }
            catch
            {
            }
            if (str == null)
            {
                return "";
            }
            return str;
        }

        /// <summary>
        /// 判断请求连接是否通过浏览器连接
        /// </summary>
        /// <returns></returns>
        public static bool IsBrowserGet()
        {
            string[] strArray = new string[] { "ie", "opera", "netscape", "mozilla", "konqueror", "firefox" };
            string str = HttpContext.Current.Request.Browser.Type.ToLower();
            for (int i = 0; i < strArray.Length; i++)
            {
                if (str.IndexOf(strArray[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取客户端浏览器类型
        /// </summary>
        /// <returns></returns>
        public static string GetBrowser()
        {
            try
            {
                return HttpContext.Current.Request.Browser.Type.ToLower();
            }
            catch
            {
                return "非浏览器设备.";
            }
        }

        /// <summary>
        /// 判断客户端请求是否是GET请求
        /// </summary>
        /// <returns></returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("GET");
        }

        /// <summary>
        /// 判断客户端请求是否为Post请求
        /// </summary>
        /// <returns></returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");
        }

        /// <summary>
        /// 判断请求是否通过搜索引擎连接查看
        /// ["google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou"]
        /// </summary>
        /// <returns></returns>
        public static bool IsSearchEnginesGet()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
            {
                string[] strArray = new string[] { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou" };
                string str = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (str.IndexOf(strArray[i]) >= 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 返回当前页面目录的url
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <returns></returns>
        public static string GetHomeBaseUrl(string FileName)
        {
            string Script_Name = GetScriptName;
            return string.Format("{0}/{1}", Script_Name.Remove(Script_Name.LastIndexOf("/")), FileName);
        }

        /// <summary>
        /// 获取当前页面的扩展名
        /// </summary>
        public static string GetScriptNameExt
        {
            get
            {
                return GetScriptName.Substring(GetScriptName.LastIndexOf(".") + 1);
            }
        }

        /// <summary>
        /// 获取当前访问页面地址
        /// </summary>
        public static string GetScriptName
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString();
            }
        }

        /// <summary>
        /// 保存请求的文件
        /// </summary>
        /// <param name="path">绝对文件路径</param>
        public static void SaveRequestFile(string path)
        {
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                HttpContext.Current.Request.Files[0].SaveAs(path);
            }
        }
    }
}
