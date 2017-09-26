using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace AopPatterns2
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            //动态注入拦截器Calllogger,启动类代理拦截
            //builder.RegisterType<Circle>().EnableClassInterceptors();
            //builder.RegisterType<Circle2>().As<ISH>().InterceptedBy(typeof(CallLogger)).EnableInterfaceInterceptors();//无需加attr

            builder.RegisterType<Circle2>().As<ISH>().EnableInterfaceInterceptors();
            // 动态注入拦截器CallLogger
            //builder.RegisterType<Circle>().InterceptedBy(typeof(CallLogger)).EnableClassInterceptors();

            //启用类代理拦截
            builder.RegisterType<Circle>().EnableClassInterceptors();

            builder.RegisterType<TransactionInterceptor>();


            //注入拦截器到容器
            builder.Register(c => new CallLogger(Console.Out));

            //创建容器
            var container = builder.Build();

            //从容器中获取类型
            //Circle cire = container.Resolve<Circle>();
            //cire.Area();

            ISH cire2 = container.Resolve<ISH>();
            cire2.Area("a",5,8.0);












            Console.ReadKey();
        }
    }

    public class CallLogger : IInterceptor
    {
        TextWriter _output;

        public CallLogger(TextWriter _output)
        {
            this._output = _output;
        }

        public void Intercept(IInvocation invocation)
        {
            _output.WriteLine("你正在调用方法 \"{0}\"  参数是 {1}... ",
               invocation.Method.Name,
               string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));

            //在被拦截的方法执行完毕后 继续执行
            invocation.Proceed();

            _output.WriteLine("方法执行完毕，返回结果：{0}", invocation.ReturnValue);
        }
    }


    /// <summary>
    /// 事务 拦截器
    /// </summary>
    public class TransactionInterceptor : IInterceptor
    {
        //可自行实现日志器，此处可忽略
        /// <summary>
        /// 日志记录器
        /// </summary>
        //private static readonly ILog Logger = Log.GetLog(typeof(TransactionInterceptor));

        // 是否开发模式
        private bool isDev = false;
        public void Intercept(IInvocation invocation)
        {
            if (!isDev)
            {
                MethodInfo methodInfo = invocation.MethodInvocationTarget;
                if (methodInfo == null)
                {
                    methodInfo = invocation.Method;
                }

                TransactionCallHandlerAttribute transaction =
                    methodInfo.GetCustomAttributes<TransactionCallHandlerAttribute>(true).FirstOrDefault();
                if (transaction != null)
                {
                    TransactionOptions transactionOptions = new TransactionOptions();
                    //设置事务隔离级别
                    transactionOptions.IsolationLevel = transaction.IsolationLevel;
                    //设置事务超时时间为60秒
                    transactionOptions.Timeout = new TimeSpan(0, 0, transaction.Timeout);
                    using (TransactionScope scope = new TransactionScope(transaction.ScopeOption, transactionOptions))
                    {
                        try
                        {
                            //实现事务性工作
                            invocation.Proceed();
                            scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            // 记录异常
                            throw ex;
                        }
                    }
                }
                else
                {
                    // 没有事务时直接执行方法
                    invocation.Proceed();
                }
            }
            else
            {
                // 开发模式直接跳过拦截
                invocation.Proceed();
            }
        }
    }


    /// <summary>
    /// 开启事务属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class TransactionCallHandlerAttribute : Attribute
    {
        /// <summary>
        /// 超时时间
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// 事务范围
        /// </summary>
        public TransactionScopeOption ScopeOption { get; set; }

        /// <summary>
        /// 事务隔离级别
        /// </summary>
        public IsolationLevel IsolationLevel { get; set; }

        public TransactionCallHandlerAttribute()
        {
            Timeout = 60;
            ScopeOption = TransactionScopeOption.Required;
            IsolationLevel = IsolationLevel.ReadCommitted;
        }
    }



























    [Intercept(typeof(CallLogger))]
    public class Circle
    {
        /// <summary>
        /// 必须是虚方法
        /// </summary>
        public virtual void Area()
        {
            Console.WriteLine("AAAAAAAAAAA");
        }
    }

    public interface ISH
    {
        string Area(string a,int b,double c);
        void AddArticle(string name);
    }

    [Intercept(typeof(CallLogger))]
    public class Circle2 : ISH
    {
        //重写父类抽象方法
        public string Area(string a, int b, double c)
        {
            Console.WriteLine("你正在调用圆求面积的方法");
            return (b * c).ToString();
        }

        [TransactionCallHandler]
        public void AddArticle(string name)
        {
            //BasArticle model = new BasArticle();
            //model.ArticleID = Guid.Empty;//故意重复，判断是否会回滚。  
            //model.Code = TimestampId.GetInstance().GetId();
            //model.Name = name;
            //model.Status = 1;
            //model.Creater = "测试";
            //model.Editor = "测试";
            //this._basArticleRepository.Insert(model);
        }
    }
}
