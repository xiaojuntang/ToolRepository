using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Core
{
    /// <summary>
    /// 输出响应类
    /// </summary>
    [Serializable]
    public class DataResponse<T> where T : class
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
        public T Data { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DataResponse()
        {
            Code = Codes.Success;
            Message = string.Empty;
        }
    }
}
