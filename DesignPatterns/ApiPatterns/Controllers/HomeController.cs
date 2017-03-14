using ApiPatterns.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ApiPatterns.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }

    public class RTest
    {
        public int ID { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime Time { get; set; }

        public double Money { get; set; }
        [JsonIgnore]
        public decimal Con { get; set; }
    }






    public interface IRequest
    {
        ResultObject Validate();
    }

    public interface IResponse
    {

    }







    public class ListResponseBase<T> : IResponse
    {
        public List<T> List { get; set; }
    }

    public class PageResponseBase<T> : ListResponseBase<T>
    {
        /// <summary>
        /// 页码数        
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 总条数        
        ///  </summary>
        public long TotalCount { get; set; }
        /// <summary>
        /// 每页条数       
        ///  </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总页数       
        /// </summary>
        public long PageCount { get; set; }
    }








    public class ResultObject
    {
        /// <summary>
        /// 等于0表示成功        
        ///  </summary>
        public int Code { get; set; }
        /// <summary>
        /// code不为0时，返回错误消息        
        /// </summary>
        public string Msg { get; set; }
    }

    public class ResultObject<TResponse> : ResultObject where TResponse : IResponse
    {
        public ResultObject()
        {
        }
        public ResultObject(TResponse data)
        {
            Data = data;
        }        /// <summary>
                 /// 返回的数据        /// </summary>
        public TResponse Data { get; set; }

    }

    public class ApiResult
    {
        public ApiResult() { }
        public ApiResult(ResultObject data)
        {
            ResultData = data;
        }
        /// 返回的数据        /// </summary>
        public ResultObject ResultData { get; set; }

    }



    public abstract class UserRequestBase : IRequest
    {
        public int ApiUserID { get; set; }
        public string ApiUserName { get; set; }
        // ......可以添加其他要专递的登录用户相关的信息
        /// <summary>
        /// 参数验证
        /// </summary>
        /// <returns></returns>
        public abstract ResultObject Validate();
    }



    [JsonConverter(typeof(CustomDateConverter))]
    public class ApiTestController : AsyncController
    {

        //[JsonConverter(typeof(CustomDateConverter), "yyyy年MM月dd日")]
        public ApiResult Apis<TRequest>(TRequest request, Func<TRequest, ResultObject> handle)
        {
            try
            {
                var requestBase = request as IRequest;
                if (requestBase != null)
                {                    //处理需要登录用户的请求
                    var userRequest = request as UserRequestBase;
                    if (userRequest != null)
                    {
                        //var loginUser = LoginUser.GetUser();
                        //if (loginUser != null)
                        //{
                        //    userRequest.ApiUserID = loginUser.UserID;
                        //    userRequest.ApiUserName = loginUser.UserName;
                        //}
                    }
                    var validResult = requestBase.Validate();
                    if (validResult != null)
                    {
                        return new ApiResult(validResult);
                    }
                }
                var result = handle(request); //处理请求
                return new ApiResult(result);
            }
            catch (Exception exp)
            {                //异常日志：
                return new ApiResult
                {
                    ResultData = new ResultObject
                    {
                        Code = 1,
                        Msg = "系统异常：" + exp.Message
                    }
                };
            }
        }

        public ApiResult Api(Func<ResultObject> handle)
        {
            try
            {
                var result = handle();//处理请求
                return new ApiResult(result);
            }
            catch (Exception exp)
            {                //异常日志
                return new ApiResult { ResultData = new ResultObject { Code = 1, Msg = "系统异常：" + exp.Message } };
            }
        }

        /// <summary>
        /// 异步api
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="request"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public Task<ApiResult> ApiAsync<TRequest, TResponse>(TRequest request, Func<TRequest, Task<TResponse>> handle) where TResponse : ResultObject
        {
            return handle(request).ContinueWith(x =>
            {
                return Api(() => x.Result);
            });
        }
    }
}
