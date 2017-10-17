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
* 创建时间: 2017/10/17 9:34:21
* 更新时间: 2017/10/17 9:34:21
* 
* 功 能： N/A
* 类 名： CallTraceSink
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────────────────────────────
* V0.01 2017/10/17 9:34:21 唐晓军 初版
*
* Copyright (c) 2017 Lir Corporation. All rights reserved.
*┌──────────────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．                                                  │
*│　版权所有：北京凤凰学易科技有限公司　　　　　　　　　　　　　                                                      │
*└──────────────────────────────────────────────────────────┘
************************************************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Core
{
    /// <summary>
    /// Attribute在拦截机制上的应用
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CallTraceAttribute : ContextAttribute
    {
        public CallTraceAttribute() : base("CallTrace")
        {
        }

        //重载ContextAttribute方法，创建一个上下文环境属性
        public override void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
        {
            ctorMsg.ContextProperties.Add(new CallTraceProperty());
        }
    }



    public class CallTraceSink : IMessageSink //实现IMessageSink
    {
        private IMessageSink nextSink;  //保存下一个接收器

        //在构造器中初始化下一个接收器
        public CallTraceSink(IMessageSink next)
        {
            nextSink = next;
        }

        //必须实现的IMessageSink接口属性
        public IMessageSink NextSink
        {
            get
            {
                return nextSink;
            }
        }

        //实现IMessageSink的接口方法，当消息传递的时候，该方法被调用
        public IMessage SyncProcessMessage(IMessage msg)
        {
            //拦截消息，做前处理
            Preprocess(msg);
            //传递消息给下一个接收器
            IMessage retMsg = nextSink.SyncProcessMessage(msg);
            //调用返回时进行拦截，并进行后处理
            Postprocess(msg, retMsg);
            return retMsg;
        }

        //IMessageSink接口方法，用于异步处理，我们不实现异步处理，所以简单返回null,
        //不管是同步还是异步，这个方法都需要定义
        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            return null;
        }

        //我们的前处理方法，用于检查库存，出于简化的目的，我们把检查库存和发送邮件都写在一起了，
        //在实际的实现中，可能也需要把Inventory对象绑定到一个上下文环境，
        //另外，可以将发送邮件设计为另外一个接收器，然后通过NextSink进行安装
        private void Preprocess(IMessage msg)
        {
            //检查是否是方法调用，我们只拦截Order的Submit方法。
            IMethodCallMessage call = msg as IMethodCallMessage;

            if (call == null)
                return;

            if (call.MethodName == "Submit")
            {
                string product = call.GetArg(0).ToString(); //获取Submit方法的第一个参数
                int qty = (int)call.GetArg(1); //获取Submit方法的第二个参数

                //调用Inventory检查库存存量
                if (new Inventory().Checkout(product, qty))
                    Console.WriteLine("Order availible");
                else
                {
                    Console.WriteLine("Order unvailible");
                    SendEmail();
                }
            }
        }

        //后处理方法，用于记录订单提交信息，同样可以将记录作为一个接收器
        //我们在这里处理，仅仅是为了演示
        private void Postprocess(IMessage msg, IMessage retMsg)
        {
            IMethodCallMessage call = msg as IMethodCallMessage;

            if (call == null)
                return;
            Console.WriteLine("Log order information");
        }

        private void SendEmail()
        {
            Console.WriteLine("Send email to manager");
        }
    }

    [CallTrace]
    public class Order:ContextBoundObject
    {
        private int orderId;
        private string product;
        private int quantity;

        public Order(int orderId)
        {
            this.orderId = orderId;
        }
        public void Submit(string product, int quantity)
        {
            this.product = product;
            this.quantity = quantity;
        }


        //public void Submit()
        //{
        //    Inventory inventory = new Inventory(); //创建库存对象

        //    //检查库存
        //    if (inventory.Checkout(product, quantity))
        //    {
        //        //Logbook.Log("Order" + orderId + " available");
        //        inventory.Update(product, quantity);
        //    }
        //    else
        //    {
        //        //Logbook.Log("Order" + orderId + " unavailable");
        //        SendEmail();
        //    }
        //}

        public string ProductName
        {
            get { return product; }
            set { product = value; }
        }

        public int OrderId
        {
            get { return orderId; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public void SendEmail()
        {
            Console.WriteLine("Send email to manager");
        }
    }

    public class Inventory
    {

        private Hashtable inventory = new Hashtable();

        public Inventory()
        {
            inventory["Item1"] = 100;
            inventory["Item2"] = 200;
        }

        public bool Checkout(string product, int quantity)
        {
            int qty = GetQuantity(product);
            return qty >= quantity;
        }

        public int GetQuantity(string product)
        {
            int qty = 0;
            if (inventory[product] != null)
                qty = (int)inventory[product];
            return qty;
        }

        public void Update(string product, int quantity)
        {
            int qty = GetQuantity(product);
            inventory[product] = qty - quantity;
        }
    }



    public class CallTraceProperty : IContextProperty, IContributeObjectSink
    {
        public CallTraceProperty()
        {
        }

        //IContributeObjectSink的接口方法，实例化消息接收器
        public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink next)
        {
            return new CallTraceSink(next);
        }

        //IContextProperty接口方法，如果该方法返回ture,在新的上下文环境中激活对象
        public bool IsNewContextOK(Context newCtx)
        {
            return true;
        }

        //IContextProperty接口方法，提供高级使用
        public void Freeze(Context newCtx)
        {
        }

        //IContextProperty接口属性
        public string Name
        {
            get { return "OrderTrace"; }
        }
    }
}
