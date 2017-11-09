using Common.Net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common.UnitTest
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Order order1 = new Order(100);
            //order1.Submit("Item1", 150);

            //Order order2 = new Order(101);
            //order2.Submit("Item2", 150);


            //System.Reflection.MemberInfo info = typeof(MyTes); //通过反射得到MyClass类的信息

            ////得到施加在MyClass类上的定制Attribute 
            //CodeReviewAttribute att =
            //           (CodeReviewAttribute)Attribute.GetCustomAttribute(info, typeof(CodeReviewAttribute));
            //if (att != null)
            //{
            //    Console.WriteLine("代码检查人:{0}", att.Reviewer);
            //    Console.WriteLine("检查时间:{0}", att.Date);
            //    Console.WriteLine("注释:{0}", att.Comment);
            //}


            MyTes a = new MyTes();
            try
            {
                a.A("a1", 101, "c1");
                a.B("a1", 101, "c1");

                a.A("a2", 2, "c2");
            }
            catch (Exception ex)
            {

                throw;
            }



            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }


    //[CodeReview("tangxiaojun", "20171017", "需求改进行的地方")]
    [Interceptor]
    public class MyTes : ContextBoundObject
    {

        public MyTes()
        {

        }

        [InterceptorMethod]
        public string A(string a, [InRange(100, 200)]int b, string c)
        {
            return a + c;
        }


        public string B(string a, [InRange(100, 200)]int b, string c)
        {
            return a + c;
        }
    }
}
