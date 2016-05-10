using System;
using System.Text;
using System.Xml;
using Climb.Utility;
using Climb.Utility.SystemExt;

namespace Climb.Core
{
    /// <summary>
    /// 返回结果类
    /// </summary>
    [Serializable]
    public class OperationResult
    {
        /// <summary>
        /// 错误类型
        /// </summary>
        public OperateType ErrorOptionEnum { get; set; }

        /// <summary>
        /// 存储错误消息
        /// </summary>
        private StringBuilder MessageInfo = new StringBuilder();

        /// <summary>
        /// 当前错误消息的长度
        /// </summary>
        public int MsgLength
        {
            get
            {
                return MessageInfo.Length;
            }
        }


        /// <summary>
        /// 重载返回结果的json对象
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return SerializeExt.JsonSerializer(this);
        }

        /// <summary>
        /// 设置消息
        /// </summary>
        /// <param name="msg">如果需要设置错误消息 可以从此字段设置</param>
        public void SetMessage(string msg)
        {
            MessageInfo.AppendLine(msg);
        }

        /// <summary>
        /// 获得消息
        /// </summary>
        /// <returns></returns>
        public string GetMessage()
        {
            return MessageInfo.ToString();
        }
    }

    /// <summary>
    /// 对result 的对象扩展类 可以实现得到一个类型
    /// </summary>
    /// <typeparam name="T">返回clinet结果 但是必须是支持序列化的类型</typeparam>
    [Serializable]
    public class OperationResult<T> : OperationResult
    { 
        public T ObjectResult { get; set; }
    }
}
