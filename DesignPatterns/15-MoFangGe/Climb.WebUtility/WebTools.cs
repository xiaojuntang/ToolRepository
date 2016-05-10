
/**************************************************
* 文 件 名：WebTools.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014/9/5 15:35:07
* 文件说明：常用的web工具类
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web;
using System.Web.SessionState;

namespace Climb.WebUtility
{
    /// <summary>
    /// 常用web 工具类
    /// </summary>
    public static class WebTools
    {
        #region 类内部调用

        /// <summary>
        /// HttpContext Current
        /// </summary>
        public static HttpContext Context
        {
            get { return HttpContext.Current; }
        }

        /// <summary>
        /// 获取当前请求的session对象
        /// </summary>
        public static HttpSessionState Seession
        {
            get { return HttpContext.Current.Session; }
        }

        /// <summary>
        /// 获取当前请求对象
        /// </summary>
        public static HttpRequest Request
        {
            get { return Context.Request; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static HttpResponse Response
        {
            get { return Context.Response; }
        }
        #endregion

    }
}
