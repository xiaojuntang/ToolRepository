using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Core
{
    [Flags]
    [Serializable]
    public enum Codes
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 空
        /// </summary>
        Empty,
        /// <summary>
        /// 无数据
        /// </summary>
        NoData,
        /// <summary>
        /// 参数错误
        /// </summary>
        ParamError,
        /// <summary>
        /// 异常
        /// </summary>
        Error,
        /// <summary>
        /// 失败
        /// </summary>
        Failure
    }
}
