// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace UserServiceClient.Models
{
    using System.Linq;

    /// <summary>
    /// 通用返回结果
    /// </summary>
    public partial class PageResponseEntityListTSubject
    {
        /// <summary>
        /// Initializes a new instance of the PageResponseEntityListTSubject
        /// class.
        /// </summary>
        public PageResponseEntityListTSubject() { }

        /// <summary>
        /// Initializes a new instance of the PageResponseEntityListTSubject
        /// class.
        /// </summary>
        /// <param name="code">执行结果状态码</param>
        /// <param name="bussCode">业务状态码</param>
        /// <param name="message">执行结果信息</param>
        /// <param name="pageIndex">页号</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalCount">总数量</param>
        /// <param name="totalPageCount">总页数</param>
        /// <param name="data">执行结果内容</param>
        public PageResponseEntityListTSubject(int? code = default(int?), int? bussCode = default(int?), string message = default(string), int? pageIndex = default(int?), int? pageSize = default(int?), int? totalCount = default(int?), int? totalPageCount = default(int?), System.Collections.Generic.IList<TSubject> data = default(System.Collections.Generic.IList<TSubject>))
        {
            Code = code;
            BussCode = bussCode;
            Message = message;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPageCount = totalPageCount;
            Data = data;
        }

        /// <summary>
        /// Gets or sets 执行结果状态码
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "Code")]
        public int? Code { get; set; }

        /// <summary>
        /// Gets or sets 业务状态码
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "BussCode")]
        public int? BussCode { get; set; }

        /// <summary>
        /// Gets or sets 执行结果信息
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "Message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets 页号
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "pageIndex")]
        public int? PageIndex { get; set; }

        /// <summary>
        /// Gets or sets 页大小
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "pageSize")]
        public int? PageSize { get; set; }

        /// <summary>
        /// Gets or sets 总数量
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "totalCount")]
        public int? TotalCount { get; set; }

        /// <summary>
        /// Gets or sets 总页数
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "totalPageCount")]
        public int? TotalPageCount { get; set; }

        /// <summary>
        /// Gets or sets 执行结果内容
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "Data")]
        public System.Collections.Generic.IList<TSubject> Data { get; set; }

    }
}
