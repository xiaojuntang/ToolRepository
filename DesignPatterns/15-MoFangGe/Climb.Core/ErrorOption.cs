using System.ComponentModel;

namespace Climb.Core
{
    /// <summary>
    /// 列举错误类型
    /// </summary>
    [Description("错误类型的枚举")]
    public enum OperateType
    {
        /// <summary>
        /// 用户没有登录
        /// </summary>
        [Description("没有登录")]
        NoLogin,
        /// <summary>
        /// 登录失败
        /// </summary>
        [Description("登录失败")]
        LoginError,
        /// <summary>
        /// 登录成功
        /// </summary>
        [Description("登录成功")]
        LoginSucess,
        /// <summary>
        /// 操作成功
        /// </summary>
        [Description("操作成功")]
        OperateOk,
        /// <summary>
        /// 操作失败
        /// </summary>
        [Description("操作失败")]
        OperateFail,
        /// <summary>
        /// 输入验证码有误
        /// </summary>
        [Description("验证码失败")]
        CehckCodeError,
        /// <summary>
        /// 输入错误 或者输入参数有误
        /// </summary>
        [Description("输入错误")]
        InputError,
        /// <summary>
        /// 返回结果为空
        /// </summary>
        [Description("结果为空")]
        ResultEmpty,
        /// <summary>
        /// 用户没有权限访问
        /// </summary>
        [Description("没有权限")]
        NoRule,
        /// <summary>
        /// 无法执行此次操作
        /// </summary>
        [Description("无法执行此操作")]
        NoDoWork,
        /// <summary>
        /// 返回结果没有相应
        /// </summary>
        [Description("没有结果响应")]
        NoResult,
        /// <summary>
        /// 程序错误 
        /// </summary>
        [Description("程序错误")]
        ServiceError


    }

}
