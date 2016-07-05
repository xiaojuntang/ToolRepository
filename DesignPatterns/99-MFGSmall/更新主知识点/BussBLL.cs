using Common.Net.DbProvider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 更新主知识点
{
    public class BussBLL
    {
        public static void GetQuestions(string subject)
        {
            List<Q1> list = BussDAL.GetQuestions(subject);
            if (list != null && list.Count > 0)
            {
                DataTable tmp = new DataTable("mfg_t_secque2");
                tmp.Columns.Add("f_secid", typeof(int));
                tmp.Columns.Add("f_qid", typeof(int));
                tmp.Columns.Add("f_state", typeof(short));
                tmp.Columns.Add("f_subject", typeof(string));
                foreach (Q1 item in list)
                {
                    DataRow dr = tmp.NewRow();
                    dr["f_secid"] = item.f_mainsec;
                    dr["f_qid"] = item.f_id;
                    dr["f_state"] = 1;
                    dr["f_subject"] = item.f_subject;
                    tmp.Rows.Add(dr);
                    string[] array = item.f_secorder.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (array.Length > 1)
                    {
                        //有从知识点
                        foreach (string csec in array)
                        {
                            int _sec = int.Parse(csec);
                            if (_sec != item.f_mainsec)
                            {
                                DataRow _dr = tmp.NewRow();
                                _dr["f_secid"] = _sec;
                                _dr["f_qid"] = item.f_id;
                                _dr["f_state"] = 0;
                                _dr["f_subject"] = item.f_subject;
                                tmp.Rows.Add(_dr);
                            }
                        }
                    }
                    if (tmp.Rows.Count >= 30000)
                    {
                        int total = MySqlHelper.BulkInsert(tmp, DataBase.CResourceKF);
                        if (total > 0 && total == tmp.Rows.Count)
                        {
                            Console.WriteLine("[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "1111入库");
                            tmp.Clear();
                        }
                    }
                }

                if (tmp.Rows.Count > 0)
                {
                    int total = MySqlHelper.BulkInsert(tmp, DataBase.CResourceKF);
                    if (total > 0 && total == tmp.Rows.Count)
                    {
                        Console.WriteLine("[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "入库");
                        tmp.Clear();
                    }
                }
            }
        }
    }
}
