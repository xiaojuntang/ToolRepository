using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Core.Response
{
    /// <summary>
    /// 
    /// </summary>
    public class DtoCommon
    {
    }

    /// <summary>
    /// 通用整型键值对
    /// </summary>
    public class DtoIntKv
    {
        /// <summary>
        /// 键
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// 键2
        /// </summary>
        public int KeySec { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; }
    }

    /// <summary>
    /// 通用整型键值对
    /// </summary>
    public class DtoStringKv
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 键2
        /// </summary>
        public string KeySec { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; }
    }

    /// <summary>
    /// 分页基类
    /// </summary>
    public class PageBase
    {
        /// <summary>
        /// 当前页。从1计数
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页记录数。默认20
        /// </summary>
        public int PageSize { get; set; }
    }
}
