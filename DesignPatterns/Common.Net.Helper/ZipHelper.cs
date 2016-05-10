using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
/***************************************************************************** 
*        filename :ZipHelper 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.42000 
*        新建项输入的名称:   ZipHelper 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Common.Net.Helper 
*        文件名:             ZipHelper 
*        创建系统时间:       2016/2/3 10:17:46 
*        创建年份:           2016 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Helper
{
    public class ZipHelper
    {
        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="sourceFilePath">待压缩的文件或文件夹路径</param>
        /// <param name="destinationZipFilePath">保存压缩文件的文件名</param>
        /// <param name="level">压缩文件等级</param>
        /// <returns>返回-2说明被压缩文件已经存在，返回1说明压缩成功</returns>            
        public static int CreateFolderZip(string sourceFilePath, string destinationZipFilePath, int level)
        {
            if (sourceFilePath[sourceFilePath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
                sourceFilePath += System.IO.Path.DirectorySeparatorChar;
            if (File.Exists(destinationZipFilePath))
            {
                return -2;
            }
            else
            {
                ZipOutputStream zipStream = new ZipOutputStream(File.Create(destinationZipFilePath));
                zipStream.SetLevel(level);  // 压缩级别 0-9
                CreateZipFiles(sourceFilePath, zipStream);
                zipStream.Finish();
                zipStream.Close();
                return 1;
            }
        }
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="sourceFilePath">待压缩的文件或文件夹路径</param>
        /// <param name="destinationZipFilePath">保存压缩文件的文件名</param>
        /// <param name="level">压缩文件等级</param>
        /// <returns>返回-2说明被压缩文件已经存在，返回1说明压缩成功</returns>            
        public static int CreateFileZip(string sourceFilePath, string destinationZipFilePath, int level)
        {
            if (!Directory.Exists(destinationZipFilePath.Substring(0, destinationZipFilePath.LastIndexOf("\\"))))
            {
                Directory.CreateDirectory(destinationZipFilePath.Substring(0, destinationZipFilePath.LastIndexOf("\\")));
            }
            if (File.Exists(destinationZipFilePath))
            {
                return -2;
            }
            else
            {
                ZipOutputStream zipStream = new ZipOutputStream(File.Create(destinationZipFilePath));
                zipStream.SetLevel(level);  // 压缩级别 0-9

                Crc32 crc = new Crc32();
                FileStream fileStream = File.OpenRead(sourceFilePath);
                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);
                string tempFile = sourceFilePath.Substring(sourceFilePath.LastIndexOf("\\") + 1);
                ZipEntry entry = new ZipEntry(tempFile);
                entry.DateTime = DateTime.Now;
                entry.Size = fileStream.Length;
                fileStream.Close();
                crc.Reset();
                crc.Update(buffer);
                entry.Crc = crc.Value;
                zipStream.PutNextEntry(entry);
                zipStream.Write(buffer, 0, buffer.Length);

                zipStream.Finish();
                zipStream.Close();
                return 1;
            }
        }
        /// <summary>
        /// 递归压缩文件
        /// </summary>
        /// <param name="sourceFilePath">待压缩的文件或文件夹路径</param>
        /// <param name="zipStream">打包结果的zip文件路径（类似 D:\WorkSpace\a.zip）,全路径包括文件名和.zip扩展名
        /// <param name="staticFile"></param>
        private static void CreateZipFiles(string sourceFilePath, ZipOutputStream zipStream)
        {
            Crc32 crc = new Crc32();
            string[] filesArray = Directory.GetFileSystemEntries(sourceFilePath);
            foreach (string file in filesArray)
            {
                if (Directory.Exists(file))                     //如果当前是文件夹，递归
                {
                    CreateZipFiles(file, zipStream);
                }
                else                                            //如果是文件，开始压缩
                {
                    FileStream fileStream = File.OpenRead(file);
                    byte[] buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, buffer.Length);
                    string tempFile = file.Substring(sourceFilePath.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(tempFile);
                    entry.DateTime = DateTime.Now;
                    entry.Size = fileStream.Length;
                    fileStream.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    zipStream.PutNextEntry(entry);
                    zipStream.Write(buffer, 0, buffer.Length);
                }
            }
        }

    }
}
