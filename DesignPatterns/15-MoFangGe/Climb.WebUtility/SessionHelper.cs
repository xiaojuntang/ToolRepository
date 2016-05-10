
/**************************************************
* 文 件 名：SessionHelper.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014/9/5 15:33:37
* 文件说明：
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/
using System;

namespace Climb.WebUtility
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SessionHelper
    {
        /// <summary>
        /// 添加Session，调动有效期为20分钟
        /// </summary>
        /// <param name="key">Session对象名称</param>
        /// <param name="tObj"></param>
        public static void AddObject<T>(string key, T tObj)
        {
            WebTools.Seession.Add(key,tObj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="tObj"></param>
        /// <typeparam name="T"></typeparam>
        public static void SetObject<T>(string key, T tObj)
        {
            T t = GetObject<T>(key);
            if (t == null)
            {
                AddObject(key, tObj);
            }
        }

        /// <summary>
        /// 读取某个Session对象值
        /// </summary>
        /// <param name="key">Session对象名称</param>
        /// <returns>Session对象值</returns>
        public static T GetObject<T>(string key)
        {
            T t =(T) WebTools.Seession[key];
            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="tFunc"></param>
        /// <returns></returns>
        public static T GetObject<T>(string key,Func<T> tFunc)
        {
            T t = GetObject<T>(key);
            if (t != null) return t;
            t = tFunc();
            AddObject(key,tFunc);
            return t;
        }

        /// <summary>
        /// 删除某个Session对象
        /// </summary>
        /// <param name="key">Session对象名称</param>
        public static void RemoveObject(string key)
        {
            WebTools.Seession.Remove(key);
        }
    }

}
