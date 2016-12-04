using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Security
{
    public class RsaCryptoService : EncryptStrategy
    {
        /// <summary> 
        /// RSA加密+base64 
        /// </summary> 
        /// <param name="publickey">公钥</param> 
        /// <param name="content">原文</param> 
        /// <returns>加密后的密文字符串</returns> 
        public override string Encrypt(string publickey, string content)
        {
            //最大文件加密块 
            int MAX_ENCRYPT_BLOCK = 245;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(publickey);
            byte[] contentByte = Encoding.UTF8.GetBytes(content);
            int inputLen = contentByte.Length;
            int offSet = 0;
            byte[] cache;
            int i = 0;
            System.IO.MemoryStream aMS = new System.IO.MemoryStream();
            // 对数据分段加密 
            while (inputLen - offSet > 0)
            {
                byte[] temp = new byte[MAX_ENCRYPT_BLOCK];
                if (inputLen - offSet > MAX_ENCRYPT_BLOCK)
                {
                    Array.Copy(contentByte, offSet, temp, 0, MAX_ENCRYPT_BLOCK);
                    cache = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
                }
                else
                {
                    Array.Copy(contentByte, offSet, temp, 0, inputLen - offSet); cache = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
                }
                aMS.Write(cache, 0, cache.Length); i++;
                offSet = i * MAX_ENCRYPT_BLOCK;
            }
            cipherbytes = aMS.ToArray();
            return Convert.ToBase64String(cipherbytes);
        }

        /// <summary> 
        /// RSA解密 
        /// </summary> 
        /// <param name="privatekey">私钥</param> 
        /// <param name="content">密文（RSA+base64）</param> 
        /// <returns>解密后的字符串</returns> 
        public override string Decrypt(string privatekey, string content)
        {
            //最大文件解密块 
            int MAX_DECRYPT_BLOCK = 256;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(privatekey);
            byte[] contentByte = Convert.FromBase64String(content);
            int inputLen = contentByte.Length;
            // 对数据分段解密 
            int offSet = 0;
            int i = 0;
            byte[] cache;
            System.IO.MemoryStream aMS = new System.IO.MemoryStream();
            while (inputLen - offSet > 0)
            {
                byte[] temp = new byte[MAX_DECRYPT_BLOCK];
                if (inputLen - offSet > MAX_DECRYPT_BLOCK)
                {
                    Array.Copy(contentByte, offSet, temp, 0, MAX_DECRYPT_BLOCK);
                    cache = rsa.Decrypt(temp, false);
                }
                else
                {
                    Array.Copy(contentByte, offSet, temp, 0, inputLen - offSet); cache = rsa.Decrypt(temp, false);
                }
                aMS.Write(cache, 0, cache.Length); i++; offSet = i * MAX_DECRYPT_BLOCK;
            }
            cipherbytes = aMS.ToArray();
            return Encoding.UTF8.GetString(cipherbytes);
        }
    }
}
