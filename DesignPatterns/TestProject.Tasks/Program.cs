using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestProject.Tasks
{
    class Program
    {
        static void Main(string[] args)
        {
            ChangeFileName.GetFileList();

            //Parallel.For(0, 10, i => Console.WriteLine(i));

            //NewMethod4();
            Console.ReadLine();
        }

        private static void NewMethod()
        {
            Task<int> t1 = Task.Factory.StartNew<int>(() =>
            {
                Thread.Sleep(1000);
                return 1;
            });
            Task<int> t2 = Task.Factory.StartNew<int>(() =>
            {
                Thread.Sleep(2000);
                return 2;
            });
            Task<int> t3 = Task.Factory.StartNew<int>(() =>
            {
                Thread.Sleep(3000);
                return 3;
            });
            var all = Task.WhenAll(t1, t2, t3);
            while (true)
            {
                Console.WriteLine("{0} IsCompleted:{1}", DateTime.Now.ToString("HH:mm:ss fff"), all.IsCompleted);
                Thread.Sleep(200);
                if (all.IsCompleted)
                    break;
            }
            var c = all.Result;
            Console.WriteLine(c.Sum());
        }

        public async static void NewMethod2()
        {
            Console.WriteLine("异步方法调用开始");
            var result1 = TMothd1();
            var result2 = TMothd2();
            var result3 = TMothd3();
            Console.WriteLine("异步方法完成");
            int r1 = await result1;
            int r2 = await result2;
            int r3 = await result3;
            Console.WriteLine("{0},{1},{2}", r1, r2, r3);
        }

        /// <summary>
        /// http://www.cnblogs.com/pengstone/archive/2012/12/23/2830238.html
        /// </summary>
        private static void NewMethod3()
        {
            Task<string[]> parent = new Task<string[]>(state =>
            {
                Console.WriteLine(state);
                string[] result = new string[2];
                //创建并启动子任务
                new Task(() => { result[0] = "我是子任务1。"; }, TaskCreationOptions.AttachedToParent).Start();
                new Task(() => { result[1] = "我是子任务2。"; }, TaskCreationOptions.AttachedToParent).Start();
                return result;
            }, "我是父任务，并在我的处理过程中创建多个子任务，所有子任务完成以后我才会结束执行。");
            //任务处理完成后执行的操作
            parent.ContinueWith(t =>
            {
                Array.ForEach(t.Result, r => Console.WriteLine(r));
            });
            //启动父任务
            parent.Start();
            Console.Read();
        }

        private static void NewMethod4()
        {
            //定义一个与方法声明相同的委托来异步执行方法
            Func<int> apm = () =>
            {
                //耗时的计算任务
                Thread.Sleep(2000);
                int sum = 0;
                Console.WriteLine(".........");
                for (int i = 0; i < 100; i++)
                {
                    sum += i;
                }
                return sum;
            };
            apm.BeginInvoke(state =>
            {
                int sum = apm.EndInvoke(state);
                Console.WriteLine("使用委托异步执行方法，结果为：{0}", sum.ToString());
            }, null);
        }

        public async static Task<int> TMothd1()
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5 * 1000);
                Console.WriteLine("TMothd1 完成");
                return 1;
            });
        }

        public async static Task<int> TMothd2()
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(10 * 1000);
                Console.WriteLine("TMothd2 完成");
                return 2;
            });
        }

        public async static Task<int> TMothd3()
        {
            return await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(15 * 1000);
                Console.WriteLine("TMothd3 完成");
                return 3;
            });
        }
    }
}
