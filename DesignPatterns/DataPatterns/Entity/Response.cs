using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPatterns.Entity
{
    public class Response
    {
        /// <summary>
        /// 返回代码. 0-失败，1-成功，其他-具体见方法返回值说明
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public dynamic Data { get; set; }
    }
}
