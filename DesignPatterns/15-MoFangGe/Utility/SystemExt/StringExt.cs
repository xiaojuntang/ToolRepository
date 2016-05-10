
/**************************************************
* 文 件 名：StringExt.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014/9/1 18:01:32
* 文件说明：string字符串扩展类
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/

using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Climb.Utility.SystemExt
{
    /// <summary>
    /// 字符串类扩展
    /// </summary>
    public static class StringExt
    {
        #region 判断字符串是否是空值操作
        /// <summary>
        /// 判断字符串空值
        /// </summary>
        /// <param name="strValue">字符串</param>
        /// <returns>是null 或者是空则返回 true</returns>
        public static bool IsNullOrEmpty(this string  strValue)
        {
            if (strValue != null)
                return (strValue.Length == 0);
            return true;
        }

        /// <summary>
        /// 输入字符串如果是空或者是null 则用另外一个字符串代替
        /// </summary>
        /// <param name="strValue">输入字符串</param>
        /// <param name="defValue">代替字符串</param>
        /// <returns>如果是null或者是空返回代替字符串，否则返回输入字符串</returns>
        public static string IsNullOrEmpty(this string strValue, string defValue)
        {
            return strValue.IsNullOrEmpty() ? defValue : strValue;
        }
        #endregion

        #region 判断字符串是否匹配

        /// <summary>
        /// 判断字符串是否匹配正则表达式
        /// </summary>
        /// <param name="strValue">输入字符串</param>
        /// <param name="pattern">正则语法</param>
        /// <returns>匹配则返回true</returns>
        public static bool IsMatch(this string strValue, string pattern)
        {
            return strValue != null && Regex.IsMatch(strValue, pattern);
        }

        #endregion

        #region 得到匹配的字符串
        /// <summary>
        /// 得到匹配的字符串
        /// </summary>
        /// <param name="strValue">输入字符串</param>
        /// <param name="pattern">正则语法</param>
        /// <returns>返回正则表达式的字符串</returns>
        public static string Match(this string strValue, string pattern)
        {
            if (strValue == null) return "";
            return Regex.Match(strValue, pattern).Value;
        }
        #endregion

        #region 判断字符串是否是整形
        /// <summary>
        /// 判断字符串是否是整形
        /// </summary>
        /// <param name="strValue">输入字符串</param>
        /// <returns>数字返回true</returns>
        public static bool IsInt(this string strValue)
        {
            int i;
            return int.TryParse(strValue, out i);
        }
        #endregion

        #region 字符串转换数字
        /// <summary>
        /// 转换成int数字
        /// </summary>
        /// <param name="number">字符串</param>
        /// <returns>如果转换成功返回数字 转换失败返回0</returns>
        public static int ToInt(this string number)
        {
            int resNum;
            int.TryParse(number, out resNum);
            return resNum;
        }
        /// <summary>
        /// 转换成long数字
        /// </summary>
        /// <param name="number">字符串</param>
        /// <returns>如果转换成功返回数字 转换失败返回0</returns>
        public static long ToLong(this string number)
        {
            long resNum;
            long.TryParse(number, out resNum);
            return resNum;
        }
        /// <summary>
        /// 转换成double数字
        /// </summary>
        /// <param name="number">字符串</param>
        /// <returns>如果转换成功返回数字 转换失败返回0.0</returns>
        public static double ToDouble(this string number)
        {
            double resNum;
            double.TryParse(number, out resNum);
            return resNum;
        }
        #endregion

        #region 获取GUID值
        /// <summary>
        /// 获取GUID值
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public static string NewGuid36
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }

        /// <summary>
        ///  获取GUID值去掉 -为32位的值
        /// </summary>
        public static string NewGuid32
        {
            get
            {
                return NewGuid36.Replace("-", "");
            }
        }

        #endregion

        #region 换行字符
        /// <summary>
        /// 获取换行字符
        /// </summary>
        public static string NewLine
        {
            get
            {
                return Environment.NewLine;
            }
        }
        #endregion

        #region 字符串反转
        /// <summary>
        /// 字符串反转
        /// </summary>
        /// <param name="strValue">目标字符串</param>
        /// <returns>结果字符串</returns>
        public static string ReverseString(this string strValue)
        {
            if (strValue.IsNullOrEmpty()) return string.Empty;
            var chars = strValue.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }
        #endregion

        #region 字符串拆根据正则拆分成数组
        /// <summary>
        /// 字符串 根据正则split数组
        /// </summary>
        /// <param name="value">字符串的值</param>
        /// <param name="regexPattern">正则表达式</param>
        /// <param name="options">正则匹配的枚举</param>
        /// <returns>返回字符串数组</returns>
        public static string[] SplitReg(this string value, string regexPattern, RegexOptions options)
        {
            return Regex.Split(value, regexPattern, options);
        }

        /// <summary>
        /// 字符串 根据正则split数组
        /// </summary>
        /// <param name="value">字符串的值</param>
        /// <param name="regexPattern">正则表达式</param>
        /// <returns>返回字符串数组</returns>
        public static string[] SplitReg(this string value, string regexPattern)
        {
            return Regex.Split(value, regexPattern, RegexOptions.None);
        }

        #endregion

        #region 替换字符串
        /// <summary>
        /// 根据正则匹配替换字符串
        /// </summary>
        /// <param name="value">字符串的值</param>
        /// <param name="regexPattern">正则表达式</param>
        /// <param name="replaceValue">替换目标字符串</param>
        /// <returns>返回替换的字符串值</returns>
        public static string ReplaceRegWith(this string value, string regexPattern, string replaceValue)
        {
            return ReplaceWith(value, regexPattern, replaceValue, RegexOptions.None);
        }

        /// <summary>
        /// 根据正则匹配替换字符串
        /// </summary>
        /// <param name="value">字符串的值</param>
        /// <param name="regexPattern">正则表达式</param>
        /// <param name="replaceValue">替换目标字符串</param>
        /// <param name="options">正则对象的枚举</param>
        /// <returns>返回替换的字符串值</returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public static string ReplaceWith(this string value, string regexPattern, string replaceValue, RegexOptions options)
        {
            return Regex.Replace(value, regexPattern, replaceValue, options);
        }
        #endregion

        #region 截取字符串
        /// <summary>
        /// 字符串截取 如果截取的长度大于当前字符串串长度那么直接返回字符串否则正常截取字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">截取的长度</param>
        /// <returns>返回截取后的字符串</returns>
        public static string SubStringExt(string str, int length)
        {
            return length > str.Length ? str : str.Substring(length);
        }

        /// <summary>
        /// 截取过长的字符串  后边显示...
        /// </summary>
        /// <param name="text">字符串</param>
        /// <param name="length">截取的长度</param>
        /// <returns>返回字目标字符串</returns>
        public static string TrimWithElipsis(this string text, int length)
        {
            if (text.Length <= length) return text;
            return text.Substring(0, length) + "...";

        }
        #endregion

        #region 得到字符串长度，一个汉字长度为2

        /// <summary>
        /// 获取字符串长度 一个中文占两个字符
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回字符串长度</returns>
        public static int CnLength(this string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        /// <summary>截取字符串，支持中英文混合长度
        /// </summary>
        /// <param name="subStringStr">要截取的字符串</param>
        /// <param name="length">指定截取的长度，按单字节长度算，如：“小红回家了”，需要的长度为10（如果是9也可以）</param>
        /// <returns></returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public static string CnSubString(this string subStringStr, int length)
        {
            var cnStrLength = CnLength(subStringStr);
            if (length >= cnStrLength)
            {
                return subStringStr;
            }
            char[] charAry = subStringStr.ToCharArray();
            StringBuilder sb = new StringBuilder();
            int cnLenth = 0;
            int charLength = charAry.Length;//获取char数组长度
            for (int i = 0; i < charLength; i++)
            {
                sb.Append(charAry[i]);
                cnLenth += CnLength(charAry[i].ToString(CultureInfo.InvariantCulture));
                if (cnLenth >= length)
                {
                    break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 截取字符串，支持中英文混合长度字符串太长显示省略号
        /// </summary>
        /// <param name="text">字符串</param>
        /// <param name="length">要截取的字符串的长度</param>
        /// <returns>返回字符串</returns>
        public static string CnTrimWithElipsis(this string text, int length)
        {
            int cnLength = CnLength(text);
            if (cnLength <= length) return text;
            return text.CnSubString(length) + "...";

        }

        #endregion

        #region 全交半角转换

        /// <summary>
        /// 转全角(SBC case)
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>全角字符串</returns>
        public static string ToSbc(this string input)
        {
                 char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 转半角(DBC case)
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        public static string ToDbc(this string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        #endregion

        #region bool字符串转换bool
        /// <summary>
        /// 将布尔的字符串表示形式转换为它的等效布尔值
        /// </summary>
        /// <param name="input">包含要转换的数字的字符串</param>
        /// <returns>如果 input 转换成功，则为返回转换后的布尔值；否则为 false</returns>
        public static bool ToBoolean(this string input)
        {
            if (input.Equals("true", StringComparison.CurrentCultureIgnoreCase)) return true;
            else if (input.Equals("false", StringComparison.InvariantCultureIgnoreCase)) return false;
            else return false; // 考虑是否抛出异常?
        }
        #endregion

        #region 判断是否为json

        /// <summary>
        ///     判断指定字符串是否对象（Object）类型的Json字符串格式
        /// </summary>
        /// <param name="input">要判断的Json字符串</param>
        /// <returns></returns>
        public static bool IsJsonObjectString(string input)
        {
            return input != null && input.StartsWith("{") && input.EndsWith("}");
        }

        /// <summary>
        ///     判断指定字符串是否集合类型的Json字符串格式
        /// </summary>
        /// <param name="input">要判断的Json字符串</param>
        /// <returns></returns>
        public static bool IsJsonArrayString(string input)
        {
            return input != null && input.StartsWith("[") && input.EndsWith("]");
        }
        #endregion
    }
}
