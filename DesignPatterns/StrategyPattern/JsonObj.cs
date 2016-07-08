using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrategyPattern
{
    class JsonObj
    {
    }

    /// <summary>
    /// JSON序列化策略
    /// </summary>
    public interface IJsonStrategy
    {
        /// <summary>
        /// 对象转JSON
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        string Serialize(object obj);

        /// <summary>
        /// JSON转对象
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="json">字符串</param>
        /// <returns></returns>
        T Deserialize<T>(string json) where T : class;
    }

    public class FastJson : IJsonStrategy
    {
        public T Deserialize<T>(string json) where T : class
        {
            throw new NotImplementedException();
        }

        public string Serialize(object obj)
        {
            throw new NotImplementedException();
        }
    }

    public class NewtonsoftJson : IJsonStrategy
    {
        public T Deserialize<T>(string json) where T : class
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public string Serialize(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
    }

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
