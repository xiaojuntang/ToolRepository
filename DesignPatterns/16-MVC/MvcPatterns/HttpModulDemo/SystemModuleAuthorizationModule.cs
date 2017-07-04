using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcPatterns.HttpModulDemo
{
    /// <summary>  
    /// 说明：检查用户是否有权使用模块的Module  
    /// 联系：www.hello-code.com  
    /// </summary>  
    public class SystemModuleAuthorizationModule : IHttpModule
    {

        #region IHttpModule 成员  

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
        }

        void context_AcquireRequestState(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;

            // 如果用户未登录，则无需检查模块授权，因为请求会被用户登录Module重定向到登录页面。  
            if (application.Session["UserName"] == null)
                return;

            // 获取用户名和Url  
            string userName = application.Session["UserName"].ToString();
            string url = application.Request.Url.ToString();

            // 如果用户没有被授权，请求被终止，并打印提示信息。  
            if (!Validator.CanUseModule(userName, url))
            {
                application.CompleteRequest();
                application.Response.Write(string.Format("对不起！{0}，您无权访问此模块！", userName));
            }
        }

        #endregion
    }
}