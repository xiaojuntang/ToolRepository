/***************************************************************************** 
*        filename :TransactionAop 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   TransactionAop 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       AopPatterns 
*        文件名:             TransactionAop 
*        创建系统时间:       2016/1/4 14:06:27 
*        创建年份:           2016 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace AopPatterns
{
    public sealed class TransactionAop : IMessageSink
    {
        private IMessageSink nextSink; //保存下一个接收器
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next">接收器</param>
        public TransactionAop(IMessageSink nextSink)
        {
            this.nextSink = nextSink;
        }
        /// <summary>
        ///  IMessageSink接口方法，用于异步处理，我们不实现异步处理，所以简单返回null,
        ///  不管是同步还是异步，这个方法都需要定义
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="replySink"></param>
        /// <returns></returns>
        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            return null;
        }
        /// <summary>
        /// 下一个接收器
        /// </summary>
        public IMessageSink NextSink
        {
            get { return nextSink; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IMessage SyncProcessMessage(IMessage msg)
        {
            IMessage retMsg = null;
            IMethodCallMessage call = msg as IMethodCallMessage;
            if (call == null || (Attribute.GetCustomAttribute(call.MethodBase, typeof(TransactionMethodAttribute))) == null)
                retMsg = nextSink.SyncProcessMessage(msg);
            else
            {
                //此处换成自己的数据库连接
                using (SqlConnection Connect = new SqlConnection(""))
                {
                    Connect.Open();
                    SqlTransaction SqlTrans = Connect.BeginTransaction();
                    //讲存储存储在上下文
                    CallContext.SetData(TransactionAop.ContextName, SqlTrans);
                    //传递消息给下一个接收器 - > 就是指执行你自己的方法
                    retMsg = nextSink.SyncProcessMessage(msg);
                    if (SqlTrans != null)
                    {
                        IMethodReturnMessage methodReturn = retMsg as IMethodReturnMessage;
                        Exception except = methodReturn.Exception;
                        if (except != null)
                        {
                            SqlTrans.Rollback();
                            //可以做日志及其他处理
                        }
                        else
                        {
                            SqlTrans.Commit();
                        }
                        SqlTrans.Dispose();
                        SqlTrans = null;
                    }
                }
            }
            return retMsg;
        }
        /// <summary>
        /// 用于提取、存储SqlTransaction
        /// </summary>
        public static string ContextName
        {
            get { return "TransactionAop"; }
        }
    }


    /// <summary>
    /// 标注类某方法内所有数据库操作加入事务控制
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class TransactionAttribute : ContextAttribute, IContributeObjectSink
    {
        /// <summary>
        /// 标注类某方法内所有数据库操作加入事务控制，请使用TransactionMethodAttribute同时标注
        /// </summary>
        public TransactionAttribute()
            : base("Transaction")
        { }
        public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink next)
        {
            return new TransactionAop(next);
        }
    }
    /// <summary>
    /// 标示方法内所有数据库操作加入事务控制
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TransactionMethodAttribute : Attribute
    {
        /// <summary>
        /// 标示方法内所有数据库操作加入事务控制
        /// </summary>
        public TransactionMethodAttribute()
        {
        }
    }
}
