using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Security.Cryptography;
using System.Web.Caching;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
using Microsoft.Win32;
using System.Management;
using System.Net;


namespace Climb.Utility
{
    /// <summary>
    /// WinForm和WebForm的工具类(WinForm和WebForm下面已使用)
    /// </summary>
    public class Utility
    {
        #region 取指定字符
        /// <summary>
        /// 取指定字符
        /// </summary>
        /// <param name="str">目标字符</param>
        /// <param name="i">字符索引</param>
        /// <returns></returns>
        public static string StringAt(string str, int i)
        {

            return str.Substring(i, 1);
        }
        #endregion

        #region 合并ArrayList
        /// <summary>
        /// 合并ArrayList
        /// </summary>
        /// <param name="list">ArrayList的List</param>
        /// <returns></returns>
        public static ArrayList ArrayListMerger(List<ArrayList> list)
        {
            ArrayList tmpList = new ArrayList();
            for (int i = 0; i < list.Count; i++)
            {
                ArrayList arr = list[i];
                for (int j = 0; j < arr.Count; j++)
                {
                    tmpList.Add(arr[j]);
                }
            }
            return tmpList;
        }
        #endregion

        #region 取开始到结束的值
        /// <summary>
        /// 取开始到结束的值
        /// </summary>
        /// <param name="content">目标字符</param>
        /// <param name="start">开始字符</param>
        /// <param name="end">结束字符</param>
        /// <returns></returns>
        public static string GetStartToEndString(string content, string start, string end)
        {
            content = content.Replace("\r", "");
            content = content.Replace("\n", "");
            Match m = Regex.Match(content, start + "(.+)" + end, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return m.Groups[1].Value;
        }
        #endregion

        #region 字符转List(针对每个字符)
        /// <summary>
        /// 字符转List(针对每个字符)
        /// </summary>
        /// <param name="str">目标字符</param>
        /// <returns></returns>
        public static List<string> StrSplit(string str)
        {
            List<string> arr = new List<string>();
            for (int i = 0; i < str.Length; i++)
            {
                arr.Add(str.Substring(i, 1));
            }
            return arr;
        }
        #endregion

        #region 字符转List
        /// <summary>
        /// 字符转List
        /// </summary>
        /// <param name="str">目标字符</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static List<string> StrSplit(string str, string separator)
        {
            List<string> arr = new List<string>();
            str = str.Replace(separator, "|");
            string[] strs = str.Split('|');
            foreach (string s in strs)
            {
                arr.Add(s);
            }
            return arr;
        }

        /// <summary>
        /// 字符转List
        /// </summary>
        /// <param name="str">目标字符</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static List<int> StrSplitToInt(string str, string separator)
        {
            List<int> arr = new List<int>();
            str = str.Replace(separator, "|");
            string[] strs = str.Split('|');
            foreach (string s in strs)
            {
                arr.Add(Convert.ToInt32(s));
            }
            return arr;
        }

        #endregion

        #region ArrayList分页
        /// <summary>
        /// ArrayList分页
        /// </summary>
        /// <param name="dataList">目标数据</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <returns></returns>
        public static ArrayList PageList(ArrayList dataList, int pageIndex, int pageSize)
        {
            ArrayList arr = new ArrayList();
            int startIndex = (pageIndex - 1) * pageSize;
            for (int i = 0; i < pageSize; i++)
            {
                try
                {
                    arr.Add(dataList[startIndex + i]);
                }
                catch
                {
                    break;
                }
            }
            return arr;
        }
        #endregion

        #region 获取总页数
        /// <summary>
        /// 获取总页数
        /// </summary>
        /// <param name="count">总记录数</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <returns></returns>
        public static int PageCount(int count, int pageSize)
        {
            if (count % pageSize == 0)
            {
                return (count / pageSize);
            }
            else
            {
                return (count / pageSize) + 1;
            }
        }
        #endregion

        #region IEnumerable转换为ArrayList
        /// <summary>
        /// IEnumerable转换为ArrayList
        /// </summary>
        /// <param name="ie">目标IEnumerable(这个类无泛型)</param>
        /// <returns></returns>
        public static ArrayList IEnumerableToList(IEnumerable ie)
        {
            IEnumerator ie_ = ie.GetEnumerator();
            ArrayList arr = new ArrayList();
            if (ie_ != null)
            {
                while (ie_.MoveNext())
                {
                    arr.Add(ie_.Current);
                }
            }
            return arr;
        }
        #endregion

        #region IEnumerator转换为ArrayList
        /// <summary>
        /// IEnumerator转换为ArrayList
        /// </summary>
        /// <param name="ie">目标IEnumerator</param>
        /// <returns></returns>
        public static ArrayList IEnumeratorToList(IEnumerator ie)
        {
            ArrayList arr = new ArrayList();
            if (ie != null)
            {
                while (ie.MoveNext())
                {
                    arr.Add(ie.Current);
                }
            }
            return arr;
        }
        #endregion

        #region IEnumerator转换为ArrayList
        /// <summary>
        /// IEnumerator转换为ArrayList
        /// </summary>
        /// <param name="ie">目标IEnumerator(泛型)</param>
        /// <returns></returns>
        public static ArrayList IEnumeratorToList(IEnumerator<object> ie)
        {
            ArrayList arr = new ArrayList();
            if (ie != null)
            {
                while (ie.MoveNext())
                {
                    arr.Add(ie.Current);
                }
            }
            return arr;
        }
        #endregion

        #region 获取随机ArrayList(超过长度的话)
        /// <summary>
        /// 获取随机ArrayList(超过长度的话)
        /// </summary>
        /// <param name="aList">目标ArrayList</param>
        /// <param name="_length">返回长度</param>
        /// <returns></returns>
        public static ArrayList GetRandArrayList(ArrayList aList, int _length)
        {
            if (aList.Count > _length)
            {
                //生成无重复索引列表
                List<int> tempRanIndex = GetRandIndex(aList.Count);

                ArrayList tmpList = new ArrayList();
                for (int i = 0; i < _length; i++)
                {
                    tmpList.Add(aList[tempRanIndex[i]]);
                }
                return tmpList;
            }
            else
            {
                return aList;
            }
        }
        #endregion

        #region 生成无重复索引
        /// <summary>
        /// 生成无重复索引
        /// </summary>
        /// <param name="_count">长度</param>
        /// <returns></returns>
        public static List<int> GetRandIndex(int _count)
        {
            List<int> tempList = new List<int>();
            Random rd = new Random();
            ArrayList intTempArr = new ArrayList();

            int[] intArr = new int[_count + 1];
            //填充数组intTempArr
            for (int i = 0; i < _count + 1; i++)
            {
                intTempArr.Add(i);
            }

            //生成随机数
            for (int j = 0; j < intArr.Length; j++)
            {
                int temp = rd.Next(intTempArr.Count - 1);
                int tempValue = (int)intTempArr[temp];
                intArr[j] = tempValue;
                intTempArr.RemoveAt(temp);
                tempList.Add(tempValue);
            }
            tempList.RemoveAt(tempList.Count - 1);
            return tempList;
        }
        #endregion

        #region 获取随机字符

        /// <summary>
        /// 枚举(获取随机字符Util.GetRandString())
        /// </summary>
        public enum RandString : int
        {
            /// <summary>
            /// 全部小写字母
            /// </summary>
            Lower = 1,
            /// <summary>
            /// 全部大写字母
            /// </summary>
            Upper = 2,
            /// <summary>
            /// 全部数字
            /// </summary>
            Number = 3,
            /// <summary>
            /// 大写字母和小写字母
            /// </summary>
            LowerAndUpper = 4,
            /// <summary>
            /// 大写字母和数字
            /// </summary>
            UpperAndNumber = 5,
            /// <summary>
            /// 小写字母和数字
            /// </summary>
            LowerAndNumber = 6,
            /// <summary>
            /// 大写字母,小写字母和数字
            /// </summary>
            All = 7
        }
        /// <summary>
        /// 获取随机字符
        /// </summary>
        /// <param name="type">返回类型(1-7)</param>
        /// <param name="_length">字符长度</param>
        /// <returns></returns>
        public static string GetRandString(RandString type, int _length)
        {
            int _type = (int)type;
            string str1 = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            string str2 = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string str3 = "0,1,2,3,4,5,6,7,8,9";
            string content = null;
            if (_type == 1)
            {
                content = str1;
            }
            else if (_type == 2)
            {
                content = str2;
            }
            else if (_type == 3)
            {
                content = str3;
            }
            else if (_type == 4)
            {
                content = str1 + "," + str2;
            }
            else if (_type == 5)
            {
                content = str2 + "," + str3;
            }
            else if (_type == 6)
            {
                content = str1 + "," + str3;
            }
            else if (_type == 7)
            {
                content = str1 + "," + str2 + "," + str3;
            }

            string[] strs = content.Split(',');
            string output = "";
            Random r = new Random();
            for (int i = 0; i < _length; i++)
            {
                output += strs[r.Next(0, strs.Length)];
            }
            return output;
        }
        #endregion

        #region 获取随机字符
        /// <summary>
        /// 根据当前日期获取随机文件名
        /// </summary>
        public static String RandNameToTime
        {
            get
            {
                System.DateTime dt = System.DateTime.Now;
                //String sName = dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString();
                //sName += dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString();
                //sName += dt.Millisecond.ToString();
                String sName = dt.ToString("yyyyMMddHHmmssffff");
                return sName;
            }
        }
        #endregion

    

        #region 发送E-Mail
        /// <summary>
        /// 发送E-Mail
        /// </summary>
        /// <param name="email">Email地址(不用输入用户)</param>
        /// <param name="pwd">密码</param>
        /// <param name="smtp">smtp服务器(smtp.163.com)</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="files">附件列表(泛型)</param>
        /// <param name="toMail">目标Email</param>
        /// <returns></returns>
        public static bool ToMail(string email, string pwd, string smtp, string title, string content, List<string> files, string toMail)
        {
            return ToMail(email, pwd, smtp, false, title, content, files, toMail);
        }

        /// <summary>
        /// 发送E-Mail
        /// </summary>
        /// <param name="email">Email地址(不用输入用户)</param>
        /// <param name="pwd">密码</param>
        /// <param name="smtp">smtp服务器(smtp.163.com)</param>
        /// <param name="useSSL">使用SSL</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="files">附件列表(泛型)</param>
        /// <param name="toMail">目标Email</param>
        /// <returns></returns>
        public static bool ToMail(string email, string pwd, string smtp, bool useSSL, string title, string content, List<string> files, string toMail)
        {
            List<string> toMails = new List<string>();
            toMails.Add(toMail);
            return SmtpMail(email, pwd, smtp, useSSL, title, content, files, toMails);
        }
        #endregion

        #region 发送E-Mail(支持群发)
        /// <summary>
        /// 发送E-Mail(支持群发)
        /// </summary>
        /// <param name="email">Email地址(不用输入用户)</param>
        /// <param name="pwd">密码</param>
        /// <param name="smtp">smtp服务器(smtp.163.com)</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="files">附件列表(泛型)</param>
        /// <param name="toMails">目标Email(泛型)</param>
        /// <returns></returns>
        public static bool ToMail(string email, string pwd, string smtp, string title, string content, List<string> files, List<string> toMails)
        {
            return SmtpMail(email, pwd, smtp, title, content, files, toMails);
        }

        /// <summary>
        /// 发送E-Mail(支持群发)
        /// </summary>
        /// <param name="email">Email地址(不用输入用户)</param>
        /// <param name="pwd">密码</param>
        /// <param name="smtp">smtp服务器(smtp.163.com)</param>
        /// <param name="useSSL">使用SSL</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="files">附件列表(泛型)</param>
        /// <param name="toMails">目标Email(泛型)</param>
        /// <returns></returns>
        public static bool ToMail(string email, string pwd, string smtp, bool useSSL, string title, string content, List<string> files, List<string> toMails)
        {
            return SmtpMail(email, pwd, smtp, useSSL, title, content, files, toMails);
        }
        //#endregion

        #region 发送E-Mail 私有方法(支持群发)
        /// <summary>
        /// 发送E-Mail 私有方法(支持群发)
        /// </summary>
        /// <param name="email">Email地址(不用输入用户)</param>
        /// <param name="pwd">密码</param>
        /// <param name="smtp">smtp服务器(smtp.163.com:25)</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="files">附件列表(泛型)</param>
        /// <param name="toMails">目标Email(泛型)</param>
        /// <returns></returns>
        protected static bool SmtpMail(string email, string pwd, string smtp, string title, string content, List<string> files, List<string> toMails)
        {
            return SmtpMail(email, pwd, smtp, false, title, content, files, toMails);
        }

        /// <summary>
        /// 发送E-Mail 私有方法(支持群发)
        /// </summary>
        /// <param name="email">Email地址(不用输入用户)</param>
        /// <param name="pwd">密码</param>
        /// <param name="smtp">smtp服务器(smtp.163.com:25)</param>
        /// <param name="useSSL">使用SSL</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="files">附件列表(泛型)</param>
        /// <param name="toMails">目标Email(泛型)</param>
        /// <returns></returns>
        protected static bool SmtpMail(string email, string pwd, string smtp, bool useSSL, string title, string content, List<string> files, List<string> toMails)
        {
            bool isOK = false;
            try
            {
                DoSmtpMail(email, pwd, smtp, useSSL, title, content, files, toMails);
                isOK = true;
            }
            catch
            {
                isOK = false;
            }
            return isOK;
        }
        #endregion

        #region 发送Email（这个方法没有返回bool，能获取错误信息）
        /// <summary>
        /// 发送E-Mail
        /// </summary>
        /// <param name="email">Email地址(不用输入用户)</param>
        /// <param name="pwd">密码</param>
        /// <param name="smtp">smtp服务器(smtp.163.com)</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="files">附件列表(泛型)</param>
        /// <param name="toMail">目标Email</param>
        public static void DoToMail(string email, string pwd, string smtp, string title, string content, List<string> files, string toMail)
        {
            DoToMail(email, pwd, smtp, false, title, content, files, toMail);
        }

        /// <summary>
        /// 发送E-Mail
        /// </summary>
        /// <param name="email">Email地址(不用输入用户)</param>
        /// <param name="pwd">密码</param>
        /// <param name="smtp">smtp服务器(smtp.163.com)</param>
        /// <param name="useSSL">使用SSL</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="files">附件列表(泛型)</param>
        /// <param name="toMail">目标Email</param>
        public static void DoToMail(string email, string pwd, string smtp, bool useSSL, string title, string content, List<string> files, string toMail)
        {
            List<string> toMails = new List<string>();
            toMails.Add(toMail);
            DoSmtpMail(email, pwd, smtp, useSSL, title, content, files, toMails);
        }

        /// <summary>
        /// 发送E-Mail(支持群发)
        /// </summary>
        /// <param name="email">Email地址(不用输入用户)</param>
        /// <param name="pwd">密码</param>
        /// <param name="smtp">smtp服务器(smtp.163.com)</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="files">附件列表(泛型)</param>
        /// <param name="toMails">目标Email(泛型)</param>
        public static void DoToMail(string email, string pwd, string smtp, string title, string content, List<string> files, List<string> toMails)
        {
            DoSmtpMail(email, pwd, smtp, title, content, files, toMails);
        }

        /// <summary>
        /// 发送E-Mail(支持群发)
        /// </summary>
        /// <param name="email">Email地址(不用输入用户)</param>
        /// <param name="pwd">密码</param>
        /// <param name="smtp">smtp服务器(smtp.163.com)</param>
        /// <param name="useSSL">使用SSL</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="files">附件列表(泛型)</param>
        /// <param name="toMails">目标Email(泛型)</param>
        public static void DoToMail(string email, string pwd, string smtp, bool useSSL, string title, string content, List<string> files, List<string> toMails)
        {
            DoSmtpMail(email, pwd, smtp, useSSL, title, content, files, toMails);
        }

        /// <summary>
        /// 发送Email（这个方法没有返回bool，能获取错误信息）
        /// </summary>
        /// <param name="email">Email地址(不用输入用户)</param>
        /// <param name="pwd">密码</param>
        /// <param name="smtp">smtp服务器(smtp.163.com:25)</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="files">附件列表(泛型)</param>
        /// <param name="toMails">目标Email(泛型)</param>
        protected static void DoSmtpMail(string email, string pwd, string smtp, string title, string content, List<string> files, List<string> toMails)
        {
            DoSmtpMail(email, pwd, smtp, false, title, content, files, toMails);
        }

        /// <summary>
        /// 发送Email（这个方法没有返回bool，能获取错误信息）
        /// </summary>
        /// <param name="email">Email地址(不用输入用户)</param>
        /// <param name="pwd">密码</param>
        /// <param name="smtp">smtp服务器(smtp.163.com:25)</param>
        /// <param name="useSSL">使用SSL</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="files">附件列表(泛型)</param>
        /// <param name="toMails">目标Email(泛型)</param>
        protected static void DoSmtpMail(string email, string pwd, string smtp, bool useSSL, string title, string content, List<string> files, List<string> toMails)
        {
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.IsBodyHtml = true;
            mail.Subject = title;
            mail.Body = content;
            mail.BodyEncoding = System.Text.Encoding.Default;

            if (files != null)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    mail.Attachments.Add(new System.Net.Mail.Attachment(files[i]));
                }
            }

            for (int i = 0; i < toMails.Count; i++)
            {
                mail.To.Add(toMails[i]);
            }

            mail.From = new System.Net.Mail.MailAddress(email);
            System.Net.Mail.SmtpClient smtpobj = new System.Net.Mail.SmtpClient();
            smtpobj.EnableSsl = useSSL;
            if (StrSplit(smtp, ":").Count == 1)
            {
                smtpobj.Host = smtp;
            }
            else
            {
                smtpobj.Host = StrSplit(smtp, ":")[0];
                smtpobj.Port = Convert.ToInt32(StrSplit(smtp, ":")[1]);
            }
            smtpobj.Credentials = new System.Net.NetworkCredential(email, pwd);
            smtpobj.Send(mail);
        }
        #endregion

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
            DateTime minTime = new DateTime();
            DateTime maxTime = new DateTime();

            System.TimeSpan ts = new System.TimeSpan(time1.Ticks - time2.Ticks);

            // 获取两个时间相隔的秒数
            double dTotalSecontds = ts.TotalSeconds;
            int iTotalSecontds = 0;

            if (dTotalSecontds > System.Int32.MaxValue)
            {
                iTotalSecontds = System.Int32.MaxValue;
            }
            else if (dTotalSecontds < System.Int32.MinValue)
            {
                iTotalSecontds = System.Int32.MinValue;
            }
            else
            {
                iTotalSecontds = (int)dTotalSecontds;
            }


            if (iTotalSecontds > 0)
            {
                minTime = time2;
                maxTime = time1;
            }
            else if (iTotalSecontds < 0)
            {
                minTime = time1;
                maxTime = time2;
            }
            else
            {
                return time1;
            }

            int maxValue = iTotalSecontds;

            if (iTotalSecontds <= System.Int32.MinValue)
                maxValue = System.Int32.MinValue + 1;

            int i = random.Next(System.Math.Abs(maxValue));

            return minTime.AddSeconds(i);
        }
        #endregion

        #region 得到字符串长度，一个汉字长度为2
        /// <summary>
        /// 得到字符串长度，一个汉字长度为2
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static int StrLength(string inputString)
        {
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;
            }
            return tempLen;
        }
        #endregion

        #region 截取字符串
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="inputString">目标字符串</param>
        /// <param name="len">长度</param>
        /// <returns>返回截取后的字符</returns>
        public static string GetTruncate(string inputString, int len)
        {
            return GetTruncate(inputString, len, "...");
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="inputString">目标字符串</param>
        /// <param name="len">长度</param>
        /// <param name="Omission">省略号</param>
        /// <returns>返回截取后的字符</returns>
        public static string GetTruncate(string inputString, int len, string Omission)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }
            //如果截过则加上半个省略号 
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
            if (mybyte.Length > len)
                tempString += Omission;

            return tempString;
        }
        #endregion

        #region 验证一组字符的长度
        /// <summary>
        /// 验证一组字符的长度
        /// </summary>
        /// <param name="str_list">字符列表</param>
        /// <param name="len">限制长度</param>
        /// <returns></returns>
        public static bool StrLenValid(IList<string> str_list, int len)
        {
            bool b = true;
            for (int i = 0; i < str_list.Count; i++)
            {
                if (str_list[i].Length > len)
                {
                    b = false;
                    break;
                }
            }
            return b;
        }
        #endregion

        #region 执行CMD
        /// <summary>
        /// 执行CMD
        /// </summary>
        /// <param name="cmd">命令参数</param>
        /// <returns></returns>
        public static StreamReader ExecuteCMD(string cmd)
        {
            try
            {
                using (Process process = new Process())
                {
                    ProcessStartInfo psi = new ProcessStartInfo("cmd.exe");
                    psi.UseShellExecute = false;
                    psi.RedirectStandardOutput = true;
                    process.StartInfo = psi;
                    psi.Arguments = "/c " + cmd;
                    process.Start();
                    StreamReader sr = process.StandardOutput;
                    return sr;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 执行程序(马上执行)
        /// <summary>
        /// 执行程序(马上执行)
        /// </summary>
        /// <param name="ApplicationPath">程序路径</param>
        /// <param name="Arguments">命令参数</param>
        /// <returns></returns>
        public static Process ExecuteApplication(string ApplicationPath, string Arguments)
        {
            Process process = Execute(ApplicationPath, Arguments);
            try
            {
                process.Start();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return process;
        }
        #endregion

        #region 执行程序
        /// <summary>
        /// 执行程序
        /// </summary>
        /// <param name="ApplicationPath">程序路径</param>
        /// <param name="Arguments">命令参数</param>
        /// <param name="IsStart">是否马上执行</param>
        /// <returns></returns>
        public static Process ExecuteApplication(string ApplicationPath, string Arguments, bool IsStart)
        {
            Process process = Execute(ApplicationPath, Arguments);
            if (IsStart)
            {
                try
                {
                    process.Start();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return process;
        }
        #endregion

        #region 执行程序 (私有方法)
        /// <summary>
        /// 执行程序 (私有方法)
        /// </summary>
        /// <param name="ApplicationPath">本地程序路径</param>
        /// <param name="Arguments">命令参数</param>
        /// <returns></returns>
        protected static Process Execute(string ApplicationPath, string Arguments)
        {
            Process process = new Process();
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ApplicationPath;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            process.StartInfo = psi;
            psi.Arguments = Arguments;
            return process;
        }
        #endregion

        #region 合并DataTable(每个表的结构必须一样)
        /// <summary>
        /// 合并DataTable(每个表的结构必须一样)
        /// </summary>
        /// <param name="dataTables">DataTable集合</param>
        /// <param name="sort">排序(id desc)</param>
        /// <returns></returns>
        public static DataTable DataTableMerger(DataTableCollection dataTables, string sort)
        {
            DataTable dt = new DataTable();
            foreach (DataColumn dc in dataTables[0].Columns)
            {
                dt.Columns.Add(dc.ColumnName);
            }

            foreach (DataTable d in dataTables)
            {
                foreach (DataRow dr in d.Rows)
                {
                    DataRow item = dt.NewRow();
                    item.ItemArray = dr.ItemArray;
                    dt.Rows.Add(item);
                }
            }

            DataView dv = dt.DefaultView;
            dv.Sort = sort;
            return dv.ToTable();
        }
        #endregion

        #region 合并DataTable(每个表的结构必须一样)
        /// <summary>
        /// 合并DataTable(每个表的结构必须一样)
        /// </summary>
        /// <param name="dataTables">DataTable集合</param>
        /// <param name="sort">排序(id desc)</param>
        /// <param name="top">显示前几个</param>
        /// <returns></returns>
        public static DataTable DataTableMerger(DataTableCollection dataTables, string sort, int top)
        {
            DataTable tmp = DataTableMerger(dataTables, sort);
            DataTable dt = new DataTable();
            foreach (DataColumn dc in tmp.Columns)
            {
                dt.Columns.Add(dc.ColumnName);
            }

            for (int i = 0; i < top; i++)
            {
                try
                {
                    DataRow item = dt.NewRow();
                    item.ItemArray = tmp.Rows[i].ItemArray;
                    dt.Rows.Add(item);
                }
                catch
                {
                    break;
                }
            }
            return dt;
        }
        #endregion

        #region DataTable分页
        /// <summary>
        /// DataTable分页
        /// </summary>
        /// <param name="srcDataTable">源DataTable</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示记录</param>
        /// <returns></returns>
        public static DataTable DataTablePage(DataTable srcDataTable, int pageIndex, int pageSize)
        {
            DataTable dt = new DataTable();
            foreach (DataColumn dc in srcDataTable.Columns)
            {
                dt.Columns.Add(dc.ColumnName);
            }

            int startIndex = (pageIndex - 1) * pageSize;
            for (int i = 0; i < pageSize; i++)
            {
                try
                {
                    DataRow item = dt.NewRow();
                    item.ItemArray = srcDataTable.Rows[startIndex + i].ItemArray;
                    dt.Rows.Add(item);
                }
                catch
                {
                    break;
                }
            }
            return dt;
        }
        #endregion

        #region DataTable条件查询
        /// <summary>
        /// DataTable条件查询
        /// </summary>
        /// <param name="srcDataTable">源DataTable</param>
        /// <param name="where">条件查询(id=1)</param>
        /// <returns></returns>
        public static DataTable DataTableWhere(DataTable srcDataTable, string where)
        {
            DataTable dt = new DataTable();
            foreach (DataColumn dc in srcDataTable.Columns)
            {
                dt.Columns.Add(dc.ColumnName);
            }
            DataRow[] drs = srcDataTable.Select(where);
            foreach (DataRow dr in drs)
            {
                DataRow item = dt.NewRow();
                item.ItemArray = dr.ItemArray;
                dt.Rows.Add(item);
            }
            return dt;
        }
        #endregion

        #region 转繁体
        ///// <summary>
        ///// 转繁体
        ///// </summary>
        ///// <param name="src"></param>
        ///// <returns></returns>
        //public static string ToBIG(string src)
        //{
        //    return Microsoft.VisualBasic.Strings.StrConv(src, Microsoft.VisualBasic.VbStrConv.TraditionalChinese, 0);
        //}
        //#endregion

        //#region 转简体
        ///// <summary>
        ///// 转简体
        ///// </summary>
        ///// <param name="src"></param>
        ///// <returns></returns>
        //public static string ToGB(string src)
        //{
        //    return Microsoft.VisualBasic.Strings.StrConv(src, Microsoft.VisualBasic.VbStrConv.SimplifiedChinese, 0);
        //}
        #endregion

        #region Base64序列化对象(String方式)
        /// <summary>
        /// Base64序列化对象(String方式)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectSerializeForString(Object obj)
        {
            IFormatter ifor = new BinaryFormatter();
            string str = "";
            using (MemoryStream sm = new MemoryStream())
            {
                ifor.Serialize(sm, obj);                        //序列化
                sm.Seek(0, SeekOrigin.Begin);

                byte[] bytes = new byte[sm.Length];
                bytes = sm.ToArray();
                str = Convert.ToBase64String(bytes);
                str = HttpContext.Current.Server.UrlEncode(str);//编码
            }
            return str;
        }
        #endregion

        #region Base64反序列化对象(String方式)
        /// <summary>
        /// Base64反序列化对象(String方式)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static object ObjectDeserializeForString(string str)
        {
            str = HttpContext.Current.Server.UrlDecode(str);    //解码
            byte[] bt = Convert.FromBase64String(str);
            Stream smNew = new MemoryStream(bt);
            IFormatter fmNew = new BinaryFormatter();
            object obj = fmNew.Deserialize(smNew);              //反序列化
            return obj;
        }
        #endregion

        #region 序列化对象(byte[]方式)
        /// <summary>
        /// 序列化对象(byte[]方式)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] ObjectSerializeForBytes(object data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream rems = new MemoryStream())
            {
                try
                {
                    formatter.Serialize(rems, data);
                    return rems.GetBuffer();
                }
                catch
                {
                    return new byte[0];
                }
            }
        }
        #endregion

        #region 反序列化对象(byte[]方式)
        /// <summary>
        /// 反序列化对象(byte[]方式)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object ObjectDeserializeForBytes(byte[] data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream rems = new MemoryStream(data))
            {
                data = null;
                try
                {
                    return formatter.Deserialize(rems);
                }
                catch
                {
                    return null;
                }
            }
        }
        #endregion

        #region 对象实例化
        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="type">类型的Type</param>
        /// <returns></returns>
        public static object ObjectInstance(Type type)
        {
            return type.Assembly.CreateInstance(type.FullName);
        }
        #endregion

        #region 日期随机函数
        /// <summary>
        /// 日期随机函数
        /// </summary>
        /// <param name="ra">随机数</param>
        /// <returns></returns>
        public static string DateRndName(Random ra)
        {
            DateTime d = DateTime.Now;
            string s = null, y, m, dd, h, mm, ss;
            y = d.Year.ToString();
            m = d.Month.ToString();
            if (m.Length < 2) m = "0" + m;
            dd = d.Day.ToString();
            if (dd.Length < 2) dd = "0" + dd;
            h = d.Hour.ToString();
            if (h.Length < 2) h = "0" + h;
            mm = d.Minute.ToString();
            if (mm.Length < 2) mm = "0" + mm;
            ss = d.Second.ToString();
            if (ss.Length < 2) ss = "0" + ss;
            s += y + m + dd + h + mm + ss;
            s += ra.Next(100, 999).ToString();
            return s;
        }
        #endregion

        #region 获取2位小数的Double
        /// <summary>
        /// 获取2位小数的Double
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double Get2BitDouble(double d)
        {
            string s = d.ToString();
            int index = s.IndexOf(".");
            if (index >= 0)
            {
                try
                {
                    s = s.Substring(0, index + 3);
                }
                catch
                {
                    s = s.Substring(0, index + 2);
                }
                return Convert.ToDouble(s);
            }
            else
            {
                return Convert.ToDouble(s);
            }
        }
        #endregion

        #region 输入新的宽度，然后按比例返回图片大小
        /// <summary>
        /// 输入新的宽度，然后按比例计算高度
        /// </summary>
        /// <param name="srcSize">源大小</param>
        /// <param name="newWidth">新的宽度</param>
        /// <returns></returns>
        public static int GetProportionHeight(Size srcSize, int newWidth)
        {
            return (srcSize.Height * newWidth) / srcSize.Width;
        }
        /// <summary>
        /// 输入新的高度，然后按比例计算宽度
        /// </summary>
        /// <param name="srcSize">源大小</param>
        /// <param name="newHeight">新的高度</param>
        /// <returns></returns>
        public static int GetProportionWidth(Size srcSize, int newHeight)
        {
            return (srcSize.Width * newHeight) / srcSize.Height;
        }
        #endregion

        #region 获取应用程序的根目录
        /// <summary>
        /// 获取应用程序的根目录
        /// </summary>
        /// <returns></returns>
        public static string GetBasePath()
        {
            System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();
            //WebDev.WebServer      visual studio web server
            //xxx.vhost             Winform
            //w3wp                  IIS7
            //aspnet_wp             IIS6
            string processName = p.ProcessName.ToLower();
            if (processName == "aspnet_wp" || processName == "w3wp" || processName == "webdev.webserver")
            {
                if (System.Web.HttpContext.Current != null)
                    return System.Web.HttpContext.Current.Server.MapPath("~/").TrimEnd(new char[] { '\\' });
                else //当控件在定时器的触发程序中使用时就为空
                {
                    return System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[] { '\\' });
                }
            }
            else
            {
                return System.Windows.Forms.Application.StartupPath;
            }
        }
        #endregion

        #region 获取对应日期的农历 (格式:二零零八年,鼠年,戊子,腊月,初十 )
        /// <summary>
        /// 获取对应日期的农历 (格式:二零零八年,鼠年,戊子,腊月,初十 )
        /// </summary>
        /// <param name="dtDay">公历日期</param>
        /// <returns></returns>
        public static string GetLunarCalendar(DateTime dtDay)
        {
            //天干
            string[] TianGan = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };

            //地支
            string[] DiZhi = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };

            //十二生肖
            string[] ShengXiao = { "鼠", "牛", "虎", "兔", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };

            //农历日期
            string[] DayName = {"*","初一","初二","初三","初四","初五",
                                                 "初六","初七","初八","初九","初十",
                                                 "十一","十二","十三","十四","十五",
                                                 "十六","十七","十八","十九","二十",
                                                 "廿一","廿二","廿三","廿四","廿五",
                                                 "廿六","廿七","廿八","廿九","三十"};

            //农历月份
            string[] MonthName = { "*", "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "腊" };

            //公历月计数天
            int[] MonthAdd = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
            //农历数据
            int[] LunarData = {2635,333387,1701,1748,267701,694,2391,133423,1175,396438
                                             ,3402,3749,331177,1453,694,201326,2350,465197,3221,3402
                                             ,400202,2901,1386,267611,605,2349,137515,2709,464533,1738
                                             ,2901,330421,1242,2651,199255,1323,529706,3733,1706,398762
                                             ,2741,1206,267438,2647,1318,204070,3477,461653,1386,2413
                                             ,330077,1197,2637,268877,3365,531109,2900,2922,398042,2395
                                             ,1179,267415,2635,661067,1701,1748,398772,2742,2391,330031
                                             ,1175,1611,200010,3749,527717,1452,2742,332397,2350,3222
                                             ,268949,3402,3493,133973,1386,464219,605,2349,334123,2709
                                             ,2890,267946,2773,592565,1210,2651,395863,1323,2707,265877};
            string sYear = dtDay.Year.ToString();
            string sMonth = dtDay.Month.ToString();
            string sDay = dtDay.Day.ToString();
            int year;
            int month;
            int day;
            try
            {
                year = int.Parse(sYear);
                month = int.Parse(sMonth);
                day = int.Parse(sDay);
            }
            catch
            {
                year = DateTime.Now.Year;
                month = DateTime.Now.Month;
                day = DateTime.Now.Day;
            }

            int nTheDate;
            int nIsEnd;
            int k, m, n, nBit, i;
            string calendar = string.Empty;
            //计算到初始时间1921年2月8日的天数：1921-2-8(正月初一)
            nTheDate = (year - 1921) * 365 + (year - 1921) / 4 + day + MonthAdd[month - 1] - 38;
            if ((year % 4 == 0) && (month > 2))
                nTheDate += 1;
            //计算天干，地支，月，日
            nIsEnd = 0;
            m = 0;
            k = 0;
            n = 0;
            while (nIsEnd != 1)
            {
                if (LunarData[m] < 4095)
                    k = 11;
                else
                    k = 12;
                n = k;
                while (n >= 0)
                {
                    //获取LunarData[m]的第n个二进制位的值
                    nBit = LunarData[m];
                    for (i = 1; i < n + 1; i++)
                        nBit = nBit / 2;
                    nBit = nBit % 2;
                    if (nTheDate <= (29 + nBit))
                    {
                        nIsEnd = 1;
                        break;
                    }
                    nTheDate = nTheDate - 29 - nBit;
                    n = n - 1;
                }
                if (nIsEnd == 1)
                    break;
                m = m + 1;
            }
            year = 1921 + m;
            month = k - n + 1;
            day = nTheDate;
            //return year + "-" + month + "-" + day;

            if (k == 12)
            {
                if (month == LunarData[m] / 65536 + 1)
                    month = 1 - month;
                else if (month > LunarData[m] / 65536 + 1)
                    month = month - 1;
            }
            //年
            calendar = IntToChinese(year) + "年,";
            //生肖
            calendar += ShengXiao[(year - 4) % 60 % 12].ToString() + "年,";
            // //天干
            calendar += TianGan[(year - 4) % 60 % 10].ToString();
            // //地支
            calendar += DiZhi[(year - 4) % 60 % 12].ToString() + ",";

            //农历月
            if (month < 1)
                calendar += "闰" + MonthName[-1 * month].ToString() + "月,";
            else
                calendar += MonthName[month].ToString() + "月,";

            //农历日
            //calendar += DayName[day].ToString() + "日";
            calendar += DayName[day].ToString();

            return calendar;

        }
        #endregion

        #region 多个数字转为中文形式
        /// <summary>
        /// 多个数字转为中文形式
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string IntToChinese(int number)
        {
            string output = null;
            for (int i = 0; i < number.ToString().Length; i++)
            {
                short sh = Convert.ToInt16(StringAt(number.ToString(), i));
                output += ShortToChinese(sh);
            }
            return output;
        }
        #endregion

        #region 一个数字转为中文形式
        /// <summary>
        /// 一个数字转为中文形式
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ShortToChinese(short number)
        {
            if (number.ToString().Length >= 2)
            {
                throw new Exception("IntToChinese的参数不能大于10");
            }

            switch (number)
            {
                case 0:
                    return "零";
                case 1:
                    return "一";
                case 2:
                    return "二";
                case 3:
                    return "三";
                case 4:
                    return "四";
                case 5:
                    return "五";
                case 6:
                    return "六";
                case 7:
                    return "七";
                case 8:
                    return "八";
                case 9:
                    return "九";
                default:
                    throw new Exception("IntToChinese的参数不正确");
            }
        }
        #endregion

        #region 获取当前执行的.Net运行在哪个操作系统
        /// <summary>
        /// 获取当前执行的.Net运行在哪个操作系统 
        /// </summary>
        /// <returns></returns>
        public static string GetDotNetOSName()
        {
            string s = null;
            try
            {
                s = System.Environment.OSVersion.Version.Major.ToString() + "." + System.Environment.OSVersion.Version.Minor.ToString();
                if (s == "4.10")
                {
                    return "Windows 98";
                }
                else if (s == "4.9")
                {
                    return "Windows Me";
                }
                else if (s == "5.0")
                {
                    return "Windows 2000";
                }
                else if (s == "5.1")
                {
                    return "Windows XP";
                }
                else if (s == "5.2")
                {
                    return "Windows 2003";
                }
                else if (s == "6.0")
                {
                    string tmp = "";
                    if (System.Environment.OSVersion.Version.Build == 6000)
                    {
                        tmp = "Windows Vista";
                    }
                    else if (System.Environment.OSVersion.Version.Build == 6001)
                    {
                        tmp = "Windows 2008";
                    }
                    else
                    {
                        return "未知版本:" + s + "." + System.Environment.OSVersion.Version.Build.ToString();
                    }

                    //System.IntPtr.Size
                    //返回4为32位,返回8为64位.
                    if (IntPtr.Size == 4)
                    {
                        tmp += " X86";
                    }
                    else if (IntPtr.Size == 8)
                    {
                        tmp += " X64";
                    }
                    else
                    {
                        //未知
                    }
                    return tmp;
                }
                else if (s == "6.1")
                {
                    string tmp = "";

                    tmp = "Windows 7";

                    //System.IntPtr.Size
                    //返回4为32位,返回8为64位.
                    if (IntPtr.Size == 4)
                    {
                        tmp += " X86";
                    }
                    else if (IntPtr.Size == 8)
                    {
                        tmp += " X64";
                    }
                    else
                    {
                        //未知
                    }
                    return tmp;
                }

                else if (s == "6.2")
                {
                    string tmp = "";

                    tmp = "Windows 8";

                    //System.IntPtr.Size
                    //返回4为32位,返回8为64位.
                    if (IntPtr.Size == 4)
                    {
                        tmp += " X86";
                    }
                    else if (IntPtr.Size == 8)
                    {
                        tmp += " X64";
                    }
                    else
                    {
                        //未知
                    }
                    return tmp;
                }
                else
                {
                    return "未知版本:" + s;
                }
            }
            catch (Exception ex)
            {
                return "未知版本:" + ex.Message;
            }
        }
        #endregion

        #region 获取系统目录路径
        /// <summary>
        /// 获取系统目录路径
        /// </summary>
        /// <returns></returns>
        public static string GetOSRootDir()
        {
            return Environment.ExpandEnvironmentVariables("%SystemRoot%");
        }
        #endregion

        #region 获取系统ProgramFiles路径
        /// <summary>
        /// 获取系统ProgramFiles路径
        /// </summary>
        /// <returns></returns>
        public static string GetOSProgramFilesDir()
        {
            return Environment.ExpandEnvironmentVariables("%ProgramFiles%");
        }
        #endregion

        #region 获取.Net语言版本
        /// <summary>
        /// 获取.Net语言版本
        /// </summary>
        /// <returns></returns>
        public static string GetDotNetLanguage()
        {
            return System.Globalization.CultureInfo.InstalledUICulture.EnglishName;
        }
        #endregion

        #region 返回时间提醒语
        /// <summary>
        /// 返回时间提醒语
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetDateMessge(DateTime dt)
        {
            if (dt.Hour == 1)
            {
                return "凌晨一点已过，别忘了休息喔！";
            }
            else if (dt.Hour == 2)
            {
                return "该休息了，身体可是革命的本钱啊！";
            }
            else if (dt.Hour == 3)
            {
                return "夜深人静，只有你敲击鼠标的声音...";
            }
            else if (dt.Hour == 4)
            {
                return "四点过了，你明天不上班？？？";
            }
            else if (dt.Hour == 5)
            {
                return "该去晨运了！！！";
            }
            else if (dt.Hour == 6)
            {
                return "你知道吗，此时是国内网络速度最快的时候！";
            }
            else if (dt.Hour == 7)
            {
                return "新的一天又开始了，祝你过得快乐！";
            }
            else if (dt.Hour == 8 || dt.Hour == 9 || dt.Hour == 10)
            {
                return "上午好！今天你看上去好精神哦！";
            }
            else if (dt.Hour == 11)
            {
                return "十一点过了，快下班了吧？";
            }
            else if (dt.Hour == 12)
            {
                return "十二点过了，还不下班？";
            }
            else if (dt.Hour == 13 || dt.Hour == 14)
            {
                return "你不睡午觉？";
            }
            else if (dt.Hour == 15 || dt.Hour == 16 || dt.Hour == 17)
            {
                return "下午好！";
            }
            else if (dt.Hour == 18 || dt.Hour == 19)
            {
                return "吃晚饭了吧？";
            }
            else if (dt.Hour == 20 || dt.Hour == 21 || dt.Hour == 22)
            {
                return "今晚又在这玩电脑了，没节目？";
            }
            else if (dt.Hour == 23)
            {
                return "真是越玩越精神，不打算睡了？";
            }
            else if (dt.Hour == 0)
            {
                return "凌晨了，还不睡？";
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 检测系统是否装有Python
        /// <summary>
        /// 检测系统是否装有Python
        /// </summary>
        /// <returns></returns>
        public static bool ExistsPython()
        {
            string _root = "SOFTWARE\\Python\\PythonCore";
            RegistryKey _rk = Registry.LocalMachine.OpenSubKey(_root);
            try
            {
                string tmp = _rk.GetSubKeyNames()[_rk.GetSubKeyNames().Length - 1];
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 获取系统的Python版本号
        /// <summary>
        /// 获取系统的Python版本号
        /// </summary>
        /// <returns></returns>
        public static string GetPythonVersion()
        {
            if (!ExistsPython())
            {
                throw new Exception("系统没有安装Python或没有检测到系统注册表Python的相关信息");
            }

            string _root = "SOFTWARE\\Python\\PythonCore";
            RegistryKey _rk = Registry.LocalMachine.OpenSubKey(_root);
            try
            {
                string tmp = _rk.GetSubKeyNames()[_rk.GetSubKeyNames().Length - 1];
                return tmp;
            }
            catch
            {
                return "未知";
            }
        }
        #endregion

        #region 获取系统的Perl版本号
        /// <summary>
        /// 获取系统的Perl版本号
        /// </summary>
        /// <returns></returns>
        public static string GetPerlVersion()
        {
            if (!ExistsPerl())
            {
                throw new Exception("系统没有安装Perl或没有检测到系统注册表Perl的相关信息");
            }

            string _root = "SOFTWARE\\Perl";
            RegistryKey _rk = Registry.LocalMachine.OpenSubKey(_root);
            try
            {
                string perlPath = _rk.GetValue("BinDir").ToString();
                FileVersionInfo perlInfo = FileVersionInfo.GetVersionInfo(perlPath);
                return perlInfo.FileVersion;
            }
            catch
            {
                return "未知";
            }
        }
        #endregion

        #region 检测系统是否装有Perl
        /// <summary>
        /// 检测系统是否装有Perl
        /// </summary>
        /// <returns></returns>
        public static bool ExistsPerl()
        {
            string _root = "SOFTWARE\\Perl";
            RegistryKey _rk = Registry.LocalMachine.OpenSubKey(_root);
            try
            {
                string perlPath = _rk.GetValue("BinDir").ToString();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 获取Java版本号,先使用CLASSPATH获取JDK的路径，然后读取java.exe的版本号
        /// <summary>
        /// 获取Java版本号,先使用CLASSPATH获取JDK的路径，然后读取java.exe的版本号
        /// </summary>
        /// <returns></returns>
        public static string GetJavaVersion()
        {
            if (!ExistsJava())
            {
                throw new Exception("系统没有安装Java或没有检测到系统环境变量Java的相关信息");
            }

            string version = null;

            foreach (DictionaryEntry enValue in Environment.GetEnvironmentVariables())
            {
                string key = enValue.Key.ToString();
                string value = enValue.Value.ToString();

                if (key.ToUpper() == "CLASSPATH")
                {
                    if (value.IndexOf(".") == 0)
                    {
                        value = value.Remove(0, 1);
                    }
                    if (value.IndexOf(";") == 0)
                    {
                        value = value.Remove(0, 1);
                    }


                    DirectoryInfo libPath = new DirectoryInfo(value);
                    FileInfo javaPath = new FileInfo(libPath.Parent.FullName + "\\bin\\java.exe");
                    if (javaPath.Exists)
                    {
                        FileVersionInfo javaInfo = FileVersionInfo.GetVersionInfo(javaPath.FullName);
                        version = javaInfo.FileVersion;
                        break;
                    }

                }
            }

            if (version == null)
            {
                version = "未知";
            }

            return version;
        }
        #endregion

        #region 检测系统是否安装有JAVA
        /// <summary>
        /// 检测系统是否安装有JAVA
        /// </summary>
        /// <returns></returns>
        public static bool ExistsJava()
        {
            foreach (DictionaryEntry enValue in Environment.GetEnvironmentVariables())
            {
                string key = enValue.Key.ToString();
                string value = enValue.Value.ToString();

                if (key.ToUpper() == "CLASSPATH")
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 获取PHP版本,使用php.ini读取文件，然后读取php.exe来获取版本号
        /// <summary>
        /// 获取PHP版本,使用php.ini读取文件，然后读取php.exe来获取版本号
        /// </summary>
        /// <returns></returns>
        public static string GetPHPVersion()
        {
            if (!File.Exists(@"C:\WINDOWS\php.ini"))
            {
                throw new Exception("系统没有安装PHP或没有检测到“C:\\WINDOWS\\php.ini”这个文件");
            }

            string version = null;
            try
            {
                using (StreamReader sr = new StreamReader(@"C:\WINDOWS\php.ini"))
                {
                    string phpIniLine;
                    do
                    {
                        phpIniLine = sr.ReadLine();
                        if (!String.IsNullOrEmpty(phpIniLine))
                        {
                            if (phpIniLine.Contains("extension_dir"))
                            {
                                phpIniLine = phpIniLine.Replace("extension_dir", "");
                                phpIniLine = phpIniLine.Replace("=", "");
                                phpIniLine = phpIniLine.Replace("\"", "");
                                phpIniLine = phpIniLine.Replace("/", "\\");

                                if (Directory.Exists(phpIniLine))
                                {
                                    DirectoryInfo extpath = new DirectoryInfo(phpIniLine);
                                    DirectoryInfo extParent = extpath.Parent;
                                    if (File.Exists(extParent.FullName + "\\" + "php.exe"))
                                    {
                                        FileVersionInfo phpInfo = FileVersionInfo.GetVersionInfo(extParent.FullName + "\\" + "php.exe");
                                        version = phpInfo.FileVersion;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    while (phpIniLine != null);
                }
            }
            catch
            {
                version = "未知";
            }
            return version;
        }
        #endregion

        #region 获取int数组里面的最小数值
        /// <summary>
        /// 获取int数组里面的最小数值
        /// </summary>
        /// <param name="intList"></param>
        /// <returns></returns>
        public static int IntMin(IList<int> intList)
        {
            if (intList.Count == 1)
            {
                return intList[0];
            }

            int tmp = 0;
            for (int i = 1; i < intList.Count; i++)
            {
                if (i == 1)
                {
                    if (intList[i] < intList[i - 1])
                    {
                        tmp = intList[i];
                    }
                    else if (intList[i] > intList[i - 1])
                    {
                        tmp = intList[i - 1];
                    }
                    else
                    {
                        tmp = intList[i];
                    }
                }
                else
                {
                    if (intList[i] < tmp)
                    {
                        tmp = intList[i];
                    }
                    else if (intList[i] > tmp)
                    {
                        //tmp = tmp;
                    }
                    else
                    {
                        tmp = intList[i];
                    }
                }
            }
            return tmp;
        }
        #endregion

        #region 获取int数组里面的最大数值
        /// <summary>
        /// 获取int数组里面的最大数值
        /// </summary>
        /// <param name="intList"></param>
        /// <returns></returns>
        public static int IntMax(IList<int> intList)
        {
            if (intList.Count == 1)
            {
                return intList[0];
            }

            int tmp = 0;
            for (int i = 1; i < intList.Count; i++)
            {
                if (i == 1)
                {
                    if (intList[i] > intList[i - 1])
                    {
                        tmp = intList[i];
                    }
                    else if (intList[i] < intList[i - 1])
                    {
                        tmp = intList[i - 1];
                    }
                    else
                    {
                        tmp = intList[i];
                    }
                }
                else
                {
                    if (intList[i] > tmp)
                    {
                        tmp = intList[i];
                    }
                    else if (intList[i] < tmp)
                    {
                        //tmp = tmp;
                    }
                    else
                    {
                        tmp = intList[i];
                    }
                }
            }
            return tmp;
        }
        #endregion

        #region 计算百分比
        /// <summary>
        /// 计算百分比
        /// </summary>
        /// <param name="Current">当前进度</param>
        /// <param name="Max">最大值</param>
        /// <returns></returns>
        public static int GetPercentage(long Current, long Max)
        {
            if (Current > Max)
            {
                throw new Exception("当前值(Current)不能大于最大值(Max)");
            }

            double d = (double)Current / (double)Max;
            d = d * 100;
            d = Math.Round(d);
            return (int)d;
        }
        #endregion

        #region 合并二进制数据
        /// <summary>
        /// 合并二进制数据
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static byte[] MergerBytes(byte[] current, byte[] target)
        {
            List<byte[]> BytesList = new List<byte[]>();
            BytesList.Add(current);
            BytesList.Add(target);
            return MergerBytes(BytesList);
        }
        /// <summary>
        /// 合并二进制数据
        /// </summary>
        /// <param name="BytesList"></param>
        /// <returns></returns>
        public static byte[] MergerBytes(IList<byte[]> BytesList)
        {
            List<byte> fanhui = new List<byte>();
            foreach (byte[] bytes in BytesList)
            {
                if (bytes != null)
                {
                    foreach (byte b in bytes)
                    {
                        fanhui.Add(b);
                    }
                }
            }
            return fanhui.ToArray();
        }
        #endregion

        #region 获取硬盘序列号
        /// <summary>
        /// 获取硬盘序列号
        /// </summary>
        /// <returns></returns>
        public static string[] GetDriversID()
        {
            ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moc1 = cimobject1.GetInstances();
            List<string> vals = new List<string>();
            foreach (ManagementObject mo in moc1)
            {
                string HDid = (string)mo.Properties["Model"].Value;
                vals.Add(HDid);
            }
            return vals.ToArray();
        }
        #endregion

        #region 输入角度，然后计算弧度
        /// <summary>
        /// 输入角度，然后计算弧度，公式是：弧度=角度*Math.PI/180
        /// </summary>
        /// <param name="Angle">输入角度</param>
        /// <returns></returns>
        public static double GetRadian(double Angle)
        {
            return Angle * Math.PI / 180;
        }
        #endregion

        #region 输入弧度，然后计算角度
        /// <summary>
        /// 输入弧度，然后计算角度，公式是：角度=弧度*180/Math.PI
        /// </summary>
        /// <param name="Radian">输入弧度</param>
        /// <returns></returns>
        public static double GetAngle(double Radian)
        {
            return Radian * 180 / Math.PI;
        }
        #endregion

        #region 不区分大小写的替换
        /// <summary>
        /// 不区分大小写的替换
        /// </summary>
        /// <param name="original">原字符串</param>
        /// <param name="pattern">需替换字符</param>
        /// <param name="replacement">被替换内容</param>
        /// <returns></returns>
        public static string ReplaceEx(string original, string pattern, string replacement)
        {
            int count, position0, position1;
            count = position0 = position1 = 0;
            string upperString = original.ToUpper();
            string upperPattern = pattern.ToUpper();
            int inc = (original.Length / pattern.Length) * (replacement.Length - pattern.Length);
            char[] chars = new char[original.Length + Math.Max(0, inc)];
            while ((position1 = upperString.IndexOf(upperPattern, position0)) != -1)
            {
                for (int i = position0; i < position1; ++i) chars[count++] = original[i];
                for (int i = 0; i < replacement.Length; ++i) chars[count++] = replacement[i];
                position0 = position1 + pattern.Length;
            }
            if (position0 == 0) return original;
            for (int i = position0; i < original.Length; ++i) chars[count++] = original[i];
            return new string(chars, 0, count);
        }
        #endregion

        #region html中的特殊字符相关

        /// <summary>
        /// 替换html中的特殊字符
        /// </summary>
        /// <param name="theString">需要进行替换的文本。</param>
        /// <returns>替换完的文本。</returns>
        public static string HtmlEncode(string theString)
        {
            if (theString == null || theString == "")
            {
                return theString;
            }
            theString = theString.Replace(">", "&gt;");
            theString = theString.Replace("<", "&lt;");
            theString = theString.Replace("  ", " &nbsp;");
            theString = theString.Replace("\"", "&quot;");
            theString = theString.Replace("'", "&#39;");
            theString = theString.Replace("\r\n", "<br/> ");
            return theString;
        }

        /// <summary>
        /// 恢复html中的特殊字符
        /// </summary>
        /// <param name="theString">需要恢复的文本。</param>
        /// <returns>恢复好的文本。</returns>
        public static string HtmlDecode(string theString)
        {
            if (theString == null || theString == "")
            {
                return theString;
            }
            theString = theString.Replace("&gt;", ">");
            theString = theString.Replace("&lt;", "<");
            theString = theString.Replace(" &nbsp;", "  ");
            theString = theString.Replace("&quot;", "\"");
            theString = theString.Replace("&#39;", "'");
            theString = theString.Replace("<br/> ", "\r\n");
            return theString;
        }
        #endregion

        #region IP地址互转整数
        /// <summary>
        /// 将IP地址转为整数形式
        /// </summary>
        /// <returns>整数</returns>
        public static long GetIP2Long(IPAddress ip)
        {
            int x = 3;
            long o = 0;
            foreach (byte f in ip.GetAddressBytes())
            {
                o += (long)f << 8 * x--;
            }
            return o;
        }
        /// <summary>
        /// 将整数转为IP地址
        /// </summary>
        /// <returns>IP地址</returns>
        public static IPAddress GetLong2IP(long l)
        {
            byte[] b = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                b[3 - i] = (byte)(l >> 8 * i & 255);
            }
            return new IPAddress(b);
        }
        #endregion

        #region 字符转Unicode
        /// <summary>
        /// 字符转Unicode
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns></returns>
        public static string StrToUnicode(string str)
        {
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    //将中文字符转为10进制整数，然后转为16进制unicode字符
                    outStr += "\\u" + ((int)str[i]).ToString("x");
                }
            }
            return outStr;
        }
        #endregion

        #region Unicode转字符
        /// <summary>
        /// Unicode转字符
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns></returns>
        public static string UnicodeToStr(string str)
        {
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                string[] strlist = str.Replace("\\", "").Split('u');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        //将unicode字符转为10进制整数，然后转为char中文字符
                        outStr += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);
                    }
                }
                catch (FormatException ex)
                {
                    outStr = ex.Message;
                }
            }
            return outStr;
        }
        #endregion

        #region 获取MAC地址
        /// <summary>
        /// 获取MAC地址 
        /// </summary>
        /// <returns></returns>
        public static string GetNetCardMacAddress()
        {
            ManagementClass mc;
            ManagementObjectCollection moc;
            mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            moc = mc.GetInstances();
            string str = "";
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                    str = mo["MacAddress"].ToString();

            }
            return str;
        }
        #endregion

        #region 获取硬盘序列号
        /// <summary>
        /// 获取硬盘(C盘)序列号
        /// </summary>
        /// <returns></returns>
        public static string GetDiskVolumeSerialNumber()
        {
            return GetDiskVolumeSerialNumber("c");
        }

        /// <summary>
        /// 获取硬盘序列号
        /// </summary>
        /// <returns></returns>
        public static string GetDiskVolumeSerialNumber(string driver)
        {
            ManagementObject disk;
            disk = new ManagementObject("win32_logicaldisk.deviceid=\"" + driver + ":\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }
        #endregion

        #region 时间戳相关
        /// <summary>
        /// 时间戳转DateTime
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeFrom(long timeStamp)
        {
            return DateTime.Parse("1970-01-01 00:00:00").AddSeconds(timeStamp);
        }
        /// <summary>
        /// DateTime转时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long UnixTimeTo(DateTime dateTime)
        {
            return (dateTime.Ticks - DateTime.Parse("1970-01-01 00:00:00").Ticks) / 10000000;
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

        #region 数组转成字符串
        /// <summary>
        /// 数组转成字符串(用，隔开)
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ArrayToString(Array list)
        {
            return ArrayToString(list, ",");
        }
        /// <summary>
        /// 数组转成字符串
        /// </summary>
        /// <param name="list"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ArrayToString(Array list, string s)
        {
            string str = "";
            for (int i = 0; i < list.Length; i++)
            {
                str += list.GetValue(i).ToString();
                if (i + 1 < list.Length)
                {
                    str += s;
                }
            }

            return str;
        }
        #endregion
    }
}
