using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Core
{
    /// <summary>
    /// 分页类
    /// </summary>
    /// <typeparam name="T">分页对象</typeparam>
    [Serializable]
    public class PagingResponse<T> where T : class
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalNumber;

        /// <summary>
        /// 当前页记录列表
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// 分页
        /// </summary>
        public string PageNavigate { get; set; }
    }
}
