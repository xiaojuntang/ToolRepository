using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPatterns.Entity
{
    /// <summary>
    /// 系统查询参数
    /// </summary>
    public class QueryParameter
    {
        /// <summary>
        /// 编号 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// 结果日期
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
