using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Json
{
    /// <summary>
    /// 默认使用Newtonsoft.Json进行序列化与反序列化
    /// </summary>
    public class JsonHelper
    {
        private static readonly JsonContext Context;

        static JsonHelper()
        {
            Context = new JsonContext(new NewtonsoftJson());
        }
        /// <summary>
        /// JSON序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Serialize(object obj)
        {
            return Context.Serialize(obj);
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public T Deserialize<T>(string json) where T : class
        {
            return Context.Deserialize<T>(json);
        }
    }
}
