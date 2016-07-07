/***************************************************************************** 
*        filename :Consts 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.42000 
*        新建项输入的名称:   Consts 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Common.Net.Core 
*        文件名:             Consts 
*        创建系统时间:       2016/2/3 10:01:54 
*        创建年份:           2016 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Core
{
    /// <summary>
    /// 常量类
    /// </summary>
    public class FlagConst
    {
        /// <summary>
        /// 非法字符提示符
        /// </summary>
        public static readonly string PARA_ERROR = "PARA_ERROR";

        #region 特殊字符
        public static readonly string CHARACTER_KEY0 = "~";
        public static readonly string CHARACTER_KEY1 = "!";
        public static readonly string CHARACTER_KEY2 = "@";
        public static readonly string CHARACTER_KEY3 = "#";
        public static readonly string CHARACTER_KEY4 = "$";
        public static readonly string CHARACTER_KEY5 = "%";
        public static readonly string CHARACTER_KEY6 = "^";
        public static readonly string CHARACTER_KEY7 = "&";
        public static readonly string CHARACTER_KEY8 = "*";
        public static readonly string CHARACTER_STRING_DOT = ",";
        public static readonly string CHARACTER_TREELINE = "|";
        public static readonly char CHARACTER_CHAR_DOT = ',';
        public static readonly char CHARACTER_CHAR_UNDERLINE = '_';
        #endregion
    }
}
