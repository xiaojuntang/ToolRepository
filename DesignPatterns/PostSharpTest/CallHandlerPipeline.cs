using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace PostSharpTest
{
    public class CallHandlerPipeline

    {

        private object _target;

        private IList<ICallHandler> _callHandlers;



        public CallHandlerPipeline(object target) : this(new List<ICallHandler>(), target) { }



        public CallHandlerPipeline(IList<ICallHandler> callHandlers, object target)

        {

            if (target == null)

            {

                throw new ArgumentNullException("target");

            }



            if (callHandlers == null)

            {

                throw new ArgumentNullException("callHandlers");

            }



            this._target = target;

            this._callHandlers = callHandlers;

        }



        public void Invoke(InvocationContext context)

        {

            Stack<object> correlationStates = new Stack<object>();

            Stack<ICallHandler> callHandlerStack = new Stack<ICallHandler>();



            //Preinvoke.

            foreach (ICallHandler callHandler in this._callHandlers)

            {

                correlationStates.Push(callHandler.PreInvoke(context));

                if (context.Reply != null && context.Reply.Exception != null && callHandler.ReturnIfError)

                {

                    context.Reply = new ReturnMessage(context.Reply.Exception, context.Request);

                    return;

                }

                callHandlerStack.Push(callHandler);

            }



            //Invoke Target Object.

            object[] copiedArgs = Array.CreateInstance(typeof(object), context.Request.Args.Length) as object[];

            context.Request.Args.CopyTo(copiedArgs, 0);

            try

            {

                object returnValue = context.Request.MethodBase.Invoke(this._target, copiedArgs);

                context.Reply = new ReturnMessage(returnValue, copiedArgs, copiedArgs.Length, context.Request.LogicalCallContext, context.Request);

            }

            catch (Exception ex)

            {

                context.Reply = new ReturnMessage(ex, context.Request);

            }



            //PostInvoke.

            while (callHandlerStack.Count > 0)

            {

                ICallHandler callHandler = callHandlerStack.Pop();

                object correlationState = correlationStates.Pop();

                callHandler.PostInvoke(context, correlationState);

            }

        }



        public void Sort()

        {

            ICallHandler[] callHandlers = this._callHandlers.ToArray<ICallHandler>();

            ICallHandler swaper = null;

            for (int i = 0; i < callHandlers.Length - 1; i++)

            {

                for (int j = i + 1; j < callHandlers.Length; j++)

                {

                    if (callHandlers[i].Ordinal > callHandlers[j].Ordinal)

                    {

                        swaper = callHandlers[i];

                        callHandlers[i] = callHandlers[j];

                        callHandlers[j] = swaper;

                    }

                }

            }



            this._callHandlers = callHandlers.ToList<ICallHandler>();

        }



        public void Combine(CallHandlerPipeline pipeline)

        {

            if (pipeline == null)

            {

                throw new ArgumentNullException("pipeline");

            }



            foreach (ICallHandler callHandler in pipeline._callHandlers)

            {

                this.Add(callHandler);

            }

        }



        public void Combine(IList<ICallHandler> callHandlers)

        {

            if (callHandlers == null)

            {

                throw new ArgumentNullException("callHandlers");

            }



            foreach (ICallHandler callHandler in callHandlers)

            {

                this.Add(callHandler);

            }

        }



        public ICallHandler Add(ICallHandler callHandler)

        {

            if (callHandler == null)

            {

                throw new ArgumentNullException("callHandler");

            }



            this._callHandlers.Add(callHandler);

            return callHandler;

        }

    }
}
