
/**************************************************
* 文 件 名：HtmlHelper.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014/9/5 15:17:19
* 文件说明：
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Climb.Utility;
using Climb.Utility.SystemExt;

namespace Climb.WebUtility
{
    /// <summary>
    /// html 帮助类
    /// </summary>
    public sealed class HtmlHelper
    {
        /// <summary>过滤html标签，并把标签替换成相应空格
        /// 过滤html标签，并把标签替换成相应空格
        /// </summary>
        /// <param name="html">string 要过滤的字符串</param>
        /// <returns>string 过滤后结果</returns>
        public  static string DeleteHtmlTag(string html)
        {
            var reg = new Regex(@"<(.|\n)+?>");
            MatchCollection matches = reg.Matches(html);
            return  reg.Replace(html, "");
        }

        /// <summary>
        /// 获取纯文本
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetHtmlText(string html)
        {
            html = DeleteHtmlTag(html);
            return Regex.Replace(html, RegexExt.PatternText, "");
        }



     }
}
