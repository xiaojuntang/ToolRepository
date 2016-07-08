using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Json
{
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
}
