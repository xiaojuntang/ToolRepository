
/**************************************************
* 文 件 名：ConfigHelper.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014/9/22 20:19:19
* 文件说明：
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/
using System;
using System.Configuration;

namespace Climb.Utility
{
    /// <summary>
    /// 配置文件 帮助类
    /// </summary>
    public sealed class ConfigHelper
    {
        /// <summary>
        /// 获取appsetings
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static string GetAppsetingString(string keys)
        {

            return ConfigurationManager.AppSettings[keys];
        }
        /// <summary>
        /// 获取connection节点
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static string GetConnectString(string keys)
        {
            ConnectionStringSettingsCollection connectStrings = ConfigurationManager.ConnectionStrings;
            string constr;
            if (connectStrings != null && connectStrings.Count > 0)
            {
                if (connectStrings[keys] != null)
                {
                    constr = connectStrings[keys].ConnectionString;
                }
                else
                {
                    throw new Exception("请检查配置节点" + keys);
                }
            }
            else
            {
                throw new Exception("请检查配置节点" + keys);
            }
            return constr;
        }
    }
}
