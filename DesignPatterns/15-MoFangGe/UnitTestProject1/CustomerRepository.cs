using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace UnitTestProject1
{
    public class CustomerRepository : TranslateInfo<Customer>, ICustomerRepository
    {
        private string constr = string.Empty;

        public CustomerRepository()
            : this(ConfigurationManager.AppSettings["ConStr"].ToString())
        {
        }
        public CustomerRepository(string connectionString)
        {
            constr = connectionString;
        }

        /// <summary>
        /// 表名
        /// </summary>
        protected string TableName
        {
            get
            {
                return "Customer";
            }
        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Customer FindSingle(Climb.Core.Query query)
        {
            Customer info = new Customer();

            // 执行操作
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                MySqlCommand cmd = con.CreateCommand();
                TranslateInto(query, cmd, this.TableName);

                con.Open();
                using (MySqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        info = GetInfo(read);
                    }
                }
            }

            return info;
        }
        /// <summary>
        /// 获取实体列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<Customer> FindList(Climb.Core.Query query)
        {
            List<Customer> list = new List<Customer>();

            // 执行操作
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                MySqlCommand cmd = con.CreateCommand();
                TranslateInto(query, cmd, this.TableName);

                con.Open();
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
        /// 分页获取实体列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Customer> FindList(Climb.Core.Query query, int index, int count)
        {
            List<Customer> list = new List<Customer>();

            // 执行操作
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                MySqlCommand cmd = con.CreateCommand();
                TranslateInto(query, cmd, this.TableName);
                cmd.CommandText = string.Format("{0} limit {1},{2}", cmd.CommandText, index, count);
                con.Open();
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
        /// 获取所有实体信息
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Customer> FindListAll(string orderBy)
        {
            List<Customer> list = new List<Customer>();

            // 执行操作
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                MySqlCommand cmd = con.CreateCommand();

                orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : " f_id ";
                string sql = string.Format("select * from {0} order by {1}", this.TableName, orderBy);
                cmd.CommandText = sql;
                con.Open();
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
        /// 获取个数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int FindCount(Climb.Core.Query query)
        {
            int result = 0;

            // 执行操作
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                MySqlCommand cmd = con.CreateCommand();

                string sqlQuery = TranslateWhere(query, cmd);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format("select count(1) from {0} {1} ", this.TableName, sqlQuery);
                con.Open();
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return result;
        }

        /// <summary>
        /// 实体获取
        /// </summary>
        /// <param name="read"></param>
        /// <returns></returns>
        private Customer GetInfo(IDataReader read)
        {
            Customer c = new Customer()
            {
                Id = int.Parse(read["id"].ToString()),
                Name = read["name"].ToString(),
                Status = int.Parse(read["Status"].ToString())
            };
            return c;
        }

        public int Update(Climb.Core.Update update)
        {
            int result = 0;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                try
                {
                    MySqlCommand cmd = con.CreateCommand();
                    TranslateInto(update, cmd, this.TableName);
                    con.Open();
                    result = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    throw ex;
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
