/***************************************************************************** 
*        filename :ExtendHttpRequest 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.42000 
*        新建项输入的名称:   ExtendHttpRequest 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Common.Net.Func 
*        文件名:             ExtendHttpRequest 
*        创建系统时间:       2016/2/3 10:00:38 
*        创建年份:           2016 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Func
{
    public static class ExtendHttpRequest
    {
        #region 数据成员
        /// <summary>PC（包括iE、google、sarfri）
        /// </summary>
        static readonly string[] PC = { "Windows NT", "Macintosh" };
        /// <summary>安卓
        /// </summary>
        static readonly string[] Android = { "Android" };
        /// <summary>IOS（"iPhone", "iPod", "iPad" ）
        /// </summary>
        static readonly string[] IOS = { "iPhone", "iPod", "iPad" };
        /// <summary>windows phone
        /// </summary>
        static readonly string[] WP = { "Windows Phone" };
        /// <summary>微信
        /// </summary>
        static readonly string[] WX = { "micromessenger" };
        #endregion


        #region 方法成员

        /// <summary>获取客户端类型----pc还是移动端
        /// </summary>
        /// <param name="rqst"></param>
        /// <returns>1:pc/未知   ， 2:mobile</returns>   
        public static int GetClientType(this System.Web.HttpRequest _rqst)
        {
            var userAgent = _rqst.UserAgent;
            if (userAgent != null)
            {

                if (PC.Any(source => userAgent.IndexOf(source, StringComparison.Ordinal) > -1))//判断是否是pc端
                {
                    return 1;
                }
                else if (new string[][] { Android, IOS, WP, WX }.Any(source => source.Any(ua => userAgent.IndexOf(ua, StringComparison.Ordinal) > -1)))//判断是否是移动端。
                {
                    return 0;
                }

            }
            return 1;
        }
        #endregion
    }
}
