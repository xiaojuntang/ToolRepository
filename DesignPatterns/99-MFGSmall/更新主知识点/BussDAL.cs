using Common.Net.DbProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 更新主知识点
{
    public class BussDAL
    {
        /// <summary>
        /// 获取试题
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public static List<Q1> GetQuestions(string subject)
        {
            List<Q1> list = new List<Q1>();
            string sqlText = string.Format("SELECT f_id,f_mainsec,f_secorder,f_subject FROM mfg_t_questionmanage WHERE f_subject='{0}' And f_isapp=1;", subject);
            MySqlHelper.FindList(sqlText, (a) =>
             {
                 if (a.HasRows)
                 {
                     while (a.Read())
                     {
                         if (a.GetInt32("f_mainsec") != 0)
                         {
                             if (!a.IsDBNull(2) && !string.IsNullOrEmpty(a.GetString(2)))
                             {
                                 Q1 q1 = new Q1();
                                 q1.f_id = a.GetInt32("f_id");
                                 q1.f_mainsec = a.GetInt32("f_mainsec");
                                 q1.f_secorder = a.GetString("f_secorder");
                                 q1.f_subject = a.GetString("f_subject");
                                 list.Add(q1);
                             }
                         }
                     }
                 }
             }, null, System.Data.CommandType.Text, DataBase.CResourceKF);
            return list;
        }
    }
}
