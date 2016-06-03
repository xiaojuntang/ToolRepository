using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Core
{
    /// <summary>
    /// 时间处理类
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// 返回标准日期格式 例：yyyy-MM-dd
        /// </summary>
        public static string GetDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 返回指定日期格式
        /// </summary>
        /// <param name="datetimestr"></param>
        /// <param name="replacestr"></param>
        /// <returns></returns>
        public static string GetDate(string datetimestr, string replacestr)
        {
            if (datetimestr == null)
            {
                return replacestr;
            }
            if (datetimestr.Equals(""))
            {
                return replacestr;
            }
            try
            {
                datetimestr = Convert.ToDateTime(datetimestr).ToString("yyyy-MM-dd").Replace("1900-01-01", replacestr);
            }
            catch
            {
                return replacestr;
            }
            return datetimestr;
        }

        /// <summary>
        /// 返回标准时间格式 例：HH:mm:ss
        /// </summary>
        public static string GetTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 返回标准时间格式 例：yyyy-MM-dd HH:mm:ss
        /// </summary>
        public static string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 返回相对于当前时间的相对天数 当前时间加relativeday天的时间
        /// </summary>
        /// <param name="relativeday">天数</param>
        /// <returns></returns>
        public static string GetDateTime(int relativeday)
        {
            return DateTime.Now.AddDays(relativeday).ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 返回标准时间格式 例：yyyy-MM-dd HH:mm:ss:fffffff
        /// </summary>
        public static string GetDateTimeF()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
        }

        /// <summary>
        /// 返回标准时间 
        /// </summary>
        /// <param name="fDateTime"></param>
        /// <param name="formatStr">时间格式</param>
        /// <returns></returns>
        public static string GetStandardDateTime(string fDateTime, string formatStr)
        {
            if (fDateTime == "0000-0-0 0:00:00")
            {
                return fDateTime;
            }
            DateTime s = Convert.ToDateTime(fDateTime);
            return s.ToString(formatStr);
        }

        /// <summary>
        /// 返回标准时间 yyyy-MM-dd HH:mm:ss
        /// </sumary>
        public static string GetStandardDateTime(string fDateTime)
        {
            return GetStandardDateTime(fDateTime, "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 当前时间加times分钟的时间
        /// </summary>
        /// <param name="times"></param>
        /// <returns></returns>
        public static string AdDeTime(int times)
        {
            string newtime = (DateTime.Now).AddMinutes(times).ToString();
            return newtime;
        }

        /// <summary>返回本年有多少天</summary>
        /// <param name="iYear">年份</param>
        /// <returns>本年的天数</returns>
        public static int GetDaysOfYear(int iYear)
        {
            int cnt = 0;
            if (IsRuYear(iYear))
            {
                //闰年多 1 天 即：2 月为 29 天
                cnt = 366;

            }
            else
            {
                //--非闰年少1天 即：2 月为 28 天
                cnt = 365;
            }
            return cnt;
        }

        /// <summary>本年有多少天</summary>
        /// <param name="dt">日期</param>
        /// <returns>本天在当年的天数</returns>
        public static int GetDaysOfYear(DateTime idt)
        {
            int n;

            //取得传入参数的年份部分，用来判断是否是闰年

            n = idt.Year;
            if (IsRuYear(n))
            {
                //闰年多 1 天 即：2 月为 29 天
                return 366;
            }
            else
            {
                //--非闰年少1天 即：2 月为 28 天
                return 365;
            }

        }

        /// <summary>本月有多少天</summary>
        /// <param name="iYear">年</param>
        /// <param name="Month">月</param>
        /// <returns>天数</returns>
        public static int GetDaysOfMonth(int iYear, int Month)
        {
            int days = 0;
            switch (Month)
            {
                case 1:
                    days = 31;
                    break;
                case 2:
                    if (IsRuYear(iYear))
                    {
                        //闰年多 1 天 即：2 月为 29 天
                        days = 29;
                    }
                    else
                    {
                        //--非闰年少1天 即：2 月为 28 天
                        days = 28;
                    }

                    break;
                case 3:
                    days = 31;
                    break;
                case 4:
                    days = 30;
                    break;
                case 5:
                    days = 31;
                    break;
                case 6:
                    days = 30;
                    break;
                case 7:
                    days = 31;
                    break;
                case 8:
                    days = 31;
                    break;
                case 9:
                    days = 30;
                    break;
                case 10:
                    days = 31;
                    break;
                case 11:
                    days = 30;
                    break;
                case 12:
                    days = 31;
                    break;
            }

            return days;


        }

        /// <summary>本月有多少天</summary>
        /// <param name="dt">日期</param>
        /// <returns>天数</returns>
        public static int GetDaysOfMonth(DateTime dt)
        {
            //--------------------------------//
            //--从dt中取得当前的年，月信息  --//
            //--------------------------------//
            int year, month, days = 0;
            year = dt.Year;
            month = dt.Month;

            //--利用年月信息，得到当前月的天数信息。
            switch (month)
            {
                case 1:
                    days = 31;
                    break;
                case 2:
                    if (IsRuYear(year))
                    {
                        //闰年多 1 天 即：2 月为 29 天
                        days = 29;
                    }
                    else
                    {
                        //--非闰年少1天 即：2 月为 28 天
                        days = 28;
                    }

                    break;
                case 3:
                    days = 31;
                    break;
                case 4:
                    days = 30;
                    break;
                case 5:
                    days = 31;
                    break;
                case 6:
                    days = 30;
                    break;
                case 7:
                    days = 31;
                    break;
                case 8:
                    days = 31;
                    break;
                case 9:
                    days = 30;
                    break;
                case 10:
                    days = 31;
                    break;
                case 11:
                    days = 30;
                    break;
                case 12:
                    days = 31;
                    break;
            }

            return days;

        }

        /// <summary>返回当前日期的星期名称</summary>
        /// <param name="dt">日期</param>
        /// <returns>星期名称</returns>
        public static string GetWeekNameOfDay(DateTime idt)
        {
            string dt, week = "";

            dt = idt.DayOfWeek.ToString();
            switch (dt)
            {
                case "Mondy":
                    week = "星期一";
                    break;
                case "Tuesday":
                    week = "星期二";
                    break;
                case "Wednesday":
                    week = "星期三";
                    break;
                case "Thursday":
                    week = "星期四";
                    break;
                case "Friday":
                    week = "星期五";
                    break;
                case "Saturday":
                    week = "星期六";
                    break;
                case "Sunday":
                    week = "星期日";
                    break;

            }
            return week;
        }

        /// <summary>返回当前日期的星期编号</summary>
        /// <param name="dt">日期</param>
        /// <returns>星期数字编号</returns>
        public static string GetWeekNumberOfDay(DateTime idt)
        {
            string dt, week = "";

            dt = idt.DayOfWeek.ToString();
            switch (dt)
            {
                case "Mondy":
                    week = "1";
                    break;
                case "Tuesday":
                    week = "2";
                    break;
                case "Wednesday":
                    week = "3";
                    break;
                case "Thursday":
                    week = "4";
                    break;
                case "Friday":
                    week = "5";
                    break;
                case "Saturday":
                    week = "6";
                    break;
                case "Sunday":
                    week = "7";
                    break;
            }
            return week;
        }

        /// <summary>返回两个日期之间相差的天数</summary>
        /// <param name="dt">两个日期参数</param>
        /// <returns>天数</returns>
        public static int DiffDays(DateTime dtfrm, DateTime dtto)
        {
            int diffcnt = 0;
            //diffcnt = dtto- dtfrm ;

            return diffcnt;
        }

        /// <summary>判断当前日期所属的年份是否是闰年，私有函数</summary>
        /// <param name="dt">日期</param>
        /// <returns>是闰年：True ，不是闰年：False</returns>
        private static bool IsRuYear(DateTime idt)
        {
            //形式参数为日期类型 
            //例如：2003-12-12
            int n;
            n = idt.Year;

            if ((n % 400 == 0) || (n % 4 == 0 && n % 100 != 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>判断当前年份是否是闰年，私有函数</summary>
        /// <param name="dt">年份</param>
        /// <returns>是闰年：True ，不是闰年：False</returns>
        private static bool IsRuYear(int iYear)
        {
            //形式参数为年份
            //例如：2003
            int n;
            n = iYear;

            if ((n % 400 == 0) || (n % 4 == 0 && n % 100 != 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 将输入的字符串转化为日期。如果字符串的格式非法，则返回当前日期。
        /// </summary>
        /// <param name="strInput">输入字符串</param>
        /// <returns>日期对象</returns>
        public static DateTime ConvertStringToDate(string strInput)
        {
            DateTime oDateTime;

            try
            {
                oDateTime = DateTime.Parse(strInput);
            }
            catch (Exception)
            {
                oDateTime = DateTime.Today;
            }

            return oDateTime;
        }

        /// <summary>
        /// 将日期对象转化为格式字符串
        /// </summary>
        /// <param name="oDateTime">日期对象</param>
        /// <param name="strFormat">
        /// 格式：
        ///		"SHORTDATE"===短日期
        ///		"LONGDATE"==长日期
        ///		其它====自定义格式
        /// </param>
        /// <returns>日期字符串</returns>
        public static string ConvertDateToString(DateTime oDateTime, string strFormat)
        {
            string strDate = "";

            try
            {
                switch (strFormat.ToUpper())
                {
                    case "SHORTDATE":
                        strDate = oDateTime.ToShortDateString();
                        break;
                    case "LONGDATE":
                        strDate = oDateTime.ToLongDateString();
                        break;
                    default:
                        strDate = oDateTime.ToString(strFormat);
                        break;
                }
            }
            catch (Exception)
            {
                strDate = oDateTime.ToShortDateString();
            }

            return strDate;
        }

        /// <summary>
        /// 判断是否为合法日期，必须大于1800年1月1日
        /// </summary>
        /// <param name="strDate">输入日期字符串</param>
        /// <returns>True/False</returns>
        public static bool IsDateTime(string strDate)
        {
            try
            {
                DateTime oDate = DateTime.Parse(strDate);
                if (oDate.CompareTo(DateTime.Parse("1800-1-1")) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 当前的时间一天的起始时间
        /// </summary>
        /// <returns> 返回时间字符串</returns>
        public static string GetBeginDayStr()
        {
            return DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00.000";
        }
        /// <summary>
        /// 前的时间一天的结束时间
        /// </summary>
        /// <returns> 返回时间字符串</returns>
        public static string GetEndDayStr()
        {
            return DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59.999";
        }
        /// <summary>
        /// 获取指定时间到当前时间的时间差字符串
        /// </summary>
        /// <param name="time">指定的时间</param>
        /// <param name="time2">目标时间</param>
        /// <param name="outformat">默认输出格式</param>
        /// <returns></returns>
        public static string GetTimeSpanString(DateTime time2, DateTime time, string outformat)
        {
            TimeSpan ts = time2.Subtract(time);
            if (ts.Days > 30)
            {
                return time.ToString(outformat);
            }
            else if (ts.Days > 0)
            {
                return ts.Days + "天前";
            }
            else if (ts.Hours > 0)
            {
                return ts.Hours + "小时前";
            }
            else if (ts.Minutes > 0)
            {
                return ts.Minutes + "分钟前";
            }
            else
            {
                return "刚刚";
            }
        }

        /// <summary>
        /// 获取指定时间到当前时间的日期差字符串
        /// </summary>
        /// <param name="time">指定的时间</param>
        /// <param name="time2">目标时间</param>
        /// <param name="outformat">默认输出格式</param>
        /// <returns>今天，昨天，前天，几天前（小于30天），日期（大于30天）</returns>
        public static string GetDateSpanString(DateTime time, DateTime time2, string outformat)
        {
            TimeSpan ts = time2.Date.Subtract(time.Date);
            if (ts.Days > 30)
            {
                return time.ToString(outformat);
            }
            else if (ts.Days > 2)
            {
                return ts.Days + "天前";
            }
            else if (ts.Days > 1)
            {

                return "前天";
            }
            else if (ts.Days > 0)
            {
                return "昨天";
            }
            else
            {
                return "今天";
            }
        }
        /// <summary>
        /// 获取指定时间到当前时间的日期差字符串
        /// </summary>
        /// <param name="time">指定的时间</param>
        /// <param name="time2">目标时间</param>
        /// <param name="hourstyle">小时[-分钟][-秒]输出格式</param>
        /// <param name="outformat">默认输出格式</param>
        /// <returns>今天6：10，昨天6：10，前天6：10，几天前（小于30天），日期（大于30天）</returns>
        public static string GetDateSpanStringWithTime(DateTime time2, DateTime time, string hourstyle, string outformat)
        {
            TimeSpan ts = time2.Date.Subtract(time.Date);
            if (ts.Days > 30)
            {
                return time.ToString(outformat);
            }
            else if (ts.Days > 2)
            {
                return ts.Days + "天前";
            }
            else if (ts.Days > 1)
            {
                return "前天" + time.ToString(hourstyle);
            }
            else if (ts.Days > 0)
            {
                return "昨天" + time.ToString(hourstyle);
            }
            else
            {
                return "今天" + time.ToString(hourstyle);
            }
        }
        /// <summary>
        /// 返回bool类型 判断是否小于当前时间 小于则返回true,不小于返回false
        /// </summary>
        /// <returns>小于当前时间则返回false,不小于当前时间返回true</returns>
        public static bool IsMinnerTime(DateTime time)
        {
            DateTime currentTime = System.DateTime.Now;//获取当前系统时间
            if (currentTime > time)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 得到时间的字符串
        /// </summary>
        /// <param name="td">要解析的时间</param>
        /// <param name="spliter">日期分隔符（大写X代表汉字分割符）</param>
        /// <param name="len">返回的时间长度1-6分别为年 月 日 时 分 秒（如果为空，则返回1-14的纯时间数字）</param>
        /// <returns>解析完成的字符串</returns>
        public static string getNow(DateTime td, string spliter, int len)
        {
            string rtStr = string.Empty;
            if (spliter == string.Empty)
            {

                rtStr = td.ToString("yyyyMMddhhmmssfff");
                return rtStr.Substring(0, len > 17 ? 17 : len);
            }
            else
            {
                string[] tmpDt = new string[7];
                if (spliter == "X")
                {
                    tmpDt[0] = td.Year.ToString() + "年";
                    tmpDt[1] = td.Month.ToString() + "月";
                    tmpDt[2] = td.Day.ToString() + "日";
                    tmpDt[3] = td.Hour.ToString() + "时";
                    tmpDt[4] = td.Minute.ToString() + "分";
                    tmpDt[5] = td.Second.ToString() + "秒";
                    tmpDt[6] = td.Millisecond.ToString() + "毫秒";
                }
                else
                {
                    tmpDt[0] = td.Year.ToString();
                    tmpDt[1] = spliter + td.Month.ToString();
                    tmpDt[2] = spliter + td.Day.ToString();
                    tmpDt[3] = " " + td.Hour.ToString();
                    tmpDt[4] = ":" + td.Minute.ToString();
                    tmpDt[5] = ":" + td.Second.ToString();
                    tmpDt[6] = " " + td.Millisecond.ToString();
                }
                for (int i = 0; i < len; i++)
                {
                    if (i > 6) break;
                    rtStr += tmpDt[i];
                }
                return rtStr;

            }
        }
        /// <summary>
        /// 得到当前时间的字符串
        /// </summary>
        /// <param name="spliter">日期分隔符（大写X代表汉字分割符）</param>
        /// <param name="len">返回的时间长度1-6分别为年 月 日 时 分 秒（如果为空，则返回1-14的纯时间数字）</param>
        /// <returns>解析完成的字符串</returns>        
        public static string getNow(string spliter, int len)
        {
            return getNow(DateTime.Now, spliter, len);
        }
        /// <summary>
        /// 计算两个日期相隔的天数和小时数
        /// </summary>
        /// <param name="dt1">开始日期</param>
        /// <param name="dt2">截止日期</param>
        /// <returns>相隔的天数和小时数</returns>
        public static string GetDayAndHours(this DateTime dt1, DateTime dt2)
        {
            double jg = (dt1 - dt2).TotalDays;
            //取相隔天数
            double iDay = Math.Floor(jg);
            //取相隔小时数
            double iHour = Math.Ceiling((jg - iDay) * 24);
            return string.Format("{0}天{1}小时", iDay, iHour);
        }
    }
}

