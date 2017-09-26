using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Common.Net.Core
{
    /// <summary>
    /// 文件实用类
    /// </summary>
    public sealed class FileUtil
    {
        /// <summary>
        /// 以utf8格式读取磁盘某个文本文件
        /// </summary>
        /// <param name="fileName">要读取的文件名（物理路径加文件名）</param>
        /// <returns>返回文件内容，出错返回空字符串</returns>
        public static string LoadString(string fileName)
        {
            if (!File.Exists(fileName))
                return string.Empty;
            try
            {
                var sr = new StreamReader(fileName, Encoding.UTF8);
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

        /// <summary>
        /// 读取Excel表格的数据
        /// </summary>
        /// <param name="filePath">文件的物理路径</param>
        /// <param name="sheetIndex">第几个sheet页</param>
        /// <returns>获取到的数据集（dataReader形式)</returns>
        public static OleDbDataReader GetExcel(string filePath, int sheetIndex)
        {
            OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';");
            conn.Open();
            OleDbCommand command = new OleDbCommand("SELECT * FROM [sheet" + sheetIndex.ToString() + "$]", conn);
            command.CommandType = CommandType.Text;
            OleDbDataReader dr = null;
            try
            {
                dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                return dr;
            }
            catch (Exception)
            {
                if (dr != null)
                    dr.Close();
                conn.Close();
                return null;
            }
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="fileOrPath">文件或目录</param>
        public static void CreateDirectory(string fileOrPath)
        {
            if (fileOrPath == null) return;
            var path = fileOrPath.Contains(".") ? Path.GetDirectoryName(fileOrPath) : fileOrPath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="encoding"></param>
        /// <param name="isCache"></param>
        /// <returns></returns>
        public static string ReadFile(string filename, Encoding encoding, bool isCache)
        {
            string body;
            if (isCache)
            {
                body = (string)HttpContext.Current.Cache[filename];
                if (body == null)
                {
                    body = ReadFile(filename, encoding, false);
                    HttpContext.Current.Cache.Add(filename, body, new System.Web.Caching.CacheDependency(filename), DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, null);
                }
            }
            else
            {
                using (StreamReader sr = new StreamReader(filename, encoding))
                {
                    body = sr.ReadToEnd();
                }
            }
            return body;
        }

        /// <summary>
        /// 备份文件
        /// </summary>
        /// <param name="sourceFileName">源文件名</param>
        /// <param name="destFileName">目标文件名</param>
        /// <param name="overwrite">当目标文件存在时是否覆盖</param>
        /// <returns>操作是否成功</returns>
        public static bool BackupFile(string sourceFileName, string destFileName, bool overwrite)
        {
            if (!System.IO.File.Exists(sourceFileName))
            {
                throw new FileNotFoundException(sourceFileName + "文件不存在！");
            }
            if (!overwrite && System.IO.File.Exists(destFileName))
            {
                return false;
            }
            try
            {
                System.IO.File.Copy(sourceFileName, destFileName, true);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 备份文件,当目标文件存在时覆盖
        /// </summary>
        /// <param name="sourceFileName">源文件名</param>
        /// <param name="destFileName">目标文件名</param>
        /// <returns>操作是否成功</returns>
        public static bool BackupFile(string sourceFileName, string destFileName)
        {
            return BackupFile(sourceFileName, destFileName, true);
        }

        /// <summary>
        /// 恢复文件
        /// </summary>
        /// <param name="backupFileName">备份文件名</param>
        /// <param name="targetFileName">要恢复的文件名</param>
        /// <param name="backupTargetFileName">要恢复文件再次备份的名称,如果为null,则不再备份恢复文件</param>
        /// <returns>操作是否成功</returns>
        public static bool RestoreFile(string backupFileName, string targetFileName, string backupTargetFileName)
        {
            try
            {
                if (!System.IO.File.Exists(backupFileName))
                {
                    throw new FileNotFoundException(backupFileName + "文件不存在！");
                }
                if (backupTargetFileName != null)
                {
                    if (!System.IO.File.Exists(targetFileName))
                    {
                        throw new FileNotFoundException(targetFileName + "文件不存在！无法备份此文件！");
                    }
                    else
                    {
                        System.IO.File.Copy(targetFileName, backupTargetFileName, true);
                    }
                }
                System.IO.File.Delete(targetFileName);
                System.IO.File.Copy(backupFileName, targetFileName);
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }

        /// <summary>
        /// 恢复文件
        /// </summary>
        /// <param name="backupFileName">备份文件名</param>
        /// <param name="targetFileName">恢复的文件名</param>
        /// <returns></returns>
        public static bool RestoreFile(string backupFileName, string targetFileName)
        {
            return RestoreFile(backupFileName, targetFileName, null);
        }

        /// <summary>
        /// 返回文件的二进制流
        /// </summary>
        /// <param name="filename">文件路径</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>byte[]</returns>
        public static byte[] FileToBytes(string filename, Encoding encoding)
        {
            if (File.Exists(filename))
            {
                using (FileStream stream = new FileStream(filename, FileMode.Open))
                {
                    byte[] array = new byte[stream.Length];
                    stream.Read(array, 0, array.Length);
                    return array;
                }
            }
            else
                return null;
        }

        /// <summary>
        /// 为string对象扩充一个把内容以utf8的格式写入到磁盘文件的方法
        /// </summary>
        /// <param name="fileName">要保存的文件名（物理路径加文件名）</param>
        /// <param name="content">要保存的字符串内容</param>
        /// <param name="append">是否追加，否则覆盖文件，是则追加文件</param>
        /// <returns>成功返回"OK"，失败返回失败的错误描述</returns>
        public static string SaveToFile(string content, string fileName, bool append)
        {
            StreamWriter sw = null;
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                }
                sw = new StreamWriter(fileName, append, Encoding.UTF8);
                try
                {
                    sw.Write(content);
                }
                catch (Exception err)
                {
                    return err.ToString();
                }
                return "OK";
            }
            catch (Exception err)
            {
                return err.Message;
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }

        }

        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetExtension(string fileName)
        {
            int i = fileName.LastIndexOf(".") + 1;
            string Name = fileName.Substring(i);
            return Name;
        }
    }
}
