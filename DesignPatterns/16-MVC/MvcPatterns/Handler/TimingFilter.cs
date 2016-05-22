using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcPatterns.Handler
{
    /// <summary>
    /// 性能监控
    /// </summary>
    public class TimingFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            GetTimer(filterContext, "action").Start();
            base.OnActionExecuting(filterContext);
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            GetTimer(filterContext, "action").Stop();
            base.OnActionExecuted(filterContext);
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var renderTimer = GetTimer(filterContext, "render");
            renderTimer.Stop();
            var actionTimer = GetTimer(filterContext, "action");
            if (actionTimer.ElapsedMilliseconds >= 100 || renderTimer.ElapsedMilliseconds >= 100)
            {
                //LogHelper.WriteLog("运营监控(" + filterContext.RouteData.Values["controller"] + ")", String.Format(
                //        "【{0}/{1}】,执行:{2}ms,渲染:{3}ms",
                //        filterContext.RouteData.Values["controller"],
                //        filterContext.RouteData.Values["action"],
                //        actionTimer.ElapsedMilliseconds,
                //        renderTimer.ElapsedMilliseconds
                //    ));
            }
            base.OnResultExecuted(filterContext);
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            GetTimer(filterContext, "render").Start();
            base.OnResultExecuting(filterContext);
        }
        private Stopwatch GetTimer(ControllerContext context, string name)
        {
            string key = "__timer__" + name;
            if (context.HttpContext.Items.Contains(key))
            {
                return (Stopwatch)context.HttpContext.Items[key];
            }

            Stopwatch result = new Stopwatch();
            context.HttpContext.Items[key] = result;
            return result;
        }
    }
}