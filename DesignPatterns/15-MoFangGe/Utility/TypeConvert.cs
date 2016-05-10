/**************************************************
* 文 件 名：MFGTypeConvert.cs
* 文件版本：1.0
* 创 建 人：ssh
* 联系方式： Email:shaoshouhe@mofangge.com  
* 创建日期：2015/6/18  
* 文件说明：类型转换类
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Climb.Utility
{
    /// <summary>
    /// 类型转化类
    /// </summary>
    public class TypeConvert
    {
        #region 将Object转换成Float
        /// <summary>
        /// 将Object转换成Float
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static float ObjectToFloat(object strValue)
        {
            return ObjectToFloat(strValue.ToString(), 0f);
        }

        /// <summary>
        /// 将Object转换成Float
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static float ObjectToFloat(object strValue, float defValue)
        {
            if (strValue == null)
            {
                return defValue;
            }
            return StrToFloat(strValue.ToString(), defValue);
        }
        #endregion

        #region 将Object转换成Int
        /// <summary>
        /// 将Object转换成Int
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static int ObjectToInt(object expression)
        {
            return ObjectToInt(expression, 0);
        }

        /// <summary>
        /// 将Object转换成Int
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static int ObjectToInt(object expression, int defValue)
        {
            if (expression != null)
            {
                return StrToInt(expression.ToString(), defValue);
            }
            return defValue;
        }
        #endregion

        #region 将string转换成Bool
        /// <summary>
        /// 将string转换成Bool
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            if (expression != null)
            {
                return StrToBool(expression, defValue);
            }
            return defValue;
        }

        /// <summary>
        /// 将string转换成Bool
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static bool StrToBool(string expression, bool defValue)
        {
            if (expression != null)
            {
                if (string.Compare(expression, "true", true) == 0)
                {
                    return true;
                }
                if (string.Compare(expression, "false", true) == 0)
                {
                    return false;
                }
            }
            return defValue;
        }
        #endregion

        #region 将string转换成Float
        /// <summary>
        /// 将string转换成Float
        /// </summary>
        /// <param name="strValue">需要转换的字符串</param>
        /// <returns></returns>
        public static float StrToFloat(object strValue)
        {
            if (strValue == null)
            {
                return 0f;
            }
            return StrToFloat(strValue.ToString(), 0f);
        }

        /// <summary>
        /// 将string转换成Float
        /// </summary>
        /// <param name="strValue">需要转换的字符串</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            if (strValue == null)
            {
                return defValue;
            }
            return StrToFloat(strValue.ToString(), defValue);
        }

        /// <summary>
        /// 将string转换成Float
        /// </summary>
        /// <param name="strValue">需要转换的字符串</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static float StrToFloat(string strValue, float defValue)
        {
            if ((strValue == null) || (strValue.Length > 10))
            {
                return defValue;
            }
            float result = defValue;
            if ((strValue != null) && Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
            {
                float.TryParse(strValue, out result);
            }
            return result;
        }

       
        #endregion

        #region 将string转换成Int
        /// <summary>
        /// 将string转换成Int
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <returns></returns>
        public static int StrToInt(string str)
        {
            return StrToInt(str, 0);
        }

        /// <summary>
        /// 将string转换成Int
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static int StrToInt(string str, int defValue)
        {
            int num;
            if (!((!string.IsNullOrEmpty(str) && (str.Trim().Length < 11)) && Regex.IsMatch(str.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$")))
            {
                return defValue;
            }
            if (int.TryParse(str, out num))
            {
                return num;
            }
            return Convert.ToInt32(StrToFloat(str, (float)defValue));
        }
        #endregion

        #region 将string转换成DateTime
        /// <summary>将字符串转换成日期</summary>
        /// <param name="StrDateString">字符串</param>
        /// <returns>返回日期数据</returns>
        public static DateTime StrToDate(string StrDateString)
        {
            DateTime udate = DateTime.MinValue;
            try
            { udate = DateTime.Parse(StrDateString); }
            catch
            { }
            return udate;
        }

        /// <summary>
        /// 将字符串转换成日期
        /// </summary>
        /// <param name="strDateString">字符串格式</param>
        /// <param name="format">指定 的日期格式</param>
        /// <returns>返回转换后的日期</returns>
        public static DateTime StrToDate(string strDateString, IFormatProvider format)
        {
            DateTime udate = DateTime.MinValue;
            try
            {
                udate = DateTime.Parse(strDateString, format);
            }
            catch
            {

            }

            return udate;
        }

        /// <summary>
        /// 将字符串转换成日期
        /// </summary>
        /// <param name="obj">指定 的日期格式</param>
        /// <returns>返回转换后的日期</returns>
        public static string StrToDate(object obj)
        {
            string r = string.Empty;
            try
            {

                r = Convert.ToDateTime(obj).ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch
            {
                // ignored
            }

            return r;
        }

        #endregion

        #region 将DateTime转换成string类型

        /// <summary>将日期数据转换成字符串</summary>
        /// <param name="VarDateTime">日期数据</param>
        /// <param name="IntDateType">转换格式：0-普通,1-长日期,2-长时间,3-短日期,4-短时间,5-中文日期,6-中文时间,7-中文日期时间</param>
        /// <returns>返回字符串</returns>
        public static string DateToStr(System.DateTime VarDateTime, int IntDateType)
        {
            string udate = "";
            switch (IntDateType)
            {
                case 1: udate = VarDateTime.ToLongDateString(); break;
                case 2: udate = VarDateTime.ToLongTimeString(); break;
                case 3: udate = VarDateTime.ToShortDateString(); break;
                case 4: udate = VarDateTime.ToShortTimeString(); break;
                case 5:
                    {
                        udate = VarDateTime.Year.ToString() + "年";
                        udate += VarDateTime.Month.ToString() + "月";
                        udate += VarDateTime.Day.ToString() + "日";
                        break;
                    }
                case 6:
                    {
                        udate = VarDateTime.Hour.ToString() + "时";
                        udate += VarDateTime.Minute.ToString() + "分";
                        udate += VarDateTime.Second.ToString() + "秒";
                        break;
                    }
                case 7:
                    {
                        udate = VarDateTime.Year.ToString() + "年";
                        udate += VarDateTime.Month.ToString() + "月";
                        udate += VarDateTime.Day.ToString() + "日 ";
                        udate += VarDateTime.Hour.ToString() + "时";
                        udate += VarDateTime.Minute.ToString() + "分";
                        udate += VarDateTime.Second.ToString() + "秒";
                        break;
                    }
                default: udate = VarDateTime.ToString(); break;
            }
            return udate;
        }

        /// <summary>将日期数据转换成字符串</summary>
        /// <param name="VarDateTime">日期数据</param>
        /// <param name="StrDateType">转换格式</param>
        /// <returns>返回字符串</returns>
        public static string DateToStr(System.DateTime VarDateTime, string StrDateType)
        {
            if (StrDateType.Length > 0)
            {
                return VarDateTime.ToString(StrDateType);
            }
            else
            {
                return VarDateTime.ToString();
            }
        }

        /// <summary>
        /// 日期格式yyyy-MM-dd
        /// </summary>
        /// <param name="str">时间串</param>
        /// <returns>默认返回最小时间</returns>
        public static string StrToDate2(object str)
        {
            DateTime udate = DateTime.MinValue;
            try
            {
                DateTime.TryParse(str.ToString(), out udate);

            }
            catch (Exception)
            {


            }

            return udate.ToString("yyyy-MM-dd");
        }

        #endregion
    }
}
