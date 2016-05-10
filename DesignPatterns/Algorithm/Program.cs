using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            var g = A.a;

            Dictionary<string, int> dic = new Dictionary<string, int>();
            dic.Add("192.168.1.1", 1);
            dic.Add("192.168.1.2", 2);
            dic.Add("192.168.1.3", 1);
            dic.Add("192.168.1.4", 10);

            var locator = new KetamaNodeLocator(dic);
            string a = locator.GetPrimary("123");
            string b = locator.GetPrimary("abc");
            Console.ReadLine();
        }

        static int aaa = 0;
        /// <summary>
        /// 无法中途跳出
        /// </summary>
        /// <returns></returns>
        private static string Test()
        {
            if (aaa < 5)
            {
                aaa++;
                var c = Test();
                Console.WriteLine(aaa);
                return "2";
            }
            else
                return "1";
        }
    }

    public class A
    {
        public static int a;
        A()
        {
            a = 1;
        }
        static A()
        {
            a = 2;
        }
    }
}
