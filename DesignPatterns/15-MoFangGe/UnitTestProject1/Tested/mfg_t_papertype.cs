using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.Tested
{
    public partial class mfg_t_papertype : AutoIdentity<mfg_t_papertypeInfo>, Imfg_t_papertype
    {
        #region 抽象类实现

        /// <summary>
        /// 自增ID
        /// </summary>
        public override string AutoIdentityName
        {
            get { return "F_ID"; }
        }

        /// <summary>
        /// 表名
        /// </summary>
        protected override string TableName
        {
            get
            {
                return "mfg_t_papertype";
            }
        }

        #endregion

        #region 使用主键对表进行操作

        public mfg_t_papertypeInfo GetInfoByPK(string pkID)
        {
            mfg_t_papertypeInfo info = new mfg_t_papertypeInfo();

            using (MySqlConnection con = new MySqlConnection(base.ConStr))
            {
                MySqlCommand cmd = con.CreateCommand();
                string sql = string.Format("select * from {0} where {1}=?pkID ", this.TableName, this.AutoIdentityName);
                cmd.Parameters.AddWithValue("?pkID", pkID);
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

        #endregion

        #region 实体操作

        public override mfg_t_papertypeInfo GetInfo(System.Data.IDataReader dr)
        {
            mfg_t_papertypeInfo c = new mfg_t_papertypeInfo()
            {
                f_id = int.Parse(dr["f_id"].ToString()),
                f_name = dr["f_name"].ToString(),
                f_class = dr["f_class"].ToString()
            };
            return c;
        }

        #endregion
    }
}
