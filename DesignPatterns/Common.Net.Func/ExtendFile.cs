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

namespace Common.Net.Func
{
    public sealed class ExtendFile
    {
        /// <summary>
        /// 以utf8格式读取磁盘某个文本文件
        /// </summary>
        /// <param name="fileName">要读取的文件名（物理路径加文件名）</param>
        /// <returns>返回文件内容，出错返回空字符串</returns>
        public static string LoadString(string fileName)
        {
            StreamReader sr = null;
            if (!File.Exists(fileName))
                return string.Empty;
            else
            {
                try
                {
                    sr = new StreamReader(fileName, System.Text.Encoding.UTF8);
                    try
                    {
                        return sr.ReadToEnd();
                    }
                    catch
                    {
                        return string.Empty;
                    }
                    finally
                    {
                        sr.Close();
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 下载磁盘文件 filepage 物理路径
        /// </summary>
        /// <param name="_page">下载文件的页面page</param>
        /// <param name="filePath">要下载的文件的物理路径</param>
        /// <param name="saveName">推送到客户端的文件名称</param>
        public static void downFile(System.Web.UI.Page _page, string filePath, string saveName)
        {
            _page.Response.ClearHeaders();
            _page.Response.Clear();
            _page.Response.Buffer = true;
            _page.Response.Charset = "UTF-8";
            _page.EnableViewState = false;
            _page.Response.ContentEncoding = System.Text.Encoding.UTF8;
            _page.Response.AddHeader("content-disposition", "attachment;filename=" +
                HttpUtility.UrlEncode(saveName, System.Text.Encoding.UTF8));
            _page.Response.WriteFile(filePath);
        }
        /// <summary>
        /// 下载磁盘文件 filepage 物理路径（浏览器下载）
        /// </summary>
        /// <param name="_page"></param>
        /// <param name="filePath"></param>
        /// <param name="saveName"></param>
        public static void DownFile(System.Web.UI.Page _page, string filePath, string saveName)
        {
            //以字符流的形式下载文件 
            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            _page.Response.ContentType = "application/octet-stream";
            //通知浏览器下载文件而不是打开 
            _page.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(saveName, System.Text.Encoding.UTF8));
            _page.Response.BinaryWrite(bytes);
            _page.Response.Flush();
            _page.Response.End();
        }
        /// <summary>把文本写到客户段 _type :excel txt word html
        /// 把文本写到客户段 _type :excel txt word html
        /// </summary>
        /// <param name="_page">页面类</param>
        /// <param name="Content">文本内容</param>
        /// <param name="saveName">推送到客户端的文件名字</param>
        /// <param name="_type">文件的格式</param>
        public static void WriteFile(System.Web.UI.Page _page, string Content, string saveName, string _type)
        {
            _page.Response.ClearHeaders();
            _page.Response.Clear();
            _page.Response.Buffer = true;
            _page.Response.Charset = "UTF-8";
            string fileNameTemp = saveName;

            string UserAgent = _page.Request.ServerVariables["http_user_agent"].ToLower();
            if (UserAgent.IndexOf("firefox") > 0)
            {
                //火狐不需要编码  mxk 修改
                //fileNameTemp =_page.Server .UrlEncode (fileNameTemp);
            }
            else
            {
                fileNameTemp = HttpUtility.UrlEncode(saveName, System.Text.Encoding.UTF8);

            }

            _page.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileNameTemp);

            _page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            _page.Response.ContentType = "application/ms-" + _type;
            _page.EnableViewState = false;
            _page.Response.Write(Content);
            _page.Response.End();

        }

        public static void WriteFile(System.Web.HttpContext _page, string Content, string saveName, string _type)
        {
            _page.Response.ClearHeaders();
            _page.Response.Clear();
            _page.Response.Buffer = true;
            _page.Response.Charset = "UTF-8";
            string fileNameTemp = saveName;

            string UserAgent = _page.Request.ServerVariables["http_user_agent"].ToLower();
            if (UserAgent.IndexOf("firefox") > 0)
            {
                //火狐不需要编码  mxk 修改
                //fileNameTemp =_page.Server .UrlEncode (fileNameTemp);
            }
            else
            {
                fileNameTemp = HttpUtility.UrlEncode(saveName, System.Text.Encoding.UTF8);

            }

            _page.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileNameTemp);

            _page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            _page.Response.ContentType = "application/ms-" + _type;
            _page.Response.Write(Content);
            _page.Response.End();

        }



        /// <summary>得到上传文件的物理路径，包括文件的名称。不包括扩展名
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
        /// <summary>读取Excel 文件
        /// 读取Excel 文件（filePath）中 sheet sheetIndex个表格 的数据 如果有标题请去掉
        /// </summary>
        /// <param name="filePath">文件的物理路径</param>
        /// <param name="sheetIndex">第几个sheet页</param>
        /// <returns>获取到的数据集（dataReader形式)</returns>
        public static OleDbDataReader GetExcel(string filePath, int sheetIndex)
        {
            OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';");
            try
            {
                conn.Open();
            }
            catch (Exception err)
            {
                return null;
            }
            OleDbCommand myCm = new OleDbCommand("SELECT * FROM [sheet" + sheetIndex.ToString() + "$]", conn);
            myCm.CommandType = CommandType.Text;
            OleDbDataReader myDr = null;
            try
            {
                myDr = myCm.ExecuteReader(CommandBehavior.CloseConnection);
                return myDr;
            }
            catch (Exception err)
            {
                if (myDr != null)
                    myDr.Close();
                conn.Close();
                return null;
            }
        }
    }
}
