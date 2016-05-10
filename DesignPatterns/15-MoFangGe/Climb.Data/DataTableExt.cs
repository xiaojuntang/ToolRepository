
/**************************************************
* 文 件 名：DataTableExt.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014年9月2日 17:59:59
* 文件说明：Data 数据扩展类
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization; 
using System.Linq;
using System.Reflection;

namespace Climb.Data
{
    /// <summary>
    /// DataTable扩展
    /// </summary>
    public sealed class DataTableExt
    {
        #region 给datatable 添加一个自增列
        /// <summary>
        /// 给DataTable增加一个自增列
        /// 如果DataTable 存在 identityid 字段  则 直接返回DataTable 不做任何处理
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns>返回Datatable 增加字段 identityid </returns>
        public static DataTable AddIdentityColumn(DataTable dt)
        {
            if (dt.Columns.Contains("identityid")) return dt;
            dt.Columns.Add("identityid");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["identityid"] = (i + 1).ToString(CultureInfo.InvariantCulture);
            }
            return dt;
        }
        #endregion

        #region 检查DataTable 是否有数据行
        /// <summary>
        /// 检查DataTable 是否有数据行
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public static bool IsHasRows(DataTable dt)
        {
            return dt != null && dt.Rows.Count > 0;
        }
        #endregion

        #region 通过字符列表创建表字段

        /// <summary>
        /// 通过字符列表创建表字段，字段格式可以是：
        /// 1) a,b,c,d,e
        /// </summary>
        /// <param name="aryStrings"></param>
        /// <returns></returns>
        public static DataTable CreateTable(params string[] aryStrings)
        {

            if (aryStrings.Length <= 0) return null;
            DataTable dt = new DataTable();
            foreach (string item in aryStrings)
            {
                dt.Columns.Add(item);
            }
            return dt;
        }


        //public static DataTable CreateTable<T>(T t1)
        //    where T : new()
        //{
        //    if (null == t1) return null;
        //    DataTable myDataTable = new DataTable();
        //    return myDataTable;
        //}

        #endregion

        #region 获得从DataRowCollection转换成的DataRow数组
        /// <summary>
        /// 获得从DataRowCollection转换成的DataRow数组
        /// </summary>
        /// <param name="drc">DataRowCollection</param>
        /// <returns></returns>
        public static DataRow[] GetDataRowArray(DataRowCollection drc)
        {
            int count = drc.Count;
            DataRow[] drs = new DataRow[count];
            for (int i = 0; i < count; i++)
            {
                drs[i] = drc[i];
            }
            return drs;
        }
        #endregion

        #region 将DataRow数组转换成DataTable，注意行数组的每个元素须具有相同的数据结构
        /// <summary>
        /// 将DataRow数组转换成DataTable，注意行数组的每个元素须具有相同的数据结构，
        /// 否则当有元素长度大于第一个元素时，抛出异常
        /// </summary>
        /// <param name="rows">行数组</param>
        /// <returns></returns>
        public static DataTable GetTableFromRows(DataRow[] rows)
        {
            if (rows.Length <= 0)
            {
                return new DataTable();
            }
            DataTable dt = rows[0].Table.Clone();
            dt.DefaultView.Sort = rows[0].Table.DefaultView.Sort;
            foreach (DataRow t in rows)
            {
                dt.LoadDataRow(t.ItemArray, true);
            }
            return dt;
        }
        #endregion

        #region 排序表的视图
        /// <summary>
        /// 排序表的视图
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public static DataTable SortedTable(DataTable dt, params string[] sorts)
        {
            if (dt.Rows.Count <= 0) return dt;
            string tmp = sorts.Aggregate("", (current, t) => current + (t + ","));
            dt.DefaultView.Sort = tmp.TrimEnd(',');
            return dt;
        }
        #endregion

        #region 根据条件过滤表的内容
        /// <summary>
        /// 根据条件过滤表的内容
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static DataTable FilterDataTable(DataTable dt, string condition)
        {
            if (condition.Trim() == "")
            {
                return dt;
            }
            DataTable newdt = dt.Clone();
            DataRow[] dr = dt.Select(condition);
            foreach (DataRow t in dr)
            {
                newdt.ImportRow(t);
            }
            return newdt;
        }
        #endregion

        #region 将泛型集合转换成datatable
        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="propertyName">需要返回的列的列名</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTable<T>(IList<T> list, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();
            if (propertyName != null)
                propertyNameList.AddRange(propertyName);

            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                        {
                            result.Columns.Add(pi.Name, pi.PropertyType);
                        }
                    }
                }

                foreach (T t in list)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(t, null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(t, null);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTable<T>(IList<T> list)
        {
            return ToDataTable(list, null);
        }

        /// <summary>
        /// 实体列表转换成DataTable
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="list"> 实体列表</param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(IList<T> list)
            where T : class
        {
            if (list == null || list.Count <= 0)
            {
                return null;
            }
            DataTable dt = new DataTable(typeof(T).Name);

            PropertyInfo[] myPropertyInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            int length = myPropertyInfo.Length;
            bool createColumn = true;

            foreach (T t in list)
            {
                if (t == null)
                {
                    continue;
                }

                DataRow row = dt.NewRow();
                for (int i = 0; i < length; i++)
                {
                    PropertyInfo pi = myPropertyInfo[i];
                    string name = pi.Name;
                    if (createColumn)
                    {
                        DataColumn column = new DataColumn(name, pi.PropertyType);
                        dt.Columns.Add(column);
                    }

                    row[name] = pi.GetValue(t, null);
                }

                if (createColumn)
                {
                    createColumn = false;
                }

                dt.Rows.Add(row);
            }
            return dt;

        }

        #endregion

        #region datatable 转换实体类
        /// <summary>
        /// DataTable转换成实体列表
        /// </summary>
        /// <typeparam name="T">实体 T </typeparam>
        /// <param name="table">datatable</param>
        /// <returns></returns>
        public static IList<T> DataTableToList<T>(DataTable table)
            where T : class
        {
            if (!IsHasRows(table))
                return new List<T>();

            IList<T> list = new List<T>();
            foreach (DataRow dr in table.Rows)
            {
                T model = Activator.CreateInstance<T>();
                foreach (DataColumn dc in dr.Table.Columns)
                {
                    object drValue = dr[dc.ColumnName];
                    PropertyInfo pi = model.GetType().GetProperty(dc.ColumnName);

                    if (pi != null && pi.CanWrite && (drValue != null && !Convert.IsDBNull(drValue)))
                    {
                        pi.SetValue(model, drValue, null);
                    }
                }

                list.Add(model);
            }
            return list;
        }
        #endregion

    }
}
