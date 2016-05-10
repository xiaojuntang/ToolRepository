/***************************************************************************** 
*        filename :SecurityFunc 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.42000 
*        新建项输入的名称:   SecurityFunc 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Common.Net.Func 
*        文件名:             SecurityFunc 
*        创建系统时间:       2016/2/3 9:55:30 
*        创建年份:           2016 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Net.Func
{
    public class SecurityDES
    {
        /// <summary>加密
        /// 加密
        /// </summary>
        /// <param name="pToEncrypt">加密的字符串</param>
        /// <param name="sKey">密钥</param>
        /// <returns></returns>
        public static string Encrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //把字符串放到byte数组中  
            //原来使用的UTF8编码，我改成Unicode编码了，不行  
            byte[] inputByteArray = Encoding.GetEncoding("gb2312").GetBytes(pToEncrypt);
            //byte[]  inputByteArray=Encoding.Unicode.GetBytes(pToEncrypt);    

            //建立加密对象的密钥和偏移量  
            //原文使用ASCIIEncoding.ASCII方法的GetBytes方法  
            //使得输入密码必须输入英文文本  
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
            ret.ToString();
            return ret.ToString();
        }

        /// <summary>解密方法  
        /// 解密方法  
        /// </summary>
        /// <param name="pToDecrypt">要解密的后的字符串</param>
        /// <param name="sKey">解密的密钥</param>
        /// <returns></returns>
        public static string Decrypt(string pToDecrypt, string sKey)
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

            //建立加密对象的密钥和偏移量，此值重要，不能修改  
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

        /// <summary>
        /// HMACSHA1  加密算法
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="key">keys</param>
        /// <returns></returns>
        public static string HMACSHA1(string dataSource, string key)
        {
            byte[] byteBuffer = System.Text.Encoding.ASCII.GetBytes(key);
            byte[] dataBuffer = System.Text.Encoding.ASCII.GetBytes(dataSource);
            System.Security.Cryptography.HMACSHA1 hmac = new HMACSHA1(byteBuffer);
            byte[] hashBytes = hmac.ComputeHash(dataBuffer);
            return Convert.ToBase64String(hashBytes);
        }


        /// <summary>加密字符串的方法
        /// 得到加密字符串的方法 此方法 用于夸网站时候传递的验证串
        /// </summary>
        /// /*编写人：mxk  日期：2011-08-13*/
        /// <returns>返回加密后的字符串</returns>
        public static string AscEncodeString()
        {
            StringBuilder sb = new StringBuilder("@" + Guid.NewGuid().ToString() + "#");
            sb.Append("ZwwyLlZmzW@");
            sb.Append((DateTime.Now.Minute * 60 + DateTime.Now.Second).ToString() + "#");
            sb.Append(DateTime.Now.ToString("yyyyMMdd") + "@");
            return SecurityDES.Encrypt(sb.ToString(), "bbddZKA*");
        }
        /// <summary>解析字符串的方法  
        /// 解密的方法 根据网站间传递的加密串进行解密
        /// </summary>
        ///*编写人：mxk  日期：2011-08-13*/
        /// <param name="decodeStr">需要解密的字符串</param>
        /// <returns>成功返回true 验证失败返回false</returns>
        public static bool DscDecodeString(string decodeStr)
        {
            bool _isbool = true;
            string _decodeStr = SecurityDES.Decrypt(decodeStr, "bbddZKA*");
            string[] _deAry = _decodeStr.Split('#');
            if (_deAry.Length == 3)
            {
                string[] _splitAry = _deAry[1].Split('@');
                if (_splitAry.Length == 2)
                {
                    if ((DateTime.Now.Minute * 60 + DateTime.Now.Second) - int.Parse(_splitAry[1]) < 60)
                    {
                        _isbool = true;
                    }
                    else
                    {
                        _isbool = false;
                    }
                }
                else
                {
                    _isbool = false;
                }
            }
            else
            {
                _isbool = false;
            }
            return _isbool;
        }
    }
}
