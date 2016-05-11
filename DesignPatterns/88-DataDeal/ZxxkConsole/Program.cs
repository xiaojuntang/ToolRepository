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
                        model.OrderNumber = a.IsDBNull(4) ? 0 : a.GetInt32(4);
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
            List<HW_ZujuanNodes> GradeNodes = T002(pId);
            if (GradeNodes != null)
            {
                GradeNodes.ForEach(m =>
                {
                    string str = string.Format("1{0}-{1}", m.NodeID, m.NodeName);
                    Console.WriteLine(str);
                    LogHelper.Debug(str);
                    #region 语文
                    List<HW_ZujuanNodes> SubjectNodes = T002(m.NodeID);
                    if (SubjectNodes != null)
                    {
                        SubjectNodes.ForEach(n =>
                        {
                            string str1 = string.Format("2----{0}-{1}", n.NodeID, n.NodeName);
                            Console.WriteLine(str1); LogHelper.Debug(str1);
                            #region 人教版
                            List<HW_ZujuanNodes> VerNodes = T002(n.NodeID);
                            if (VerNodes != null)
                            {
                                VerNodes.ForEach(x =>
                                {
                                    string str3 = string.Format("3--------{0}-{1}", x.NodeID, x.NodeName);
                                    Console.WriteLine(str3); LogHelper.Debug(str3);
                                    #region 必修一
                                    List<HW_ZujuanNodes> MaterialNodes = T002(x.NodeID);
                                    if (MaterialNodes != null)
                                    {
                                        MaterialNodes.ForEach(y =>
                                        {
                                            string str4 = string.Format("4------------{0}-{1}", y.NodeID, y.NodeName);
                                            Console.WriteLine(str4); LogHelper.Debug(str4);
                                            #region 知识点
                                            List<HW_ZujuanNodes> KonlgNodes = T002(y.NodeID);
                                            if (KonlgNodes != null)
                                            {
                                                KonlgNodes.ForEach(o =>
                                                {
                                                    string str5 = string.Format("5----------------{0}-{1}", o.NodeID, o.NodeName);
                                                    Console.WriteLine(str5); LogHelper.Debug(str5);
                                                    #region 66666666666666666666666666666666666666
                                                    List<HW_ZujuanNodes> GGGGG = T002(o.NodeID);
                                                    if (GGGGG != null)
                                                    {
                                                        GGGGG.ForEach(k =>
                                                        {
                                                            string str6 = string.Format("6---------------------{0}-{1}", k.NodeID, k.NodeName);
                                                            Console.WriteLine(str6); LogHelper.Debug(str6);
                                                            #region 7777777777777777777777777777777777
                                                            List<HW_ZujuanNodes> TTTTTT = T002(k.NodeID);
                                                            if (TTTTTT != null)
                                                            {
                                                                TTTTTT.ForEach(g =>
                                                                {
                                                                    string str7 = string.Format("7--------------------------{0}-{1}", g.NodeID, g.NodeName);
                                                                    Console.WriteLine(str7); LogHelper.Debug(str7);
                                                                });
                                                            }
                                                            #endregion
                                                        });
                                                    }
                                                    #endregion
                                                });
                                            }
                                            #endregion
                                        });
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
