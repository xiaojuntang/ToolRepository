
/**************************************************
* 文 件 名：MFGJavaScript.cs
* 文件版本：1.0
* 创 建 人：ssh
* 联系方式： Email:shaoshouhe@mofangge.com  
* 创建日期：2015/6/18  
* 文件说明：JS注册类
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Climb.WebUtility
{
    /// <summary>
    /// JS注册类
    /// </summary>
    public class JavaScript
    {
        /// <summary>
        /// 弹出信息提示框,并且页面回退
        /// </summary>
        /// <param name="info">提示信息</param>
        public static void OpenWindow(string info)
        {
            if (info == false.ToString()) info = "失败";
            System.Web.HttpContext.Current.Response.Write("<script>alert('" + info + "');history.back(-1);</script>");
            System.Web.HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 弹出信息提示框（向页面输出脚本方式）
        /// </summary>
        /// <param name="info">提示信息</param>
        public static void MsgBox(string info)
        {
            string sScript = String.Format("alert('{0}');", info.Replace("'", "\\'"));
            WriteScript(sScript);
        }

        /// <summary>
        /// 向客户端写入脚本块
        /// </summary>
        /// <param name="sScript">脚本块代码(不包含script标记)</param>
        public static void WriteScript(string sScript)
        {
            string sOut = String.Format("<script language='javascript'>{0}</script>", sScript);
            System.Web.HttpContext.Current.Response.Write(sOut);
        }

        /// <summary>
        /// 弹出信息提示框且重定向页面
        /// </summary>
        /// <param name="info">提示信息</param>
        /// <param name="url">要重定向到的页面</param>
        public static void OpenWindowAndRef(string info, string url)
        {
            System.Web.HttpContext.Current.Response.Write("<script>alert('" + info + "');window.location.href='" + url + "';</script>");
            System.Web.HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 弹出信息提示框且关闭
        /// </summary>
        /// <param name="info">提示信息</param>
        public static void OpenWindowAndClose(string info)
        {
            System.Web.HttpContext.Current.Response.Write("<script>window.opener=null;alert('" + info + "');window.close();</script>");
        }
        /// <summary>
        /// 弹出信息提示框且重新加载窗口
        /// </summary>
        /// <param name="info">提示信息</param>
        public static void OpenWindowAndReload(string info)
        {
            System.Web.HttpContext.Current.Response.Write("<script>alert('" + info + "');window.location.href=window.location.href;</script>");
        }

        /// <summary>
        /// 弹出信息提示框且关闭并且刷新父窗口
        /// </summary>
        /// <param name="info">提示信息</param>
        public static void OpenWindowAndCloseAndRef(string info)
        {
            System.Web.HttpContext.Current.Response.Write("<script>alert('" + info + "');window.top.location.reload();window.close();</script>");
        }

        /// <summary>
        /// 重定向页面
        /// </summary>
        /// <param name="url">要重定向到的页面</param>
        public static void RedirectPage(string url)
        {
            System.Web.HttpContext.Current.Response.Redirect(url);
        }

        /// <summary>
        /// Top跳转页面
        /// </summary>
        /// <param name="url">跳转的页面地址</param>
        public static void TopWebRedirectPage(string url)
        {
            System.Web.HttpContext.Current.Response.Write("<script>window.top.location.href='" + url + "';</script>");
        }

        /// <summary>
        /// 默认跳转页面
        /// </summary>
        /// <param name="url">跳转的页面地址</param>
        public static void WebRedirectPage(string url)
        {
            System.Web.HttpContext.Current.Response.Write("<script>window.location.href='" + url + "';</script>");
        }
    }
}
