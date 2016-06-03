using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Core
{
    /// <summary>
    /// 数据转换
    /// </summary>
    public class TransformationData
    {
        #region 将DataTable的数据转换为实体类集合

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
        /// 将数据表转换为实体类。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dt">数据表</param>
        /// <returns></returns>
        public static IList<T> ConvertDataReaderToEntityList<T>(SqlDataReader dr) where T : new()
        {
            var type = typeof(T);
            var list = new List<T>();
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
                    catch (Exception ex)
                    {
                        continue;
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
        #endregion
    }
}
