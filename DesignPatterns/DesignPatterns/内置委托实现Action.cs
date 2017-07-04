using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns
{
    public delegate void MyDelegate<T>(T arg);
    public class 内置委托实现Action
    {
        static void M()
        {
            string s = "123";
            int a = 123;
            MyDelegate<string> md = new MyDelegate<string>(DelegateEvent);
            md(s);

            MyDelegate<int> md2 = new MyDelegate<int>(DelegateEvent2);
            md2(a);
        }

        static void DelegateEvent(string a)
        {
            Console.WriteLine(a);
        }

        static void DelegateEvent2(int a)
        {
            Console.WriteLine(a);
        }
    }


    public class Test
    {
        private static string ccc = "d";
        static void M()
        {
            string s = "123";
            int a = 123;
            Action<string> md = new Action<string>(DelegateEvent);
            md(s);

            Action<int> md2 = new Action<int>(DelegateEvent2);
            md2(a);
        }

        static void DelegateEvent(string a)
        {
            Console.WriteLine(a);
        }

        static void DelegateEvent2(int a)
        {
            Console.WriteLine(a);
        }

        public void dddd()
        {
            string abc = ccc;
        }
    }
}
