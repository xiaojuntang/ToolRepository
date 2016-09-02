using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using ServiceStack;

namespace Common.Net.Core
{
    /// <summary>
    /// 数据转换
    /// </summary>
    public class TransformationData
    {
        #region DataTable转对象

        /// <summary>
        /// 将List转换成DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ConvertListToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dt = new DataTable();
            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dt.Columns.Add(property.Name, property.PropertyType);
            }
            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }
                dt.Rows.Add(values);
            }
            return dt;
        }
        /// <summary>
        /// 将数据表转换为实体类。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dt">数据表</param>
        /// <returns></returns>
        public static IList<T> ConvertDataTableToEntityList<T>(DataTable dt) where T : new()
        {
            var type = typeof(T);
            var list = new List<T>();
            if (dt.Rows.Count == 0)
            {
                return list;
            }

            var pros = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (DataRow dr in dt.Rows)
            {
                var t = new T();
                foreach (var p in pros)
                {
                    if (p.CanWrite)
                    {
                        if (dt.Columns.Contains(p.Name) && !Convert.IsDBNull(dr[p.Name]))
                        {
                            p.SetValue(t, dr[p.Name], null);
                        }
                    }
                }
                list.Add(t);
            }
            return list;
        }

        /// <summary>
        /// 将数据某条记录转换为实体类。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dt">数据某条记录</param>
        /// <returns></returns>
        public static T ConvertDataTableToEntitySingle<T>(DataTable dt) where T : new()
        {
            var type = typeof(T);
            var list = new T();
            if (dt.Rows.Count == 0)
            {
                return list;
            }

            var pros = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (DataRow dr in dt.Rows)
            {
                var t = new T();
                foreach (var p in pros)
                {
                    if (p.CanWrite)
                    {
                        if (dt.Columns.Contains(p.Name) && !Convert.IsDBNull(dr[p.Name]))
                        {
                            p.SetValue(t, dr[p.Name], null);
                        }
                    }
                }

                list = t;
            }
            return list;
        }

        /// <summary>
        /// DataTable 转换成泛型List
        /// </summary>
        /// <typeparam name="T">实体对象类</typeparam>
        /// <param name="table">数据DatatTable</param>
        /// <returns> List<实体对象></returns>
        public static List<T> ConvertDataTableToList<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }
            List<T> list = new List<T>();
            T t = default(T);
            PropertyInfo[] propertypes = null;
            string tempName = string.Empty;
            foreach (DataRow row in table.Rows)
            {
                t = Activator.CreateInstance<T>();
                propertypes = t.GetType().GetProperties();
                foreach (PropertyInfo pro in propertypes)
                {
                    if (!pro.CanWrite)
                    {
                        continue;
                    }
                    tempName = pro.Name;
                    if (table.Columns.Contains(tempName.ToUpper()))
                    {
                        object value = row[tempName];
                        if (value is System.DBNull)
                        {
                            value = null;
                            if (pro.PropertyType.FullName == "System.String")
                            {
                                value = string.Empty;
                            }
                        }
                        if (pro.PropertyType.IsGenericType && pro.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) && value != null)
                        {
                            pro.SetValue(t, Convert.ChangeType(value, Nullable.GetUnderlyingType(pro.PropertyType)), null);
                        }
                        else
                        {
                            pro.SetValue(t, value, null);
                        }
                    }
                }
                list.Add(t);
            }
            return list;
        }
        #endregion

        #region DataReader转对象
        /// <summary>
        /// 将数据表转换为实体类。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dr">数据表</param>
        /// <returns></returns>
        public static IList<T> ConvertDataReaderToEntityList<T>(SqlDataReader dr) where T : new()
        {
            //var type = typeof(T);
            //var list = new List<T>();
            //if (!dr.HasRows)
            //{
            //    return list;
            //}

            //var pros = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            //while (dr.Read())
            //{
            //    var t = new T();
            //    foreach (var p in pros)
            //    {
            //        try
            //        {
            //            //dr.GetOrdinal(p.Name)如果字段不存在会抛异常
            //            if (p.CanWrite)
            //            {
            //                if (dr.GetOrdinal(p.Name) != -1 && !Convert.IsDBNull(dr[p.Name]))
            //                {
            //                    p.SetValue(t, dr[p.Name], null);
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            continue;
            //        }
            //    }
            //    list.Add(t);
            //}
            //return list;
            IList<T> list = new List<T>();
            while (dr.Read())
            {
                T t = Activator.CreateInstance<T>();
                Type obj = t.GetType();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    object tempValue = null;
                    if (dr.IsDBNull(i))
                    {
                        if (obj.GetProperty(dr.GetName(i)) != null)
                        {
                            string typeFullName = obj.GetProperty(dr.GetName(i)).PropertyType.FullName;
                            tempValue = GetDbNullValue(typeFullName);
                        }
                    }
                    else
                    {
                        tempValue = dr.GetValue(i);
                    }
                    if (obj.GetProperty(dr.GetName(i)) != null)
                    {
                        obj.GetProperty(dr.GetName(i)).SetValue(t, tempValue, null);
                    }

                }
                list.Add(t);
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> QueryList<T>(IDataReader reader) where T : class, new()
        {
            List<T> list = new List<T>();
            //using (reader)
            //{
            //    if (reader.Read())
            //    {
            //        int fcount = reader.FieldCount;
            //        INamedMemberAccessor[] accessors = new INamedMemberAccessor[fcount];
            //        DelegatedReflectionMemberAccessor drm = new DelegatedReflectionMemberAccessor();
            //        for (int i = 0; i < fcount; i++)
            //        {
            //            accessors[i] = drm.FindAccessor<T>(reader.GetName(i));
            //        }
            //        do
            //        {
            //            T t = new T();
            //            for (int i = 0; i < fcount; i++)
            //            {
            //                if (!reader.IsDBNull(i))
            //                    accessors[i].SetValue(t, reader.GetValue(i));
            //            }
            //            list.Add(t);
            //        } while (reader.Read());
            //    }
            //}
            return list;
        }



        /// <summary>
        /// 将数据某条记录转换为实体类。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dt">数据某条记录</param>
        /// <returns></returns>
        public static T ConvertDataReaderToEntitySingle<T>(SqlDataReader dr) where T : new()
        {
            var type = typeof(T);
            var list = new T();
            if (!dr.HasRows)
            {
                return list;
            }

            var pros = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            while (dr.Read())
            {
                var t = new T();
                foreach (var p in pros)
                {
                    try
                    {
                        //dr.GetOrdinal(p.Name)如果字段不存在会抛异常
                        if (p.CanWrite)
                        {
                            if (dr.GetOrdinal(p.Name) != -1 && !Convert.IsDBNull(dr[p.Name]))
                            {
                                p.SetValue(t, dr[p.Name], null);
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                list = t;
            }
            return list;
        }







        /// <summary>  
        /// DataReader转换为obj list  
        /// </summary>  
        /// <typeparam name="T">泛型</typeparam>  
        /// <param name="rdr">datareader</param>  
        /// <returns>返回泛型类型</returns>  
        public static IList<T> DataReader2List<T>(SqlDataReader rdr)
        {
            IList<T> list = new List<T>();
            while (rdr.Read())
            {
                T t = System.Activator.CreateInstance<T>();
                Type obj = t.GetType();
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    object tempValue = null;
                    if (rdr.IsDBNull(i))
                    {
                        string typeFullName = obj.GetProperty(rdr.GetName(i)).PropertyType.FullName;
                        tempValue = GetDbNullValue(typeFullName);
                    }
                    else
                    {
                        tempValue = rdr.GetValue(i);
                    }
                    obj.GetProperty(rdr.GetName(i)).SetValue(t, tempValue, null);
                }
                list.Add(t);
            }
            return list;
        }

        /// <summary>  
        /// 返回值为DBnull的默认值  
        /// </summary>  
        /// <param name="typeFullName">数据类型的全称，类如：system.int32</param>  
        /// <returns>返回的默认值</returns>  
        private static object GetDbNullValue(string typeFullName)
        {
            //typeFullName = typeFullName.ToLower();

            if (typeFullName == "System.String")
            {
                return String.Empty;
            }

            if (typeFullName == "System.Int32")
            {
                return 0;
            }
            if (typeFullName == "System.DateTime")
            {
                return DateTime.MinValue;
            }
            if (typeFullName == "System.Boolean")
            {
                return false;
            }
            return null;
        }
        #endregion

        #region 字典与对象转换

        /// <summary>
        /// 字典转换对象
        /// </summary>
        /// <param name="dictionary">字典</param>
        /// <typeparam name="T">对象</typeparam>
        /// <returns></returns>
        public static T DicToEntity<T>(Dictionary<string, string> dictionary) where T : new()
        {
            if (dictionary == null)
                return default(T);
            else
            {
                return dictionary.ToJson().FromJson<T>();
            }
        }

        /// <summary>
        /// 字典列表转对象列表
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="dictionarys">字典</param>
        /// <returns></returns>
        public static List<T> DicToList<T>(List<Dictionary<string, string>> dictionarys) where T : new()
        {
            if (dictionarys == null)
            {
                return new List<T>();
            }
            return dictionarys.Select(d => DicToEntity<T>(d)).ToList();
        }
        #endregion
    }
}
