using Newtonsoft.Json;
/***************************************************************************** 
*        filename :JsonHelper 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.42000 
*        新建项输入的名称:   JsonHelper 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Common.Net.Helper 
*        文件名:             JsonHelper 
*        创建系统时间:       2016/2/1 15:47:42 
*        创建年份:           2016 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Common.Net.Helper
{
    public static class JsonHelper
    {
        /// <summary>
        /// Json字符串转List<T>
        /// 例：List<QueUpdateModel> model = JsonHelper.JsonToObj(pa, typeof(List<QueUpdateModel>)) as List<QueUpdateModel>;
        /// 注意：对象的属性要使用 [DataContract]  [DataMember]
        /// </summary>
        /// <param name="jsonString">Json字符串</param>
        /// <param name="t">List<T></param>
        /// <returns></returns>
        public static Object JsonToObj(string jsonString, Type t)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(t);
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
                {
                    return serializer.ReadObject(ms);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Json序列化,用于发送到客户端
        /// </summary>
        public static string ConvertToJson(this object item)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(item.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, item);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// Json反序列化,用于接收客户端Json后生成对应的对象
        /// </summary>
        public static T FromJsonTo<T>(this string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                T jsonObject = (T)ser.ReadObject(ms);
                return jsonObject;
            }
        }

        /// <summary>
        /// 格式化json
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static string FormartJson(this string jsonString)
        {
            //格式化json字符串
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(jsonString);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return jsonString;
            }
        }

        /// <summary> 
        /// 返回对象序列化 
        /// </summary> 
        /// <param name="obj">源对象</param> 
        /// <returns>json数据</returns> 
        public static string ToJson(object obj)
        {
            JavaScriptSerializer serialize = new JavaScriptSerializer();
            return serialize.Serialize(obj);
        }
        /// <summary> 
        /// 控制深度 
        /// </summary> 
        /// <param name="obj">源对象</param> 
        /// <param name="recursionDepth">深度</param> 
        /// <returns>json数据</returns> 
        public static string ToJson(object obj, int recursionDepth)
        {
            JavaScriptSerializer serialize = new JavaScriptSerializer();
            serialize.RecursionLimit = recursionDepth;
            return serialize.Serialize(obj);
        }

        /// <summary> 
        /// DataTable转为json 
        /// </summary> 
        /// <param name="dt">DataTable</param> 
        /// <returns>json数据</returns> 
        public static string ToJson(this DataTable dt)
        {
            List<object> dic = new List<object>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc].ToString());
                }
                dic.Add(result);
            }
            return ToJson(dic);
        }

        #region ServiceStack
        /// <summary>
        /// 摘要：将对象序列化成Json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJosn(object obj)
        {
            return "";//ServiceStack.Text.JsonSerializer.SerializeToString(obj);
        }

        /// <summary>
        /// 摘要：将Json转成对象
        /// 注意：数据名称必须相同
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        //public static T ToObject<T>(string jsonStr)
        //{
        //    return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(jsonStr);
        //} 
        #endregion
    }
}
