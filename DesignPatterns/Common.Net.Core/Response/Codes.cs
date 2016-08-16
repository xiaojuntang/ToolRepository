using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Core
{
    /// <summary>
    /// 代码说明
    /// </summary>
    [Flags]
    public enum Codes : int
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 10000,
        /// <summary>
        /// 无数据
        /// </summary>
        Empty = 10001,
        /// <summary>
        /// 参数错误
        /// </summary>
        ParamError = 10002,
        /// <summary>
        /// 异常
        /// </summary>
        Error = 10003,
        /// <summary>
        /// 失败
        /// </summary>
        Failure = 10004,
        /// <summary>
        /// 签名错误
        /// </summary>
        SignError = 10005
    }
}
