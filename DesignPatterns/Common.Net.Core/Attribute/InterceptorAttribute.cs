/************************************************************************************************************************
* 命名空间: Common.Net.Core.Attribute
* 项目描述:
* 版本名称: v1.0.0.0
* 作　　者: 唐晓军
* 所在区域: 北京
* 机器名称: DESKTOP-F6QRRBM
* 注册组织: 学科网（www.zxxk.com）
* 项目名称: 学易作业系统
* CLR版本:  4.0.30319.42000
* 创建时间: 2017/10/17 13:21:38
* 更新时间: 2017/10/17 13:21:38
* 
* 功 能： N/A
* 类 名： InterceptorAttribute
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────────────────────────────
* V0.01 2017/10/17 13:21:38 唐晓军 初版
*
* Copyright (c) 2017 Lir Corporation. All rights reserved.
*┌──────────────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．                                                  │
*│　版权所有：北京凤凰学易科技有限公司　　　　　　　　　　　　　                                                      │
*└──────────────────────────────────────────────────────────┘
************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;

namespace Common.Net.Core
{
    /// <summary>
    /// Interceptor
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class InterceptorAttribute : ContextAttribute, IContributeObjectSink
    {
        public InterceptorAttribute() : base("Interceptor")
        {

        }

        public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink nextSink)
        {
            return new AopHandler(nextSink);
        }
    }

    /// <summary>
    /// InterceptorMethod
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class InterceptorMethodAttribute : Attribute
    {
        //public InterceptorMethodAttribute()
        //{

        //}

        //public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink nextSink)
        //{
        //    return new MyAopHandler(nextSink);
        //}
    }

    /// <summary>
    /// AopHandler
    /// </summary>
    public sealed class AopHandler : IMessageSink
    {
        private static Dictionary<string, Type> dictionary = new Dictionary<string, Type>();

        /// <summary>
        /// 接收器
        /// </summary>
        private IMessageSink nextSink;
        public IMessageSink NextSink
        {
            get { return nextSink; }
        }
        public AopHandler(IMessageSink nextSink)
        {
            this.nextSink = nextSink;
        }

        /// <summary>
        /// 同步处理方法
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IMessage SyncProcessMessage(IMessage msg)
        {
            IMessage retMsg = null;

            //方法调用消息接口
            IMethodCallMessage call = msg as IMethodCallMessage;
            if (call == null || (Attribute.GetCustomAttribute(call.MethodBase, typeof(InterceptorMethodAttribute))) == null)
            {
                //如果被调用的方法没打InterceptorMethodAttribute标签
                retMsg = nextSink.SyncProcessMessage(msg);
            }
            else
            {
                //如果打了InterceptorMethodAttribute标签


                #region 参数验证
                string typeName = msg.Properties["__TypeName"].ToString();
                Type objType = null;
                if (!dictionary.TryGetValue(typeName, out objType))
                {
                    objType = Type.GetType(typeName);
                    dictionary.Add(typeName, objType);
                }
                MethodInfo[] methodInfos = objType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (MethodInfo mi in methodInfos)
                {
                    if (mi.DeclaringType.FullName.Contains("Common.UnitTest"))
                    {
                        ParameterInfo[] parameters = mi.GetParameters();
                        for (int index = 0; index < parameters.Length; index++)
                        {
                            var paramInfo = parameters[index];
                            var attributes = paramInfo.GetCustomAttributes(typeof(ArgumentValidationAttribute), false);
                            if (attributes.Length == 0)
                                continue;
                            object value = (msg.Properties["__Args"] as object[])[index];
                            foreach (ArgumentValidationAttribute attr in attributes)
                            {
                                attr.Validate(value, paramInfo.Name);
                            }
                        }
                    }
                }
                #endregion

                //string m = (msg)._MethodName;

                //MessageBox.Show("执行之前");
                retMsg = nextSink.SyncProcessMessage(msg);
                //MessageBox.Show("执行之后");
            }
            return retMsg;
        }

        /// <summary>
        /// 异步处理方法（不需要）
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="replySink"></param>
        /// <returns></returns>
        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            return null;
        }
    }
}
