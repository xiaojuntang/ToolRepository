using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 更新主知识点
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> subjects = new List<string>() { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14" };

            foreach (string item in subjects)
            {
                ShowMessage("开始科目：" + item);
                BussBLL.GetQuestions(item);
                ShowMessage("结束科目：" + item);
            }
            Console.ReadLine();
        }

        static void ShowMessage(string message)
        {
            Console.WriteLine("[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message);
        }
    }
}
