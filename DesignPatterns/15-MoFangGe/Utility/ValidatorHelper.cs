
/**************************************************
* 文 件 名：EncryptExt.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014/9/3 14:17:41
* 文件说明：验证类 封装一些常用的验证方法
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/

using System.Net;
using System.Text.RegularExpressions;
using Climb.Utility.SystemExt;

namespace Climb.Utility
{
    /// <summary>
    /// 验证类
    /// </summary>
    public sealed class ValidatorHelper
    {
        #region 验证手机号
        /// <summary>
        /// 验证输入字符串为18位的手机号码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static bool IsMobile(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^1[358]\d{9}$");
        }
        #endregion

        #region 验证是否是有效邮箱地址
        /// <summary>
        /// 验证是否是有效邮箱地址
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmail(string email)
        {
            return RegexExt.PatterngEmail.IsMatch(email);
        }
        #endregion

        #region 验证是否只含有汉字
        /// <summary>
        /// 验证是否只含有汉字
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool IsOnllyChinese(string inputStr)
        {
            return RegexExt.PatternChina.IsMatch(inputStr);
        }
        #endregion

        #region 判读是否是IP地址
        /// <summary>
        /// 判读是否是IP地址
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool IsIpStr(string inputStr)
        {
            IPAddress ip;
            return IPAddress.TryParse(inputStr, out ip);
        }
        #endregion

        #region 检测用户名格式是否有效
        /// <summary>
        /// 检测用户名格式是否有效 判断用户名的长度（4-20个字符）及内容（只能是汉字、字母、下划线、数字）是否合法
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool IsValidUserName(string userName)
        {
            int userNameLength = userName.CnLength();
            return userNameLength >= 4 && userNameLength <= 20 && Regex.IsMatch(userName, @"^([\u4e00-\u9fa5A-Za-z_0-9]{0,})$");
        }

        #endregion

        #region 密码有效性
        /// <summary>
        /// 密码有效性
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, @"^[A-Za-z_0-9]{6,16}$");
        }
        #endregion

        #region  int有效性
        /// <summary>
        /// int有效性
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsValidInt(string val)
        {
            return RegexExt.PatternNumber.IsMatch(val);
        }
        #endregion

        #region 验证身份证是否合法  15 和  18位两种
        /// <summary>
        /// 验证身份证是否合法  15 和  18位两种
        /// </summary>
        /// <param name="idCard">要验证的身份证</param>
        public static bool IsIdCard(string idCard)
        {
            switch (idCard.Length)
            {
                case 15:
                    return Regex.IsMatch(idCard, @"^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$");
                case 18:
                    return Regex.IsMatch(idCard, @"^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[A-Z])$", RegexOptions.IgnoreCase);
                default:
                    return false;
            }
        }
        #endregion

        #region 邮编有效性
        /// <summary>
        /// 邮编有效性
        /// </summary>
        /// <param name="zip"></param>
        /// <returns></returns>
        public static bool IsValidZip(string zip)
        {
            Regex rx = new Regex(@"^\d{6}$", RegexOptions.None);
            Match m = rx.Match(zip);
            return m.Success;
        }
        #endregion

        #region Url有效性
        /// <summary>
        /// Url有效性
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        static public bool IsValidUrl(string url)
        {
            return Regex.IsMatch(url, @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&%\$#\=~])*[^\.\,\)\(\s]$");
        }

        #endregion

        #region 判断是否为base64字符串
        /// <summary>
        /// 判断是否为base64字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBase64String(string str)
        {
            return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");
        }
        #endregion

        #region 日期检查

        /// <summary>
        /// 判断输入的字符是否为日期
        /// </summary>
        /// <param name="strValue">需要验证的字符串</param>
        /// <returns></returns>
        public static bool IsDate(string strValue)
        {
            return Regex.IsMatch(strValue, @"^((\d{2}(([02468][048])|([13579][26]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|([1-2][0-9])))))|(\d{2}(([02468][1235679])|([13579][01345789]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))))");
        }

        /// <summary>
        /// 判断输入的字符是否为日期,如2004-07-12 14:25|||1900-01-01 00:00|||9999-12-31 23:59
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool IsDateHourMinute(string strValue)
        {
            return Regex.IsMatch(strValue, @"^(19[0-9]{2}|[2-9][0-9]{3})-((0(1|3|5|7|8)|10|12)-(0[1-9]|1[0-9]|2[0-9]|3[0-1])|(0(4|6|9)|11)-(0[1-9]|1[0-9]|2[0-9]|30)|(02)-(0[1-9]|1[0-9]|2[0-9]))\x20(0[0-9]|1[0-9]|2[0-3])(:[0-5][0-9]){1}$");
        }

        #endregion

        #region SQL安全性

        /// <summary>
        /// 检测SQL语句是否安全
        /// </summary>
        /// <param name="str">需要验证的字符串</param>
        /// <returns></returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        #endregion

    }
}
