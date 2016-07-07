using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Net.Core
{
    /// <summary>
    /// 数据转换库
    /// </summary>
    public static class DataConverter
    {
        #region 基础判别函数
        /// <summary>
        /// 对象是否非空
        /// 为空返回 false
        /// 不为空返回 true
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <returns>bool值</returns>
        public static bool NotNull(object Object) { return !IsNull(Object, false); }
        /// <summary>
        /// 对象是否非空
        /// 为空返回 false
        /// 不为空返回 true
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <param name="IsRemoveSpace">是否去除空格</param>
        /// <returns>bool值</returns>
        public static bool NotNull(object Object, bool IsRemoveSpace) { return !IsNull(Object, IsRemoveSpace); }
        /// <summary>
        /// 对象是否为空
        /// 为空返回 true
        /// 不为空返回 false
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <returns>bool值</returns>
        public static bool IsNull(object Object) { return IsNull(Object, false); }
        /// <summary>
        /// 对象是否为空
        /// 为空返回 true
        /// 不为空返回 false
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <param name="IsRemoveSpace">是否去除空格</param>
        /// <returns>bool值</returns>
        public static bool IsNull(object Object, bool IsRemoveSpace)
        {
            if (Object == null) return true;
            string Objects = Object.ToString();
            if (Objects == "") return true;
            if (IsRemoveSpace)
            {
                if (Objects.Replace(" ", "") == "") return true;
                if (Objects.Replace("　", "") == "") return true;
            }
            return false;
        }
        /// <summary>
        /// 对象是否为bool值
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <returns>bool值</returns>
        public static bool IsBool(object Object) { return IsBool(Object, false); }
        /// <summary>
        /// 判断是否为bool值
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <param name="Default">默认bool值</param>
        /// <returns>bool值</returns>
        public static bool IsBool(object Object, bool Default)
        {
            if (IsNull(Object)) return Default;
            try { return bool.Parse(Object.ToString()); }
            catch { return Default; }
        }
        /// <summary>
        /// 是否邮件地址
        /// </summary>
        /// <param name="Mail">等待验证的邮件地址</param>
        /// <returns>bool</returns>
        public static bool IsMail(string Mail) { return Regex.IsMatch(Mail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"); }
        /// <summary>
        /// 是否URL地址
        /// </summary>
        /// <param name="HttpUrl">等待验证的Url地址</param>
        /// <returns>bool</returns>
        public static bool IsHttp(string HttpUrl) { return Regex.IsMatch(HttpUrl, @"^(http|https):\/\/[A-Za-z0-9%\-_@]+\.[A-Za-z0-9%\-_@]{2,}[A-Za-z0-9\.\/=\?%\-&_~`@[\]:+!;]*$"); }
        /// <summary>
        /// 取字符左函数
        /// </summary>
        /// <param name="Object">要操作的 string  数据</param>
        /// <param name="MaxLength">最大长度</param>
        /// <returns>string</returns>
        public static string Left(object Object, int MaxLength)
        {
            if (IsNull(Object)) return "";
            return Object.ToString().Substring(0, Math.Min(Object.ToString().Length, MaxLength));
        }
        /// <summary>
        /// 取字符中间函数
        /// </summary>
        /// <param name="Object">要操作的 string  数据</param>
        /// <param name="StarIndex">开始的位置索引</param>
        /// <param name="MaxLength">最大长度</param>
        /// <returns>string</returns>
        public static string Mid(string Object, int StarIndex, int MaxLength)
        {
            if (IsNull(Object)) return "";
            if (StarIndex >= Object.Length) return "";
            return Object.Substring(StarIndex, MaxLength);
        }
        /// <summary>
        /// 取字符右函数
        /// </summary>
        /// <param name="Object">要操作的 string  数据</param>
        /// <param name="MaxLength">最大长度</param>
        /// <returns>string</returns>
        public static string Right(object Object, int MaxLength)
        {
            if (IsNull(Object)) return "";
            int i = Object.ToString().Length;
            if (i < MaxLength) { MaxLength = i; i = 0; } else { i = i - MaxLength; }
            return Object.ToString().Substring(i, MaxLength);
        }

        /// <summary>
        /// 判断是否包含Http头的地址
        /// </summary>
        /// <param name="Object">要操作的对象</param>
        /// <returns>bool</returns>
        public static bool IsHttpUrl(string Object)
        {
            if (IsNull(Object)) return false;
            if (Left(Object, 7).ToLower() == "http://") return true;
            return false;
        }
        /// <summary>
        /// 关键字上色处理
        /// </summary>
        /// <param name="Object">要操作的 string  数据</param>
        /// <param name="Keys">关键字</param>
        /// <param name="Style">样式名称</param>
        /// <returns>string</returns>
        public static string AddColors(string Object, string Keys, string Style)
        {
            StringBuilder Builders = new StringBuilder(Object);
            Builders.Replace(Keys, "<span class=\"" + Style + "\">" + Keys + "</span>");
            return Builders.ToString();
        }
        /// <summary>
        /// 数字前导加0
        /// </summary>
        /// <param name="Int">要操作的 int  数据</param>
        /// <param name="MaxLength">最大长度</param>
        /// <returns>string</returns>
        public static string AddZeros(int Int, int MaxLength) { return AddZeros(Int.ToString(), MaxLength); }
        /// <summary>
        /// 数字前导加0
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="MaxLength">默认长度不加0</param>
        /// <returns>字符</returns>
        public static string AddZeros(string Object, int MaxLength)
        {
            int iLength = Object.Length;
            if (iLength < MaxLength)
            {
                for (int i = 1; i <= (MaxLength - iLength); i++) Object = "0" + Object;
            }
            return Object;
        }
        /// <summary>
        /// 重新获取所有ID排序
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>字符</returns>
        public static string ToLoadID(string Object)
        {
            if (IsNull(Object)) return "";
            StringBuilder Builder = new StringBuilder("");
            string[] Atemp = Object.Split(',');
            for (int i = 0; i < Atemp.Length; i++)
            {
                bool IsTrue = false; int Int = IsInt(Atemp[i].ToString(), out IsTrue);
                if (IsTrue) Builder.Append(((i != 0) ? "," : "") + Int.ToString());
            }
            return Builder.ToString();
        }
        #endregion
        #region 字符转换函数
        /// <summary>
        /// 字符 int  转换为 char
        /// </summary>
        /// <param name="Int">字符[int]</param>
        /// <returns>char</returns>
        public static char IntToChar(int Int) { return (char)Int; }
        /// <summary>
        /// 字符 int  转换为字符 string
        /// </summary>
        /// <param name="Int">字符 int</param>
        /// <returns>字符 string</returns>
        public static string IntToString(int Int) { return IntToChar(Int).ToString(); }
        /// <summary>
        /// 字符 string  转换为字符 int
        /// </summary>
        /// <param name="Strings">字符 string</param>
        /// <returns>字符 int</returns>
        public static int StringToInt(string Strings)
        {
            if (IsNull(Strings)) return -100; char[] chars = Strings.ToCharArray(); return (int)chars[0];
        }
        /// <summary>
        /// 字符 string  转换为 char
        /// </summary>
        /// <param name="Strings">字符 string</param>
        /// <returns>char</returns>
        public static char StringToChar(string Strings) { return IntToChar(StringToInt(Strings)); }
        #endregion
        #region 操作 int  数据
        /// <summary>
        /// 对象是否为 int  类型数据
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <param name="IsTrue">返回是否转换成功</param>
        /// <returns>int值</returns>
        private static int IsInt(object Object, out bool IsTrue)
        {
            try { IsTrue = true; return int.Parse(Object.ToString()); }
            catch { IsTrue = false; return 0; }
        }
        /// <summary>
        /// 转换成为 int  数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>int 数据</returns>
        public static int ToInt(object Object) { return ToInt(Object, 0); }
        /// <summary>
        /// 转换成为 int  数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <returns>int 数据</returns>
        public static int ToInt(object Object, int Default) { return ToInt(Object, Default, 0, 999999999); }
        /// <summary>
        /// 转换成为 int  数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <param name="MinInt"> 下界限定的最小值 , 若超过范围 , 则返回 默认值</param>
        /// <returns>int 数据</returns>
        public static int ToInt(object Object, int Default, int MinInt) { return ToInt(Object, Default, MinInt, 999999999); }
        /// <summary>
        /// 转换成为 int  数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <param name="MinInt"> 下界限定的最小值 , 若超过范围 , 则返回 默认值</param>
        /// <param name="MaxInt">上界限定的最大值 , 若超过范围 , 则返回 默认值</param>
        /// <returns>int 数据</returns>
        public static int ToInt(object Object, int Default, int MinInt, int MaxInt)
        {
            bool IsTrue = false;
            int Int = IsInt(Object, out IsTrue);
            if (!IsTrue) return Default;
            if (Int < MinInt || Int > MaxInt) return Default;
            return Int;
        }
        #endregion
        #region 操作 long  数据
        /// <summary>
        /// 对象是否为 long  类型数据
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <param name="IsTrue">返回是否转换成功</param>
        /// <returns>long值</returns>
        private static long IsLong(object Object, out bool IsTrue)
        {
            try { IsTrue = true; return long.Parse(Object.ToString()); }
            catch { IsTrue = false; return 0; }
        }
        /// <summary>
        /// 转换成为 Long 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>Long 数据</returns>
        public static long ToLong(object Object) { return ToLong(Object, 0); }
        /// <summary>
        /// 转换成为 Long 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <returns>Long 数据</returns>
        public static long ToLong(object Object, long Default) { return ToLong(Object, Default, -9223372036854775808, 9223372036854775807); }
        /// <summary>
        /// 转换成为 long 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">转换不成功返回的默认值</param>
        /// <param name="MinLong">下界限定的最小值 , 若超过范围 , 则返回 默认值</param>
        /// <returns>long 数据</returns>
        public static long ToLong(object Object, long Default, long MinLong) { return ToLong(Object, Default, MinLong, 9223372036854775807); }
        /// <summary>
        /// 转换成为 long 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <param name="MinLong">下界限定的最小值 , 若超过范围 , 则返回 默认值</param>
        /// <param name="MaxLong">上界限定的最大值 , 若超过范围 , 则返回 默认值</param>
        /// <returns>long 数据</returns>
        public static long ToLong(object Object, long Default, long MinLong, long MaxLong)
        {
            bool IsTrue = false;
            long Long = IsLong(Object, out IsTrue);
            if (!IsTrue) return Default;
            if (Long < MinLong || Long > MaxLong) return Default;
            return Long;
        }
        #endregion
        #region 操作 float  数据
        /// <summary>
        /// 对象是否为 float  类型数据
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <param name="IsTrue">返回是否转换成功</param>
        /// <returns>float值</returns>
        private static float IsFloat(object Object, out bool IsTrue)
        {
            try { IsTrue = true; return float.Parse(Object.ToString()); }
            catch { IsTrue = false; return 0; }
        }
        /// <summary>
        /// 转换成为 float 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>float 数据</returns>
        public static float ToFloat(object Object) { return ToFloat(Object, 0); }
        /// <summary>
        /// 转换成为 float 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <returns>float 数据</returns>
        public static float ToFloat(object Object, float Default) { return ToFloat(Object, Default, 0, 999999999); }
        /// <summary>
        /// 转换成为 float 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <param name="MinFloat"> 小于等于 转换成功后,下界限定的最小值,若超过范围 则返回 默认值</param>
        /// <returns>float 数据</returns>
        public static float ToFloat(object Object, float Default, float MinFloat) { return ToFloat(Object, Default, MinFloat, 999999999); }
        /// <summary>
        /// 转换成为 float 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <param name="MinFloat"> 下界限定的最小值 , 若超过范围 , 则返回 默认值</param>
        /// <param name="MaxFloat"> 上界限定的最大值 , 若超过范围 , 则返回 默认值</param>
        /// <returns>float 数据</returns>
        public static float ToFloat(object Object, float Default, float MinFloat, float MaxFloat)
        {
            bool IsTrue = false;
            float Float = IsFloat(Object, out IsTrue);
            if (!IsTrue) return Default;
            if (Float < MinFloat || Float > MaxFloat) return Default;
            return Float;
        }
        #endregion
        #region 操作 decimal  数据
        /// <summary>
        /// 对象是否为 decimal  类型数据
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <param name="IsTrue">返回是否转换成功</param>
        /// <returns>decimal值</returns>
        private static decimal IsDecimal(object Object, out bool IsTrue)
        {
            try { IsTrue = true; return decimal.Parse(Object.ToString()); }
            catch { IsTrue = false; return 0; }
        }
        /// <summary>
        /// 转换成为 decimal 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>decimal 数据</returns>
        public static decimal ToDecimal(object Object) { return ToDecimal(Object, 0); }
        /// <summary>
        /// 转换成为 decimal 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <returns>decimal 数据</returns>
        public static decimal ToDecimal(object Object, decimal Default) { return ToDecimal(Object, Default, 0, 999999999); }

        /// <summary>
        /// 转换成为 decimal 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <param name="MinFloat"> 小于等于 转换成功后,下界限定的最小值,若超过范围 则返回 默认值</param>
        /// <returns>decimal 数据</returns>
        public static decimal ToDecimal(object Object, decimal Default, decimal MinFloat) { return ToDecimal(Object, Default, MinFloat, 999999999); }
        /// <summary>
        /// 转换成为 decimal 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <param name="MinDecimal"> 下界限定的最小值 , 若超过范围 , 则返回 默认值</param>
        /// <param name="MaxDecimal"> 上界限定的最大值 , 若超过范围 , 则返回 默认值</param>
        /// <returns>decimal 数据</returns>
        public static decimal ToDecimal(object Object, decimal Default, decimal MinDecimal, decimal MaxDecimal)
        {
            bool IsTrue = false;
            decimal Decimal = IsDecimal(Object, out IsTrue);
            if (!IsTrue) return Default;
            if (Decimal < MinDecimal || Decimal > MaxDecimal) return Default;
            return Decimal;
        }
        #endregion
        #region 操作 DateTime 数据
        /// <summary>
        /// 是否为时间格式
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <param name="IsTrue">返回是否转换成功</param>
        /// <returns>DateTime</returns>
        public static DateTime IsTime(object Object, out bool IsTrue)
        {
            IsTrue = false;
            if (IsNull(Object)) return DateTime.Now;
            try { IsTrue = true; return DateTime.Parse(Object.ToString()); }
            catch { IsTrue = false; return DateTime.Now; }
        }
        /// <summary>
        /// 操作 DateTime  数据
        /// </summary>
        /// <param name="Object">要操作的字符</param>
        /// <returns>DateTime</returns>
        public static DateTime ToTime(string Object) { return ToTime(Object, DateTime.Now); }
        /// <summary>
        /// 字符串转换为时间函数
        /// </summary>
        /// <param name="Object">要操作的字符</param>
        /// <param name="Default">默认时间</param>
        /// <returns>DateTime</returns>
        public static DateTime ToTime(string Object, DateTime Default)
        {
            if (IsNull(Object)) return Default;
            bool IsTrue = false;
            DateTime Time = IsTime(Object, out IsTrue);
            if (!IsTrue) return Default;
            return Time;
        }
        /// <summary>
        /// 获得当前时间
        /// </summary>
        /// <param name="Style">时间样式</param>
        /// <returns>string</returns>
        public static string ToNow(string Style) { return DateTime.Now.ToString(Style); }
        /// <summary>
        /// 转换字符串为格式化时间字符串
        /// </summary>
        /// <param name="Object">要操作的字符</param>
        /// <returns>string</returns>
        public static string ToTimes(string Object) { return ToTimes(Object, "yyyy-MM-dd HH:mm:ss"); }
        /// <summary>
        /// 转换字符串为格式化时间字符串
        /// </summary>
        /// <param name="Object">要操作的字符</param>
        /// <param name="Style">格式化样式</param>
        /// <returns>string</returns>
        public static string ToTimes(string Object, string Style) { return ToTimes(Object, DateTime.Now, Style); }
        /// <summary>
        /// 转换字符串为格式化时间字符串
        /// </summary>
        /// <param name="Object">要操作的字符</param>
        /// <param name="Default">默认时间</param>
        /// <returns>string</returns>
        public static string ToTimes(string Object, DateTime Default) { return ToTimes(Object, Default, "yyyy-MM-dd HH:mm:ss"); }
        /// <summary>
        /// 转换字符串为格式化时间字符串
        /// </summary>
        /// <param name="Object">要操作的字符</param>
        /// <param name="Default">默认时间</param>
        /// <param name="Style">格式化样式</param>
        /// <returns>string</returns>
        public static string ToTimes(string Object, DateTime Default, string Style)
        {
            if (IsNull(Object)) return Default.ToString(Style);
            bool IsTrue = false;
            DateTime Time = IsTime(Object, out IsTrue);
            if (!IsTrue) return Default.ToString(Style);
            return Time.ToString(Style);
        }
        #endregion
        #region 操作 权限 数据
        /// <summary>
        /// 判断权限 返回当前索引的权限的数字
        /// </summary>
        /// <param name="PowerChars">权限数组</param>
        /// <param name="Index">当前权限数组中某个权限的索引</param>
        /// <returns>int</returns>
        public static int ToPowerValue(char[] PowerChars, int Index)
        {
            if (Index < 0) return 1;
            if (PowerChars == null) return 0;
            if (Index >= PowerChars.Length) return 0;
            return (PowerChars[Index] == '0') ? 0 : 1;
        }
        /// <summary>
        /// 获取当前权限数组中某个索引的权限
        /// </summary>
        /// <param name="PowerChars">权限数组</param>
        /// <param name="Index">当前权限数组中某个权限的索引</param>
        /// <returns>bool</returns>
        public static bool IsPower(char[] PowerChars, int Index) { return ((ToPowerValue(PowerChars, Index) == 0) ? false : true); }
        /// <summary>
        /// 获取当前权限字符串中某个索引的权限
        /// </summary>
        /// <param name="Powers">权限字符串 101110000的形式</param>
        /// <param name="Index">当前权限数组中某个权限的索引</param>
        /// <returns>bool</returns>
        public static bool IsPower(string Powers, int Index)
        {
            if (IsNull(Powers)) return false;
            return IsPower(Powers.ToCharArray(), Index);
        }
        /// <summary>
        /// 将权限数组切换到权限字符串
        /// </summary>
        /// <param name="PowerChars">PowerChars 权限数组</param>
        /// <returns>string</returns>
        public static string GetPower(char[] PowerChars) { return new string(PowerChars); }
        /// <summary>
        /// 在权限数组中更改某个索引的权限
        /// 并返回权限数组
        /// </summary>
        /// <param name="PowerChars">PowerChars 权限数组</param>
        /// <param name="Index">当前权限数组中某个权限的索引</param>
        /// <param name="PowerValue">要更改的值</param>
        /// <returns>新的char[]权限数组</returns>
        public static char[] GetPower(char[] PowerChars, int Index, string PowerValue)
        {
            if (PowerChars == null) return null;
            if (Index < 0) return PowerChars;
            if (Index >= PowerChars.Length) return PowerChars;
            PowerChars[Index] = StringToChar(PowerValue);
            return PowerChars;
        }
        /// <summary>
        /// 在权限数组中更改某个索引的权限
        /// 并返回权限字符串
        /// </summary>
        /// <param name="PowerChars">PowerChars 权限数组</param>
        /// <param name="Index">当前权限数组中某个权限的索引</param>
        /// <param name="PowerValue">要更改的值</param>
        /// <returns>权限字符串</returns>
        public static string GetPowers(char[] PowerChars, int Index, string PowerValue)
        {
            char[] Powers = GetPower(PowerChars, Index, PowerValue);
            if (Powers == null) return "";
            return new string(Powers);
        }
        /// <summary>
        /// 在权限字符串中更改某个索引的权限
        /// 并返回权限字符串
        /// </summary>
        /// <param name="Powers">权限字符串</param>
        /// <param name="Index">当前权限数组中某个权限的索引</param>
        /// <param name="PowerValue">要更改的值</param>
        /// <returns>权限字符串</returns>
        public static string GetPowers(string Powers, int Index, string PowerValue)
        {
            if (IsNull(Powers)) return "";
            return GetPowers(Powers.ToCharArray(), Index, PowerValue);
        }
        #endregion
        #region 操作 加减乘除 数据
        /// <summary>
        /// 判断上一符号和当前符号 并返回罗马值
        /// 可以操作普通的加减乘除
        /// </summary>
        /// <param name="Operator1">string</param>
        /// <param name="Operator2">string</param>
        /// <returns></returns>
        private static string Precede(string Operator1, string Operator2)
        {
            switch (Operator1)
            {
                case "+":
                case "-": return ("*/(".IndexOf(Operator2) != -1) ? "<" : ">";
                case "*":
                case "/": return ((Operator2 == "(") ? "<" : ">");
                case "(": return ((Operator2 == ")") ? "=" : "<");
                case ")": return ((Operator2 == "(") ? "?" : ">");
                case "#": return ((Operator2 == "#") ? "=" : "<");
            }
            return "?";
        }
        /// <summary>
        /// 计算当前2个值的结果
        /// </summary>
        /// <param name="Operator">运算符号</param>
        /// <param name="Value1">值1</param>
        /// <param name="Value2">值2</param>
        /// <returns>Double</returns>
        private static Double Result(char Operator, Double Value1, Double Value2)
        {
            if (Operator == '+') return (Value1 + Value2);
            if (Operator == '-') return (Value1 - Value2);
            if (Operator == '*') return (Value1 * Value2);
            if (Operator == '/') return (Value1 / Value2);
            return 0;
        }
        /// <summary>
        /// VC的eval算法
        /// </summary>
        /// <param name="Expression">要计算的表达式</param>
        /// <returns>Object</returns>
        public static Object VcEval(string Expression)
        {
            //运算符的分析数组
            Stack OperatorArr = new Stack();
            //计算值的分析数组
            Stack ValueArr = new Stack();
            //要计算的表达式的长度 索引
            int i = 0;
            //相邻的要计算的2个参数
            Double Value1 = 0;
            Double Value2 = 0;
            //单字符
            string Text = "";
            //运算符
            char Operator;
            //获取运算符、计算值数组
            MatchCollection ExpArray = Regex.Matches(Expression.Replace(" ", "") + "#", @"(((?<=(^|\())-)?\d+(\.\d+)?|\D)");

            OperatorArr.Push('#');
            Text = System.Convert.ToString(ExpArray[i++]);
            while (!(Text == "#" && System.Convert.ToString(OperatorArr.Peek()) == "#"))
            {
                if ("+-*/()#".IndexOf(Text) != -1)
                {
                    switch (Precede(OperatorArr.Peek().ToString(), Text))
                    {
                        case "<":
                            OperatorArr.Push(Text);
                            Text = System.Convert.ToString(ExpArray[i++]);
                            break;
                        case "=":
                            OperatorArr.Pop();
                            Text = System.Convert.ToString(ExpArray[i++]);
                            break;
                        case ">":
                            Operator = System.Convert.ToChar(OperatorArr.Pop());
                            Value2 = System.Convert.ToDouble(ValueArr.Pop());
                            Value1 = System.Convert.ToDouble(ValueArr.Pop());
                            ValueArr.Push(Result(Operator, Value1, Value2));
                            break;
                        default:
                            return "Error";
                    }
                }
                else
                {
                    ValueArr.Push(Text);
                    Text = System.Convert.ToString(ExpArray[i++]);
                }
            }
            return ValueArr.Pop();
        }
        #endregion
        #region 操作  随机数  数据
        /// <summary>
        /// 获取随机数类型枚举对应的数字
        /// </summary>
        /// <param name="Enum">产生随机数的枚举类型</param>
        /// <returns>int</returns>
        private static int RandInt(RandEnum Enum) { return (int)Enum; }
        /// <summary>
        /// 设置随机数类型的枚举
        /// </summary>
        public enum RandEnum : int
        {
            /// <summary>
            /// 数字
            /// </summary>
            Numeric = 0,
            /// <summary>
            /// 字母
            /// </summary>
            Letter = 1,
            /// <summary>
            /// 数字字母混合
            /// </summary>
            Blend = 2,
            /// <summary>
            /// 汉字
            /// </summary>
            Chinese = 3
        }
        /// <summary>
        /// 获取全局唯一GUID 值
        /// </summary>
        /// <returns>string</returns>
        public static string GetGuid() { return GetGuid(100, true); }
        /// <summary>
        /// 获取当前时间的刻度值
        /// </summary>
        /// <param name="MaxLength">最大长度</param>
        /// <returns>int</returns>
        public static int GetGuid(int MaxLength) { return ToInt(Right(DateTime.Now.Ticks.ToString(), 9)); }
        /// <summary>
        /// 获取全局唯一GUID 值
        /// </summary>
        /// <param name="MaxLength">最大长度</param>
        /// <param name="IsRemove">是否去掉特殊字符</param>
        /// <returns>string</returns>
        public static string GetGuid(int MaxLength, bool IsRemove)
        {
            string Guids = Guid.NewGuid().ToString();
            if (IsRemove) { Guids = Guids.Replace("{", ""); Guids = Guids.Replace("}", ""); Guids = Guids.Replace("-", ""); }
            Guids = Left(Guids, MaxLength);
            return Guids;
        }
        /// <summary>
        /// 获取指定类型的随机数
        /// </summary>
        /// <param name="Enum">产生随机数的枚举类型</param>
        /// <param name="MaxLength">最大长度</param>
        /// <returns>string</returns>
        public static string GetGuid(RandEnum Enum, int MaxLength)
        {
            return GetGuid(Enum, "GB2312", MaxLength);
        }
        /// <summary>
        /// 获取指定类型的随机数
        /// </summary>
        /// <param name="Enum">产生随机数的枚举类型</param>
        /// <param name="Encode">产生随机中文的编码</param>
        /// <param name="MaxLength">最大长度</param>
        /// <returns>string</returns>
        public static string GetGuid(RandEnum Enum, string Encode, int MaxLength)
        {
            if (Enum == RandEnum.Chinese) return GetGuid(Encode, MaxLength);
            string Guids = "";
            for (int i = 1; i <= MaxLength; i++)
            {
                int MinInt, MaxInt, Num;
                Random Rand = new Random(GetGuid(9) + i * 1000);
                Num = (Enum == RandEnum.Blend) ? ((Rand.Next(0, 10) <= 4) ? (int)RandEnum.Numeric : (int)RandEnum.Letter) : (int)Enum;
                MinInt = Num == 0 ? 48 : 97;
                MaxInt = Num == 0 ? 57 : 122;
                Guids += IntToString(Rand.Next(MinInt, MaxInt));
                Rand = null;
            }
            return Guids;
        }
        /// <summary>
        /// 获取随机产生的中文
        /// </summary>
        /// <param name="Encode">产生随机中文的编码</param>
        /// <param name="MaxLength">最大长度</param>
        /// <returns>string</returns>
        public static string GetGuid(string Encode, int MaxLength)
        {
            //定义返回的字符串
            string Chinese = "";
            //定义中文编码
            Encoding Ecode = Encoding.GetEncoding(Encode);
            Random Rnd = null;// new Random(GetIntRand(4));
            //int Rint = Rnd.Next(1, 100);
            //定义位码、区码的范围数
            int Wint, Qint;
            for (int i = 1; i <= MaxLength; i++)
            {
                int Rint = 0;
                //获取汉字区位
                Rnd = new Random(GetGuid(9) + i * 1000);
                Rint = Rnd.Next(16, 56);//只获取常用汉字 16-55之间
                Wint = Rint;
                //55区只89个汉字 其他 94个汉字
                Rint = (Wint == 55) ? 90 : 95;
                //定义新种子
                Rnd = new Random(GetGuid(9) + i * 3000);
                Rint = Rnd.Next(1, Rint);
                Qint = Rint;
                //两个字节变量存储产生的随机汉字区位码
                byte Fbyte = System.Convert.ToByte((Wint + 160).ToString("x"), 16);
                byte Lbyte = System.Convert.ToByte((Qint + 160).ToString("x"), 16);
                //将两个字节变量存储在字节数组中
                byte[] Nbytes = new byte[] { Fbyte, Lbyte };
                Chinese += Ecode.GetString(Nbytes);
                Rnd = null;
            }
            Ecode = null;
            return Chinese;
        }
        #endregion
        #region 检测 字符串(包含汉字)    数据
        /// <summary>
        /// 是否汉字
        /// </summary>
        /// <param name="Object">单个字符</param>
        /// <returns>bool</returns>
        private static bool IsChinese(string Object)
        {
            int Int = StringToInt(Object);
            if (Int >= 19968 && Int <= 40869) return true;
            return false;
        }
        /// <summary>
        /// 是否汉字
        /// </summary>
        /// <param name="Object">单个字符 char</param>
        /// <returns>bool</returns>
        private static bool IsChinese(char Object)
        {
            int Int = (int)Object;
            if (Int >= 19968 && Int <= 40869) return true;
            return false;
        }
        /// <summary>
        /// 获取字符串的长度
        /// </summary>
        /// <param name="Object">要检测的对象</param>
        /// <param name="IsChecked">是否判断汉字</param>
        /// <returns>int</returns>
        public static int GetLength(string Object, bool IsChecked)
        {
            if (IsNull(Object)) return 0;
            if (!IsChecked) return Object.Length;
            int iLen = 0;
            foreach (char c in Object)
            {
                iLen++;
                if (IsChinese(c)) iLen++;
            }
            return iLen;
        }
        /// <summary>
        /// 检测长度
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="MinInt">小于等于 最小长度</param>
        /// <param name="MaxInt">大于等于 最大长度</param>
        /// <returns>符合?true:false</returns>
        public static bool CheckLength(string Object, int MinInt, int MaxInt) { return CheckLength(Object, MinInt, MaxInt, false); }
        /// <summary>
        /// 检测长度
        /// </summary>
        /// <param name="Object">要检测的对象</param>
        /// <param name="MinInt">小于等于 最小长度</param>
        /// <param name="MaxInt">大于等于 最大长度</param>
        /// <param name="IsChecked">是否判断汉字</param>
        /// <returns>符合?true:false</returns>
        public static bool CheckLength(string Object, int MinInt, int MaxInt, bool IsChecked)
        {
            int Int = GetLength(Object, IsChecked);
            if (Int < MinInt || Int > MaxInt) return false;
            return true;
        }
        #endregion
        #region 操作 HTML 文本 数据
        /// <summary>
        /// 文本操作 执行HTML解码
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>字符</returns>
        public static string HtmlDecode(string Object)
        {
            if (IsNull(Object)) return "";
            StringBuilder Builder = new StringBuilder(Object);
            Builder.Replace("&amp;", "&");
            Builder.Replace("&lt;", "<");
            Builder.Replace("&gt;", ">");
            Builder.Replace("&quot;", "\"");
            Builder.Replace("&#39;", "'");
            Builder.Replace("&nbsp; &nbsp; ", "\t");
            Builder.Replace("<p></p>", "\r\n\r\n");
            Builder.Replace("<br />", "\r\n");
            Builder.Replace("<br>", "\r\n");
            return Builder.ToString();
        }
        /// <summary>
        /// 文本操作 执行HTML编码
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>字符</returns>
        public static string HtmlEncode(string Object)
        {
            if (IsNull(Object)) return "";
            StringBuilder Builder = new StringBuilder(Object);
            Builder.Replace("&", "&amp;");
            Builder.Replace("<", "&lt;");
            Builder.Replace(">", "&gt;");
            Builder.Replace("\"", "&quot;");
            Builder.Replace("'", "&#39;");
            Builder.Replace("\t", "&nbsp; &nbsp; ");
            Builder.Replace("\r\n\r\n", "<p></p>");
            Builder.Replace("\r\n", "<br />");
            return Builder.ToString();
        }
        /// <summary>
        /// 文本操作 执行TEXT解码
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>字符</returns>
        public static string TextDecode(string Object)
        {
            if (IsNull(Object)) return "";
            StringBuilder Builder = new StringBuilder(Object);
            Builder.Replace("&amp;", "&");
            Builder.Replace("&lt;", "<");
            Builder.Replace("&gt;", ">");
            Builder.Replace("&quot;", "\"");
            Builder.Replace("&#39;", "'");
            return Builder.ToString();
        }
        /// <summary>
        /// 文本操作 执行TEXT编码
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>字符</returns>
        public static string TextEncode(string Object)
        {
            if (IsNull(Object)) return "";
            StringBuilder Builder = new StringBuilder(Object);
            Builder.Replace("&", "&amp;");
            Builder.Replace("<", "&lt;");
            Builder.Replace(">", "&gt;");
            Builder.Replace("\"", "&quot;");
            Builder.Replace("'", "&#39;");
            return Builder.ToString();
        }
        #endregion

        /// <summary>
        /// 获取当前单个字符 所对应的规定的模的值
        /// 字母占26个数据表
        /// 其他字符(包含汉字)占24个数据表
        /// 一共50个用户总表
        /// </summary>
        /// <param name="Object">单个字符</param>
        /// <returns>int</returns>
        public static int GetMod(string Object)
        {
            //测试为1
            //return 1;
            //
            if (Object.Length > 1) Object = Left(Object, 1);
            int Int = StringToInt(Object.ToLower());
            //0~9
            if (Int >= 48 && Int <= 57) return (1 + Int % 48);
            //a~z
            if (Int >= 97 && Int <= 122) return (11 + Int % 97);
            //其他
            return 37 + Int % 14;
        }
        /// <summary>
        /// 获取2个时间之间的时间差
        /// </summary>
        /// <param name="starTime">第一个时间</param>
        /// <param name="endTime">第二个时间</param>
        /// <returns>double</returns>
        public static double DateDiff(DateTime starTime, DateTime endTime)
        {
            try
            {
                TimeSpan starTimeSpan = new TimeSpan(starTime.Ticks);
                TimeSpan endTimeSpan = new TimeSpan(endTime.Ticks);
                TimeSpan totalTimeSpan = starTimeSpan.Subtract(endTimeSpan).Duration();
                return totalTimeSpan.TotalMilliseconds;
            }
            catch { return -1; }
        }
        /// <summary>
        /// 计算一个时间与当前本地日期和时间的时间间隔,返回的是时间间隔的日期差的绝对值.
        /// </summary>
        /// <param name="starTime">一个日期和时间</param>
        /// <returns>double</returns>
        private static double DateDiff(DateTime starTime) { return DateDiff(starTime, DateTime.Now); }

        #region 字符串转其它数据类型
        /// <summary>
        /// 将字符串转整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this string str)
        {
            int ret = 0;
            if (!string.IsNullOrEmpty(str))
            {
                int.TryParse(str, out ret);
            }
            return ret;
        }

        /// <summary>
        /// 将字符串转化成日期类型
        /// </summary>
        /// <param name="str">日期字符串</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str)
        {
            DateTime ret;
            DateTime.TryParse(str, out ret);
            if (ret.Year == 1)
            {
                return DateTime.Parse("1900-1-1");
            }
            return ret;
        }
        #endregion
    };
}
