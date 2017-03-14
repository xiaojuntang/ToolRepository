using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;

namespace PostSharpTest
{
    public class InterceptingRealProxy<T> : RealProxy
    {
        private IDictionary<MemberInfo, CallHandlerPipeline> _callHandlerPipelines;
        public InterceptingRealProxy(object target, IDictionary<MemberInfo, CallHandlerPipeline> callHandlerPipelines) : base(typeof(T))
        {
            if (callHandlerPipelines == null)
            {
                throw new ArgumentNullException("callHandlerPipelines");
            }
            this._callHandlerPipelines = callHandlerPipelines;
        }

        public override IMessage Invoke(IMessage msg)
        {
            InvocationContext context = new InvocationContext();
            context.Request = (IMethodCallMessage)msg;
            this._callHandlerPipelines[context.Request.MethodBase].Invoke(context);
            return context.Reply;
        }
    }
}
