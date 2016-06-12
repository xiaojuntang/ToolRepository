using Common.Net.Core;
using Common.Net.DbProvider;
using Common.Net.Func;
using Common.Net.Helper;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace ZxxkConsole
{
    class Program
    {
        private static readonly ConcurrentDictionary<string, object> LockDic = new ConcurrentDictionary<string, object>();
        //public List<SecPoint> BookFrequency(string sub, string bookId)
        //{
        //    CacheHelper.Get

        //    DCaheHelper caheHelper = new DCaheHelper();
        //    string key = sub + bookId;
        //    List<SecPoint> list = caheHelper.GetCacheObject(key) as List<SecPoint>;
        //    if (list == null)
        //    {
        //        object lockObj;
        //        if (!LockDic.TryGetValue(key, out lockObj))
        //        {
        //            lockObj = new object();
        //            LockDic.TryAdd(key, lockObj);
        //        }
        //        lock (lockObj)
        //        {
        //            list = caheHelper.GetCacheObject(key) as List<SecPoint>;
        //            if (list == null)
        //            {
        //                DalSecPoint dal = new DalSecPoint();
        //                list = dal.BookFrequency(sub, bookId);
        //                caheHelper.AddObjectToCahce(key, list, addCacheEnum.recover, timeExpirationEnum.absolute,
        //                    60 * 60 * 24);
        //            }
        //        }
        //    }
        //    return list;
        //}
        static void Main(string[] args)
        {

            T001();
            //86e5dbe2774411d434142357e2b1b507





            //Dictionary<string, string> papramters = new Dictionary<string, string>();
            //papramters.Add("appId", "1105306939");
            //papramters.Add("openId", "3CDAEFAFB4A904372BF85493354EF3B1");
            //papramters.Add("openKey", "0CBE297D89432333427DB64B5647CE04");
            //papramters.Add("stage", "1002_2002_3001_43321");
            //papramters.Add("examId", "");
            //papramters.Add("groupId", "");
            //papramters.Add("groupStage", "");
            //papramters.Add("courseName", "");
            //papramters.Add("ts", "1463049864");


            //IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(papramters);
            //IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();
            //StringBuilder query = new StringBuilder();
            //while (dem.MoveNext())
            //{
            //    string key = dem.Current.Key;
            //    string value = dem.Current.Value;
            //    if (!string.IsNullOrWhiteSpace(key))
            //    {
            //        query.Append(key).Append("=").Append(value).Append("&");
            //    }
            //}
            //string appKey = "sQvjcCFFIh1ZSBoV";
            //string ddddd = query.ToString().TrimEnd('&') + appKey;



            //string a1 = "appId=1105306939&openId=3CDAEFAFB4A904372BF85493354EF3B1&openKey=0CBE297D89432333427DB64B5647CE04&stage=1002_2002_3001_43321&examId=&groupId=&groupStage=&courseName=&ts=1461463049864&sig=86e5dbe2774411d434142357e2b1b507";

            //string a2 = "appId=1105306939&openId=3CDAEFAFB4A904372BF85493354EF3B1&openKey=0CBE297D89432333427DB64B5647CE04&stage=1002_2002_3001_43321&examId=&groupId=&groupStage=&courseName=&ts=1461463049864";

            //var gg = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(ddddd, "MD5");
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
                string sql = $@"INSERT INTO [TX_TencentMata] ([MetaType] ,[MetaKey] ,[MetaValue])VALUES('{type}','{key}','{value}')";
                object result = SQLHelper.ExecuteScalar(sql, DataBase.ZYTConnString);
            }
        }

        //private static List<HW_ZujuanNodes> TA68()
        //{
        //    List<HW_ZujuanNodes> models = new List<HW_ZujuanNodes>();
        //    string sql = $@"select * from HW_ZujuanNodes where ParentNodeID= {pId}; ";
        //    SQLHelper.FindList(sql, (a) =>
        //    {
        //        if (a.HasRows)
        //        {
        //            while (a.Read())
        //            {
        //                HW_ZujuanNodes model = new HW_ZujuanNodes();
        //                model.ID = a.GetInt32(0);
        //                model.NodeID = a.GetInt32(1);
        //                model.NodeName = a.GetString(2);
        //                model.ParentNodeID = a.GetInt32(3);
        //                model.OrderNumber = a.IsDBNull(4) ? 0 : a.GetInt32(4);
        //                model.TencentID = a.IsDBNull(5) ? "" : a.GetString(5);
        //                models.Add(model);
        //            }
        //        }
        //    }, null, CommandType.Text, DataBase.ZYTConnString);
        //    return models;
        //}



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
                        model.TencentID = a.IsDBNull(5) ? "" : a.GetString(5);
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
            List<HW_ZujuanNodes> All = new List<HW_ZujuanNodes>();
            #region 高中
            List<HW_ZujuanNodes> GradeNodes = T002(pId);
            if (GradeNodes != null)
            {
                GradeNodes.ForEach(m =>
                {
                    string str = string.Format("1{0}-{1}", m.NodeID, m.NodeName);
                    Console.WriteLine(str);
                    LogHelper.Debug(str);
                    All.Add(m);
                    #region 语文
                    List<HW_ZujuanNodes> SubjectNodes = T002(m.NodeID);
                    if (SubjectNodes != null)
                    {
                        SubjectNodes.ForEach(n =>
                        {
                            string str1 = string.Format("2----{0}-{1}", n.NodeID, n.NodeName);
                            Console.WriteLine(str1); LogHelper.Debug(str1);
                            All.Add(n);
                            #region 人教版
                            List<HW_ZujuanNodes> VerNodes = T002(n.NodeID);
                            if (VerNodes != null)
                            {
                                VerNodes.ForEach(x =>
                                {
                                    string str3 = string.Format("3--------{0}-{1}", x.NodeID, x.NodeName);
                                    Console.WriteLine(str3); LogHelper.Debug(str3);
                                    All.Add(x);
                                    #region 必修一
                                    List<HW_ZujuanNodes> MaterialNodes = T002(x.NodeID);
                                    if (MaterialNodes != null)
                                    {
                                        MaterialNodes.ForEach(y =>
                                        {
                                            string str4 = string.Format("4------------{0}-{1}", y.NodeID, y.NodeName);
                                            Console.WriteLine(str4); LogHelper.Debug(str4);
                                            All.Add(y);
                                            //#region 知识点
                                            //List<HW_ZujuanNodes> KonlgNodes = T002(y.NodeID);
                                            //if (KonlgNodes != null)
                                            //{
                                            //    KonlgNodes.ForEach(o =>
                                            //    {
                                            //        string str5 = string.Format("5----------------{0}-{1}", o.NodeID, o.NodeName);
                                            //        Console.WriteLine(str5); LogHelper.Debug(str5);
                                            //        #region 66666666666666666666666666666666666666
                                            //        List<HW_ZujuanNodes> GGGGG = T002(o.NodeID);
                                            //        if (GGGGG != null)
                                            //        {
                                            //            GGGGG.ForEach(k =>
                                            //            {
                                            //                string str6 = string.Format("6---------------------{0}-{1}", k.NodeID, k.NodeName);
                                            //                Console.WriteLine(str6); LogHelper.Debug(str6);
                                            //                #region 7777777777777777777777777777777777
                                            //                List<HW_ZujuanNodes> TTTTTT = T002(k.NodeID);
                                            //                if (TTTTTT != null)
                                            //                {
                                            //                    TTTTTT.ForEach(g =>
                                            //                    {
                                            //                        string str7 = string.Format("7--------------------------{0}-{1}", g.NodeID, g.NodeName);
                                            //                        Console.WriteLine(str7); LogHelper.Debug(str7);
                                            //                    });
                                            //                }
                                            //                #endregion
                                            //            });
                                            //        }
                                            //        #endregion
                                            //    });
                                            //}
                                            //#endregion
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

        /// <summary>
        /// 查询学段-科目-版本-课本 4级目录
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        private static List<HW_ZujuanNodes> T006(int pId)
        {
            List<HW_ZujuanNodes> allNode = new List<HW_ZujuanNodes>();
            #region 高中
            List<HW_ZujuanNodes> GradeNodes = T002(pId);
            if (GradeNodes != null)
            {
                GradeNodes.ForEach(m =>
                {
                    allNode.Add(m);
                    #region 语文
                    List<HW_ZujuanNodes> SubjectNodes = T002(m.NodeID);
                    if (SubjectNodes != null)
                    {
                        SubjectNodes.ForEach(n =>
                        {
                            allNode.Add(n);
                            #region 人教版
                            List<HW_ZujuanNodes> VerNodes = T002(n.NodeID);
                            if (VerNodes != null)
                            {
                                VerNodes.ForEach(x =>
                                {
                                    allNode.Add(x);
                                    #region 必修一
                                    List<HW_ZujuanNodes> MaterialNodes = T002(x.NodeID);
                                    if (MaterialNodes != null)
                                    {
                                        MaterialNodes.ForEach(y =>
                                        {
                                            allNode.Add(y);
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
            return allNode;
        }


        private static List<HW_ZujuanNodes> GetT006()
        {
            string cacheKey = "C:J:G:001";
            List<HW_ZujuanNodes> result = CacheHelper.Get<List<HW_ZujuanNodes>>(cacheKey);
            if (result == null)
            {
                List<HW_ZujuanNodes> allNode = T006(0);
                object lockObj;
                if (!LockDic.TryGetValue(cacheKey, out lockObj)) {
                    lockObj = new object();
                    LockDic.TryAdd(cacheKey, lockObj);
                }
                lock (lockObj)
                {
                    CacheHelper.Add(cacheKey, allNode, 24 * 60 * 60 * 1000);
                }
            }
            return result;
        }
    }

    public class HW_ZujuanNodes
    {
        public int ID { get; set; }
        public int NodeID { get; set; }
        public string NodeName { get; set; }
        public int ParentNodeID { get; set; }
        public int OrderNumber { get; set; }

        public string TencentID { get; set; }
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


    static class Util
    {
        #region HttpGet
        //public static string HttpGet(string url)
        //{
        //    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
        //    request.Method = "GET";
        //    request.Accept = "*/*";
        //    request.Timeout = 150000;
        //    request.AllowAutoRedirect = false;

        //    WebResponse response = null;
        //    string responseStr = null;

        //    try
        //    {
        //        response = request.GetResponse();

        //        if (response != null)
        //        {
        //            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
        //            responseStr = reader.ReadToEnd();
        //            reader.Close();
        //        }
        //    }
        //    catch (WebException)
        //    {
        //        throw;
        //    }
        //    catch (Exception)
        //    {
        //        //其他错误，写入日志或者忽略
        //    }
        //    finally
        //    {
        //        request = null;
        //        response = null;
        //    }

        //    return responseStr;
        //}
        #endregion

        #region SignHelper
        public static string CreateOauthSignature(Dictionary<string, string> dic, string url, string method, string consumer_secret, string oauth_token_secret)
        {
            string HashKey = consumer_secret + "&" + oauth_token_secret;
            string OauthSignature = "";
            string Paras = "";
            string BaseString = method + "&" + RFC3986_UrlEncode(url) + "&";
            Paras = RFC3986_UrlEncode(dic.OrderBy(x => x.Key).ToDictionary(x => x.Key, y => y.Value).ToQueryString());
            BaseString += Paras;

            using (HMACSHA1 crypto = new HMACSHA1())
            {
                crypto.Key = Encoding.ASCII.GetBytes(HashKey);
                OauthSignature = Convert.ToBase64String(crypto.ComputeHash(Encoding.ASCII.GetBytes(BaseString)));
            }

            return OauthSignature;
        }
        public static string GetTimeStamp(bool isUtc)
        {
            DateTime NowTime = isUtc ? DateTime.UtcNow : DateTime.Now;
            return ((NowTime.Ticks - (new DateTime(1970, 1, 1)).Ticks) / 10000000).ToString();
        }
        //public static string GetNonce()
        //{
        //    return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(new Random((int)DateTime.Now.ToBinary()).Next(0, int.MaxValue).ToString().Trim(), "md5").ToLower();
        //}
        public static string RFC3986_UrlEncode(string input)
        {
            string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
            StringBuilder result = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(input);

            foreach (byte symbol in byStr)
            {
                if (unreservedChars.IndexOf((char)symbol) != -1)
                {
                    result.Append((char)symbol);
                }
                else
                {
                    result.Append('%' + Convert.ToString((char)symbol, 16).ToUpper());
                }
            }

            return result.ToString();
        }
        #endregion

        #region Extends
        public static string ToQueryString(this IDictionary<string, string> dictionary)
        {
            var sb = new StringBuilder();
            foreach (var key in dictionary.Keys)
            {
                var value = dictionary[key];
                if (value != null)
                {
                    sb.Append(key + "=" + value + "&");
                }
            }
            return sb.ToString().TrimEnd('&');
        }
        #endregion
    }
}
