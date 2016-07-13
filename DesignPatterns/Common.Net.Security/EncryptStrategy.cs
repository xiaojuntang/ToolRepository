using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Security
{
    /// <summary>
    /// 加密策略
    /// </summary>
    public abstract class EncryptStrategy
    {
        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="source">加密字符串</param>
        /// <param name="key">密匙</param>
        /// <returns></returns>
        abstract public  string Encrypt(string source, string key);

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="source">解密字符串</param>
        /// <param name="key">密匙</param>
        /// <returns></returns>
        abstract public string Decrypt(string source, string key);

        /// <summary>
        /// MD5 加密算法
        /// </summary>
        /// <param name="source">加密串</param>
        /// <returns></returns>
        public string MD5Hash(string source)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(source);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sb.Append(hashBytes[i].ToString("x2")); 
            }
            return sb.ToString();
        } 
    }
}
