/**************************************************
* 文 件 名：Extended.cs
* 文件版本：1.0
* 创 建 人：ssh
* 联系方式： Email:shaoshouhe@mofangge.com  
* 创建日期：2015/6/18  
* 文件说明：Winform和Webform的IO工具类 
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Climb.Utility
{
    public static class Extended
    {
        /// <summary>判断字符串是否有效</summary>
        /// <param name="sValueString">字符串</param>
        /// <returns>是否有效</returns>
        public static bool IsValid(this String sValueString)
        {
            if (String.IsNullOrEmpty(sValueString)) return false;
            return (sValueString.Length > 0);
        }

        /// <summary>判断字符串是否无效</summary>
        /// <param name="sValueString">字符串</param>
        /// <returns>是否有效</returns>
        public static bool IsEmpty(this String sValueString)
        {
            return (!sValueString.IsValid());
        }

        /// <summary>将逗号分隔的参数字符串拆分为数组</summary>
        /// <param name="paramString">以逗号分隔参数字符串</param>
        /// <returns>参数数组</returns>
        public static String[] SplitComma(this String paramString)
        {
            return paramString.Split(',');
        }

        /// <summary>将逗号分隔的参数字符串拆分为数组</summary>
        /// <param name="paramString">以逗号分隔参数字符串</param>
        /// <param name="removeEmpty">是否移除空字符串</param>
        /// <returns>参数数组</returns>
        public static String[] SplitComma(this String paramString, bool removeEmpty)
        {
            Char[] sDit = new Char[] { ',' };
            if (removeEmpty)
                return paramString.Split(sDit, StringSplitOptions.RemoveEmptyEntries);
            else
                return paramString.Split(sDit, StringSplitOptions.None);
        }

        /// <summary>将回车符分隔的参数字符串拆分为数组</summary>
        /// <param name="paramString">以回车符分隔参数字符串</param>
        /// <returns>参数数组</returns>
        public static String[] SplitEnter(this String paramString)
        {
            return paramString.Split('\n');
        }

        /// <summary>将回车符分隔的参数字符串拆分为数组</summary>
        /// <param name="paramString">以回车符分隔参数字符串</param>
        /// <param name="removeEmpty">是否移除空字符串</param>
        /// <returns>参数数组</returns>
        public static String[] SplitEnter(this String paramString, bool removeEmpty)
        {
            Char[] sDit = new Char[] { '\n' };
            if (removeEmpty)
                return paramString.Split(sDit, StringSplitOptions.RemoveEmptyEntries);
            else
                return paramString.Split(sDit, StringSplitOptions.None);
        }

        /// <summary>将短横线分隔的参数字符串拆分为数组</summary>
        /// <param name="paramString">以短横线分隔参数字符串</param>
        /// <returns>参数数组</returns>
        public static String[] SplitHLine(this String paramString)
        {
            return paramString.Split('-');
        }

        /// <summary>将短横线分隔的参数字符串拆分为数组</summary>
        /// <param name="paramString">以短横线分隔参数字符串</param>
        /// <param name="removeEmpty">是否移除空字符串</param>
        /// <returns>参数数组</returns>
        public static String[] SplitHLine(this String paramString, bool removeEmpty)
        {
            Char[] sDit = new Char[] { '-' };
            if (removeEmpty)
                return paramString.Split(sDit, StringSplitOptions.RemoveEmptyEntries);
            else
                return paramString.Split(sDit, StringSplitOptions.None);
        }

        /// <summary>获取日期类型的字符串表达式</summary>
        /// <param name="dateValue">日期</param>
        /// <returns>字符串</returns>
        public static String DateValue(this DateTime? dateValue)
        {
            String sVal = "";
            if (dateValue.HasValue) sVal = dateValue.Value.ToString("yyyy-MM-dd");
            return sVal;
        }

        /// <summary>获取日期类型的短日期字符串表达式</summary>
        /// <param name="dateValue">日期</param>
        /// <returns>字符串</returns>
        public static String DateShort(this DateTime? dateValue)
        {
            String sVal = "";
            if (dateValue.HasValue) sVal = dateValue.Value.ToString("yyyy-MM");
            return sVal;
        }

        /// <summary>获取日期类型的长日期字符串表达式</summary>
        /// <param name="dateValue">日期</param>
        /// <returns>字符串</returns>
        public static String DateLongs(this DateTime? dateValue)
        {
            String sVal = "";
            if (dateValue.HasValue) sVal = dateValue.Value.ToString("yyyy-MM-dd HH:mm");
            return sVal;
        }

        /// <summary>获取日期类型的字符串表达式</summary>
        /// <param name="dateValue">日期</param>
        /// <returns>字符串</returns>
        public static String DateTitle(this DateTime? dateValue)
        {
            String sVal = "";
            if (dateValue.HasValue) sVal = dateValue.Value.ToString("yyyy年MM月dd日");
            return sVal;
        }

        /// <summary>将布尔值转换为0-2的整数</summary>
        /// <param name="boolValue">布尔值</param>
        /// <returns>0-2的整数</returns>
        public static int ToInt(this bool? boolValue)
        {
            int iBool = 0;
            if (boolValue.HasValue) iBool = (boolValue.Value ? 2 : 1);
            return iBool;
        }

        /// <summary>将0-2的整数转换为布尔值</summary>
        /// <param name="intValue">0-2的整数</param>
        /// <returns>布尔值</returns>
        public static bool? ToBool(this int intValue)
        {
            bool? boolVal = null;
            if (intValue > 1)
                boolVal = true;
            else if (intValue < 1)
                boolVal = null;
            else
                boolVal = false;
            return boolVal;
        }

        /// <summary>将字符串转换为日期</summary>
        /// <param name="dateValue">字符串</param>
        /// <returns>日期</returns>
        public static DateTime? ToDate(this String dateValue)
        {
            DateTime? dtTo = null;
            try
            {
                if (dateValue.IsValid())
                {
                    DateTime dtTemp;
                    if (DateTime.TryParse(dateValue, out dtTemp))
                        dtTo = dtTemp;
                }
            }
            catch
            { }
            return dtTo;
        }

        /// <summary>获取日期类型的SQL参数表达式</summary>
        /// <param name="dateValue">日期</param>
        /// <returns>参数表达式</returns>
        public static object SQLValue(this DateTime? dateValue)
        {
            if (dateValue.HasValue)
                return dateValue.Value;
            else
                return System.DBNull.Value;
        }

        /// <summary>合并字典</summary>
        /// <param name="dicList">字典集合</param>
        /// <param name="dicItem">选择字典</param>
        /// <param name="sDivide">分隔符号</param>
        /// <returns>字符串</returns>
        public static String MergeList(this Dictionary<String, String> dicList, String sCode, String sDivide)
        {
            String sText = "";
            foreach (var dic in dicList)
            {
                if (sText.Length > 0) sText += sDivide;
                sText += (dic.Key == sCode) ? "■" : "□";
                sText += dic.Value;
            }
            return sText;
        }

        /// <summary>合并字典</summary>
        /// <param name="dicList">字典集合</param>
        /// <param name="dicItem">选择字典</param>
        /// <param name="sDivide">分隔符号</param>
        /// <returns>字符串</returns>
        public static String MergeList(this Dictionary<object, string> dicList, string sCode, string sDivide)
        {
            String sText = "";
            foreach (var dic in dicList)
            {
                if (sText.Length > 0) sText += sDivide;
                sText += (dic.Key.ToString() == sCode) ? "■" : "□";
                sText += dic.Value;
            }
            return sText;
        }

        /// <summary>用Base64编码对字符串解码</summary>
        /// <param name="inputString">输入文本</param>
        /// <returns>文本</returns>
        public static String DecodeBase64(this String inputString)
        {
            return inputString.DecodeBase64(System.Text.Encoding.Default);
        }

        /// <summary>用Base64编码对字符串解码</summary>
        /// <param name="inputString">输入文本</param>
        /// <param name="isEncGB2312">中文方式</param>
        /// <returns>文本</returns>
        public static String DecodeBase64(this String inputString, bool isEncGB2312)
        {
            System.Text.Encoding enc;
            if (isEncGB2312)
                enc = System.Text.Encoding.GetEncoding("GB2312");
            else
                enc = System.Text.Encoding.GetEncoding(65001);
            return inputString.DecodeBase64(enc);
        }

        /// <summary>用Base64编码对字符串解码</summary>
        /// <param name="inputString">输入文本</param>
        /// <param name="txtEncoding">字符编码</param>
        /// <returns>文本</returns>
        public static String DecodeBase64(this String inputString, System.Text.Encoding txtEncoding)
        {
            String sOutput = "";
            try
            {
                if (inputString.IsValid())
                {
                    //Char[] sInput = inputString.ToCharArray();
                    byte[] bt = System.Convert.FromBase64String(inputString);
                    sOutput = txtEncoding.GetString(bt);
                }
            }
            catch (System.ArgumentNullException)  //base 64 字符数组为null
            { }
            catch (System.FormatException)  //长度错误，无法整除4
            { }
            return sOutput;
        }

        /// <summary>用Base64编码对字符串编码</summary>
        /// <param name="inputString">输入文本</param>
        /// <returns>文本</returns>
        public static String EncodeBase64(this String inputString)
        {
            return inputString.EncodeBase64(System.Text.Encoding.Default);
        }

        /// <summary>用Base64编码对字符串编码</summary>
        /// <param name="inputString">输入文本</param>
        /// <param name="isEncGB2312">中文方式</param>
        /// <returns>文本</returns>
        public static String EncodeBase64(this String inputString, bool isEncGB2312)
        {
            System.Text.Encoding enc;
            if (isEncGB2312)
                enc = System.Text.Encoding.GetEncoding("GB2312");
            else
                enc = System.Text.Encoding.GetEncoding(65001);
            return inputString.EncodeBase64(enc);
        }

        /// <summary>用Base64编码对字符串编码</summary>
        /// <param name="inputString">输入文本</param>
        /// <param name="txtEncoding">字符编码</param>
        /// <returns>文本</returns>
        public static String EncodeBase64(this String inputString, System.Text.Encoding txtEncoding)
        {
            String sOutput = inputString;
            try
            {
                if (sOutput.IsValid())
                {
                    byte[] bt = txtEncoding.GetBytes(inputString);
                    sOutput = System.Convert.ToBase64String(bt, 0, bt.Length);
                }
            }
            catch (System.ArgumentNullException) //二进制数组为NULL
            { }
            catch (System.ArgumentOutOfRangeException) //长度不够
            { }
            return sOutput;
        }
    }
}
