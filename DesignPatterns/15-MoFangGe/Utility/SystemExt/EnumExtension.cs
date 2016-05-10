/**************************************************
* 文 件 名：EnumExtension.cs
* 文件版本：1.0
* 创 建 人：ssh
* 联系方式： Email:shaoshouhe@mofangge.com  
* 创建日期：2015/6/18  
* 文件说明：枚举扩展
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/

using System;
using System.Reflection;

namespace Climb.Utility.SystemExt
{
     
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// 获得枚举字段的特性(Attribute)，该属性不允许多次定义。
        /// </summary>
        /// <typeparam name="T">特性类型。</typeparam>
        /// <param name="value">一个枚举的实例对象。</param>
        /// <returns>枚举字段的扩展属性。如果不存在则返回 <c>null</c> 。</returns>
        public static T GetEnumAttribute<T>(this System.Enum value) where T : Attribute
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            return Attribute.GetCustomAttribute(field, typeof(T)) as T;
        }
    }
}
