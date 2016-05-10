using Common.Net.Helper;
using Common.Net.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeplerPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            //Common.Net.Helper.LogHelper.GetLogByName("DebugFileAppender").Debug("1111111111");
            //Common.Net.Helper.LogHelper.GetLogByName().Error("2222222222");
            //Common.Net.Helper.LogHelper.GetLogByName().Info("33333333333");

            for (int i = 0; i < 1000 ; i++)
            {
                LogHelper.Error("测试");
                LogHelper.Debug("测试");
            }

            //HttpHelper ht = new HttpHelper();
            //ht.GetHtml(new HttpItem() {                    
            //});

            Console.ReadLine();
        }
    }
}
