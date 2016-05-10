using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace WCF.Client
{
    class Program
    {
        protected static WCF.Service.IHello proxy2;
        public static System.Timers.Timer _timer = new System.Timers.Timer();
        public static int Index = 0;

        static void Main(string[] args)
        {
            //定义绑定与服务地址 
            //Binding httpBinding = new BasicHttpBinding();
            //EndpointAddress httpAddr = new EndpointAddress("http://localhost:8080/wcf");
            //利用ChannelFactory创建一个IData的代理对象，指定binding与address，而不用配置文件中的  
            //var proxy = new ChannelFactory<Server.IData>(httpBinding, httpAddr).CreateChannel();
            //调用SayHello方法并关闭连接 
            //Console.WriteLine(proxy.SayHello("WCF"));
            //((IChannel)proxy).Close();

            try
            {
                //换TCP绑定试试 
                Binding tcpBinding = new NetTcpBinding();
                EndpointAddress tcpAddr = new EndpointAddress("net.tcp://192.168.160.25:8083/Hello");
                proxy2 = new ChannelFactory<WCF.Service.IHello>(tcpBinding, tcpAddr).CreateChannel();
                if (proxy2 == null)
                    Console.WriteLine("异常");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //((IChannel)proxy2).Close();
            _timer.Interval = 100;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
            Console.ReadLine();
        }

        private static void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            Console.WriteLine(proxy2.SayHello("WCF" + Index++));
        }
    }
}
