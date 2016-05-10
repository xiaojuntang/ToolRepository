
/**************************************************
* 文 件 名：DateTimeExt.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014/9/1 23:59:30
* 文件说明：时间和日期的扩展方法
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/

using System;
using System.Globalization;

namespace Climb.Utility.SystemExt
{
    /// <summary>
    /// 时间扩展类
    /// </summary>
    public sealed class DateTimeExt
    {
        #region 返回时间差
        /// <summary>
        /// 获取两个时间 的时间差
        /// </summary>
        /// <param name="datetimebTime">开始时间</param>
        /// <param name="dareruneeTime">结束时间</param>
        /// <returns>返回时间差的字符串</returns>
        public static string DateDiff(DateTime datetimebTime, DateTime dareruneeTime)
        {
            string dateDiff;
            TimeSpan ts = dareruneeTime - datetimebTime;
            if (ts.Days >= 1)
            {
                dateDiff = datetimebTime.Month.ToString(CultureInfo.InvariantCulture) + "月" +
                           datetimebTime.Day.ToString(CultureInfo.InvariantCulture) + "日";
            }
            else
            {
                if (ts.Hours > 1)
                {
                    dateDiff = ts.Hours.ToString(CultureInfo.InvariantCulture) + "小时前";
                }
                else
                {
                    dateDiff = ts.Minutes.ToString(CultureInfo.InvariantCulture) + "分钟前";
                }
            }
            return dateDiff;
        }
        #endregion

        #region 获得两个日期的间隔
        /// <summary>
        /// 获得两个日期的间隔
        /// </summary>
        /// <param name="dateTimebTime">日期一</param>
        /// <param name="datetimeeTime">日期二</param>
        /// <returns>日期间隔TimeSpan。</returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public static TimeSpan GetTimeSpan(DateTime dateTimebTime, DateTime datetimeeTime)
        {
            TimeSpan ts1 = new TimeSpan(dateTimebTime.Ticks);
            TimeSpan ts2 = new TimeSpan(datetimeeTime.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            return ts;
        }
        #endregion

        #region 格式化日期

        /// <summary>
        /// 格式化日期
        /// </summary>
        /// <param name="dateTime">日期时间</param>
        /// <param name="dateMode">显示模式
        /// 0--->yyyy-MM-dd
        /// 1--->yyyy-MM-dd HH:mm:ss
        /// 2--->yyyy/MM/dd
        /// 3--->yyyy年MM月dd日
        /// 4--->MM-dd
        /// 5--->MM/dd
        /// 6--->MM月dd日
        /// 7--->yyyy-MM
        /// 8--->yyyy/MM
        /// 9--->yyyy年MM月
        /// 没有默认时间格式
        /// </param>
        /// <returns>0-9种模式的日期</returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public static string FormatDate(DateTime dateTime, string dateMode)
        {
            switch (dateMode)
            {
                case "0":
                    return dateTime.ToString("yyyy-MM-dd");
                case "1":
                    return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                case "2":
                    return dateTime.ToString("yyyy/MM/dd");
                case "3":
                    return dateTime.ToString("yyyy年MM月dd日");
                case "4":
                    return dateTime.ToString("MM-dd");
                case "5":
                    return dateTime.ToString("MM/dd");
                case "6":
                    return dateTime.ToString("MM月dd日");
                case "7":
                    return dateTime.ToString("yyyy-MM");
                case "8":
                    return dateTime.ToString("yyyy/MM");
                case "9":
                    return dateTime.ToString("yyyy年MM月");
                case "10":
                    return dateTime.ToString("HH:mm:ss");
                case "11":
                    return dateTime.ToString("HH:mm");
                case "12":
                    return dateTime.ToString("HH时mm分ss秒");
                case "13":
                    return dateTime.ToString("HH时mm分");
                default:
                    return dateTime.ToString(CultureInfo.InvariantCulture);
            }
        }
        #endregion

        #region 格式化时间
        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="dateTime">日期时间</param>
        /// <param name="dateMode">显示模式
        /// 0--->HH:mm:ss
        /// 1--->HH:mm
        /// 2--->HH时mm分ss秒
        /// 3--->HH时mm分
        /// 4--->HH时mm分
        /// 5--->HH
        /// 没有默认时间格式
        /// </param>
        /// <returns>0-5种模式的时间</returns>
        public static string FormatTime(DateTime dateTime, string dateMode)
        {
            switch (dateMode)
            {
                case "1":
                    return dateTime.ToString("HH:mm:ss");
                case "2":
                    return dateTime.ToString("HH:mm");
                case "3":
                    return dateTime.ToString("HH时mm分ss秒");
                case "4":
                    return dateTime.ToString("HH时mm分");
                case "5":
                    return dateTime.ToString("HH");
                default:
                    return dateTime.ToString(CultureInfo.InvariantCulture);
            }
        }
        #endregion

        #region 两时间比较
        /// <summary>
        /// 两时间比较
        /// </summary>
        /// <param name="dateTime">开始时间</param>
        /// <param name="compareTime">结束时间</param>
        /// <returns>如果第一个时间大于第二个时间返回true 其他则为false</returns>
        public static bool DiffDateTime(DateTime dateTime, DateTime compareTime)
        {
            return dateTime.CompareTo(compareTime) > 0;
        }
        #endregion

        #region 一天的开始时间
        /// <summary>
        /// 一天的开始时间
        /// </summary>
        /// <returns>返回时间字符串</returns>
        public static string GetBeginDayStr()
        {
            return string.Concat(FormatDate(DateTime.Now, "0"), " 00:00:00.000");
        }
        #endregion

        #region 一天的结束时间
        /// <summary>
        /// 获取 当前的时间一天的结束时间
        /// </summary>
        /// <returns> 返回时间字符串</returns>
        public static string GetEndDayStr()
        {
            return string.Concat(FormatDate(DateTime.Now, "0"), " 23:59:59.999");
        }
        #endregion

        #region 中国式的时间格式
        ///<summary>
        ///中国式的时间格式
        ///子时：23:00-1:00
        ///丑时：1:00-3:00
        ///寅时：3:00-5:00
        ///卯时：5:00-7:00
        ///辰时：7:00-9:00
        ///巳时：9:00-11:00
        ///午时：11:00-13:00
        ///未时：13:00-15:00
        ///申时：15:00-17:00
        ///酉时：17:00-19:00
        ///戌时：19:00-21:00
        ///亥时：21:00-23:00
        /// </summary>
        /// <param name="date">时间参数</param>
        /// <returns></returns>
        public static string GetChineseDate(DateTime date)
        {
            var cnDate = new ChineseLunisolarCalendar();
            string[] arrMonth = { "", "正月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "冬月", "腊月" };
            string[] arrDay = { "", "初一", "初二", "初三", "初四", "初五", "初六", "初七", "初八", "初九", "初十", "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九", "廿一", "廿二", "廿三", "廿四", "廿五", "廿六", "廿七", "廿八", "廿九", "三十" };
            string[] arrYear = { "", "甲子", "乙丑", "丙寅", "丁卯", "戊辰", "己巳", "庚午", "辛未", "壬申", "癸酉", "甲戌", "乙亥", "丙子", "丁丑", "戊寅", "己卯", "庚辰", "辛己", "壬午", "癸未", "甲申", "乙酉", "丙戌", "丁亥", "戊子", "己丑", "庚寅", "辛卯", "壬辰", "癸巳", "甲午", "乙未", "丙申", "丁酉", "戊戌", "己亥", "庚子", "辛丑", "壬寅", "癸丑", "甲辰", "乙巳", "丙午", "丁未", "戊申", "己酉", "庚戌", "辛亥", "壬子", "癸丑", "甲寅", "乙卯", "丙辰", "丁巳", "戊午", "己未", "庚申", "辛酉", "壬戌", "癸亥" };

            var lYear = cnDate.GetYear(date);
            var sYear = arrYear[cnDate.GetSexagenaryYear(date)];
            var lMonth = cnDate.GetMonth(date);
            var lDay = cnDate.GetDayOfMonth(date);

            //获取第几个月是闰月,等于0表示本年无闰月
            var leapMonth = cnDate.GetLeapMonth(lYear);
            var sMonth = arrMonth[lMonth];
            //如果今年有闰月  
            if (leapMonth > 0)
            {
                //闰月数等于当前月份  
                sMonth = lMonth == leapMonth ? string.Format("闰{0}", arrMonth[lMonth - 1]) : sMonth;
                sMonth = lMonth > leapMonth ? arrMonth[lMonth - 1] : sMonth;
            }
            return string.Format("{0}年{1}{2}", sYear, sMonth, arrDay[lDay]);
        }
        #endregion

        #region 最大的天数
        /// <summary>
        /// 获取日期月的最大天数
        /// </summary>
        /// <param name="dt">时间</param>
        /// <returns>返回天数</returns>
        public static int DaysInMonth(DateTime dt)
        {
            return DateTime.DaysInMonth(dt.Year, dt.Month);
        }
        #endregion

        #region 获取年龄
        /// <summary>
        /// 获取年龄
        /// </summary>
        /// <param name="dateOfBirth">出生日期</param>
        /// <returns>返回年龄</returns>
        public static int CalculateAge(DateTime dateOfBirth)
        {
            return CalculateAge(dateOfBirth, DateTime.Today);
        }
        /// <summary>
        /// 获取年龄
        /// </summary>
        /// <param name="dateOfBirth">出生日期</param>
        /// <param name="referenceDate">比较的时间</param>
        /// <returns>返回年龄</returns>
        public static int CalculateAge(DateTime dateOfBirth, DateTime referenceDate)
        {
            int years = referenceDate.Year - dateOfBirth.Year;
            if (referenceDate.Month < dateOfBirth.Month || (referenceDate.Month == dateOfBirth.Month && referenceDate.Day < dateOfBirth.Day)) --years;
            return years;
        }
        #endregion

        #region 时间是否是今天
        /// <summary>
        /// 时间是否是当天
        /// </summary>
        /// <param name="dt">日期时间</param>
        /// <returns>返回bool 值  是今天 返回true</returns>
        public static bool IsToday(DateTime dt)
        {
            return (dt.Date == DateTime.Today);
        }

        #endregion

        #region 时间是否是工作日
        /// <summary>
        /// 时间是否是工作日
        /// </summary>
        /// <param name="date">时间</param>
        /// <returns>是工作日返回true</returns>
        public static bool IsWeekDay(DateTime date)
        {
            return !IsWeekend(date);
        }
        
        #endregion

        #region 时间是否是周末
        /// <summary>
        /// 时间是否是周末
        /// </summary>
        /// <param name="value">时间日期</param>
        /// <returns>返回bool值  是周末返回true</returns>
        public static bool IsWeekend(DateTime value)
        {
            return value.DayOfWeek == DayOfWeek.Sunday || value.DayOfWeek == DayOfWeek.Saturday;
        }
        #endregion

        private static readonly TimeSpan OneMinute = new TimeSpan(0, 1, 0);
        private static readonly TimeSpan TwoMinutes = new TimeSpan(0, 2, 0);
        private static readonly TimeSpan OneHour = new TimeSpan(1, 0, 0);
        private static readonly TimeSpan TwoHours = new TimeSpan(2, 0, 0);
        private static readonly TimeSpan OneDay = new TimeSpan(1, 0, 0, 0);
        private static readonly TimeSpan TwoDays = new TimeSpan(2, 0, 0, 0);
        private static readonly TimeSpan OneWeek = new TimeSpan(7, 0, 0, 0);
        private static readonly TimeSpan TwoWeeks = new TimeSpan(14, 0, 0, 0);
        private static readonly TimeSpan OneMonth = new TimeSpan(31, 0, 0, 0);
        private static readonly TimeSpan TwoMonths = new TimeSpan(62, 0, 0, 0);
        private static readonly TimeSpan OneYear = new TimeSpan(365, 0, 0, 0);
        private static readonly TimeSpan TwoYears = new TimeSpan(730, 0, 0, 0);

        #region 时间和当前时间 对比 返回 字符串提示 如昨天 未来 现在 一个小时前 昨天
        /// <summary>
        /// 时间和当前时间 对比 返回 字符串提示
        /// </summary>
        /// <param name="date">时间</param>
        /// <returns>返回字符串提示 未来 现在</returns>
        public static string ToAgo(DateTime date)
        {
            TimeSpan timeSpan = GetTimeSpan(DateTime.Now, date);
            if (timeSpan < TimeSpan.Zero) return "未来";
            if (timeSpan < OneMinute) return "现在";
            if (timeSpan < TwoMinutes) return "1 分钟前";
            if (timeSpan < OneHour) return string .Format("{0} 分钟前", timeSpan.Minutes);
            if (timeSpan < TwoHours) return "1 小时前";
            if (timeSpan < OneDay) return string.Format("{0} 小时前", timeSpan.Hours);
            if (timeSpan < TwoDays) return "昨天";
            if (timeSpan < OneWeek) return string.Format("{0} 天前", timeSpan.Days);
            if (timeSpan < TwoWeeks) return "1 周前";
            if (timeSpan < OneMonth) return string.Format("{0} 周前", timeSpan.Days / 7);
            if (timeSpan < TwoMonths) return "1 月前";
            if (timeSpan < OneYear) return string.Format("{0} 月前", timeSpan.Days / 31);
            if (timeSpan < TwoYears) return "1 年前";

            return string.Format("{0} 年前", timeSpan.Days / 365);
        }
        #endregion

        #region 得到随机日期
        /// <summary>
        /// 得到随机日期
        /// </summary>
        /// <param name="time1">起始日期</param>
        /// <param name="time2">结束日期</param>
        /// <returns></returns>
        public static DateTime GetRandomTime(DateTime time1, DateTime time2)
        {
            Random random = new Random();
            DateTime minTime;
            new DateTime();

            var ts = new TimeSpan(time1.Ticks - time2.Ticks);

            // 获取两个时间相隔的秒数
            double dTotalSecontds = ts.TotalSeconds;
            int iTotalSecontds;

            if (dTotalSecontds > Int32.MaxValue)
            {
                iTotalSecontds = Int32.MaxValue;
            }
            else if (dTotalSecontds < Int32.MinValue)
            {
                iTotalSecontds = Int32.MinValue;
            }
            else
            {
                iTotalSecontds = (int)dTotalSecontds;
            }


            if (iTotalSecontds > 0)
            {
                minTime = time2;
            }
            else if (iTotalSecontds < 0)
            {
                minTime = time1;
            }
            else
            {
                return time1;
            }

            int maxValue = iTotalSecontds;

            if (iTotalSecontds <= Int32.MinValue)
                maxValue = Int32.MinValue + 1;

            int i = random.Next(Math.Abs(maxValue));

            return minTime.AddSeconds(i);
        }
        #endregion
    }

}
