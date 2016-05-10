
/**************************************************
* 文 件 名：ArraryExt.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014/9/1 23:59:30
* 文件说明：集合类的扩展方法 提供常量数组 判断数组是否为null 或者为空   合并数组  随机数组一项
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/

using System;
using System.Collections;
using System.Text;

namespace Climb.Utility.SystemExt
{
    /// <summary>
    /// 数组扩展
    /// </summary>
    public static class ArraryExt
    {
       

        #region 判断数组是否为空
        /// <summary>
        /// 判断数组是否为空
        /// </summary>
        /// <param name="array">当前数组</param>
        /// <returns>如果为空或者为null则返回true 否则返回false</returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public static bool IsNullEmpty(this Array array)
        {
            return array == null || array.Length == 0;
        }

        /// <summary>
        /// 判断集合是否为空或者为null 
        /// </summary>
        /// <param name="list">当前集合</param>
        /// <returns>如果为空或者为null则返回true 否则返回false</returns>
        public static bool IsNullEmpty(this ICollection list)
        {
            return (list == null) || (list.Count == 0);
        }
        #endregion

        #region 合并数组
        /// <summary>
        /// 合并两条数组
        /// </summary>
        /// <param name="t1">数组t1</param>
        /// <param name="t2">数组t2</param>
        /// <typeparam name="T">数组类型</typeparam>
        /// <returns>返回合并后的数组</returns>
        public static T[] CombineArrary<T>(T[] t1, T[] t2)
        {
            var tAry = new T[t1.Length + t2.Length];
            Array.Copy(t1, 0, tAry, 0, t1.Length);
            Array.Copy(t2, 0, tAry, t1.Length, t2.Length);
            return tAry;
        }
        #endregion

        #region 随机数组里的一项

        /// <summary>
        /// 获取数组中的随机一项
        /// </summary>
        /// <param name="t1">数组t1</param>
        /// <typeparam name="T">对象t类型</typeparam>
        /// <returns>返回某个对象</returns>
        public static T GetRand<T>(T[] t1)
        {
            T newObj = default(T);
            if (t1.IsNullEmpty()) return newObj;
            if (t1.Length == 1)
            {
                newObj = t1[0];
            }
            else
            {
                var randLen = RandomExt.GetRandomInt(0, t1.Length - 1);
                newObj = t1[randLen];
            }
            return newObj;
        }

        #endregion

        #region 扁平数组

        /// <summary>
        /// 将数组扁平化
        /// </summary>
        /// <param name="aryStrings">数组</param>
        /// <param name="separator">扁平化处理的字符串</param>
        /// <returns>返回字符串</returns>
        public static string ArrayJoin(ICollection aryStrings, string separator)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string argstr in aryStrings)
            {
                builder.Append(separator);
                builder.Append(argstr);
            }
            return builder.ToString();
        }
        #endregion




    }
}
