using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Core
{
    /// <summary>
    /// 泛型输出响应类
    /// </summary>
    /// <typeparam name="T">对象</typeparam>
    public class OutputResponse<T> where T : class
    {
        /// <summary>
        /// 返回代码.具体见方法返回值说明
        /// </summary>
        public Codes Code { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public OutputResponse()
        {
            Code = Codes.Success;
        }
    }
}
