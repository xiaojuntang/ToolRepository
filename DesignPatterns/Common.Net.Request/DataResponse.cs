using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Request
{
    public class DataResponse<T>
    {
        public T data { get; set; }

        public int code { get; set; }

        public string msg { get; set; }

        public DataResponse() { }

        public DataResponse(T data)
        {
            this.data = data;
        }

        public DataResponse(int code)
        {
            this.code = code;
        }

        public DataResponse(int code, string message)
        {
            this.code = code;
            this.msg = message;
        }
    }
}
