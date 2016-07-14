using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Net.Security
{
    /// <summary>
    /// DES对称加密算法
    /// </summary>
    public class DESCryptoService : EncryptStrategy
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="pToEncrypt">加密的字符串</param>
        /// <param name="sKey">密钥</param>
        /// <returns></returns>
        public override string Encrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //把字符串放到byte数组中  
            //原来使用的UTF8编码，我改成Unicode编码了，不行  
            byte[] inputByteArray = Encoding.GetEncoding("gb2312").GetBytes(pToEncrypt);
            //byte[]  inputByteArray=Encoding.Unicode.GetBytes(pToEncrypt);
            //建立加密对象的密钥和偏移量  
            //原文使用ASCIIEncoding.ASCII方法的GetBytes方法  
            //使得输入密码必须输入英文文本  
            MD5 md5 = MD5.Create();
            sKey = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(sKey))).Substring(0, 8);
            //sKey = FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            //Write  the  byte  array  into  the  crypto  stream  
            //(It  will  end  up  in  the  memory  stream)  
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //Get  the  data  back  from  the  memory  stream,  and  into  a  string  
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                //Format  as  hex  
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        /// <summary> 
        /// 解密方法  
        /// </summary>
        /// <param name="pToDecrypt">要解密的后的字符串</param>
        /// <param name="sKey">解密的密钥</param>
        /// <returns></returns>
        public override string Decrypt(string pToDecrypt, string sKey)
        {
            if (string.IsNullOrEmpty(pToDecrypt) || !Regex.IsMatch(pToDecrypt, @"^[\w\d]+$"))
                return string.Empty;
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //Put  the  input  string  into  the  byte  array  
            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            MD5 md5 = MD5.Create();
            sKey = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(sKey))).Substring(0, 8);
            //建立加密对象的密钥和偏移量，此值重要，不能修改  
            //sKey = FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            //Flush  the  data  through  the  crypto  stream  into  the  memory  stream  
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //Get  the  decrypted  data  back  from  the  memory  stream  
            //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象  
            //StringBuilder ret = new StringBuilder();
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
    }
}
