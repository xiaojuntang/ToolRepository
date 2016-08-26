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

    public class DataResponse2<T> where T : class
    {
        /// <summary>
        /// 返回代码
        /// </summary>
        private int _ret;
        /// <summary>
        /// 返回消息
        /// </summary>
        private string _msg;
        /// <summary>
        /// 消息体
        /// </summary>
        private T _body;

        /// <summary>
        /// 返回代码
        /// </summary>
        public int Ret
        {
            get { return _ret; }
            set { _ret = value; }
        }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Msg
        {
            get { return _msg; }
            set { _msg = value; }
        }

        /// <summary>
        /// 消息体
        /// </summary>
        public T Body
        {
            get { return _body; }
            set { _body = value; }
        }
    }
}
