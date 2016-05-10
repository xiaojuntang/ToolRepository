
/**************************************************
* 文 件 名：FileDirExt.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014/9/3 16:22:03
* 文件说明：文件和文件夹操作
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/

using System;
using System.Globalization;
using System.IO;

namespace Climb.Utility.IOExt
{

  
    /// <summary>
    /// 文件夹操作
    /// </summary>
    public class DirectoryExt
    {
        #region 创建一个目录
        /// <summary>
        /// 创建一个目录
        /// </summary>
        /// <param name="dirPath">目录的绝对路径</param>
        // ReSharper disable once MemberCanBePrivate.Global
        public static void CreateDirectory(string dirPath)
        {
            //如果目录不存在则创建该目录
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }

        #region 生成日期目录

        /// <summary>
        /// 生成日期 文件夹    格式：yyyy\mm\dd
        /// </summary>
        /// <remarks>
        /// 生成时间目录   返回 例如： c:\directory\2009\03\01
        /// </remarks>
        /// <param name="rootPath">绝对路径   [在此目录下建日期目录]</param>
        /// <returns>返回完整路径  </returns>
        public static string CreateDirectoryByDate(string rootPath)
        {
            return CreateDirectoryByDate(rootPath, "yyyy-MM-dd");
        }

        /// <summary>
        /// 相应格式生成日期目录
        /// </summary>
        /// <remarks>
        /// formatString:
        ///              yyyy-MM-dd        :2009\03\01
        ///              yyyy-MM-dd-HH     :2009\03\01\01
        /// </remarks>
        /// <param name="rootPath">绝对路径   [在此目录下建日期目录]</param>
        /// <param name="formatString">格式</param>
        /// <returns>返回完整路径 </returns>
        public static string CreateDirectoryByDate(string rootPath, string formatString)
        {
            if (!IsExistDirectory(rootPath))
                throw new DirectoryNotFoundException("the rootPath is not found");

            //小时目录
            bool hour;

            switch (formatString)
            {
                case "yyyy-MM-dd":
                    hour = false;
                    break;
                case "yyyy-MM-dd-HH":
                    hour = true;
                    break;
                default:
                    hour = false;
                    break;
            }

            string tempPath = Path.Combine(rootPath, DateTime.Now.Year.ToString(CultureInfo.InvariantCulture)) ;

            CreateDirectory(tempPath);

            tempPath = tempPath + "\\" + DateTime.Now.Month.ToString("00");

            CreateDirectory(tempPath);

            tempPath = tempPath + "\\" + DateTime.Now.Day.ToString("00");

            CreateDirectory(tempPath);

            if (hour)
            {
                tempPath = tempPath + "\\" + DateTime.Now.Hour.ToString("00");

                CreateDirectory(tempPath);
            }

            return tempPath;
        }

        #endregion
        #endregion

        #region 删除指定目录
        /// <summary>
        /// 删除指定目录及其所有子目录
        /// </summary>
        /// <param name="dirPath">指定目录的绝对路径</param>
        public static void DeleteDirectory(string dirPath)
        {
            if (Directory.Exists(dirPath))
            {
                Directory.Delete(dirPath, true);
            }
            else
            {
                throw new Exception("文件路径不存在！");
            }
        }
        #endregion

        #region 清空指定目录
        /// <summary>
        /// 清空指定目录下所有文件及子目录,但该目录依然保存.
        /// </summary>
        /// <param name="dirPath">指定目录的绝对路径</param>
        public static void ClearDirectory(string dirPath)
        {
            if (Directory.Exists(dirPath))
            {
                //删除目录中所有的文件
                string[] fileNames = GetFileNames(dirPath);
                foreach (var item in fileNames)
                {
                    File.Delete(item);
                }
                //删除目录中所有的子目录
                string[] directoryNames = GetDirectoryDirs(dirPath);
                foreach (var item in directoryNames)
                {
                    Directory.Delete(item);
                }
            }
        }


        #endregion

        #region 获取指定目录里的文件
        /// <summary>
        /// 获取指定目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static string[] GetFileNames(string directoryPath)
        {
            //如果目录不存在，则抛出异常
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            //获取文件列表
            return Directory.GetFiles(directoryPath);
        }

        /// <summary>
        /// 获取指定目录及子目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            //如果目录不存在，则抛出异常
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            try
            {
                return Directory.GetFiles(directoryPath, searchPattern, isSearchChild ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }
            catch (IOException ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        #endregion

        #region 获取指定目录文件夹目录集合
        /// <summary>
        /// 获取某个文件夹下的文件目录集合
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
        /// <returns>返回文件夹的路径集合</returns>
        public static string[] GetDirectoryDirs(string dirPath)
        {
            return Directory.GetDirectories(dirPath);
        }
        #endregion

        #region 目录可写与空间计算

        /// <summary>
        ///检查目录是否可写，如果可以，返回True，否则False
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsWriteable(string path)
        {
            if (!Directory.Exists(path))
            {
                // if the directory is not exist
                try
                {
                    // if you can create a new directory, it's mean you have write right
                    Directory.CreateDirectory(path);
                }
                catch
                {
                    return false;
                }
            }


            try
            {
                string testFileName = ".test." + Guid.NewGuid().ToString().Substring(0, 5);
                string testFilePath = Path.Combine(path, testFileName);
                File.WriteAllLines(testFilePath, new[] { "test" });
                File.Delete(testFilePath);
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion

        #region 检查磁盘是否有足够的可用空间

        /// <summary>
        /// 检查磁盘是否有足够的可用空间
        /// </summary>
        /// <param name="path"></param>
        /// <param name="requiredSpace"></param>
        /// <returns></returns>
        public static bool IsDiskSpaceEnough(string path, ulong requiredSpace)
        {
            string root = Path.GetPathRoot(path);
            ulong freeSpace = GetFreeSpace(root);

            return requiredSpace <= freeSpace;
        }
        #endregion

        #region 获取驱动盘符的可用空间大小
        /// <summary>
        /// 获取驱动盘符的可用空间大小
        /// </summary>
        /// <param name="driveName">Direve name</param>
        /// <returns>free space (byte)</returns>
        public static ulong GetFreeSpace(string driveName)
        {
            ulong freefreeBytesAvailable;
            try
            {
                DriveInfo drive = new DriveInfo(driveName);
                freefreeBytesAvailable = (ulong)drive.AvailableFreeSpace;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }

            return freefreeBytesAvailable;
        }
        /// <summary>
        /// 计算字节的 kb数量
        /// </summary>
        /// <param name="byteCount"></param>
        /// <returns></returns>
        public static ulong ConvertByteCountToKByteCount(ulong byteCount)
        {
            return byteCount / 1024;
        }
        /// <summary>
        /// 计算字节mb数量
        /// </summary>
        /// <param name="kByteCount"></param>
        /// <returns></returns>
        public static ulong ConvertKByteCountToMByteCount(ulong kByteCount)
        {
            return kByteCount/1024;
        }

        /// <summary>
        /// 计算gb书了
        /// </summary>
        /// <param name="kByteCount"></param>
        /// <returns></returns>
        public static float ConvertMByteCountToGByteCount(float kByteCount)
        {
            return kByteCount / 1024;
        }

        #endregion

        #region 目录操作

        #region 获取指定目录中的子目录列表
        /// <summary>
        /// 获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static string[] GetDirectories(string directoryPath)
        {
            return Directory.GetDirectories(directoryPath);
        }

        /// <summary>
        /// 获取指定目录及子目录中所有子目录列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                return Directory.GetDirectories(directoryPath, searchPattern, isSearchChild ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }
            catch (IOException ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        #endregion
  

        /// <summary>
        /// 确保文件夹被创建
        /// </summary>
        /// <param name="filePath">文件夹全名（含路径）</param>
        public static void AssertDirExist(string filePath)
        {
            DirectoryInfo dir = new DirectoryInfo(filePath);
            if (!dir.Exists)
            {
                dir.Create();
            }
        }

        /// <summary>
        /// 检测指定目录是否存在
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        /// <summary>
        /// 检测指定目录是否为空
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static bool IsEmptyDirectory(string directoryPath)
        {
            try
            {
                //判断是否存在文件
                string[] fileNames = GetFileNames(directoryPath);
                if (fileNames.Length > 0)
                {
                    return false;
                }

                //判断是否存在文件夹
                string[] directoryNames = GetDirectories(directoryPath);
                if (directoryNames.Length > 0)
                {
                    return false;
                }

                return true;
            }
            catch (IOException ex)
            {
                throw new ArgumentException(ex.Message );
            }
        }

        /// <summary>
        /// 检测指定目录中是否存在指定的文件,若要搜索子目录请使用重载方法.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        public static bool ContainFile(string directoryPath, string searchPattern)
        {
            try
            {
                //获取指定的文件列表
                string[] fileNames = GetFileNames(directoryPath, searchPattern, false);

                //判断指定文件是否存在
                return fileNames.Length != 0;
            }
            catch (IOException ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        /// <summary>
        /// 检测指定目录中是否存在指定的文件
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static bool ContainFile(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                //获取指定的文件列表
                string[] fileNames = GetFileNames(directoryPath, searchPattern, true);

                //判断指定文件是否存在
                return fileNames.Length != 0;
            }
            catch (IOException ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        /// <summary>
        /// 取系统目录
        /// </summary>
        /// <returns></returns>
        public static string GetSystemDirectory()
        {
            return Environment.SystemDirectory;
        }

        /// <summary>
        /// 取系统的特别目录
        /// </summary>
        /// <param name="folderType"></param>
        /// <returns></returns>
        public static string GetSpeicalFolder(Environment.SpecialFolder folderType)
        {
            return Environment.GetFolderPath(folderType);
        }

        /// <summary>
        /// 返回当前系统的临时目录
        /// </summary>
        /// <returns></returns>
        public static string GetTempPath()
        {
            return Path.GetTempPath();
        }

        /// <summary>
        /// 取当前目录
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }

        /// <summary>
        /// 设当前目录
        /// </summary>
        /// <param name="path"></param>
        public static void SetCurrentDirectory(string path)
        {
            Directory.SetCurrentDirectory(path);
        }

        /// <summary>
        /// 取路径中不充许存在的字符
        /// </summary>
        /// <returns></returns>
        public static char[] GetInvalidPathChars()
        {
            return Path.GetInvalidPathChars();
        }

        /// <summary>
        /// 取系统所有的逻辑驱动器
        /// </summary>
        /// <returns></returns>
        public static DriveInfo[] GetAllDrives()
        {
            return DriveInfo.GetDrives();
        }

        #endregion
    }
}
