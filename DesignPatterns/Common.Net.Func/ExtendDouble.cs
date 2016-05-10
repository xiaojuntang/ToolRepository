/***************************************************************************** 
*        filename :ExtendDouble 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.42000 
*        新建项输入的名称:   ExtendDouble 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Common.Net.Func 
*        文件名:             ExtendDouble 
*        创建系统时间:       2016/2/3 9:59:03 
*        创建年份:           2016 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Func
{
    public static class ExtendDouble
    {
        /// <summary>
        /// 格式化浮点数 整数部分不够 ct 位前边补0 ，小数不够 dec位 后边补 0
        /// </summary>
        /// <param name="num">浮点数参数本身</param>
        /// <param name="ct">整数部分前边需要补零个数</param>
        /// <param name="dec">小数部分后便需要补零个数</param>
        /// <returns></returns>
        public static string format(this double num, int ct, int dec)
        {
            string formatStr = string.Empty;
            for (int i = 1; i <= ct; i++)
                formatStr += "0";
            formatStr += ".";

            for (int i = 1; i <= dec; i++)
                formatStr += "0";
            return string.Format("{0:" + formatStr + "}", num);
        }
    }
}
