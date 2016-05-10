using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.DB.MongoDB
{
    /// <summary>
    /// MongoDB分页类
    /// </summary>
    public class PagerInfo
    {
        /// <summary>
        /// 第几页
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
