using Common.Net.Core;
using Common.Net.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeplerPatterns
{
    class Program
    {
        /// <summary>
        /// 邀请码生成
        /// </summary>
        /// <returns></returns>
        public string GetInvitationCode()
        {

            //自定义进制，长度为34。  0和1与o和l容易混淆，不包含在进制中。

            char[] r = new char[] { 'Q', 'w', 'E', '8', 'a', 'S', '2', 'd', 'Z', 'x', '9', 'c', '7', 'p', 'O', '5', 'i', 'K', '3', 'm', 'j', 'U', 'f', 'r', '4', 'V', 'y', 'L', 't', 'N', '6', 'b', 'g', 'H' };

            char[] b = new char[] { 'q', 'W', 'e', '5', 'A', 's', '3', 'D', 'z', 'X', '8', 'C', '2', 'P', 'o', '4', 'I', 'k', '9', 'M', 'J', 'u', 'F', 'R', '6', 'v', 'Y', 'T', 'n', '7', 'B', 'G', 'h' };

            char[] buf = new char[33];

            int s = 6;//生成六位的邀请码

            int binLen = r.Length;

            int charPos = 33;

            //以当前的毫秒数作为标准

            int id = DateTime.Now.Millisecond;

            while (id / binLen > 0)

            {
                string t2 = $"{id}_{id}";
                int k = (int)(id % binLen);

                buf[--charPos] = r[k];

                id /= binLen;

            }

            buf[--charPos] = r[(int)(id % binLen)];

            String str = new String(buf, charPos, (33 - charPos));

            //长度不够6位时自动随机补全

            if (str.Length < s)

            {

                StringBuilder sb = new StringBuilder();

                Random rd = new Random();

                for (int i = 1; i <= s - str.Length; i++)

                {

                    sb.Append(b[rd.Next(33)]);

                }

                str += sb.ToString();

            }

            return str.ToUpper();
        }
        static void Main(string[] args)
        {




            //Common.Net.Helper.LogHelper.GetLogByName("DebugFileAppender").Debug("1111111111");
            //Common.Net.Helper.LogHelper.GetLogByName().Error("2222222222");
            //Common.Net.Helper.LogHelper.GetLogByName().Info("33333333333");
            List<TestA> list = new List<TestA>();
            for (int i = 0; i < 10; i++)
            {
                TestA test = new TestA();
                test.A = i.ToString();
                test.B = i * 3;
                list.Add(test);
            }

            CacheHelper.Add("C:U:001", list);

            if (CacheHelper.IsExist("C:U:001"))
            {
                List<TestA> tes = CacheHelper.Get("C:U:001") as List<TestA>;
            }




            //for (int i = 0; i < 1000 ; i++)
            //{
            //    LogHelper.Error("测试");
            //    LogHelper.Debug("测试");
            //}

            //HttpHelper ht = new HttpHelper();
            //ht.GetHtml(new HttpItem() {                    
            //});

            Console.ReadLine();
        }
    }

    public class TestA
    {
        public string A { get; set; }

        public int B { get; set; }
    }



}
