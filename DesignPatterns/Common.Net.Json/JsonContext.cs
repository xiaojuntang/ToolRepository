using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Json
{
    /// <summary>
    /// Json上下文
    /// </summary>
    public class JsonContext
    {
        private readonly IJsonStrategy _strategy;

        public JsonContext(IJsonStrategy strategy)
        {
            this._strategy = strategy;
        }

        /// <summary>
        /// JSON序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Serialize(object obj)
        {
            return _strategy.Serialize(obj);
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public T Deserialize<T>(string json) where T : class
        {
            return _strategy.Deserialize<T>(json);
        }
    }
}
