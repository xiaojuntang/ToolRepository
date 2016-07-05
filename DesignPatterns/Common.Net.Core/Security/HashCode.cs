using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Core
{
    /// <summary>
    /// 得到随机安全码（哈希加密）。
    /// </summary>
    public class HashEncode
    {
        public HashEncode()
        {

        }
        /// <summary>
        /// 得到随机哈希加密字符串
        /// </summary>
        /// <returns></returns>
        public static string GetSecurity()
        {
            return HashEncoding(GetRandomValue());
        }
        /// <summary>
        /// 得到一个随机数值
        /// </summary>
        /// <returns></returns>
        public static string GetRandomValue()
        {
            Random Seed = new Random();
            return Seed.Next(1, int.MaxValue).ToString();
        }
        /// <summary>
        /// 哈希加密一个字符串
        /// </summary>
        /// <param name="security"></param>
        /// <returns></returns>
        public static string HashEncoding(string security)
        {
            byte[] Value;
            UnicodeEncoding Code = new UnicodeEncoding();
            byte[] Message = Code.GetBytes(security);
            SHA512Managed Arithmetic = new SHA512Managed();
            Value = Arithmetic.ComputeHash(Message);
            security = "";
            foreach (byte o in Value)
            {
                security += (int)o + "O";
            }
            return security;
        }
    }
}
