using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Security
{
    /// <summary>
    /// 加密上下文
    /// </summary>
    public class EncryptContext
    {
        private EncryptStrategy strategy;
        public EncryptContext()
        {
            this.strategy = new DES();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strategy">使用策略</param>
        public EncryptContext(EncryptStrategy strategy)
        {
            this.strategy = strategy;
        }

        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="source">加密字符串</param>
        /// <param name="key">密匙</param>
        /// <returns></returns>
        public string Encrypt(string source, string key)
        {
            return this.strategy.Encrypt(source, key);
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="source">解密字符串</param>
        /// <param name="key">密匙</param>
        /// <returns></returns>
        public string Decrypt(string source, string key)
        {
            return this.strategy.Decrypt(source, key);
        }

        /// <summary>
        /// MD5 加密算法
        /// </summary>
        /// <param name="source">加密串</param>
        /// <returns></returns>
        public string MD5Hash(string source)
        {
            return this.strategy.MD5Hash(source);
        }
    }
}
