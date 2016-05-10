using Climb.Core;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    public abstract partial class AutoIdentity<T> : DALBase<T>, IAutoIdentity<T>
    where T : BaseEntity
    {
        /// <summary>
        /// 自增长列
        /// </summary>
        public virtual string AutoIdentityName
        {
            get { return "F_ID"; }
        }

        /// <summary>
        ///  获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetInfo(int id)
        {
            T info = default(T);

            using (MySqlConnection con = new MySqlConnection(base.ConStr))
            {
                MySqlCommand cmd = con.CreateCommand();
                string sql = string.Format("select * from {0} where {1}=?id ", this.TableName, this.AutoIdentityName);
                cmd.Parameters.AddWithValue("?id", id);
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                con.Open();
                using (MySqlDataReader read = cmd.ExecuteReader())
                {
                    if (read.Read())
                    {
                        info = GetInfo(read);
                    }
                }
            }

            return info;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>1：success 0:filed</returns>
        public int Delete(int id)
        {
            int result = 0;
            using (MySqlConnection con = new MySqlConnection(base.ConStr))
            {
                MySqlCommand cmd = con.CreateCommand();

                cmd.Parameters.AddWithValue("?ID", id);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format(" DELETE FROM {0} WHERE {1}=?ID ", this.TableName, this.AutoIdentityName);
                con.Open();
                result = cmd.ExecuteNonQuery();
            }

            return result;
        }

        /// <summary>
        /// 更新操作
        /// </summary>
        /// <param name="dic">更新字段添加到的字典</param>
        /// <param name="id">自增ID</param>
        /// <returns>1成功 0 失败</returns>
        public int Update(Dictionary<string, object> dic, int id)
        {
            int result = 0;

            if (dic != null && dic.Count > 0 && id > 0)
            {
                using (MySqlConnection con = new MySqlConnection(base.ConStr))
                {
                    MySqlCommand cmd = con.CreateCommand();
                    StringBuilder setsql = new StringBuilder();

                    foreach (var item in dic)
                    {
                        var field = item.Key;
                        var value = item.Value;

                        setsql.Append(field).Append("=").Append("?" + field).Append(',');
                        cmd.Parameters.AddWithValue("?" + field, value);
                    }

                    cmd.Parameters.AddWithValue("?ID", id);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = string.Format(" UPDATE {0} SET {1} WHERE {2}=?ID ", this.TableName, setsql.ToString().TrimEnd(','), this.AutoIdentityName);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }

        /// <summary>
        /// 保存实体信息
        /// </summary>
        /// <param name="t"></param>
        /// <param name="id">非自增ID</param>
        /// <returns></returns>
        public int Save(T t, int id)
        {
            int result = 0;

            using (MySqlConnection con = new MySqlConnection(base.ConStr))
            {
                try
                {
                    MySqlCommand cmd = con.CreateCommand();
                    PropertyInfo[] pInfo = t.GetType().GetProperties();
                    StringBuilder fileds = new StringBuilder();
                    StringBuilder values = new StringBuilder();

                    foreach (PropertyInfo item in pInfo)
                    {
                        if (item.Name.ToLower() == "f_dataext")
                        {
                            continue;
                        }

                        if (item.Name == this.AutoIdentityName)
                        {
                            string param = "?" + item.Name;
                            fileds.Append(item.Name).Append(",");
                            values.Append(param).Append(",");
                            cmd.Parameters.AddWithValue(param, id);
                        }
                        else
                        {
                            object value = item.GetValue(t, null);
                            if (value == null || value == "")
                            {
                                continue;
                            }
                            string param = "?" + item.Name;
                            fileds.Append(item.Name).Append(",");
                            values.Append(param).Append(",");
                            cmd.Parameters.AddWithValue(param, value);
                        }
                    }

                    string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2}) ", this.TableName, fileds.ToString().TrimEnd(','), values.ToString().TrimEnd(','));
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;

                    con.Open();
                    result = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    throw;
                }
                finally
                {
                    con.Close();
                }
            }

            return result;
        }
    }
}
