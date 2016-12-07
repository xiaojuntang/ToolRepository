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


    public class DataResponse3<T>
    {
        public T data { get; set; }

        public int code { get; set; }

        public string message { get; set; }

        public DataResponse3() { }

        public DataResponse3(int code)
        {
            this.code = code;
        }

        public DataResponse3(Result result)
        {
            this.code = result.CodeAndMessage.code;
            this.message = result.CodeAndMessage.message;
        }

        public DataResponse3(int code, string message)
        {
            this.code = code;
            this.message = message;
        }

        public DataResponse3(T data)
        {
            this.data = data;
        }
    }

    public class Result
    {
        private Dictionary<string, string> _dicCode;
        public ResultCodeAndMessage CodeAndMessage;

        public Dictionary<string, string> GetdicCode()
        {
            this._dicCode = new Dictionary<string, string>(){
                {"300101","用户请求过期"},
                {"300102","用户日调用量超限"},
                {"300103","服务每秒调用量超限"},
                {"300201","url无法解析"},
                {"300202","请求缺少apikey"},
                {"300203","请求缺少signature"},
                {"300204","服务没有取到apikey或secretkey"},
                {"300205","apikey不存在"},
                {"300206","api不存在"},
                {"300207","api已关闭服务"},
                {"300208","余额不足,请充值"},
                {"300209","未通过签名验证"},
                {"300210","服务商响应status非200"},
                {"300211","请求参数错误"},
                {"300301","内部错误"},
                {"300302","系统繁忙稍后再试"}
              };
            return this._dicCode;
        }

        public void SetdicCode(Dictionary<string, string> dicCode)
        {
            this._dicCode = dicCode;
        }
    }

    public class ResultCodeAndMessage
    {
        public int code { get; set; }

        public string message { get; set; }
    }



    public class ConvertApiRepsonse<T>
    {
        public static DataResponse3<T> Convert()
        {
            DataResponse3<T> dataResponse = new DataResponse3<T>();




            return dataResponse;
        } 
    }
}
