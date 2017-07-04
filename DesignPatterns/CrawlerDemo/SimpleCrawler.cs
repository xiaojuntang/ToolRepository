using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerDemo
{
    public class SimpleCrawler
    {

    }

    /// <summary>
    /// 爬虫启动事件
    /// </summary>
    public class OnStartEventArgs
    {
        public Uri Uri { get; set; }

        public OnStartEventArgs(Uri uri)
        {
            this.Uri = uri;
        }
    }

    /// <summary>
    /// 爬虫完成事件
    /// </summary>
    public class OnCompleteEventArgs
    {
        /// <summary>
        /// URL地址
        /// </summary>
        public Uri Uri { get; private set; }
        /// <summary>
        /// 任务线程ID
        /// </summary>
        public int ThreadId { get; private set; }
        /// <summary>
        /// 页面源代码
        /// </summary>
        public string PageSource { get; private set; }
        /// <summary>
        /// 爬虫请求执行时间
        /// </summary>
        public long Milliseconds { get; private set; }

        public OnCompleteEventArgs(Uri uri,int threadId,string pageSource,long milliseconds)
        {
            this.Uri = uri;
            this.ThreadId = threadId;
            this.Milliseconds = milliseconds;
            this.PageSource = pageSource;
        }
    }



}
