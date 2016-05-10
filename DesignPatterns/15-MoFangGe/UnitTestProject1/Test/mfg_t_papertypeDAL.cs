using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.Test
{
    public class mfg_t_papertypeDAL : TranslateInfo<mfg_t_papertype>, Imfg_t_papertypeDAL
    {
        private string constr = string.Empty;

        public mfg_t_papertypeDAL()
            : this(ConfigurationManager.AppSettings["ConStr"].ToString())
        {
        }
        public mfg_t_papertypeDAL(string connectionString)
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
                return "mfg_t_papertype";
            }
        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public mfg_t_papertype FindSingle(Climb.Core.Query query)
        {
            mfg_t_papertype info = new mfg_t_papertype();

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
        /// 实体获取
        /// </summary>
        /// <param name="read"></param>
        /// <returns></returns>
        private mfg_t_papertype GetInfo(IDataReader read)
        {
            mfg_t_papertype c = new mfg_t_papertype()
            {
                f_id = int.Parse(read["f_id"].ToString()),
                f_name = read["f_name"].ToString(),
                f_class = (read["f_class"].ToString())
            };
            return c;
        }

        public List<mfg_t_papertype> FindList(Climb.Core.Query query)
        {
            List<mfg_t_papertype> list = new List<mfg_t_papertype>();

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

        public List<mfg_t_papertype> FindList(Climb.Core.Query query, int index, int count)
        {
            throw new NotImplementedException();
        }

        public int Update(Climb.Core.Update update)
        {
            throw new NotImplementedException();
        }

       
    }
}
