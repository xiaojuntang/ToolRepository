using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SingletonPatterns
{
    public class Tst {
        public string A()
        {
            return "";
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            GenericSingleton<Tst>.GetInstance().A();
        }
    }

    #region 单例模式版本---1
    /// <summary>
    /// 该版本主要存在的问题（线程安全问题）当2个请求同时访问时可能会出现2个实例尽管不会异常
    /// 不能保证运行过程中只有一个实例
    /// </summary>
    public sealed class Singleton1
    {
        private static Singleton1 _instance;
        private Singleton1() { }

        public static Singleton1 Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Singleton1();
                return _instance;
            }
        }
    }
    #endregion

    #region 单例模式版本---2
    /// <summary>
    /// 使用在静态私有字段上通过new的形式，来保证在该类第一次被调用的时候创建实例，是不错的方式
    /// 但有一点需要注意的是，C#其实并不保证实例创建的时机，因为C#规范只是在IL里标记该静态字段是BeforeFieldInit
    /// 也就是说静态字段可能在第一次被使用的时候创建，也可能你没使用了，它也帮你创建了，也就是周期更早
    /// 我们不能确定到底是什么创建的实例。
    /// </summary>
    public sealed class Singleton2
    {
        private static readonly Singleton2 _instance = new Singleton2();

        /// <summary>
        /// 私有构造函数，确保用户在外部不能实例化新的实例
        /// </summary>
        private Singleton2() { }

        public static Singleton2 Instance
        {
            get
            {
                return _instance;
            }
        }
    }
    #endregion

    #region 单例模式版本---3
    /// <summary>
    /// 使用volatile来修饰，是个不错的注意，确保instance在被访问之前被赋值实例，一般情况都是用这种方式来实现单例。
    /// </summary>
    public sealed class Singleton3
    {
        private static volatile Singleton3 _instance = null;

        /// <summary>
        /// Lock对象 线程安全使用
        /// </summary>
        public static readonly object syncObject = new object();

        private Singleton3() { }

        public static Singleton3 Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncObject)
                    {
                        //Double-Check Locking 双重检查锁定
                        if (_instance == null)
                            _instance = new Singleton3();
                    }
                }
                return _instance;
            }
        }
    }
    #endregion

    #region 单例模式版本---4
    /// <summary>
    /// 这种方式，其实是很不错的，因为他确实保证了是个延迟初始化的单例（通过加静态构造函数）
    /// 但是该静态构造函数里没有东西哦，所以能有时候会引起误解，尤其是在code review或者代码优化的时候
    /// 不熟悉的人可能直接帮你删除了这段代码，那就又回到了版本2了哦，所以还是需要注意的
    /// 不过如果你在这个时机正好有代码需要执行的话，那也不错。
    /// </summary>
    public class Singleton4
    {
        /// <summary>
        /// 因为下面声明了静态构造函数，所以在第一次访问该类之前，new Singleton()语句不会执行
        /// </summary>
        private static readonly Singleton4 _instance = new Singleton4();

        public static Singleton4 Instance
        {
            get { return _instance; }
        }

        private Singleton4() { }

        /// <summary>
        /// 声明静态构造函数就是为了删除IL里的BeforeFieldInit标记,以去静态自动在使用之前被初始化
        /// </summary>
        static Singleton4() { }
    }
    #endregion

    #region 泛型单例模式的实现---5
    public class GenericSingleton<T> where T : class //,new
    {
        private static T instance;
        private static readonly object locker = new object();

        public static T GetInstance()
        {
            //没有第一重 instance == null 的话，每一次有线程进入 GetInstance()时，
            //均会执行锁定操作来实现线程同步，
            //非常耗费性能 增加第一重instance ==null 
            //成立时的情况下执行一次锁定以实现线程同步
            if (instance == null)
            {
                //Double-Check Locking 双重检查锁定
                lock (locker)
                {
                    if (instance == null)
                    {
                        //instance = new T();
                        //需要非公共的无参构造函数，不能使用new T() ,
                        //new不支持非公共的无参构造函数
                        instance = Activator.CreateInstance(typeof(T), true) as T;
                        //第二个参数防止异常：“没有为该对象定义无参数的构造函数。”
                    }
                }
            }
            return instance;
        }
        #endregion
    }

    #region 单例模式版本---6

    #endregion

    public class AnotherResource
    {
        public void Dispose() { }
    }

    public class SampleClass : IDisposable
    {
        //演示创建一个非托管资源          
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        //演示创建一个托管资源          
        private AnotherResource managedResource = new AnotherResource();

        private bool disposed = false;
        /// <summary>          
        /// 实现IDisposable中的Dispose方法         
        /// </summary>          
        public void Dispose()
        {
            //必须为true             
            Dispose(true);
            //通知垃圾回收机制不再调用终结器（析构器）         
            GC.SuppressFinalize(this);
        }
        /// <summary>         
        /// 不是必要的，提供一个Close方法仅仅是为了更符合其他语言（如C++）的规范          
        /// </summary>          
        public void Close()
        {
            Dispose();
        }

        /// <summary>       
        /// 必须，以备程序员忘记了显式调用Dispose方法      
        /// </summary>         
        ~SampleClass()
        {
            //必须为false           
            Dispose(false);
        }
        /// <summary>          
        /// 非密封类修饰用protected virtual
        /// 密封类修饰用private
        /// </summary>         
        /// <param name="disposing"></param>    
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                // 清理托管资源              
                if (managedResource != null)
                {
                    managedResource.Dispose();
                    managedResource = null;
                }
            }
            // 清理非托管资源           
            if (nativeResource != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(nativeResource);
                nativeResource = IntPtr.Zero;
            }
            //让类型知道自己已经被释放           
            disposed = true;
        }
    }


    /// <summary>
    /// 单例模式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : class, new()
    {
        static object lockt = new object();
        static T t = null;
        /// <summary>
        /// 加锁单例
        /// </summary>
        public static T Current
        {
            get
            {
                if (t == null)
                {
                    lock (lockt)
                    {
                        t = t ?? new T();
                    }
                }
                return t;
            }
        }

        /// <summary>
        /// 不加锁单例
        /// </summary>
        public static T CurrentUnLock
        {
            get
            {
                if (t == null)
                {
                    t = t ?? new T();
                }
                return t;
            }
        }
    }
}
}

