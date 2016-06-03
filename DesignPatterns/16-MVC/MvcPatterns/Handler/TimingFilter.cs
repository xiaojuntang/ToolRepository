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
    public class TimingFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RequestLog(filterContext);
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
            Stopwatch renderTimer = GetTimer(filterContext, "render");
            renderTimer.Stop();
            Stopwatch actionTimer = GetTimer(filterContext, "action");
            string controller = filterContext.RouteData.Values["controller"].ToString();
            string action = filterContext.RouteData.Values["action"].ToString();
            string todayKey = string.Format("{2}-{0}-{1}", controller, action, DateTime.Now.ToString("yyyyMMdd"));
            string yesterDayKey = string.Format("{2}-{0}-{1}", controller, action, DateTime.Now.AddDays(-1).ToString("yyyyMMdd"));
            #region 监控代码
            if (Coste.Monitoring)
            {
                Task.Run(() =>
                {
                    try
                    {
                        #region 性能测试
                        int totalYester = 0, totalToDay = 0;
                        if (Coste.PAGERECORD.TryGetValue(yesterDayKey, out totalYester))
                            Coste.PAGERECORD.TryRemove(yesterDayKey, out totalYester);
                        if (Coste.PAGERECORD.TryGetValue(todayKey, out totalToDay))
                            Coste.PAGERECORD.AddOrUpdate(todayKey, 1, (key, oldValue) => oldValue + 1);
                        else
                            Coste.PAGERECORD.TryAdd(todayKey, 1);
                        #endregion
                    }
                    catch { }
                });
            }
            #endregion

            if (actionTimer.ElapsedMilliseconds >= 200 || renderTimer.ElapsedMilliseconds >= 200)
            {
                int totalDay = 0;
                Coste.PAGERECORD.TryGetValue(todayKey, out totalDay);
                //LogHelper.Debug("运营监控(" + filterContext.RouteData.Values["controller"] + ")", String.Format(
                //        "【{0}/{1}】,执行:{2}ms,渲染:{3}ms",
                //        filterContext.RouteData.Values["controller"],
                //        filterContext.RouteData.Values["action"],
                //        actionTimer.ElapsedMilliseconds,
                //        renderTimer.ElapsedMilliseconds
                //    ));
                LogHelper.Debug(String.Format("运营监控【{0}/{1}】,执行:{2}ms,渲染:{3}ms,今日访问次数:{4}次",
                  controller, action,
                  actionTimer.ElapsedMilliseconds,
                  renderTimer.ElapsedMilliseconds,
                  totalDay
              ));
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

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="filterContext"></param>
        private static void RequestLog(ActionExecutingContext filterContext)
        {
            string controller = filterContext.RouteData.Values["controller"].ToString();
            string action = filterContext.RouteData.Values["action"].ToString();
            if (WebHelper.IsAjax())
            {
                LogHelper.Debug(String.Format("[Ajax] {0}/{1}?{2}", controller, action, filterContext.RequestContext.HttpContext.Request.Form.ToString()));
            }
            else
            {
                if (WebHelper.IsGet())
                {
                    LogHelper.Debug(String.Format("[GET] {0}/{1}?{2}", controller, action, filterContext.RequestContext.HttpContext.Request.QueryString.ToString()));
                }
                else
                {
                    LogHelper.Debug(String.Format("[POST] {0}/{1}?{2}", controller, action, filterContext.RequestContext.HttpContext.Request.Form.ToString()));
                }
            }
        }
    }
}