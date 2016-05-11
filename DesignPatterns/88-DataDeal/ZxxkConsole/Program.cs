using Common.Net.DbProvider;
using Common.Net.Func;
using Common.Net.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZxxkConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            T004(0);

            Console.ReadLine();
        }

        private static void T001()
        {
            //MetaData M = new MetaData();
            //M.stage = new Hashtable();
            //M.stage.Add("1000", "1000");
            //M.stage.Add("1001", "1001");
            //M.stage.Add("1002", "1002");
            //string josnstr = JsonHelper.ConvertToJson(M);




            string path = Application.StartupPath;
            string content = FileUtil.ReadFile(Application.StartupPath + "\\metaData.json", Encoding.UTF8, false);
            TA aa = Newtonsoft.Json.JsonConvert.DeserializeObject<TA>(content);
            if (aa != null)
            {
                Dictionary<string, string> dicStage = aa.metaData.stage;
                Dictionary<string, string> dicCourse = aa.metaData.course;
                Dictionary<string, string> dicVersion = aa.metaData.version;
                Dictionary<string, string> dicTextbook = aa.metaData.textbook;
                Add001(dicStage, "stage");
                Add001(dicCourse, "course");
                Add001(dicVersion, "version");
                Add001(dicTextbook, "textbook");

            }
        }

        public static void Add001(Dictionary<string, string> collection, string type)
        {
            foreach (var item in collection)
            {
                string key = item.Key;
                string value = item.Value;
                string sql = $@"INSERT INTO [HomeWork].[dbo].[HW_TencentMata] ([MetaType] ,[MetaKey] ,[MetaValue])VALUES('{type}','{key}','{value}')";
                //object result = SQLHelper.ExecuteScalar(sql, DataBase.ZYTConnString);
            }
        }

        private static List<HW_ZujuanNodes> T002(int pId)
        {
            List<HW_ZujuanNodes> models = new List<HW_ZujuanNodes>();
            string sql = $@"select * from HW_ZujuanNodes where ParentNodeID= {pId}; ";
            SQLHelper.FindList(sql, (a) =>
            {
                if (a.HasRows)
                {
                    while (a.Read())
                    {
                        HW_ZujuanNodes model = new HW_ZujuanNodes();
                        model.ID = a.GetInt32(0);
                        model.NodeID = a.GetInt32(1);
                        model.NodeName = a.GetString(2);
                        model.ParentNodeID = a.GetInt32(3);
                        model.OrderNumber = a.GetInt32(4);
                        models.Add(model);
                    }
                }
            }, null, CommandType.Text, DataBase.ZYTConnString);
            return models;
        }

        private static void T003(int pId)
        {
            List<HW_ZujuanNodes> D1 = T002(pId);
            if (D1 != null)
            {
                D1.ForEach(m =>
                {
                    string str = string.Format("{0}-{1}", m.NodeID, m.NodeName);
                    Console.WriteLine(str);
                    T003(m.NodeID);
                });
            }
        }

        private static void T004(int pId)
        {
            En01 entity = new En01();
            #region 高中
            List<HW_ZujuanNodes> D1 = T002(pId);
            if (D1 != null)
            {
                D1.ForEach(m =>
                {
                    string str = string.Format("{0}-{1}", m.NodeID, m.NodeName);
                    Console.WriteLine(str);
                    #region 语文
                    List<HW_ZujuanNodes> D2 = T002(m.NodeID);
                    if (D2 != null)
                    {
                        D2.ForEach(m1 =>
                        {
                            string str1 = string.Format("----{0}-{1}", m1.NodeID, m1.NodeName);
                            Console.WriteLine(str1);
                            #region 人教版
                            List<HW_ZujuanNodes> D3 = T002(m1.NodeID);
                            if (D3 != null)
                            {
                                D3.ForEach(m3 =>
                                {
                                    string str3 = string.Format("--------{0}-{1}", m3.NodeID, m3.NodeName);
                                    Console.WriteLine(str3);
                                    #region 必修一
                                    List<HW_ZujuanNodes> D4 = T002(m3.NodeID);
                                    Dictionary<string, string[]> dic4 = new Dictionary<string, string[]>();
                                    if (D4 != null)
                                    {
                                        D4.ForEach(m4 =>
                                        {
                                            string str4 = string.Format("------------{0}-{1}", m4.NodeID, m4.NodeName);
                                            Console.WriteLine(str4);
                                        });
                                        //dic4.Add("");
                                    }
                                    #endregion
                                });
                            }
                            #endregion
                        });
                    }
                    #endregion
                    //entity.configTree.ML.Add(m.NodeID.ToString(),)
                });
            }
            #endregion
        }

        private static void T005()
        {
            En01 model = new En01();
            model.configTree = new ConfigTree();
            model.configTree.ML = new Dictionary<string, Dictionary<string, Dictionary<string, string[]>>>();
            model.configTree.ML.Add("1001", new Dictionary<string, Dictionary<string, string[]>>());
        }
    }

    public class HW_ZujuanNodes
    {
        public int ID { get; set; }
        public int NodeID { get; set; }
        public string NodeName { get; set; }
        public int ParentNodeID { get; set; }
        public int OrderNumber { get; set; }
    }

    public class TA
    {
        public MetaData metaData { get; set; }

    }

    public class MetaData
    {
        public Dictionary<string, string> stage { get; set; }
        public Dictionary<string, string> course { get; set; }
        public Dictionary<string, string> version { get; set; }
        public Dictionary<string, string> textbook { get; set; }

    }

    public class En01
    {
        public ConfigTree configTree { get; set; }
    }

    public class ConfigTree
    {
        public Dictionary<string, Dictionary<string, Dictionary<string, string[]>>> ML { get; set; }
    }
}
