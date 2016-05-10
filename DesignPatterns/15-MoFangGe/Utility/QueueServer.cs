/**/
/**************************************************
* 文 件 名：QueueServer.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014/9/5 15:30:31
* 文件说明：
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/
using System;
using System.Collections.Generic;
using System.Threading;

namespace Climb.Utility
{
    /// <summary>
    /// 提供一个队列的线程处理
    /// </summary>
    /// <typeparam name="T">处理对象</typeparam>
    public class QueueServer<T> 
    {
        private Thread _thread;//线程
        /// <summary>
        /// 队列
        /// </summary>
        private readonly Queue<T> _queue = new Queue<T>();
        /// <summary>
        /// 是否是后台线程
        /// </summary>
        private bool _isBackground = false;

        private bool _disposed = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="disposed"></param>
        public QueueServer(bool disposed)
        {
            _disposed = disposed;
        }

        //protected override void Dispose(bool disposing)
        //{
        //    try
        //    {
        //        if (!this.disposed)
        //        {
        //            this.ClearItems();
        //        }
        //    }
        //    finally
        //    {
        //        base.Dispose(disposing);
        //    }
        //}

        #region  公共方法

        public void EnqueueItem(T item)
        {
            lock (_queue)
            {
                _queue.Enqueue(item);
            }
            if ((_thread != null) && _thread.IsAlive) return;
            CreateThread();
            _thread.Start();
        }

        public void ClearItems()
        {
            lock (_queue)
            {
                _queue.Clear();
            }
        }

        #endregion

        #region 线程处理

        private void CreateThread()
        {
            _thread = new Thread(new ThreadStart(ThreadProc));
            _thread.IsBackground = _isBackground;
        }

        private void ThreadProc()
        {
            T item = default(T);
            while (true)
            {
                lock (_queue)
                {
                    if (_queue.Count > 0)
                    {
                        item = _queue.Dequeue();
                    }
                    else
                    {
                        break;
                    }
                }
                try
                {
                    OnProcessItem(item);
                }
                catch
                {
                }
            }
        }

        protected virtual void OnProcessItem(T item)
        {
            if (ProcessItem != null)
            {
                ProcessItem(item);
            }
        }

        public event Action<T> ProcessItem;

        #endregion

        #region  属性

        public bool IsBackground
        {
            get
            {
                return _isBackground;
            }
            set
            {
                _isBackground = value;
                _isBackground = true;
                if ((_thread != null) && (_thread.IsAlive))
                {
                    _thread.IsBackground = _isBackground;
                }
            }
        }

        public T[] Items
        {
            get
            {
                lock (_queue)
                {
                    return _queue.ToArray();
                }
            }
        }

        public int QueueCount
        {
            get
            {
                lock (_queue)
                {
                    return _queue.Count;
                }
            }
        }

        #endregion
    }
}
