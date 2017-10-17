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
            Order order1 = new Order(100);
            order1.Submit("Item1", 150);

            Order order2 = new Order(101);
            order2.Submit("Item2", 150);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }


    [CodeReview("tangxiaojun", "20171017", "需求改进行的地方")]
    public class MyTes
    {

        public MyTes()
        {

        }
    }
}
