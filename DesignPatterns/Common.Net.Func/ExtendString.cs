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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Net.Func
{
    public static class ExtendString
    {
        /// <summary>为string 对象扩充一个把内容以utf8的格式写入到磁盘文件的方法
        /// </summary>
        /// <param name="filename">要保存的文件名（物理路径加文件名）</param>
        /// <param name="_content">要保存的字符串内容</param>
        /// <param name="_append">是否追加，否则覆盖文件，是则追加文件</param>
        /// <returns>成功返回"OK"，失败返回失败的错误描述</returns>
        public static string saveToFile(this string _content, string fileName, bool _append)
        {
            StreamWriter sw = null;
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                }
                sw = new StreamWriter(fileName, _append, System.Text.Encoding.UTF8);
                try
                {
                    sw.Write(_content);
                }
                catch (Exception err)
                {
                    return err.ToString();
                }
                return "OK";
            }
            catch (Exception err)
            {
                return err.Message;
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }

        }
        /// <summary>md5加密字符串
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string md5(this string val)
        {
            byte[] data = Encoding.Default.GetBytes(val);
            MD5 md5 = MD5CryptoServiceProvider.Create();
            byte[] result = md5.ComputeHash(data);
            //将加密后的数组以16进制转化为普遍字符串
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sBuilder.Append(result[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        #region 得到字符串长度，一个汉字长度为2

        /// <summary>得到字符串字节长度，支持中英文混合长度
        /// </summary>
        /// <param name="stringToSub"></param>
        /// <returns></returns>
        public static int EGetlength(this string stringToSub)
        {
            return EGetlength(stringToSub, true);
        }

        /// <summary>得到字符串字节长度，支持中英文混合长度
        /// </summary>
        /// <param name="stringToSub"></param>
        /// <param name="filterEmoji">是否过滤emoji表情字符,默认过滤</param>
        /// <returns></returns>
        public static int EGetlength(this string stringToSub, bool filterEmoji)
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
            return nLength;
            //以上代码mxk 2014年10月9日 11:47:14  发现得到结果不正确 不能正确的得到 中文的长度  但是考了到程序可能已被现有逻辑使用暂时先不改动若要 正确代码请 去掉下边注释的代码
            // return Encoding.Default.GetBytes(stringToSub).Length;
        }

        /// <summary>截取字符串，支持中英文混合长度
        /// </summary>
        /// <param name="stringToSub">要截取的字符串</param>
        /// <param name="length">指定截取的长度，按单字节长度算，如：“小红回家了”，需要的长度为10（如果是9也可以）</param>
        /// <param name="showF">是否显示“..”,默认显示</param>
        /// <param name="filterEmoji">是否过滤emoji表情字符,默认过滤</param>
        /// <returns></returns>
        public static string ESubString(this string stringToSub, int length)
        {
            return ESubString(stringToSub, length, true, true);
        }
        /// <summary>截取字符串，支持中英文混合长度
        /// </summary>
        /// <param name="stringToSub">要截取的字符串</param>
        /// <param name="length">指定截取的长度，按单字节长度算，如：“小红回家了”，需要的长度为10（如果是9也可以）</param>
        /// <param name="showF">是否显示“..”,默认显示</param>
        /// <param name="filterEmoji">是否过滤emoji表情字符,默认过滤</param>
        /// <returns></returns>
        public static string ESubString(this string stringToSub, int length, bool showF)
        {
            return ESubString(stringToSub, length, showF, true);
        }
        /// <summary>截取字符串，支持中英文混合长度
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
        /// create :MXK
        /// <param name="_right">验证的字符串</param>
        /// <param name="_pos">验证的位置 从1开始</param>
        /// <param name="_chckchr">需要验证的额字符</param>
        /// <returns></returns>
        public static bool StrIndexOfCheck(this string _right, int _pos, char _chckchr)
        {
            bool ischeck = false;
            if (!string.IsNullOrEmpty(_right))
            {
                if (_pos > _right.Length) return false;
                char[] _ary = _right.ToCharArray();
                if (_ary[_pos - 1] == _chckchr)
                {
                    ischeck = true;
                }
            }
            return ischeck;
        }





        /// <summary>验证 字符串是否是数字
        /// 验证 字符串是否是数字
        /// </summary>
        /// careate:mxk data:2012/05/30
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

        /// <summary>检查是否符合版本id
        /// </summary>
        /// <param name="edition">版本id 可以是string.Empty</param>
        /// <returns></returns>
        private static string ChkEditionStr(string edition)
        {
            if (edition == string.Empty)
            {
                return string.Empty;
            }
            if (edition.Length > 30)
            {
                return Consts.PARA_ERROR;
            }
            else
            {
                if (!edition.IsNumber())
                {
                    return Consts.PARA_ERROR;
                }
                else
                {
                    return edition;
                }
            }
        }
        /// <summary>验证输入是否是时间格式，返回1900-01-01格式字符串,支持空字符串（string.Empty）
        /// (时间在1900-1-1到5000-12-30之间)
        /// </summary>
        /// <param name="time">输入的时间字符串</param>
        /// <returns></returns>
        private static string ChkDateTimeStr(string time)
        {
            if (time == string.Empty)
            {
                return string.Empty;
            }
            DateTime mintime = DateTime.MinValue;
            bool timeflag = DateTime.TryParse(time, out mintime);
            if (!timeflag || mintime < DateTime.Parse("1900-1-1") || mintime > DateTime.Parse("5000-12-30"))
            {
                return Consts.PARA_ERROR;
            }
            else
            {
                return mintime.ToString("yyyy-MM-dd");
            }
        }

        public static int ToInt(this string str)
        {
            int ret = 0;
            if (str != null && str.Length > 0)
            {

                int.TryParse(str, out ret);
            }
            return ret;
        }
        public static string ToStringDb(this object str)
        {
            string res = "";
            if (str != null && str != DBNull.Value)
            {

                return str.ToString();
            }
            return res;
        }
        public static DateTime ToDateTime(this string str)
        {
            DateTime ret = new DateTime();
            if (str != null && str.Length > 0)
            {
                try
                {
                    ret = Convert.ToDateTime(str);

                }
                catch (Exception)
                {

                }
            }
            return ret;
        }
        public static int ToInt32(this string str)
        {
            int ret = 0;
            if (str != null && str.Length > 0 && IsNum(str))
            {
                try
                {
                    ret = Convert.ToInt32(str);

                }
                catch (Exception)
                {

                }
            }
            return ret;
        }
        public static bool IsNum(string Str)
        {
            bool bl = false;
            string Rx = @"^[1-9]\d*$";
            if (Regex.IsMatch(Str, Rx))
            {
                bl = true;
            }
            else
            {
                bl = false;
            }
            return bl;
        }

    }
}
