using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HeplerPatterns
{
    class TTransportTest
    {
    }

    public class ThriftPool
    {
        #region 属性

        private ThriftConfig config;

        /// <summary>
        /// 对象缓存池
        /// </summary>
        //private static Stack<TTransport> objectPool { get; set; }
        private static ConcurrentStack<TTransport> objectPool { get; set; }

        /// <summary>
        /// 同步对象
        /// </summary>
        private static AutoResetEvent resetEvent;

        /// <summary>
        /// 每取走一例，表示激活对象加1，此属性可控制对象池容量
        /// </summary>
        private static volatile int activedCount = 0;

        /// <summary>
        /// 同步对象锁
        /// </summary>
        private static object locker = new object();

        #endregion

        #region 构造函数

        public ThriftPool()
        {
            config = GetConfig();
            CreateResetEvent();
            CreateThriftPool();
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 从对象池取出一个对象
        /// </summary>
        /// <returns></returns>
        public TTransport BorrowInstance()
        {
            lock (locker)
            {
                //Console.WriteLine("借前对象池个数：{0}，工作对象个数：{1}", objectPool.Count(), activedCount);
                TTransport transport;
                //对象池无空闲对象
                if (objectPool.Count() == 0)
                {
                    //对象池已激活对象数达上限
                    if (activedCount == config.MaxActive)
                    {
                        resetEvent.WaitOne();
                    }
                    else
                    {
                        PushObject(CreateInstance());
                    }
                }
                if (!objectPool.TryPop(out transport)) throw new Exception("连接池异常");
                //transport = objectPool.Pop();
                activedCount++;
                //检查对象池存量
                //对象池存量小于最小空闲数，并且激活数小于最大激活数，添加一个对象到对象池
                if (objectPool.Count() < config.MinIdle && activedCount < config.MaxActive)
                {
                    PushObject(CreateInstance());
                }
                if (config.ValidateOnBorrow)
                {
                    ValidateOnBorrow(transport);
                }
                return transport;
            }
        }

        /// <summary>
        /// 归还一个对象
        /// </summary>
        /// <param name="instance"></param>
        public void ReturnInstance(TTransport instance)
        {
            //对象池容量达到上限，不再返回线程池,直接销毁
            if (objectPool.Count() == config.MaxIdle)
            {
                DestoryInstance(instance);
            }
            else
            {
                if (config.ValidateOnReturn)
                {
                    ValidateOnReturn(instance);
                }
                PushObject(instance);
                activedCount--;
                //发通知信号，有对象归还到对象池
                resetEvent.Set();
            }
            //Console.WriteLine("归还后对象池个数：{0}，归还后工作对象个数：{1}", objectPool.Count(), activedCount);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 创建线程同步对象
        /// </summary>
        private void CreateResetEvent()
        {
            if (resetEvent == null)
            {
                resetEvent = new AutoResetEvent(false);
            }
        }

        /// <summary>
        /// 创建对象池
        /// </summary>
        private void CreateThriftPool()
        {
            if (objectPool == null)
            {
                objectPool = new ConcurrentStack<TTransport>(); // new Stack<TTransport>();
            }
        }

        /// <summary>
        /// 添加对象到对象池
        /// </summary>
        /// <param name="transport"></param>
        private void PushObject(TTransport transport)
        {
            objectPool.Push(transport);
        }

        /// <summary>
        /// 创建一个对象
        /// </summary>
        /// <returns></returns>
        private TTransport CreateInstance()
        {
            TTransport transport = new TSocket(config.Host, config.Port);
            transport.Open();
            return transport;
        }

        /// <summary>
        /// 取出对象时校验对象
        /// </summary>
        private void ValidateOnBorrow(TTransport instance)
        {
            if (!instance.IsOpen)
            {
                instance.Open();
            }
        }

        /// <summary>
        /// 归还对象时校验对象
        /// </summary>
        private void ValidateOnReturn(TTransport instance)
        {
            if (instance.IsOpen)
            {
                instance.Close();
            }
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        /// <param name="instance"></param>
        private void DestoryInstance(TTransport instance)
        {
            instance.Flush();
            if (instance.IsOpen)
            {
                instance.Close();
            }
            instance.Dispose();
        }

        /// <summary>
        /// 得到配置参数
        /// </summary>
        /// <returns></returns>
        private ThriftConfig GetConfig()
        {
            return Utility.GetConfig();
        }

        #endregion

    }



    public class ThriftConfig
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 服务端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 传输编码
        /// </summary>
        public Encoding Encode { get; set; }

        /// <summary>
        /// 是否启用压缩
        /// </summary>
        public bool Zipped { get; set; }

        /// <summary>
        /// 连接超时
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// 可以从缓存池中分配对象的最大数量
        /// </summary>
        public int MaxActive { get; set; }

        /// <summary>
        /// 缓存池中最大空闲对象数量
        /// </summary>
        public int MaxIdle { get; set; }

        /// <summary>
        /// 缓存池中最小空闲对象数量
        /// </summary>
        public int MinIdle { get; set; }

        /// <summary>
        /// 阻塞的最大数量
        /// </summary>
        public int MaxWait { get; set; }

        /// <summary>
        /// 从缓存池中分配对象时是否验证对象
        /// </summary>
        public bool ValidateOnBorrow { get; set; }

        /// <summary>
        /// 从缓存池中归还对象时是否验证对象
        /// </summary>
        public bool ValidateOnReturn { get; set; }

        /// <summary>
        /// 从缓存池中挂起对象时是否验证对象
        /// </summary>
        public bool ValidateWhiledIdle { get; set; }
    }

    internal class Service : IDisposable
    {
        ThriftPool pool;
        TTransport transport;
        MyThriftTest.Client client;//我写的测试服务器就叫MyThriftTest
        bool disposed;
        public ThriftConfig config { get; set; }
        public Service()
        {
            disposed = false;
            pool = new ThriftPool();
            transport = pool.BorrowInstance();//从对象池取出一个对象
            TProtocol protocol = new TBinaryProtocol(transport);
            client = new MyThriftTest.Client(protocol);
        }
        public string Invoke(string arg1, string arg2)
        {
            //我写的测试方法就叫invoke
            return client.invoke(arg1, arg2);
        }
        ~Service()
        {
            Dispose(false);
        }
        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    pool.ReturnInstance(transport);//归还当前对象到对象池
                }
                // Release unmanaged resources
                disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
