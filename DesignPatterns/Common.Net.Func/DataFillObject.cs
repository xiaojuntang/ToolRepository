/***************************************************************************** 
*        filename :DataFillObject 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.42000 
*        新建项输入的名称:   DataFillObject 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Common.Net.Func 
*        文件名:             DataFillObject 
*        创建系统时间:       2016/2/3 9:49:38 
*        创建年份:           2016 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Func
{
    /// <summary>
    /// 将数据对象转换成 实例对象
    /// </summary>
    public sealed class DataFillObject
    {
        /// <summary>
        /// 判断字段是否存在
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool ReaderExists(IDataReader reader, string name)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i) == name)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 将 datatable 转转成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList<T> ConvertToList<T>(DataTable dt) where T : new()
        {
            // 定义集合 
            List<T> ts = new List<T>();
            // 获得此模型的类型 
            Type type = typeof(T);
            //定义一个临时变量 
            string tempName = string.Empty;
            //遍历DataTable中所有的数据行  
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性 
                PropertyInfo[] propertys = t.GetType().GetProperties();
                //遍历该对象的所有属性 
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;//将属性名称赋值给临时变量   
                    //检查DataTable是否包含此列（列名==对象的属性名）     
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter   
                        if (!pi.CanWrite) continue;//该属性不可写，直接跳出   
                        //取值   
                        object value = dr[tempName];
                        //如果非空，则赋给对象的属性   
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                //对象添加到泛型集合中 
                ts.Add(t);
            }
            return ts;
        }

        /// <summary>
        /// 填充单个实例 datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static void ConvertToList<T>(DataTable dt, ref T t) where T : new()
        {
            // 定义集合 
            // 获得此模型的类型 
            Type type = typeof(T);
            //定义一个临时变量 
            string tempName = string.Empty;
            //遍历DataTable中所有的数据行  
            // 获得此模型的公共属性 
            PropertyInfo[] propertys = t.GetType().GetProperties();
            //遍历该对象的所有属性 
            foreach (PropertyInfo pi in propertys)
            {
                tempName = pi.Name;//将属性名称赋值给临时变量   
                //检查DataTable是否包含此列（列名==对象的属性名）     
                if (dt.Columns.Contains(tempName))
                {
                    // 判断此属性是否有Setter   
                    if (!pi.CanWrite) continue;//该属性不可写，直接跳出   
                    //取值   
                    object value = dt.Rows[0][tempName];
                    //如果非空，则赋给对象的属性   
                    if (value != DBNull.Value)
                        pi.SetValue(t, value, null);
                }
            }
        }

        /// <summary>
        /// 将datareader 转换成对象 并且关闭链接
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IList<T> ConvertToList<T>(IDataReader reader, bool isClose) where T : new()
        {
            // 定义集合 
            List<T> ts = new List<T>();
            // 获得此模型的类型 
            Type type = typeof(T);
            //定义一个临时变量 
            string tempName = string.Empty;
            //遍历DataTable中所有的数据行  
            if (reader != null && !reader.IsClosed)
            {
                while (reader.Read())
                {
                    T t = new T();
                    PropertyInfo[] propertys = t.GetType().GetProperties();
                    foreach (PropertyInfo pi in propertys)
                    {
                        tempName = pi.Name;//将属性名称赋值给临时变量   
                        //检查DataTable是否包含此列（列名==对象的属性名）
                        if (ReaderExists(reader, tempName))
                        {
                            int Ordinal = reader.GetOrdinal(tempName);
                            if (reader.GetValue(Ordinal) != DBNull.Value)
                            {
                                // 判断此属性是否有Setter   
                                if (!pi.CanWrite) continue;//该属性不可写，直接跳出   
                                //取值   
                                object value = reader[Ordinal];
                                //如果非空，则赋给对象的属性   
                                if (value != DBNull.Value)
                                    pi.SetValue(t, value, null);
                            }
                        }

                    }
                    ts.Add(t);
                }
                if (isClose)
                {
                    reader.Close();
                }
            }
            return ts;
        }

        /// <summary>
        /// 填充单个实例 datareader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static void ConvertToList<T>(IDataReader reader, bool isClose, ref T t) where T : new()
        {
            // 获得此模型的类型 
            Type type = typeof(T);
            //定义一个临时变量 
            string tempName = string.Empty;
            //遍历DataTable中所有的数据行  
            if (reader != null && !reader.IsClosed)
            {
                while (reader.Read())
                {
                    PropertyInfo[] propertys = t.GetType().GetProperties();
                    foreach (PropertyInfo pi in propertys)
                    {
                        tempName = pi.Name;//将属性名称赋值给临时变量   
                        //检查DataTable是否包含此列（列名==对象的属性名）
                        if (ReaderExists(reader, tempName))
                        {
                            int Ordinal = reader.GetOrdinal(tempName);
                            if (reader.GetValue(Ordinal) != DBNull.Value)
                            {
                                // 判断此属性是否有Setter   
                                if (!pi.CanWrite) continue;//该属性不可写，直接跳出   
                                //取值   
                                object value = reader[Ordinal];
                                //如果非空，则赋给对象的属性   
                                if (value != DBNull.Value)
                                    pi.SetValue(t, value, null);
                            }
                        }
                    }
                }
                if (isClose)
                {
                    reader.Close();
                }
            }
        }
    }
}
