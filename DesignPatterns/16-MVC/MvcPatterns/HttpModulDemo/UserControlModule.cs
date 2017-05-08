using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MvcPatterns.HttpModulDemo
{
    public class UserControlModule : IHttpModule
    {

        #region IHttpModule

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new System.EventHandler(httpApplication_BeginRequest);

        }

        #endregion

        #region Registered event handlers
        public void httpApplication_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication httpApplication = (HttpApplication)sender;


            HttpCookie UserCookie = httpApplication.Context.Request.Cookies["UserCookie"];


            //如果用户未登录就跳转到登录界面
            if (UserCookie == null)
            {

                httpApplication.Context.Response.Redirect("/Portal/ProtalLogin.aspx");

            }

            //根据当前路径取得用户权限
            DataTable dt = new DataTable();//Authority.GetAuthorityByUserAndPath(UserCookie["UserID"], httpApplication.Context.Request.RawUrl);
            if (dt.Rows.Count == 1)
            {
                UserCookie["ISVIEW"] = dt.Rows[0]["ISVIEW"].ToString();
                UserCookie["ISADD"] = dt.Rows[0]["ISADD"].ToString();
                UserCookie["ISUPDATE"] = dt.Rows[0]["ISUPDATE"].ToString();
                UserCookie["ISDELETE"] = dt.Rows[0]["ISDELETE"].ToString();
                UserCookie["ISCONFIRM"] = dt.Rows[0]["ISCONFIRM"].ToString();

            }
            else
            {
                UserCookie["ISVIEW"] = "0";
                UserCookie["ISADD"] = "0";
                UserCookie["ISUPDATE"] = "0";
                UserCookie["ISDELETE"] = "0";
                UserCookie["ISCONFIRM"] = "0";

            }



            #endregion

            if (UserCookie["ISVIEW"].ToString().Equals("0"))
            {

                httpApplication.Context.Response.Redirect("/Portal/ProtalLogin.aspx");

            }

        }
    }
}