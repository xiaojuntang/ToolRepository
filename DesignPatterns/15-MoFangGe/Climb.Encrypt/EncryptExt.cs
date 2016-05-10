
/**************************************************
* 文 件 名：EncryptExt.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014/9/3 14:17:41
* 文件说明：加密类扩展方法
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/

using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Climb.Encrypt
{
    /// <summary>
    /// 加密类扩展
    /// </summary>
    public sealed class EncryptExt
    {
        #region Base64加密

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="text">要加密的字符串</param>
        /// <returns></returns>
        public static string EncodeBase64(string text)
        {
            try
            {
                char[] base64Code =
                {
                    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                    'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
                    'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7',
                    '8', '9', '+', '/', '='
                };
                const byte empty = (byte)0;
                ArrayList byteMessage = new ArrayList(Encoding.Default.GetBytes(text));
                int messageLen = byteMessage.Count;
                int page = messageLen / 3;
                int use;
                if ((use = messageLen % 3) > 0)
                {
                    for (int i = 0; i < 3 - use; i++)
                        byteMessage.Add(empty);
                    page++;
                }
                StringBuilder outmessage = new StringBuilder(page * 4);
                for (int i = 0; i < page; i++)
                {
                    byte[] instr = new byte[3];
                    instr[0] = (byte)byteMessage[i * 3];
                    instr[1] = (byte)byteMessage[i * 3 + 1];
                    instr[2] = (byte)byteMessage[i * 3 + 2];
                    int[] outstr = new int[4];
                    outstr[0] = instr[0] >> 2;
                    outstr[1] = ((instr[0] & 0x03) << 4) ^ (instr[1] >> 4);
                    if (!instr[1].Equals(empty))
                        outstr[2] = ((instr[1] & 0x0f) << 2) ^ (instr[2] >> 6);
                    else
                        outstr[2] = 64;
                    if (!instr[2].Equals(empty))
                        outstr[3] = (instr[2] & 0x3f);
                    else
                        outstr[3] = 64;
                    outmessage.Append(base64Code[outstr[0]]);
                    outmessage.Append(base64Code[outstr[1]]);
                    outmessage.Append(base64Code[outstr[2]]);
                    outmessage.Append(base64Code[outstr[3]]);
                }
                return outmessage.ToString();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        #endregion

        #region Base64解密

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="text">要解密的字符串</param>
        public static string DecodeBase64(string text)
        {
            //将空格替换为加号
            text = text.Replace(" ", "+");
            try
            {
                if ((text.Length % 4) != 0)
                {
                    return "包含不正确的BASE64编码";
                }
                if (!Regex.IsMatch(text, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase))
                {
                    return "包含不正确的BASE64编码";
                }
                const string base64Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
                int page = text.Length / 4;
                ArrayList outMessage = new ArrayList(page * 3);
                char[] message = text.ToCharArray();
                for (int i = 0; i < page; i++)
                {
                    byte[] instr = new byte[4];
                    instr[0] = (byte)base64Code.IndexOf(message[i * 4]);
                    instr[1] = (byte)base64Code.IndexOf(message[i * 4 + 1]);
                    instr[2] = (byte)base64Code.IndexOf(message[i * 4 + 2]);
                    instr[3] = (byte)base64Code.IndexOf(message[i * 4 + 3]);
                    byte[] outstr = new byte[3];
                    outstr[0] = (byte)((instr[0] << 2) ^ ((instr[1] & 0x30) >> 4));
                    if (instr[2] != 64)
                    {
                        outstr[1] = (byte)((instr[1] << 4) ^ ((instr[2] & 0x3c) >> 2));
                    }
                    else
                    {
                        outstr[2] = 0;
                    }
                    if (instr[3] != 64)
                    {
                        outstr[2] = (byte)((instr[2] << 6) ^ instr[3]);
                    }
                    else
                    {
                        outstr[2] = 0;
                    }
                    outMessage.Add(outstr[0]);
                    if (outstr[1] != 0)
                        outMessage.Add(outstr[1]);
                    if (outstr[2] != 0)
                        outMessage.Add(outstr[2]);
                }
                byte[] outbyte = (byte[])outMessage.ToArray(Type.GetType("System.Byte"));
                return Encoding.Default.GetString(outbyte);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        #endregion

        #region md5 32加密

        /// <summary>
        /// md5 加密
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static string MD5_32(string inputStr)
        {
            byte[] data = Encoding.Default.GetBytes(inputStr);
            MD5 md5 = MD5.Create();
            byte[] result = md5.ComputeHash(data);
            //将加密后的数组以16进制转化为普遍字符串
            StringBuilder sBuilder = new StringBuilder();
            foreach (byte t in result)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }

        #endregion

        #region SHA1 加密

        public static string Hmacsha1(string dataSource, string key)
        {
            byte[] byteBuffer = Encoding.ASCII.GetBytes(key);
            byte[] dataBuffer = Encoding.ASCII.GetBytes(dataSource);
            HMACSHA1 hmac = new HMACSHA1(byteBuffer);
            byte[] hashBytes = hmac.ComputeHash(dataBuffer);
            return Convert.ToBase64String(hashBytes); //返回长度为28字节的字符串
        }

        #endregion

        #region DESC加密 对称加密

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
            des.Key = Encoding.ASCII.GetBytes(sKey);
            des.IV = Encoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                //Format  as  hex  
                ret.AppendFormat("{0:X2}", b);
            }
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
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            //Put  the  input  string  into  the  byte  array  
            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            //建立加密对象的密钥和偏移量，此值重要，不能修改  
            des.Key = Encoding.ASCII.GetBytes(sKey);
            des.IV = Encoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            //Flush  the  data  through  the  crypto  stream  into  the  memory  stream  
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //Get  the  decrypted  data  back  from  the  memory  stream  

            return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion

        #region SHA256加密

        /// <summary>
        /// SHA256函数
        /// </summary>
        /// /// <param name="str">原始字符串</param>
        /// <returns>SHA256结果</returns>
        public static string Sha256(string str)
        {
            byte[] sha256Data = Encoding.UTF8.GetBytes(str);
            SHA256Managed sha256 = new SHA256Managed();
            byte[] result = sha256.ComputeHash(sha256Data);
            return Convert.ToBase64String(result); //返回长度为44字节的字符串
        }

        #endregion

        #region SHA384加密

        public static string Sha384(string str)
        {
            byte[] sha384Data = Encoding.UTF8.GetBytes(str);
            SHA384Managed sha384 = new SHA384Managed();
            byte[] result = sha384.ComputeHash(sha384Data);
            return Convert.ToBase64String(result); //返回长度为64字节的字符串
        }

        #endregion

        #region SHA512加密

        public static string Sha512(string str)
        {
            byte[] sha512Data = Encoding.UTF8.GetBytes(str);

            SHA512Managed sha512 = new SHA512Managed();

            byte[] result = sha512.ComputeHash(sha512Data);

            return Convert.ToBase64String(result); //返回长度为88字节的字符串
        }

        #endregion

        #region QQ密码加密的方法

        /*
            加密思路：它的原理是，先把你的密码进行3次(大家熟知的)MD5 32位加密，再把加密后的字符串与验证码组合起来，最后MD5加密一次。
         */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        public static string EncodePasswordWithVerifyCode(string password, string verifyCode)
        {
            return MD5_32(MD5_3(password) + verifyCode.ToUpper());
        }

        /// <summary>
        /// 实现md5 加密三次
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private static string MD5_3(string arg)
        {
            MD5 md5 = MD5.Create();
            byte[] buffer = Encoding.ASCII.GetBytes(arg);
            buffer = md5.ComputeHash(buffer);
            buffer = md5.ComputeHash(buffer);
            buffer = md5.ComputeHash(buffer);
            return BitConverter.ToString(buffer).Replace("-", "").ToUpper();
        }

        #endregion

        #region Rsa 加密
        //一般流程用户A用B的公钥对KEY进行加密，B收到信息后用自己的私钥进行解密获得KEY
        /// <summary>
        /// RSA获取公钥私钥
        /// </summary>
        /// <param name="strPrivateKey"></param>
        /// <param name="strPublicKey"></param>
        public void RsaCreateKey(ref string strPublicKey, ref string strPrivateKey)
        {
            if (strPublicKey == null) throw new ArgumentNullException("strPublicKey");
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(512);
            //str_PublicKey = Convert.ToBase64String(RSA.ExportCspBlob(false));
            //str_PrivateKey = Convert.ToBase64String(RSA.ExportCspBlob(true));

            strPublicKey = rsa.ToXmlString(false);
            strPrivateKey = rsa.ToXmlString(true);
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="source"></param>
        /// <param name="strPublicKey"></param>
        /// <returns></returns>
        public string RsaEncrypt(string source, string strPublicKey)
        {
            try
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {

                    rsa.FromXmlString(strPublicKey);

                    //str_PublicKey = Convert.ToBase64String(RSA.ExportCspBlob(false));
                    //str_PrivateKey = Convert.ToBase64String(RSA.ExportCspBlob(true));
                    byte[] dataToEncrypt = Encoding.UTF8.GetBytes(source);

                    byte[] bs = rsa.Encrypt(dataToEncrypt, false);
                    //str_PublicKey = Convert.ToBase64String(RSA.ExportCspBlob(false));
                    //str_PrivateKey = Convert.ToBase64String(RSA.ExportCspBlob(true));
                    string encrypttxt = Convert.ToBase64String(bs);

                    return encrypttxt;
                }

            }
            catch (CryptographicException)
            {
                return null;
            }
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="strRsa"></param>
        /// <param name="strPrivateKey"></param>
        /// <returns></returns>
        public string RsaDecrypt(string strRsa, string strPrivateKey)
        {
            try
            {
                byte[] dataToDecrypt = Convert.FromBase64String(strRsa);
                string strRe;
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(strPrivateKey);
                    //byte[] bsPrivatekey = Convert.FromBase64String(str_PrivateKey);
                    //RSA.ImportCspBlob(bsPrivatekey);

                    byte[] bsdecrypt = rsa.Decrypt(dataToDecrypt, false);

                    strRe = Encoding.UTF8.GetString(bsdecrypt);

                }
                return strRe;
            }
            catch (CryptographicException e)
            {
                return null;
            }
        }
        #endregion
    }
}



