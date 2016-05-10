//Written By ssh.cn at 2015.7.3
using Mfg.Resouce.IDAL;
using Mfg.Resouce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestProject1;

namespace Mfg.Resouce.DAL
{
    /// <summary>
    /// 题型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class DALQuestionStyle<T> : AutoIdentity<QuestionStyle<T>>, IDALQuestionStyle<T>
    {
        #region 抽象类实现

        /// <summary>
        /// 自增ID
        /// </summary>
        public override string AutoIdentityName
        {
            get { return "f_id"; }
        }

        /// <summary>
        /// 表名
        /// </summary>
        protected override string TableName
        {
            get
            {
                return "mfg_t_style";
            }
        }

        #endregion

        #region 通过主键获取实体

        //public QuestionStyle<T> GetInfoByPK(string pkID)
        //{
        //    return base.GetInfoByPk(pkID);
        //}

        #endregion

        #region 实体操作
        public override QuestionStyle<T> GetInfo(System.Data.IDataReader dr)
        {
            QuestionStyle<T> info = new QuestionStyle<T>();
            if (dr["f_id"].ToString() != "")
            {
                info.f_id = int.Parse(dr["f_id"].ToString());
            }
            if (!DBNull.Value.Equals(dr["f_name"]))
            {
                info.f_name = dr["f_name"].ToString();
            }
            if (!DBNull.Value.Equals(dr["f_subject"]))
            {
                info.f_subject = dr["f_subject"].ToString();
            }
            if (!DBNull.Value.Equals(dr["f_blcass"]))
            {
                info.f_blcass = dr["f_blcass"].ToString();
            }
            if (dr["f_styleareid"].ToString() != "")
            {
                info.f_styleareid = int.Parse(dr["f_styleareid"].ToString());
            }
            if (!DBNull.Value.Equals(dr["f_styleareaname"]))
            {
                info.f_styleareaname = dr["f_styleareaname"].ToString();
            }

            return info;
        }

        #endregion



        public QuestionStyle<T> GetInfoByPK(string pkID)
        {
            throw new NotImplementedException();
        }
    }
}
