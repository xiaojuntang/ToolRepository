using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Climb.Utility.SystemExt
{
    /// <summary>
    /// 序列化辅助类 扩展
    /// </summary>
    public class SerializeExt
    {
        #region BinaryFormatter序列化
        /// <summary>
        /// BinaryFormatter序列化
        /// </summary>
        /// <param name="item">对象</param>
        public string ToBinary<T>(T item)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, item);
                ms.Position = 0;
                byte[] bytes = ms.ToArray();
                StringBuilder sb = new StringBuilder();
                foreach (byte bt in bytes)
                {
                    sb.Append(string.Format("{0:X2}", bt));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// BinaryFormatter反序列化
        /// </summary>
        /// <param name="str">字符串序列</param>
        public T FromBinary<T>(string str)
        {
            int intLen = str.Length / 2;
            byte[] bytes = new byte[intLen];
            for (int i = 0; i < intLen; i++)
            {
                int ibyte = Convert.ToInt32(str.Substring(i * 2, 2), 16);
                bytes[i] = (byte)ibyte;
            }
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return (T)formatter.Deserialize(ms);
            }
        }
        #endregion

        #region json序列化 和json 反序列化
        // 从一个对象信息生成Json串
        /// <summary>
        /// 将对象object序列化成josn
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns>返回json 字符串</returns>
        public static string ObjectToJson(object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        // 从一个Json串生成对象信息
        /// <summary>
        /// 反将 json反序列化成对象 object
        /// </summary>
        /// <param name="jsonString">json字符串</param>
        /// <param name="obj">对象</param>
        /// <returns>返回对象</returns>
        public static object JsonToObject(string jsonString, object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                return serializer.ReadObject(mStream);
            }

        }
        #endregion

        #region 将对象序列化成xml文件
        /// <summary>
        /// xml文件存放路径
        /// </summary>
        /// <param name="filePath">xml路径</param>
        /// <param name="sourceObj">需要序列化的对象object</param>
        /// <param name="xmlRootName">xml根的名字</param>
        public static void SerializeObjectToXml(string filePath, object sourceObj, string xmlRootName)
        {
            if (string.IsNullOrEmpty(filePath) || sourceObj == null) return;
            Type type = sourceObj.GetType();
            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
            using (StreamWriter writer = new StreamWriter(fs))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                XmlSerializer xmlSerializer = new XmlSerializer(type);
                xmlSerializer.Serialize(writer, sourceObj, ns);
            }
        }
        #endregion

        #region 将xml 文件反序列化对象
        /// <summary>
        /// 将xml 文件反序列化对象
        /// </summary>
        /// <param name="filePath">xml的路径</param>
        /// <param name="type">类型T</param>
        /// <returns>成功返回对象</returns>
        public static object LoadFromXml(string filePath, Type type)
        {
            object result;
            if (!File.Exists(filePath)) return null;
            using (StreamReader reader = new StreamReader(filePath))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(type);
                result = xmlSerializer.Deserialize(reader);
            }
            return result;
        }
        #endregion

        #region 对象T 序列化
        /// <summary>  
        /// JSON序列化
        /// </summary>
        public static string JsonSerializer<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                ser.WriteObject(ms, t);
                string jsonString = Encoding.UTF8.GetString(ms.ToArray());
                //替换Json的Date字符串
                const string p = @"\\/Date\((\d+)\+\d+\)\\/";
                MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
                Regex reg = new Regex(p);
                jsonString = reg.Replace(jsonString, matchEvaluator);
                return jsonString;
            }
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {
            //将"yyyy-MM-dd HH:mm:ss"格式的字符串转为"\/Date(1294499956278+0800)\/"格式
            const string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                T obj = (T)ser.ReadObject(ms);
                return obj;
            }
        }
        #endregion

        #region 时间序列化转换
        /// <summary>
        /// 将Json序列化的时间由/Date(1294499956278+0800)转为字符串
        /// </summary>
        private static string ConvertJsonDateToDateString(Match m)
        {
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            var result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }

        /// <summary>
        /// 将时间字符串转为Json时间
        /// </summary>
        private static string ConvertDateStringToJsonDate(Match m)
        {
            var dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            var ts = dt - DateTime.Parse("1970-01-01");
            var result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            return result;
        }

        #endregion
    }
}
