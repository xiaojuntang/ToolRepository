using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcPatterns.HttpModulDemo
{
    /// <summary> 
    /// 简单防止sql注入 
    /// </summary> 
    public class SqlHttpModule : IHttpModule
    {
        public void Dispose()
        {
        }
        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
        }
        /// <summary> 
        /// 处理sql注入 
        /// </summary> 
        /// <param name="sender"></param> 
        /// <param name="e"></param> 
        private void context_AcquireRequestState(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            try
            {
                string key = string.Empty;
                string value = string.Empty;
                //url提交数据 get方式 
                if (context.Request.QueryString != null)
                {
                    for (int i = 0; i < context.Request.QueryString.Count; i++)
                    {
                        key = context.Request.QueryString.Keys[i];
                        value = context.Server.UrlDecode(context.Request.QueryString[key]);
                        if (!FilterSql(value))
                        {
                            throw new Exception("QueryString(GET) including dangerous sql key word!");
                        }
                    }
                }
                //表单提交数据 post方式 
                if (context.Request.Form != null)
                {
                    for (int i = 0; i < context.Request.Form.Count; i++)
                    {
                        key = context.Request.Form.Keys[i];
                        if (key == "__VIEWSTATE") continue;
                        value = context.Server.HtmlDecode(context.Request.Form[i]);
                        if (!FilterSql(value))
                        {
                            throw new Exception("Request.Form(POST) including dangerous sql key word!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary> 
        /// 过滤非法关键字，这个可以按照项目灵活配置 
        /// </summary> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        private bool FilterSql(string key)
        {
            bool flag = true;
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    //一般配置在公共的文件中，如xml文件，txt文本等等 
                    string sqlStr = "insert |delete |select |update |exec |varchar |drop |creat |declare |truncate |cursor |begin |open|<-- |--> ";
                    string[] sqlStrArr = sqlStr.Split('|');
                    foreach (string strChild in sqlStrArr)
                    {
                        if (key.ToUpper().IndexOf(strChild.ToUpper()) != -1)
                        {
                            flag = false;
                            break;
                        }
                    }
                }
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
    }
}