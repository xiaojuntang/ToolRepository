using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using System;
using System.IO;
using System.Linq;

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
    }
}
