using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerAndConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Info info = new Info();
            Producer pro = new Producer(info);
            Consumer con = new Consumer(info);

            Thread th1 = new Thread(new ThreadStart(pro.Produce));
            Thread th2 = new Thread(new ThreadStart(con.Consume));

            th1.Start();
            th2.Start();

            //th1.Join();
            //th2.Join();
        }
    }
}
