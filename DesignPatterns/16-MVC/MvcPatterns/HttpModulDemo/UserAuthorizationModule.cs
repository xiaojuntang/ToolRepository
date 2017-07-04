using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcPatterns.HttpModulDemo
{
    /// <summary>  
    /// 说明：检查用户登录的Module  
    /// 联系：www.hello-code.com  
    /// </summary>  
    public class UserAuthorizationModule : IHttpModule
    {
        #region IHttpModule 成员  

        public void Dispose()
        { }

        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
        }

        void context_AcquireRequestState(object sender, EventArgs e)
        {
            // 获取应用程序  
            HttpApplication application = (HttpApplication)sender;

            // 检查用户是否已经登录  
            if (application.Context.Session["UserName"] == null || application.Context.Session["UserName"].ToString().Trim() == "")
            {
                // 获取Url  
                string requestUrl = application.Request.Url.ToString();
                string requestPage = requestUrl.Substring(requestUrl.LastIndexOf('/') + 1);

                // 如果请求的页面不是登录页面，刚重定向到登录页面。  
                if (requestPage != "Login.aspx")
                    application.Server.Transfer("Login.aspx");
            }
            else
            {
                // 已经登录，向每个请求的页面打印欢迎词。  
                application.Response.Write(string.Format("欢迎您！{0}！", application.Context.Session["UserName"]));
            }
        }

        #endregion
    }

 

    public class Validator
    {
        /// <summary>  
        /// 检查用户是否被授权使用模块。  
        /// 文野可以使用模块，stwyhm可以使用模块，所有用户都可以请求限定模块以外的页面  
        /// </summary>  
        /// <param name="userName"></param>  
        /// <param name="url"></param>  
        /// <returns></returns>  
        public static bool CanUseModule(string userName, string url)
        {
            if (!url.Contains("模块"))
                return true;
            else if (userName == "文野" && url.Contains("模块1"))
                return true;
            else if (userName == "stwyhm" && url.Contains("模块2"))
                return true;
            else
                return false;
        }
    }
}