
/**************************************************
* 文 件 名：ToolsExt.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014/9/4 9:50:21
* 文件说明：
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Climb.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Tools
    {
        #region 常用数组
        /// <summary>
        /// 数字字符数组
        /// </summary>
        public static readonly char[] NumCodeArray = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        /// <summary>
        /// 大写字母数组
        /// </summary>
        public static readonly char[] UperCodeArray = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        /// <summary>
        /// 小写字母数组
        /// </summary>
        public static readonly char[] LowerCodeArray = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        #endregion

        /// <summary>
        /// 根据身份证获取出生年月
        /// </summary>
        /// <param name="id">身份证号码</param>
        /// <returns>出生年月</returns>
        public static DateTime GetBirthday(string id)
        {
            string result;
            switch (id.Length)
            {
                case 15:
                    result = "19" + id.Substring(6, 2) + "-" + id.Substring(8, 2) + "-" + id.Substring(10, 2);
                    break;
                case 18:
                    result = id.Substring(6, 4) + "-" + id.Substring(10, 2) + "-" + id.Substring(12, 2);
                    break;
                default:
                    throw new ArgumentException("身份证号码不是15或者18位！");
            }
            return Convert.ToDateTime(result);
        }

        #region 敏感字符字符转换（450821198506010034转450821********0034）
        /// <summary>
        /// 敏感字符字符转换（450821198506010034转450821********0034）
        /// </summary>
        /// <param name="number"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetPrivateString(string number, int start, int length)
        {
            bool isDo = false;
            char[] s = number.ToCharArray();
            for (int i = 0; i < s.Length; i++)
            {
                if (i == start)
                {
                    isDo = true;
                }

                if (isDo)
                {
                    s[i] = '*';
                    if (length <= 1)
                    {
                        isDo = false;
                    }
                    length--;
                }
            }
            return new String(s);
        }
        #endregion


        #region 复制对象
        /// <summary>
        /// 复制对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        /// <returns></returns>
        public static T CloneOf<T>(T serializableObject)
        {
            object objCopy = null;
            MemoryStream stream = new MemoryStream();
            BinaryFormatter binFormatter = new BinaryFormatter();
            binFormatter.Serialize(stream, serializableObject);
            stream.Position = 0;
            objCopy = (T)binFormatter.Deserialize(stream);
            stream.Close();
            return (T)objCopy;
        }
        #endregion
    }
}
