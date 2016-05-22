using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace MvcPatterns.Handler
{
    //队列临时类
    public class QueueInfo
    {
        public string medias { get; set; }
        public string proids { get; set; }
        public string host { get; set; }
        public string userid { get; set; }
        public string feedid { get; set; }
    }

    public class BusinessInfoHelper
    {
        #region 解决发布时含有优质媒体时，前台页面卡住的现象
        //原理：利用生产者消费者模式进行入列出列操作

        public readonly static BusinessInfoHelper Instance = new BusinessInfoHelper();
        private BusinessInfoHelper()
        { }

        private Queue<QueueInfo> ListQueue = new Queue<QueueInfo>();

        public void AddQueue(string medias, string proids, string host, string userid, string feedid) //入列
        {
            QueueInfo queueinfo = new QueueInfo();

            queueinfo.medias = medias;
            queueinfo.proids = proids;
            queueinfo.host = host;
            queueinfo.userid = userid;
            queueinfo.feedid = feedid;
            ListQueue.Enqueue(queueinfo);
        }

        public void Start()//启动
        {
            Thread thread = new Thread(ThreadStart);
            thread.IsBackground = true;
            thread.Start();
        }

        private void ThreadStart()
        {
            while (true)
            {
                if (ListQueue.Count > 0)
                {
                    try
                    {
                        ScanQueue();
                    }
                    catch (Exception ex)
                    {
                        //LO_LogInfo.WLlog(ex.ToString());
                    }
                }
                else
                {
                    //没有任务，休息3秒钟
                    Thread.Sleep(3000);
                }
            }
        }

        //要执行的方法
        private void ScanQueue()
        {
            while (ListQueue.Count > 0)
            {
                try
                {
                    //从队列中取出
                    QueueInfo queueinfo = ListQueue.Dequeue();

                    //取出的queueinfo就可以用了，里面有你要的东西
                    //以下就是处理程序了
                    //。。。。。。

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


        #endregion
    }
}