using System.Text.RegularExpressions;

namespace Climb.Utility.SystemExt
{
    /// <summary>
    /// 正则扩展
    /// </summary>
    public static class RegexExt
    {
        #region 常用的正则
        /// <summary>
        /// 数组的正则
        /// </summary>
        public static readonly Regex PatternNumber = new Regex(@"^[1-9]\d*\.?[0]*$");

        /// <summary>
        /// 邮箱正则
        /// </summary>
        public static readonly Regex PatterngEmail = new Regex(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 

        /// <summary>
        /// 匹配中文的正则 
        /// </summary>
        public static readonly Regex PatternChina = new Regex("[\u4e00-\u9fa5]");


        /// <summary>
        /// 获取href里的内容
        /// </summary>
        public static readonly string PatternHref = @"(h|H)(r|R)(e|E)(f|F) *= *('|"")?((\w|\\|\/|\.|:|-|_)+)('|""| *|>)?";
        /// <summary>
        /// 获取src里边的内容
        /// </summary>
        public static readonly string PatternSrc = @"(s|S)(r|R)(c|C) *= *('|"")?((\w|\\|\/|\.|:|-|_)+)('|""| *|>)?";

        /// <summary>
        /// 获取所有img标签
        /// </summary>
        public static readonly string PatternImg =
            "<img[^>]+src=\\s*(?:'(?<src>[^']+)'|\"(?<src>[^\"]+)\"|(?<src>[^>\\s]+))\\s*[^>]*>";

        /// <summary>
        /// 获取所有页面title 标签
        /// </summary>
        public static readonly string PatternTitle = @"<Title[^>]*>(?<Title>[\s\S]{10,})</Title>";

        /// <summary>
        /// 获取所有页面的
        /// </summary>
        public static readonly string PatternBody = @"<Body[^>]*>(?<Body>[\s\S]{10,})</body>";

        /// <summary>
        /// 纯文本 只有数字 字母 英文
        /// </summary>
        public const string PatternText = @"[^\w\d\u4e00-\u9fa5]";

        #endregion

    }
}
