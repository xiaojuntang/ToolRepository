
/**************************************************
* 文 件 名：ReflectionUtil.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014/9/9 17:48:59
* 文件说明：反射
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Climb.Utility.SystemExt
{

    /// <summary>
    /// 反射操作辅助类，如获取或设置字段、属性的值等反射信息。
    /// </summary>
    public sealed class ReflectionExt
    {
        #region 属性字段设置
        /// <summary>
        /// 
        /// </summary>
        static readonly BindingFlags Bf = BindingFlags.DeclaredOnly | BindingFlags.Public |
                                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        /// <summary>
        /// 执行一个对象的方法
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object InvokeMethod(object obj, string methodName, object[] args)
        {
            Type type = obj.GetType();
            object objReturn = type.InvokeMember(methodName, Bf | BindingFlags.InvokeMethod, null, obj, args);
            return objReturn;
        }
        /// <summary>
        /// 设置某个字段的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetField(object obj, string name, object value)
        {
            FieldInfo fi = obj.GetType().GetField(name, Bf);
            if (fi != null) fi.SetValue(obj, value);
        }
        /// <summary>
        /// 获取字段的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetField(object obj, string name)
        {
            FieldInfo fi = obj.GetType().GetField(name, Bf);
            if (fi == null) return null; 
                return fi.GetValue(obj);
        }
        /// <summary>
        /// 获取所有的字段集合
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static FieldInfo[] GetFields(object obj)
        {
            FieldInfo[] fieldInfos = obj.GetType().GetFields(Bf);
            return fieldInfos;
        }

        /// <summary>
        /// 设置属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetProperty(object obj, string name, object value)
        {
            PropertyInfo fieldInfo = obj.GetType().GetProperty(name, Bf);
            value = Convert.ChangeType(value, fieldInfo.PropertyType);
            fieldInfo.SetValue(obj, value, null);
        }
        /// <summary>
        /// 获取属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetProperty(object obj, string name)
        {
            PropertyInfo fieldInfo = obj.GetType().GetProperty(name, Bf);
            return fieldInfo.GetValue(obj, null);
        }
        /// <summary>
        /// 索取所有属性的集合
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetProperties(object obj)
        {
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties(Bf);
            return propertyInfos;
        } 

        #endregion

        #region 获取Description

        /// <overloads>
        ///		Get The Member Description using Description Attribute.
        /// </overloads>
        /// <summary>
        /// Get The Enum Field Description using Description Attribute.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>return description or value.ToString()</returns>
        public static string GetDescription(Enum value)
        {
            return GetDescription(value, null);
        }

        /// <summary>
        /// Get The Enum Field Description using Description Attribute and 
        /// objects to format the Description.
        /// </summary>
        /// <param name="value">Enum For Which description is required.</param>
        /// <param name="args">An Object array containing zero or more objects to format.</param>
        /// <returns>return null if DescriptionAttribute is not found or return type description</returns>
        public static string GetDescription(Enum value, params object[] args)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            string text1 = (attributes.Length > 0) ? attributes[0].Description : value.ToString();

            if ((args != null) && (args.Length > 0))
            {
                return string.Format(null, text1, args);
            }
            return text1;
        }

        /// <summary>
        ///	Get The Type Description using Description Attribute.
        /// </summary>
        /// <param name="member">Specified Member for which Info is Required</param>
        /// <returns>return null if DescriptionAttribute is not found or return type description</returns>
        public static string GetDescription(MemberInfo member)
        {
            return GetDescription(member, null);
        }

        /// <summary>
        /// Get The Type Description using Description Attribute and 
        /// objects to format the Description.
        /// </summary>
        /// <param name="member"> Specified Member for which Info is Required</param>
        /// <param name="args">An Object array containing zero or more objects to format.</param>
        /// <returns>return <see cref="String.Empty"/> if DescriptionAttribute is 
        /// not found or return type description</returns>
        public static string GetDescription(MemberInfo member, params object[] args)
        {
            string text1;

            if (member == null)
            {
                throw new ArgumentNullException("member");
            }

            if (member.IsDefined(typeof(DescriptionAttribute), false))
            {
                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])member.GetCustomAttributes(typeof(DescriptionAttribute), false);
                text1 = attributes[0].Description;
            }
            else
            {
                return String.Empty;
            }

            if ((args != null) && (args.Length > 0))
            {
                return String.Format(null, text1, args);
            }
            return text1;
        }

        #endregion

        #region 获取Attribute信息

        /// <overloads>
        /// Gets the specified object attributes
        /// </overloads>
        /// <summary>
        /// Gets the specified object attributes for assembly as specified by type
        /// </summary>
        /// <param name="attributeType">The attribute Type for which the custom attributes are to be returned.</param>
        /// <param name="assembly">the assembly in which the specified attribute is defined</param>
        /// <returns>Attribute as Object or null if not found.</returns>
        public static object GetAttribute(Type attributeType, Assembly assembly)
        {
            if (attributeType == null)
            {
                throw new ArgumentNullException("attributeType");
            }

            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }


            if (assembly.IsDefined(attributeType, false))
            {
                object[] attributes = assembly.GetCustomAttributes(attributeType, false);

                return attributes[0];
            }

            return null;
        }


        /// <summary>
        /// Gets the specified object attributes for type as specified by type
        /// </summary>
        /// <param name="attributeType">The attribute Type for which the custom attributes are to be returned.</param>
        /// <param name="type">the type on which the specified attribute is defined</param>
        /// <returns>Attribute as Object or null if not found.</returns>
        public static object GetAttribute(Type attributeType, MemberInfo type)
        {
            return GetAttribute(attributeType, type, false);
        }


        /// <summary>
        /// Gets the specified object attributes for type as specified by type with option to serach parent
        /// </summary>
        /// <param name="attributeType">The attribute Type for which the custom attributes are to be returned.</param>
        /// <param name="type">the type on which the specified attribute is defined</param>
        /// <param name="searchParent">if set to <see langword="true"/> [search parent].</param>
        /// <returns>
        /// Attribute as Object or null if not found.
        /// </returns>
        public static object GetAttribute(Type attributeType, MemberInfo type, bool searchParent)
        {
            if (attributeType == null)
            {
                return null;
            }

            if (type == null)
            {
                return null;
            }

            if (!(attributeType.IsSubclassOf(typeof(Attribute))))
            {
                return null;
            }


            if (type.IsDefined(attributeType, searchParent))
            {
                object[] attributes = type.GetCustomAttributes(attributeType, searchParent);

                if (attributes.Length > 0)
                {
                    return attributes[0];
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the collection of all specified object attributes for type as specified by type
        /// </summary>
        /// <param name="attributeType">The attribute Type for which the custom attributes are to be returned.</param>
        /// <param name="type">the type on which the specified attribute is defined</param>
        /// <returns>Attribute as Object or null if not found.</returns>
        public static object[] GetAttributes(Type attributeType, MemberInfo type)
        {
            return GetAttributes(attributeType, type, false);
        }


        /// <summary>
        /// Gets the collection of all specified object attributes for type as specified by type with option to serach parent
        /// </summary>
        /// <param name="attributeType">The attribute Type for which the custom attributes are to be returned.</param>
        /// <param name="type">the type on which the specified attribute is defined</param>
        /// <param name="searchParent">The attribute Type for which the custom attribute is to be returned.</param>
        /// <returns>
        /// Attribute as Object or null if not found.
        /// </returns>
        public static object[] GetAttributes(Type attributeType, MemberInfo type, bool searchParent)
        {
            if (type == null)
            {
                return null;
            }

            if (attributeType == null)
            {
                return null;
            }

            if (!(attributeType.IsSubclassOf(typeof(Attribute))))
            {
                return null;
            }


            if (type.IsDefined(attributeType, false))
            {
                return type.GetCustomAttributes(attributeType, searchParent);
            }

            return null;
        }

        #endregion

        #region 创建对应实例
        /// <summary>
        /// 创建对应实例
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>对应实例</returns>
        public static object CreateInstance(string type)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return (from t in assemblies let tmp = t.GetType(type) where tmp != null select t.CreateInstance(type)).FirstOrDefault();
            //return Assembly.GetExecutingAssembly().CreateInstance(type);
        }

        /// <summary>
        /// 创建对应实例
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>对应实例</returns>
        public static object CreateInstance(Type type)
        {
            return CreateInstance(type.FullName);
        } 
        #endregion
    }
}
