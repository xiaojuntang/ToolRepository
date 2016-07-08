using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Json
{
    /// <summary>
    /// 引用组件：System.Web.Extensions（微软自带了，引用下就好）
    /// </summary>
    public class JavaScriptSerializer : IJsonStrategy
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
}
