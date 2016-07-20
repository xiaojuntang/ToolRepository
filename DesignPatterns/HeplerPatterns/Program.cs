using Common.Net.Core;
using Common.Net.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HeplerPatterns
{
    class Program
    {
        /// <summary>
        /// 邀请码生成
        /// </summary>
        /// <returns></returns>
        public static string GetInvitationCode()
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

        public static string InvitationCode()
        {
            string key = "8%3H9*)!";
            string iv = "8%3H9*)!";
            string t = DateTime.Now.ToString("yyyyMMddHHmmss");
            string n = "10001";
            string a = EncryptUtils.Encrypt(n + t + "b497570b-69a2-4bd1-9380-7e74616795ab", key);
            string b = EncryptUtils.Decrypt(a, key);

            string c = EncryptUtils.MD5Encrypt16(n + t + "b497570b-69a2-4bd1-9380-7e74616795ab");
            string d = EncryptUtils.MD5Encrypt32(n + t + "b497570b-69a2-4bd1-9380-7e74616795ab");
            string e = EncryptUtils.MD5Encrypt64(n + t + "b497570b-69a2-4bd1-9380-7e74616795ab");



            //string k = EncryptUtils.Decrypt(n + t + "b497570b-69a2-4bd1-9380-7e74616795ab");

            //string v = string.Format("{0}-{1}-{2}", n, t, k);
            return "";

        }

        static void Main(string[] args)
        {
            HttpHelper

            //Console.Write("请输入要加密的字符串：");   //提示输入字符串      
            //Console.WriteLine();                  //换行输入   
            //string str = Console.ReadLine();     //记录输入的字符串      
            //string strNew = Encrypt(str);              //加密字符串      
            //Console.WriteLine("加密后的字符串：" + strNew);  //输出加密后的字符串      
            //Console.WriteLine("解密后的字符串：" + Decrypt(strNew)); //解密字符串并输出     
            //Console.ReadLine();

            for (int i = 0; i < 100; i++)
            {
                string a1 = UniqueObjectID.GenerateNewStrId();
                var a2=new UniqueObjectID(a1);
                System.Diagnostics.Debug.WriteLine(a1);
                //Console.WriteLine(UniqueObjectID.GenerateNewId());
            }
            Console.ReadLine();
        }
    }

    public class ShareCodeUtil
    {

        /** 自定义进制(0,1没有加入,容易与o,l混淆) */
        private static  char[] r = new char[] { 'q', 'w', 'e', '8', 'a', 's', '2', 'd', 'z', 'x', '9', 'c', '7', 'p', '5', 'i', 'k', '3', 'm', 'j', 'u', 'f', 'r', '4', 'v', 'y', 'l', 't', 'n', '6', 'b', 'g', 'h' };

        /** (不能与自定义进制有重复) */
        private static  char b = 'o';

        /** 进制长度 */
        private static int binLen = r.Length;

        /** 序列最小长度 */
        private static  int s = 6;

        /**
         * 根据ID生成六位随机码
         * @param id ID
         * @return 随机码
         */
        public static string toSerialCode(long id)
        {
            char[] buf = new char[32];
            int charPos = 32;

            while ((id / binLen) > 0)
            {
                int ind = (int)(id % binLen);
                // System.out.println(num + "-->" + ind);
                buf[--charPos] = r[ind];
                id /= binLen;
            }
            buf[--charPos] = r[(int)(id % binLen)];
            // System.out.println(num + "-->" + num % binLen);
            String str = new String(buf, charPos, (32 - charPos));
            // 不够长度的自动随机补全
            if (str.Length < s)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(b);
                Random rnd = new Random();
                for (int i = 1; i < s - str.Length; i++)
                {
                    sb.Append(r[rnd.Next(binLen)]);
                }
                str += sb.ToString();
            }
            return str;
        }

        public static long codeToId(String code)
        {
            char[] chs = code.ToCharArray();
            long res = 0L;
            for (int i = 0; i < chs.Length; i++)
            {
                int ind = 0;
                for (int j = 0; j < binLen; j++)
                {
                    if (chs[i] == r[j])
                    {
                        ind = j;
                        break;
                    }
                }
                if (chs[i] == b)
                {
                    break;
                }
                if (i > 0)
                {
                    res = res * binLen + ind;
                }
                else
                {
                    res = ind;
                }
                // System.out.println(ind + "-->" + res);
            }
            return res;
        }
    }
}
