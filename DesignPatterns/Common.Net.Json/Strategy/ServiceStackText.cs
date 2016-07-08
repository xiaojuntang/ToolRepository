using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Json
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceStackText : IJsonStrategy
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
    //文档http://www.cnblogs.com/blqw/p/3274229.html
}
