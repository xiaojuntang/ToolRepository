using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AopPatterns
{

    //[MonitorAOP]
    //public class Calculator : ContextBoundObject
    //{
    //    public int Add(int x, int y)
    //    {
    //        return x + y;
    //    }
    //    public int Substract(int x, int y)
    //    {
    //        return x - y;
    //    }
    //}


    class Program
    {
        static void Main(string[] args)
        {
            //Calculator cal = new Calculator();
            //cal.Add(1, 5);
            MyContextObject myContextObject = new MyContextObject();
            myContextObject.AddLines("Test");
            Console.ReadLine();
        }
    }



    public class MyContextObject:ContextBoundObject
    {
        [AOPWriter]
        public void AddLines(string meg)
        {
            Console.WriteLine("这是方法执行中" + meg);
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class AOPWriterAttribute : Attribute, IContextAttribute
    {
        public void GetPropertiesForNewContext(IConstructionCallMessage msg)
        {
            msg.ContextProperties.Add(new AOPContextProperty());
        }

        public bool IsContextOK(System.Runtime.Remoting.Contexts.Context ctx, IConstructionCallMessage msg)
        {
            return false;
        }
    }

    /// <summary>
    /// 上下文成员属性
    /// </summary>
    public class AOPContextProperty : IContextProperty, IContributeServerContextSink
    {
        public void Freeze(Context newContext)
        {

        }

        public bool IsNewContextOK(Context newCtx)
        {
            return true;
        }

        public string Name
        {
            get { return "ContextService"; }
        }

        /// <summary>
        /// 提供的服务
        /// </summary>
        /// <param name="meg"></param>
        public void WriterMessage(string meg)
        {
            Console.WriteLine(meg);
        }

        public IMessageSink GetServerContextSink(IMessageSink nextSink)
        {
            AOPWriterSink megSink = new AOPWriterSink(nextSink);
            return megSink;
        }
    }

    public class AOPWriterSink : IMessageSink
    {
        private IMessageSink m_NextSink;
        public AOPWriterSink(IMessageSink nextSink)
        {
            m_NextSink = nextSink;
        }

        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            return null;
        }

        public IMessageSink NextSink
        {
            get { return m_NextSink; }
        }

        public IMessage SyncProcessMessage(IMessage msg)
        {
            IMethodCallMessage callMessage = msg as IMethodCallMessage;
            if (callMessage.MethodName.Contains("Add"))// == "WriterLine"
            {
                Context context = Thread.CurrentContext;
                AOPContextProperty contextWriterService = context.GetProperty("ContextService") as AOPContextProperty;
                if (callMessage == null)
                {
                    return null;
                }
                IMessage retMsg = null;
                if (contextWriterService != null)
                {
                    contextWriterService.WriterMessage("方法调用之前");
                }
                retMsg = m_NextSink.SyncProcessMessage(msg);
                contextWriterService.WriterMessage("方法调用之后");
                return retMsg;
            }
            else
            {
                return m_NextSink.SyncProcessMessage(msg);
            }
        }
    }
}
