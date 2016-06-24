using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Tasks
{
    /// <summary>
    /// 使用委托实现异步下载    下载委托
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public delegate string AysnDownloadDelegate(string fileName);

    public class DownloadFile
    {
        /// <summary>
        /// 同步下载
        /// </summary>
        /// <param name="fileName"></param>
        public string Downloading(string fileName)
        {
            string filestr = string.Empty;
            Console.WriteLine("下载事件开始执行");
            System.Threading.Thread.Sleep(10000);
            Random rand = new Random();
            StringBuilder builder = new StringBuilder();
            int num;
            for (int i = 0; i < 100; i++)
            {
                num = rand.Next(1000);
                builder.Append(i);
            }
            filestr = builder.ToString();
            Console.WriteLine("下载事件执行结束");
            return filestr;
        }

        public IAsyncResult BeginLoad(string fileName)
        {
            string fileStr = string.Empty;
            AysnDownloadDelegate downDelete = new AysnDownloadDelegate(Downloading);
            return downDelete.BeginInvoke(fileName, DownLoad, downDelete);
        }

        /// <summary>
        /// 异步下载完成后事件
        /// </summary>
        /// <param name="restul"></param>
        public void DownLoad(IAsyncResult restul)
        {
            AysnDownloadDelegate aysnDelegate = restul.AsyncState as AysnDownloadDelegate;
            if (aysnDelegate != null)
            {
                string fileStr = aysnDelegate.EndInvoke(restul);
                if (!string.IsNullOrEmpty(fileStr))
                {
                    Console.WriteLine("下载文件：{0}", fileStr);
                }
                else
                {
                    Console.WriteLine("下载数据为空!");
                }
            }
            else
            {
                Console.WriteLine("下载数据为空!");
            }
        }
    }
}
