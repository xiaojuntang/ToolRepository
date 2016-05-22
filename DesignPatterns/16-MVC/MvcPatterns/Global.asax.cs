using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MvcPatterns
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ThreadPool.QueueUserWorkItem(o =>
            {
                while (true)
                {
                    if (MyExceptionAttribute.redisClient.GetListCount("errorMsg") > 0)
                    {
                        string msg = MyExceptionAttribute.redisClient.DequeueItemFromList("errorMsg");
                        if (!string.IsNullOrEmpty(msg))
                        {
                            //ILog logger = LogManager.GetLogger("testError");
                            //logger.Error(msg); //将异常信息写入Log4Net中  
                        }
                        else
                        {
                            Thread.Sleep(50);
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
            });
        }
    }
}
