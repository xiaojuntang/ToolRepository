using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcPatterns.Handler
{
    /// <summary>
    /// 异常持久化类
    /// </summary>
    public class ErrorAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// 触发异常时调用的方法
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            string message = string.Format("消息类型：{0}<br>消息内容：{1}<br>引发异常的方法：{2}<br>引发异常的对象：{3}<br>异常目录：{4}<br>异常文件：{5}"
                    , filterContext.Exception.GetType().Name
                    , filterContext.Exception.Message
                    , filterContext.Exception.TargetSite
                    , filterContext.Exception.Source
                    , filterContext.RouteData.GetRequiredString("controller")
                    , filterContext.RouteData.GetRequiredString("action"));
            //VLog.VLogFactory.CreateVLog().ErrorLog(message); //TODO:将 ex 错误对象记录到系统日志模块
            base.OnException(filterContext);
        }
    }
}