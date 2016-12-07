using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Common.Net.Request
{
   public class DeserializeHelper
    {
        /// <summary>
        /// 反序列化json字符串为对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam><param name="result">json格式的字符串</param>
        /// <returns/>
        public static DataResponse<T> DeserializeJson<T>(string result)
        {
            return JsonConvert.DeserializeObject<DataResponse<T>>(result);
            //Response.ApiResponse<T> apiResponse1 = (Response.ApiResponse<T>)null;
            //Regex regex = new Regex("\"result\":(.*)}}$", RegexOptions.IgnoreCase);
            //if (!regex.IsMatch(result))
            //    return apiResponse1;
            //Response.ApiResponse<T> apiResponse2;
            //if (string.IsNullOrEmpty(regex.Matches(result)[0].Groups[1].ToString().Replace('"', ' ')))
            //{
            //    Response.ApiResponse<string> apiResponse3 = JsonHelper.Deserialize<Response.ApiResponse<string>>(result, (Response.ApiResponse<string>)null);
            //    apiResponse2 = new Response.ApiResponse<T>()
            //    {
            //        Response = new Response.ResponseByJson<T>()
            //        {
            //            Status = new Response.Status()
            //        }
            //    };
            //    apiResponse2.Response.Status.Code = apiResponse3.Response.Status.Code;
            //    apiResponse2.Response.Status.ExceptionMessage = apiResponse3.Response.Status.ExceptionMessage;
            //    apiResponse2.Response.Status.Message = apiResponse3.Response.Status.Message;
            //}
            //else
            //    apiResponse2 = JsonHelper.Deserialize<Response.ApiResponse<T>>(result, (Response.ApiResponse<T>)null);
            //return apiResponse2;
        }
    }
}
