using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Mfg.Resource.Comm
{
    /// <summary>
    /// 解析JSON，仿Javascript风格
    /// </summary>
    public static class JSON
    {
        /// <summary>
        /// 解析Json
        /// </summary>
        /// <typeparam name="T">任意类型</typeparam>
        /// <param name="jsonString">json串</param>
        /// <returns></returns>
        public static T Parse<T>(string jsonString)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
            }
        }

        /// <summary>
        /// 转换Json
        /// </summary>
        /// <param name="jsonObject">json串</param>
        /// <returns>string</returns>
        public static string Stringify(object jsonObject)
        {
            using (var ms = new MemoryStream())
            {
                new DataContractJsonSerializer(jsonObject.GetType()).WriteObject(ms, jsonObject);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}
