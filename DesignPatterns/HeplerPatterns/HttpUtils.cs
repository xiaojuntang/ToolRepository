//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web;
//using System.Xml.Serialization;

//namespace HeplerPatterns
//{
//    public class HttpUtils
//    {
//        public static string Post(string requestUrl, SortedDictionary<string, object> paramDic)
//        {
//            string str1 = string.Empty;
//            WebRequest webRequest = WebRequest.Create(ConfigUtils.Url + requestUrl);
//            webRequest.Method = "POST";
//            Dictionary<string, string> dictionary = new Dictionary<string, string>();
//            webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
//            webRequest.Timeout = 50000;
//            StringBuilder stringBuilder1 = new StringBuilder();
//            StringBuilder stringBuilder2 = new StringBuilder();
//            foreach (KeyValuePair<string, object> keyValuePair in paramDic)
//            {
//                string key = keyValuePair.Key;
//                object obj = keyValuePair.Value;
//                if (obj != null)
//                {
//                    string str2 = HttpUtility.UrlEncode(obj.ToString().Trim(), Encoding.UTF8);
//                    stringBuilder1.Append(string.Concat(new object[4]
//                    {
//            (object) key,
//            (object) "=",
//            obj,
//            (object) "&"
//                    }));
//                    stringBuilder2.Append(key + "=" + str2 + "&");
//                }
//            }
//            if (!string.IsNullOrEmpty(stringBuilder2.ToString()))
//                str1 = stringBuilder2.ToString().Substring(0, stringBuilder2.Length - 1);
//            stringBuilder1.Append("secretkey=" + ConfigUtils.Secretkey);
//            string str3 = SignUtils.Md5(stringBuilder1.ToString());
//            webRequest.Headers.Add("signature", str3);
//            webRequest.Headers.Add("apikey", ConfigUtils.Apikey);
//            StreamWriter streamWriter1 = StreamWriter.Null;
//            if (!string.IsNullOrEmpty(str1))
//            {
//                StreamWriter streamWriter2 = new StreamWriter(webRequest.GetRequestStream());
//                streamWriter2.Write(str1);
//                streamWriter2.Close();
//            }
//            string str4 = string.Empty;
//            WebResponse response;
//            try
//            {
//                response = webRequest.GetResponse();
//            }
//            catch (WebException ex)
//            {
//                response = ex.Response;
//            }
//            if (response != null)
//            {
//                StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
//                str4 = streamReader.ReadToEnd();
//                response.Close();
//                streamReader.Dispose();
//            }
//            return str4;
//        }

//        public static T Post<T>(string requestUrl, SortedDictionary<string, object> paramsDic, T type)
//        {
//            string str = HttpUtils.Post(requestUrl, paramsDic);
//            if (string.IsNullOrEmpty(str))
//                return type;
//            try
//            {
//                return JsonConvert.DeserializeObject<T>(str);
//            }
//            catch (InvalidOperationException ex)
//            {
//                return type;
//            }
//        }



//        //  public static DataResponse<StudentDetail> QueryStudentByClassIdAndSn(int classId, string sNumber)
//        //  {
//        //      return HttpUtils.Post<DataResponse<StudentDetail>>("/student/query-student-by-classid-and-sn", new SortedDictionary<string, object>()
//        //{
//        //  {
//        //    "classid",
//        //    (object) classId
//        //  },
//        //  {
//        //    "snumber",
//        //    (object) sNumber
//        //  }
//        //}, new DataResponse<StudentDetail>());
//        //  }



//        //  public static Response AddTeacherTitle(int userId, string titleIds)
//        //  {
//        //      return HttpUtils.Post<Response>("/teachertitle/insert-teacher-title", new SortedDictionary<string, object>()
//        //{
//        //  {
//        //    "userId",
//        //    (object) userId
//        //  },
//        //  {
//        //    "titleids",
//        //    (object) titleIds
//        //  }
//        //}, new Response());
//        //  }

//        //  public static ListResponse<SchoolTitle> QueryTitleByProduct(string product)
//        //  {
//        //      return HttpUtils.Post<ListResponse<SchoolTitle>>("/school/query-title-by-product", new SortedDictionary<string, object>()
//        //{
//        //  {
//        //    "product",
//        //    (object) product
//        //  }
//        //}, new ListResponse<SchoolTitle>());
//        //  }




//        //  public static PageResponse<School> QuerySchoolPageList(int? provinceId, int? cityId, int? districtId, string keyWord, string stage, int pageIndex, int pageSize)
//        //  {
//        //      if (pageIndex <= 0)
//        //          pageIndex = 1;
//        //      return HttpUtils.Post<PageResponse<School>>("/school/query-school-page-list", new SortedDictionary<string, object>()
//        //{
//        //  {
//        //    "provinceId",
//        //    (object) provinceId
//        //  },
//        //  {
//        //    "cityId",
//        //    (object) cityId
//        //  },
//        //  {
//        //    "districtId",
//        //    (object) districtId
//        //  },
//        //  {
//        //    "keyWord",
//        //    (object) keyWord
//        //  },
//        //  {
//        //    "stage",
//        //    (object) stage
//        //  },
//        //  {
//        //    "startIndex",
//        //    (object) ((pageIndex - 1) * pageSize)
//        //  },
//        //  {
//        //    "length",
//        //    (object) pageSize
//        //  }
//        //}, new PageResponse<School>());
//        //  }

//        public class SignUtils
//        {
//            public static string Md5(string decript)
//            {
//                MD5 md5 = MD5.Create();
//                byte[] bytes = Encoding.UTF8.GetBytes(decript);
//                byte[] hash = md5.ComputeHash(bytes);
//                StringBuilder stringBuilder = new StringBuilder();
//                foreach (byte num in hash)
//                    stringBuilder.Append(num.ToString("x2"));
//                md5.Clear();
//                return stringBuilder.ToString();
//            }

//            private static string Byte2HexString(byte[] bytes)
//            {
//                StringBuilder stringBuilder = new StringBuilder(bytes.Length * 2);
//                foreach (byte num in bytes)
//                {
//                    if (((int)num & (int)byte.MaxValue) < 16)
//                        stringBuilder.Append("0");
//                    stringBuilder.Append(((int)num & (int)byte.MaxValue).ToString());
//                }
//                return stringBuilder.ToString();
//            }
//        }

//        public class XmlHelper
//        {
//            public static string Serialize<T>(T entity)
//            {
//                StringBuilder sb = new StringBuilder();
//                using (TextWriter textWriter = (TextWriter)new StringWriter(sb))
//                    new XmlSerializer(typeof(T)).Serialize(textWriter, (object)entity);
//                return sb.ToString();
//            }

//            public static T DeSerialize<T>(string xmlString)
//            {
//                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
//                byte[] numArray = new byte[0];
//                using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
//                    return (T)xmlSerializer.Deserialize((Stream)memoryStream);
//            }
//        }
//    }
//}
