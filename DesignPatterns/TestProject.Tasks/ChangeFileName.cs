using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Tasks
{
    public class ChangeFileName
    {
        private static string PATH = @"E:\初中微课\中考复习备考课\地理\专题二 中国地理";
        private static string NO = "XKWJNJZKDL";

        /// 〈summary〉  
        /// 获取文件列表  
        /// 〈/summary〉  
        public static void GetFileList()
        {
            //第一种方法
            var files = Directory.GetFiles(PATH, "*.mp4");

            foreach (var file in files)
            {
                ChangeFile(file);
                //Console.WriteLine(file);
            }
        }

        public static bool ChangeFile(string srcPath)
        {
            //int n = 3;
            //string s = n.ToString().PadLeft(4, '0'); //0003
            //s = string.Format("{0:d4}", n);          //0003
            var fileName = srcPath.Substring(srcPath.LastIndexOf("\\") + 1, srcPath.Length - 1 - srcPath.LastIndexOf("\\"));
            var s = fileName.Substring(1, 2);
            var id = "";
            if (s.Contains("讲"))
            {
                id = s.Substring(0, 1);
            }
            else
            {
                id = s;
            }

            id = (Convert.ToInt32(id) + 29).ToString();
            string SID = id.PadLeft(4, '0');
            string desPath = PATH + "\\" + NO + SID + ".mp4";
            Console.WriteLine(desPath);
            try
            {
                if (File.Exists(srcPath))
                {
                    File.Move(srcPath, desPath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
