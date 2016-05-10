using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPatterns.Entity
{
    /// <summary>
    /// 分页通用类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Paging<T> where T : class
    {
        /// <summary>
        /// 当前页。从1计数
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页记录数。默认20
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalNumber;

        /// <summary>
        /// 当前页记录列表
        /// </summary>
        public List<T> Items { get; set; }

        public Paging()
        {
            PageIndex = 1;
            PageSize = 20;
        }
    }
}
