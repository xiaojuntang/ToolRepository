using Climb.Core;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    public abstract class DALBase<T> : TranslateInfo<T>, IDALBase<T> where T : BaseEntity
    {
        #region 全局变量

        private string _constr = string.Empty;

        #endregion

        #region 构造函数

        public DALBase()
            : this(ConfigurationManager.AppSettings["ConStr"].ToString())
        {
        }

        public DALBase(string connectionString)
        {
            _constr = connectionString;
        }



        #endregion

        #region 属性

        public string ConStr
        {
            get { return _constr; }
            set { _constr = value; }
        }

        /// <summary>
        /// 表名
        /// </summary>
        protected virtual string TableName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        #endregion

        #region 查询数据

        public abstract T GetInfo(IDataReader dr);

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public T FindSingle(Query query)
        {
            T t = default(T);
            // 执行操作
            using (MySqlConnection con = new MySqlConnection(_constr))
            {
                MySqlCommand cmd = con.CreateCommand();
                TranslateInto(query, cmd, this.TableName);

                ConOpen(con);
                using (MySqlDataReader read = cmd.ExecuteReader())
                {
                    if (read.Read())
                    {
                        t = GetInfo(read);
                    }
                }
            }

            return t;
        }

        /// <summary>
        /// 查询实体列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<T> FindList(Query query)
        {
            List<T> list = new List<T>();

            // 执行操作
            using (MySqlConnection con = new MySqlConnection(_constr))
            {
                MySqlCommand cmd = con.CreateCommand();
                TranslateInto(query, cmd, this.TableName);

                ConOpen(con);
                using (MySqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        list.Add(GetInfo(read));
                    }
                }
            }

            return list;
        }
        /// <summary>
        /// 查询实体列表（分页）
        /// </summary>
        /// <param name="query"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T> FindList(Query query, int index, int count)
        {
            List<T> list = new List<T>();

            // 执行操作
            using (MySqlConnection con = new MySqlConnection(_constr))
            {
                MySqlCommand cmd = con.CreateCommand();
                TranslateInto(query, cmd, this.TableName);
                cmd.CommandText = string.Format("{0} limit {1},{2}", cmd.CommandText, index, count);
                ConOpen(con);
                using (MySqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        list.Add(GetInfo(read));
                    }
                }
            }

            return list;
        }
        /// <summary>
        /// 查询所有数据实体
        /// </summary>
        /// <returns></returns>
        public List<T> FindListAll(string orderBy)
        {
            List<T> list = new List<T>();

            // 执行操作
            using (MySqlConnection con = new MySqlConnection(_constr))
            {
                MySqlCommand cmd = con.CreateCommand();

                orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : " f_id ";
                string sql = string.Format("select * from {0} order by {1}", this.TableName, orderBy);
                cmd.CommandText = sql;
                ConOpen(con);
                using (MySqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        list.Add(GetInfo(read));
                    }
                }
            }

            return list;
        }
        /// <summary>
        /// 查询数据个数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int FindCount(Query query)
        {
            int result = 0;

            // 执行操作
            using (MySqlConnection con = new MySqlConnection(_constr))
            {
                MySqlCommand cmd = con.CreateCommand();

                string sqlQuery = TranslateWhere(query, cmd);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format("select count(1) from {0} {1} ", this.TableName, sqlQuery);
                ConOpen(con);
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return result;
        }

        public List<T> FindList(Query query, int take)
        {
            List<T> list = new List<T>();

            // 执行操作
            using (MySqlConnection con = new MySqlConnection(_constr))
            {
                MySqlCommand cmd = con.CreateCommand();
                TranslateInto(query, cmd, this.TableName);
                cmd.CommandText = string.Format("{0} limit {1}", cmd.CommandText, take);
                ConOpen(con);
                using (MySqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        list.Add(GetInfo(read));
                    }
                }
            }

            return list;
        }

        #endregion

        #region 更新数据

        /// <summary>
        /// 更新数据信息
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public int Update(Update update)
        {
            int result = 0;
            using (MySqlConnection con = new MySqlConnection(_constr))
            {
                try
                {
                    MySqlCommand cmd = con.CreateCommand();
                    TranslateInto(update, cmd, this.TableName);
                    ConOpen(con);
                    result = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    ConClose(con);
                }
            }

            return result;
        }

        public int Save(T t)
        {
            int result = 0;

            using (MySqlConnection con = new MySqlConnection(_constr))
            {
                try
                {
                    MySqlCommand cmd = con.CreateCommand();
                    PropertyInfo[] pInfo = t.GetType().GetProperties();
                    StringBuilder fileds = new StringBuilder();
                    StringBuilder values = new StringBuilder();

                    foreach (PropertyInfo item in pInfo)
                    {
                        //if (item.Name.ToLower().Equals("f_id"))
                        //{
                        //    continue;
                        //}
                        string param = "?" + item.Name;
                        fileds.Append(item.Name).Append(",");
                        values.Append(param).Append(",");
                        cmd.Parameters.AddWithValue(param, item.GetValue(t, null));
                    }

                    string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2});select LAST_INSERT_ID();", this.TableName, fileds.ToString().TrimEnd(','), values.ToString().TrimEnd(','));
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;

                    ConOpen(con);
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {

                    throw;
                }
                finally
                {
                    ConClose(con);
                }
            }

            return result;
        }
        /// <summary>
        /// 关闭数据库
        /// </summary>
        /// <param name="con"></param>
        private void ConClose(MySqlConnection con)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        /// <summary>
        /// 打开数据库
        /// </summary>
        /// <param name="con"></param>
        private void ConOpen(MySqlConnection con)
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
        }

        #endregion

    }
}