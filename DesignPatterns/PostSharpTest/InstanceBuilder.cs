using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PostSharpTest
{
    public class InstanceBuilder
    {
        public static TInterface Create<TObject, TInterface>() where TObject : TInterface
        {
            TObject target = Activator.CreateInstance<TObject>();
            InterceptingRealProxy<TInterface> realProxy = new InterceptingRealProxy<TInterface>(target, CreateCallHandlerPipeline<TObject, TInterface>(target));
            return (TInterface)realProxy.GetTransparentProxy();
        }

        public static T Create<T>()
        {
            return Create<T, T>();
        }

        public static IDictionary<MemberInfo, CallHandlerPipeline> CreateCallHandlerPipeline<TObject, TInterfce>(TObject target)
        {
            CallHandlerPipeline pipeline = new CallHandlerPipeline(target);
            object[] attributes = typeof(TObject).GetCustomAttributes(typeof(HandlerAttribute), true);
            foreach (var attribute in attributes)
            {
                HandlerAttribute handlerAttribute = attribute as HandlerAttribute;
                pipeline.Add(handlerAttribute.CreateCallHandler());
            }
            IDictionary<MemberInfo, CallHandlerPipeline> kyedCallHandlerPipelines = new Dictionary<MemberInfo, CallHandlerPipeline>();
            foreach (MethodInfo methodInfo in typeof(TObject).GetMethods())
            {
                MethodInfo declareMethodInfo = typeof(TInterfce).GetMethod(methodInfo.Name, BindingFlags.Public | BindingFlags.Instance);
                if (declareMethodInfo == null)
                {
                    continue;
                }
                kyedCallHandlerPipelines.Add(declareMethodInfo, new CallHandlerPipeline(target));
                foreach (var attribute in methodInfo.GetCustomAttributes(typeof(HandlerAttribute), true))
                {
                    HandlerAttribute handlerAttribute = attribute as HandlerAttribute;
                    kyedCallHandlerPipelines[declareMethodInfo].Add(handlerAttribute.CreateCallHandler());
                }
                kyedCallHandlerPipelines[declareMethodInfo].Combine(pipeline);
                kyedCallHandlerPipelines[declareMethodInfo].Sort();
            }
            return kyedCallHandlerPipelines;
        }
    }
}
