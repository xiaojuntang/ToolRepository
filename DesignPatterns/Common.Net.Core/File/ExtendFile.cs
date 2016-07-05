/***************************************************************************** 
*        filename :ExtendFile 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.42000 
*        新建项输入的名称:   ExtendFile 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Common.Net.Func 
*        文件名:             ExtendFile 
*        创建系统时间:       2016/2/3 10:11:18 
*        创建年份:           2016 
/*****************************************************************************/
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web;
using System.Web.UI;

namespace Common.Net.Core
{
    /// <summary>
    /// 文件扩展类
    /// </summary>
    public sealed class ExtendFile
    {
        /// <summary>
        /// 下载磁盘文件 filepage 物理路径
        /// </summary>
        /// <param name="page">下载文件的页面page</param>
        /// <param name="filePath">要下载的文件的物理路径</param>
        /// <param name="saveName">推送到客户端的文件名称</param>
        public static void DownFile(Page page, string filePath, string saveName)
        {
            page.Response.ClearHeaders();
            page.Response.Clear();
            page.Response.Buffer = true;
            page.Response.Charset = "UTF-8";
            page.EnableViewState = false;
            page.Response.ContentEncoding = System.Text.Encoding.UTF8;
            page.Response.AddHeader("content-disposition", "attachment;filename=" +
                HttpUtility.UrlEncode(saveName, System.Text.Encoding.UTF8));
            page.Response.WriteFile(filePath);
        }

        /// <summary>
        /// 下载磁盘文件 filepage 物理路径（浏览器下载）
        /// </summary>
        /// <param name="page"></param>
        /// <param name="filePath"></param>
        /// <param name="saveName"></param>
        public static void DownFile2(Page page, string filePath, string saveName)
        {
            //以字符流的形式下载文件 
            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            page.Response.ContentType = "application/octet-stream";
            //通知浏览器下载文件而不是打开 
            page.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(saveName, System.Text.Encoding.UTF8));
            page.Response.BinaryWrite(bytes);
            page.Response.Flush();
            page.Response.End();
        }

        /// <summary>
        /// 把文本写到客户端 _type :excel txt word html
        /// </summary>
        /// <param name="page">页面类</param>
        /// <param name="content">文本内容</param>
        /// <param name="saveName">推送到客户端的文件名字</param>
        /// <param name="type">文件的格式</param>
        public static void WriteFile(Page page, string content, string saveName, string type)
        {
            page.Response.ClearHeaders();
            page.Response.Clear();
            page.Response.Buffer = true;
            page.Response.Charset = "UTF-8";
            string fileNameTemp = saveName;
            string UserAgent = page.Request.ServerVariables["http_user_agent"].ToLower();
            if (UserAgent.IndexOf("firefox") > 0)
            {
                //火狐不需要编码  mxk 修改
                //fileNameTemp =_page.Server .UrlEncode (fileNameTemp);
            }
            else
            {
                fileNameTemp = HttpUtility.UrlEncode(saveName, System.Text.Encoding.UTF8);
            }
            page.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileNameTemp);
            page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            page.Response.ContentType = "application/ms-" + type;
            page.EnableViewState = false;
            page.Response.Write(content);
            page.Response.End();
        }

        /// <summary>
        /// 把文本写到客户端
        /// </summary>
        /// <param name="page"></param>
        /// <param name="content"></param>
        /// <param name="saveName"></param>
        /// <param name="type"></param>
        public static void WriteFile(HttpContext page, string content, string saveName, string type)
        {
            page.Response.ClearHeaders();
            page.Response.Clear();
            page.Response.Buffer = true;
            page.Response.Charset = "UTF-8";
            string fileNameTemp = saveName;
            string UserAgent = page.Request.ServerVariables["http_user_agent"].ToLower();
            if (UserAgent.IndexOf("firefox") > 0)
            {
                //火狐不需要编码  mxk 修改
                //fileNameTemp =_page.Server .UrlEncode (fileNameTemp);
            }
            else
            {
                fileNameTemp = HttpUtility.UrlEncode(saveName, System.Text.Encoding.UTF8);
            }
            page.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileNameTemp);
            page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            page.Response.ContentType = "application/ms-" + type;
            page.Response.Write(content);
            page.Response.End();
        }

        /// <summary>
        /// 得到上传文件的物理路径，包括文件的名称。不包括扩展名
        /// </summary>
        /// <returns></returns>
        public static string GetUploadPath()
        {
            string tmpStr = DateTime.Now.ToString("yyyyMMddHHmmss");
            string tmpPath = HttpContext.Current.Server.MapPath("/upload/" + tmpStr.Substring(0, 6));
            if (File.Exists(tmpPath))
                return tmpPath + "\\" + tmpStr;
            else
            {
                try
                {
                    Directory.CreateDirectory(tmpPath);
                }
                catch
                {
                    return string.Empty;
                }
                return tmpPath + "\\" + tmpStr;
            }
        }
    }
}
