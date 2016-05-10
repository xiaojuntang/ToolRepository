/**************************************************
* 文 件 名：EnumManager.cs
* 文件版本：1.0
* 创 建 人：ssh
* 联系方式： Email:shaoshouhe@mofangge.com  
* 创建日期：2015/6/18  
* 文件说明：枚举类型管理类
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Web.UI.WebControls;

namespace Climb.Utility.SystemExt
{
    /// <summary>
    /// 枚举类型管理类
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public static class EnumManager<TEnum>
    {
        #region 静态公共实例：方法

        /// <summary>
        /// 绑定到列表框
        /// </summary>
        /// <param name="listControl">ListControl组件</param>
        public static void BindListControl(ListControl listControl)
        {
            try
            {
                listControl.Items.Clear();
                listControl.DataSource = GetEnumValDesc();
                listControl.DataTextField = "Value";
                listControl.DataValueField = "Key";
                listControl.DataBind();
            }
            catch
            {
                // ignored
            }
        }


        /// <summary>
        /// 通过枚举值返回文本信息
        /// </summary>
        /// <param name="val">枚举值</param>
        /// <returns></returns>
        public static string GetDescByVal(int val)
        {
            try
            {
                string result = string.Empty;
                Type enumType = typeof(TEnum);
                FieldInfo[] fieldinfos = enumType.GetFields();
                foreach (FieldInfo field in fieldinfos)
                {
                    if (field.FieldType.IsEnum)
                    {
                        Object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (val == Convert.ToInt32(field.GetRawConstantValue()))
                        {
                            result = ((DescriptionAttribute)objs[0]).Description;
                            break;
                        }
                    }
                }
                return result;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 通过枚举值返回文本信息
        /// </summary>
        /// <param name="val">枚举值</param>
        /// <returns></returns>
        public static string GetNameByVal(int val)
        {
            try
            {
                string result = string.Empty;
                Type enumType = typeof(TEnum);
                FieldInfo[] fieldinfos = enumType.GetFields();
                foreach (FieldInfo field in fieldinfos)
                {
                    if (field.FieldType.IsEnum)
                    {
                        if (val == Convert.ToInt32(field.GetRawConstantValue()))
                        {
                            result = field.Name;

                            break;
                        }
                    }
                }
                return result;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        ///  返回枚举集合 Dict枚举值，描述
        /// </summary>
        /// <returns>Dic枚举值，描述
        /// </returns>
        public static Dictionary<int, string> GetEnumValDesc()
        {
            try
            {
                Dictionary<int, string> dic = new Dictionary<int, string>();
                Type enumType = typeof(TEnum);
                FieldInfo[] fieldinfos = enumType.GetFields();
                foreach (FieldInfo field in fieldinfos)
                {
                    if (field.FieldType.IsEnum)
                    {
                        Object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        dic.Add(Convert.ToInt32(field.GetRawConstantValue()), ((DescriptionAttribute)objs[0]).Description);//Dic<枚举项（名称），描述>
                    }
                }
                return dic;
            }
            catch
            {
                return new Dictionary<int, string>();
            }
        }


        ///<summary>
        /// 返回枚举集合 Dic枚举项，描述
        ///</summary>
        ///<returns>Dic枚举项，描述</returns>
        public static Dictionary<string, string> GetEnumNameDesc()
        {
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                Type enumType = typeof(TEnum);
                FieldInfo[] fieldinfos = enumType.GetFields();
                foreach (FieldInfo field in fieldinfos)
                {
                    if (field.FieldType.IsEnum)
                    {
                        Object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        dic.Add(field.Name, ((DescriptionAttribute)objs[0]).Description);
                    }
                }
                return dic;
            }
            catch
            {
                return new Dictionary<string, string>();
            }
        }

        #endregion
    }
}
