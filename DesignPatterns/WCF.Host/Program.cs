using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCF.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MyHelloHost host = new MyHelloHost())
            {
                host.Open();

                Console.ReadLine();
            }
        }
    }

    public class MyHelloHost : IDisposable
    {
        /// <summary>
        /// 定义一个服务对象
        /// </summary>
        private ServiceHost _myhost;
        /// <summary>
        /// 定义一个基地址
        /// </summary>
        public const string BaseAddress = "net.tcp://192.168.160.25:8083";//"net.pipe://192.168.160.25";
        /// <summary>
        /// 可选地址
        /// </summary>
        //public const string UserServiceAddress = "UserSerivce";
        /// <summary>
        /// 服务契约实现类型
        /// </summary>
        public static readonly Type ServiceType = typeof(WCF.Service.Hello);
        /// <summary>
        /// 服务契约接口
        /// </summary>
        public static readonly Type ContractType = typeof(WCF.Service.IHello);
        /// <summary>
        /// 使用的协议
        /// </summary>
        //public static readonly Binding binding = new NetNamedPipeBinding();
        //public static readonly BasicHttpBinding binding = new BasicHttpBinding();
        public static readonly NetTcpBinding netTcpBinding = new NetTcpBinding();

        public ServiceHost Myhost
        {
            get
            {
                return _myhost;
            }
        }

        public MyHelloHost()
        {
            CreateHelloServiceHost();
        }

        protected void CreateHelloServiceHost()
        {
            //binding.Name = "httpbing";
            //binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            //binding.Security.Mode = BasicHttpSecurityMode.None;
           
            _myhost = new ServiceHost(ServiceType, new Uri[] { new Uri(BaseAddress) });
            _myhost.AddServiceEndpoint(ContractType, netTcpBinding, "Hello");
        }

        /// <summary>
        /// 打开服务
        /// </summary>
        public void Open()
        {
            Console.WriteLine("服务启动中……");
            _myhost.Open();
            Console.WriteLine("启动成功");
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (Myhost != null)
            {
                (Myhost as IDisposable).Dispose();
            }
        }
    }
}
