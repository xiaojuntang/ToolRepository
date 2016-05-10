using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interview
{
    class Program
    {
        static void Main(string[] args)
        {
            #region MyRegion
            //int i = 10;
            //Console.WriteLine(i++);
            //Console.WriteLine(++i);
            //Console.WriteLine(i = 20);
            //Console.WriteLine(i == 20); 
            #endregion

            #region MyRegion
            //bool b = false;
            //if (b == true)
            //{
            //    Console.WriteLine("yes");
            //}
            //else
            //{
            //    Console.WriteLine("no");
            //} 
            #endregion

            //第1题
            //Que1();

            //第2题
            //var b = new B();//输出x+y=0
            //b.OutputText();//输出x+y=1

            //第3题
            //int c = Foo(3);

            //第4题
            //Console.WriteLine(Que2());
            //Console.WriteLine(Que3().Age);

            Console.ReadLine();

        }

        private static void Que1()
        {
            string test = "hello";
            test = test.ToUpper();
            test = test.Insert(0, " WORLD");
            Console.WriteLine(test);//空格HELLOWORLD
        }

        /// <summary>
        ///  1 、 1 、 2 、 3 、 5 、 8 、 13 、 21 、 34…… 求第 30 位数是多少，  用递归算法实现
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private static int Foo(int x)
        {
            if (x <= 0) return 0;
            else if (x > 0 && x <= 2) return 1;
            else return Foo(x - 1) + Foo(x - 2);
        }

        private static int Que2()
        {
            int i = 8;

            try
            {

                i++;

                Console.WriteLine("a");

                return i;//把返回值设定为i，然后“尽快”返回（没啥事就回去吧）

            }

            finally
            {

                Console.WriteLine("b");

                i++;
            }
        }

        private static Person Que3()
        {

            Person p = new Person();

            p.Age = 8;

            try
            {

                p.Age++;

                Console.WriteLine("a");

                return p;//把返回值设定为i，然后“尽快”返回（没啥事就回去吧。搞完就走）

            }

            finally
            {

                Console.WriteLine("b");

                p.Age++;

            }

        }
    }

    class Person
    {
        public int Age { get; set; }
    }

    class A
    {
        public A()
        {
            OutputText();
        }
        public virtual void OutputText()
        {
            Console.WriteLine("Hello world");
        }
    }

    class B : A
    {
        int x = 0; int y;
        public B()
        {
            y = 1;
        }

        public override void OutputText()
        {
            Console.WriteLine("x+y={0}", x + y);
        }
    }
}
