/***************************************************************************** 
*        filename :ExtendString 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.42000 
*        新建项输入的名称:   ExtendString 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Common.Net.Func 
*        文件名:             ExtendString 
*        创建系统时间:       2016/2/3 10:03:59 
*        创建年份:           2016 
/*****************************************************************************/
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Net.Core
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class ExtendString
    {
        #region 得到字符串长度，一个汉字长度为2

        /// <summary>
        /// 得到字符串字节长度，支持中英文混合长度
        /// </summary>
        /// <param name="stringToSub"></param>
        /// <returns></returns>
        public static int GetLength(this string stringToSub)
        {
            return GetLength(stringToSub, true);
        }

        /// <summary>
        /// 得到字符串字节长度，支持中英文混合长度
        /// </summary>
        /// <param name="stringToSub"></param>
        /// <param name="filterEmoji">是否过滤emoji表情字符,默认过滤</param>
        /// <returns></returns>
        public static int GetLength(this string stringToSub, bool filterEmoji)
        {
            Regex regex = new Regex("[\u4e00-\u9fa5，。.;；‘'’\"\"、?]+", RegexOptions.Compiled);

            char emoji_temp_char = '\a';//emoji临时替换符
            if (filterEmoji)
            {
                Regex regexEmoji = new Regex(@"\uD83C[\uDF00-\uDFFF]|\uD83D[\uDC00-\uDEFF]|[\u2600-\u26FF]");
                stringToSub = regexEmoji.Replace(stringToSub, emoji_temp_char.ToString());
            }
            char[] stringChar = stringToSub.ToCharArray();
            int nLength = 0;
            int i = 0;
            for (; i < stringChar.Length; i++)
            {
                if (filterEmoji && stringChar[i] == emoji_temp_char)//emoji字符占两个宽度（不是长度）
                {
                    nLength += 2;
                }
                else if (regex.IsMatch((stringChar[i]).ToString()))
                {
                    nLength += 2;
                }
                else
                {
                    nLength = nLength + 1;
                }
            }
            return Encoding.Default.GetBytes(stringToSub).Length;
        }

        /// <summary>
        /// 截取字符串，支持中英文混合长度
        /// </summary>
        /// <param name="stringToSub">要截取的字符串</param>
        /// <param name="length">指定截取的长度，按单字节长度算，如：“小红回家了”，需要的长度为10（如果是9也可以）</param>
        /// <returns></returns>
        public static string ESubString(this string stringToSub, int length)
        {
            return ESubString(stringToSub, length, true, true);
        }

        /// <summary>
        /// 截取字符串，支持中英文混合长度
        /// </summary>
        /// <param name="stringToSub">要截取的字符串</param>
        /// <param name="length">指定截取的长度，按单字节长度算，如：“小红回家了”，需要的长度为10（如果是9也可以）</param>
        /// <param name="showF">是否显示“..”,默认显示</param>
        /// <returns></returns>
        public static string ESubString(this string stringToSub, int length, bool showF)
        {
            return ESubString(stringToSub, length, showF, true);
        }

        /// <summary>
        /// 截取字符串，支持中英文混合长度
        /// </summary>
        /// <param name="stringToSub">要截取的字符串</param>
        /// <param name="length">指定截取的长度，按单字节长度算，如：“小红回家了”，需要的长度为10（如果是9也可以）</param>
        /// <param name="showF">是否显示“..”,默认显示</param>
        /// <param name="filterEmoji">是否过滤emoji表情字符,默认过滤</param>
        /// <returns></returns>
        public static string ESubString(this string stringToSub, int length, bool showF, bool filterEmoji)
        {
            Regex regex = new Regex("[\u4e00-\u9fa5，。.;；‘'’\"\"、?]+", RegexOptions.Compiled);
            char emoji_temp_char = '\a';//emoji临时替换符(回退符)
            Match emoji_match = null;
            if (filterEmoji)
            {
                Regex regexEmoji = new Regex(@"\uD83C[\uDF00-\uDFFF]|\uD83D[\uDC00-\uDEFF]|[\u2600-\u26FF]");
                emoji_match = regexEmoji.Match(stringToSub);//找到字符串中所有的emoji字符。（emoji占4个字节，需要单独处理）
                stringToSub = regexEmoji.Replace(stringToSub, emoji_temp_char.ToString());
            }
            char[] stringChar = stringToSub.ToCharArray();
            StringBuilder sb = new StringBuilder();
            int nLength = 0;
            //bool isCut = false;
            int i = 0;
            for (; i < stringChar.Length; i++)
            {
                if (filterEmoji && stringChar[i] == emoji_temp_char)//emoji字符占两个宽度（不是长度）
                {
                    sb.Append(emoji_match.Value);//把emoji放回字符串中
                    emoji_match = emoji_match.NextMatch();
                    nLength += 2;
                }
                else if (regex.IsMatch((stringChar[i]).ToString()))
                {
                    sb.Append(stringChar[i]);
                    nLength += 2;
                }
                else
                {
                    sb.Append(stringChar[i]);
                    nLength = nLength + 1;
                }

                if (nLength >= length)
                {
                    i++;
                    //isCut = true;
                    break;
                }
            }
            if (i < stringChar.Length)
                return sb.ToString() + (showF ? ".." : string.Empty);
            else
                return sb.ToString();
        }

        #endregion

        /// <summary>
        ///  根据 一个 位置 验证一个字符串 第几位 是不是和 验证的字符相同
        /// </summary>
        /// <param name="right">验证的字符串</param>
        /// <param name="pos">验证的位置 从1开始</param>
        /// <param name="chckchr">需要验证的额字符</param>
        /// <returns></returns>
        public static bool StrIndexOfCheck(this string right, int pos, char chckchr)
        {
            bool ischeck = false;
            if (!string.IsNullOrEmpty(right))
            {
                if (pos > right.Length) return false;
                char[] _ary = right.ToCharArray();
                if (_ary[pos - 1] == chckchr)
                {
                    ischeck = true;
                }
            }
            return ischeck;
        }

        /// <summary>
        /// 验证 字符串是否是数字
        /// </summary>
        /// <param name="numstr">字符串</param>
        /// <returns>是数字为true 否则为false</returns>
        public static bool IsNumber(this string numstr)
        {
            bool isbool = true;
            if (string.IsNullOrEmpty(numstr))
            {
                isbool = false;
            }
            else
            {
                foreach (char c in numstr)
                {
                    if (!char.IsDigit(c))
                    {
                        isbool = false;
                        break;
                    }
                }
            }
            return isbool;
        }

        /// <summary>
        /// 判断是否是数字类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNum(string str)
        {
            string Rx = @"^[1-9]\d*$";
            var bl = Regex.IsMatch(str, Rx);
            return bl;
        }

        /// <summary>
        /// 获得字符串的长度,一个汉字的长度为1
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetStringLength(string str)
        {
            return !string.IsNullOrEmpty(str) ? Encoding.Default.GetBytes(str).Length : 0;
        }

        /// <summary>
        /// 获得字符串中指定字符的个数
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="c">字符</param>
        /// <returns></returns>
        public static int GetCharCount(string s, char c)
        {
            return string.IsNullOrEmpty(s) ? 0 : s.Count(a => a == c);
        }

        /// <summary>
        /// 获得指定顺序的字符在字符串中的位置索引
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="order">顺序</param>
        /// <returns></returns>
        public static int IndexOf(string s, int order)
        {
            return IndexOf(s, '-', order);
        }

        /// <summary>
        /// 获得指定顺序的字符在字符串中的位置索引
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="c">字符</param>
        /// <param name="order">顺序</param>
        /// <returns></returns>
        public static int IndexOf(string s, char c, int order)
        {
            int length = s.Length;
            for (int i = 0; i < length; i++)
            {
                if (c == s[i])
                {
                    if (order == 1)
                        return i;
                    order--;
                }
            }
            return -1;
        }

        #region 分割字符串

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="splitStr">分隔字符串</param>
        /// <returns></returns>
        public static string[] SplitString(string sourceStr, string splitStr)
        {
            if (string.IsNullOrEmpty(sourceStr) || string.IsNullOrEmpty(splitStr))
                return new string[0] { };

            if (sourceStr.IndexOf(splitStr, StringComparison.Ordinal) == -1)
                return new string[] { sourceStr };

            if (splitStr.Length == 1)
                return sourceStr.Split(splitStr[0]);
            else
                return Regex.Split(sourceStr, Regex.Escape(splitStr), RegexOptions.IgnoreCase);

        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <returns></returns>
        public static string[] SplitString(string sourceStr)
        {
            return SplitString(sourceStr, ",");
        }

        #endregion

        #region 截取字符串

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="startIndex">开始位置的索引</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns></returns>
        public static string SubString(string sourceStr, int startIndex, int length)
        {
            if (!string.IsNullOrEmpty(sourceStr))
            {
                if (sourceStr.Length >= (startIndex + length))
                    return sourceStr.Substring(startIndex, length);
                else
                    return sourceStr.Substring(startIndex);
            }

            return "";
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns></returns>
        public static string SubString(string sourceStr, int length)
        {
            return SubString(sourceStr, 0, length);
        }

        #endregion

        #region 移除前导/后导字符串

        /// <summary>
        /// 移除前导字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">移除字符串</param>
        /// <returns></returns>
        public static string TrimStart(string sourceStr, string trimStr)
        {
            return TrimStart(sourceStr, trimStr, true);
        }

        /// <summary>
        /// 移除前导字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">移除字符串</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static string TrimStart(string sourceStr, string trimStr, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(sourceStr))
                return string.Empty;

            if (string.IsNullOrEmpty(trimStr) || !sourceStr.StartsWith(trimStr, ignoreCase, CultureInfo.CurrentCulture))
                return sourceStr;

            return sourceStr.Remove(0, trimStr.Length);
        }

        /// <summary>
        /// 移除后导字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">移除字符串</param>
        /// <returns></returns>
        public static string TrimEnd(string sourceStr, string trimStr)
        {
            return TrimEnd(sourceStr, trimStr, true);
        }

        /// <summary>
        /// 移除后导字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">移除字符串</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static string TrimEnd(string sourceStr, string trimStr, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(sourceStr))
                return string.Empty;

            if (string.IsNullOrEmpty(trimStr) || !sourceStr.EndsWith(trimStr, ignoreCase, CultureInfo.CurrentCulture))
                return sourceStr;

            return sourceStr.Substring(0, sourceStr.Length - trimStr.Length);
        }

        /// <summary>
        /// 移除前导和后导字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">移除字符串</param>
        /// <returns></returns>
        public static string Trim(string sourceStr, string trimStr)
        {
            return Trim(sourceStr, trimStr, true);
        }

        /// <summary>
        /// 移除前导和后导字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">移除字符串</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static string Trim(string sourceStr, string trimStr, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(sourceStr))
                return string.Empty;

            if (string.IsNullOrEmpty(trimStr))
                return sourceStr;

            if (sourceStr.StartsWith(trimStr, ignoreCase, CultureInfo.CurrentCulture))
                sourceStr = sourceStr.Remove(0, trimStr.Length);

            if (sourceStr.EndsWith(trimStr, ignoreCase, CultureInfo.CurrentCulture))
                sourceStr = sourceStr.Substring(0, sourceStr.Length - trimStr.Length);

            return sourceStr;
        }

        #endregion
    }
}
