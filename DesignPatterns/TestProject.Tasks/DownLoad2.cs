using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestProject.Tasks
{
    class DownLoad2
    {
    }

    //进度监视接口
    public interface IProgressMonitor
    {
        void OnProgress(int done, int total);
    }

    public class ProgressVeiw : IProgressMonitor
    {
        const int LENGTH = 50;
        string _last = String.Empty;

        /// <summary>
        /// 进度
        /// </summary>
        /// <param name="done">当前</param>
        /// <param name="total">共计</param>
        public void OnProgress(int done, int total)
        {
            var builder = new StringBuilder();
            builder.Append('[');
            var filled = (int)(done / (total + 0.0) * LENGTH);
            //    for (var i = 0; i Append(i ' : '_')

            builder.Append(']');

            if (done != total)
                builder.AppendFormat("   {0:p0}", done / (total + 0.0));
            else
                builder.Append("   完成！");

            //回退之前打印的字符
            //for (var i = 0; i Length; ++i)
            //    Console.Write('b');

            var state = builder.ToString();
            _last = state;
            Console.Write(state);
        }

    }


    public class Downloader2
    {
        public void Download(string resource)
        {
            //for (int i = 1, size = 10; () =>
            //{
            //    this.OnProrgess(i, size);
            //    Thread.Sleep(500);
            //}) ;



            for (int i = 1; i <= 10; i++)
            {
                this.OnProrgess(i, 10);
                Thread.Sleep(500);
            }
        }

        public void AddMonitor(IProgressMonitor monitor)
        {
            this._monitors.Add(monitor);
        }
        public void RemoveMonitor(IProgressMonitor monitor)
        {
            this._monitors.Remove(monitor);
        }
        private void OnProrgess(int done, int total)
        {
            foreach (var i in this._monitors)
            {
                i.OnProgress(done, total);
            }
        }
        private List<IProgressMonitor> _monitors = new List<IProgressMonitor>();//进度监视者集合
    }


    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        var resouce = "visual studio.iso";
    //        var downloader = new Downloader();
    //        downloader.AddMonitor(new ProgressVeiw());
    //        Console.WriteLine("正在下载 " + resouce);
    //        downloader.Download(resouce);

    //        Console.ReadKey();
    //    }
    //}
}
