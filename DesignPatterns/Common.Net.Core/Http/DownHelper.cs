using System;
using System.IO;
using System.Web;

namespace Common.Net.Core
{
    /// <summary>
    /// Web文档下载类
    /// </summary>
    public class DownHelper
    {
        readonly HttpResponse Response = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        public DownHelper()
        {
            Response = HttpContext.Current.Response;
        }

        /// <summary>
        /// 文档下载
        /// </summary>
        /// <param name="stream">文档流</param>
        /// <param name="fileName">文档名称</param>
        public void DownloadByOutputStreamBlock(System.IO.Stream stream, string fileName)
        {
            using (stream)
            {
                //将流的位置设置到开始位置。
                stream.Position = 0;
                //块大小
                long ChunkSize = 102400;
                //建立100k的缓冲区
                byte[] buffer = new byte[ChunkSize];
                //已读字节数
                long dataLengthToRead = stream.Length;

                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", $"attachment; filename={HttpUtility.UrlPathEncode(fileName)}");

                while (dataLengthToRead > 0 && Response.IsClientConnected)
                {
                    int lengthRead = stream.Read(buffer, 0, Convert.ToInt32(ChunkSize));//读取的大小
                    Response.OutputStream.Write(buffer, 0, lengthRead);
                    Response.Flush();
                    Response.Clear();
                    dataLengthToRead -= lengthRead;
                }
                Response.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fielName"></param>
        public void DownloadByTransmitFile(string filePath, string fielName)
        {
            FileInfo info = new FileInfo(filePath);
            long fileSize = info.Length;
            Response.Clear();
            Response.ContentType = "application/x-zip-compressed";
            Response.AddHeader("Content-Disposition",
                string.Format("attachment;filename={0}", HttpUtility.UrlPathEncode(fielName)));
            //不指明Content-Length用Flush的话不会显示下载进度  
            Response.AddHeader("Content-Length", fileSize.ToString());
            Response.TransmitFile(filePath, 0, fileSize);
            Response.Flush();
            Response.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        public void DownloadByWriteFile(string filePath, string fileName)
        {
            FileInfo info = new FileInfo(filePath);
            long fileSize = info.Length;
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", HttpUtility.UrlPathEncode(fileName)));

            //指定文件大小  
            Response.AddHeader("Content-Length", fileSize.ToString());
            Response.WriteFile(filePath, 0, fileSize);
            Response.Flush();
            Response.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        public void DownloadByOutputStreamBlock(string filePath, string fileName)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                //指定块大小  
                long chunkSize = 102400;
                //建立一个100K的缓冲区  
                byte[] buffer = new byte[chunkSize];
                //已读的字节数  
                long dataToRead = stream.Length;

                //添加Http头  
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", HttpUtility.UrlPathEncode(fileName)));
                Response.AddHeader("Content-Length", dataToRead.ToString());

                while (dataToRead > 0 && Response.IsClientConnected)
                {
                    int length = stream.Read(buffer, 0, Convert.ToInt32(chunkSize));
                    Response.OutputStream.Write(buffer, 0, length);
                    Response.Flush();
                    Response.Clear();
                    dataToRead -= length;
                }
                Response.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        public void DownloadByBinary(string filePath, string fileName)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                //指定块大小  
                long chunkSize = 102400;
                //建立一个100K的缓冲区  
                byte[] bytes = new byte[chunkSize];
                //已读的字节数  
                long dataToRead = stream.Length;

                //添加Http头  
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", HttpUtility.UrlPathEncode(fileName)));

                Response.AddHeader("Content-Length", bytes.Length.ToString());
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        public void DownloadByBinaryBlock(string filePath, string fileName)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                //指定块大小  
                long chunkSize = 102400;
                //建立一个100K的缓冲区  
                byte[] buffer = new byte[chunkSize];
                //已读的字节数  
                long dataToRead = stream.Length;

                //添加Http头  
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", HttpUtility.UrlPathEncode(fileName)));
                Response.AddHeader("Content-Length", dataToRead.ToString());

                while (dataToRead > 0 && Response.IsClientConnected)
                {
                    int length = stream.Read(buffer, 0, Convert.ToInt32(chunkSize));
                    Response.BinaryWrite(buffer);
                    Response.Flush();
                    Response.Clear();

                    dataToRead -= length;
                }
                Response.Close();
            }
        }
    }
}
