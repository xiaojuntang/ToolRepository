using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using System.Runtime.Remoting.Messaging;
using System.Transactions;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.PolicyInjection;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace PostSharpTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //AopTest A = new AopTest();
            //RentalAgreement agreement = null;
            //A.Accrue(agreement);
            //A.Redeem(null, 0);

            string userID = Guid.NewGuid().ToString();

            //InstanceBuilder.Create<UserManager, IUserManager>().CreateDuplicateUsers(userID, Guid.NewGuid().ToString());
            InstanceBuilder.Create<UserManager>().CreateDuplicateUsers(userID, Guid.NewGuid().ToString());

            Console.ReadLine();
        }
    }

    public class UserManager : IUserManager
    {
        [ExceptionCallHandler(Ordinal = 1, MessageTemplate = "Encounter error:\nMessage:{Message}-{Source}-{StackTrace}-{TargetSite}")]
        [TransactionScopeCallHandler(Ordinal = 2)]
        public void CreateDuplicateUsers(string userID, string userName)
        {
            int a = 0;
            int b = 0;
            var c = a / b;
        }
    }

    public interface IUserManager
    {
        void CreateDuplicateUsers(string userID, string userName);
    }



    #region MyRegion
    public class InvocationContext
    {
        public IMethodCallMessage Request { get; set; }
        public ReturnMessage Reply { get; set; }
        public IDictionary<object, object> Properties { get; set; }
    }

    public interface ICallHandler
    {
        object PreInvoke(InvocationContext context);
        void PostInvoke(InvocationContext context, object correlationState);
        int Ordinal { get; set; }
        bool ReturnIfError { get; set; }
    }

    public abstract class CallHandlerBase : ICallHandler
    {
        public abstract object PreInvoke(InvocationContext context);
        public abstract void PostInvoke(InvocationContext context, object correlationState);
        public int Ordinal { get; set; }
        public bool ReturnIfError { get; set; }
    }


    public abstract class HandlerAttribute : Attribute
    {
        public abstract ICallHandler CreateCallHandler();
        public int Ordinal { get; set; }
        public bool ReturnIfError { get; set; }
    }
    #endregion





    public class TransactionScopeCallHandler : CallHandlerBase
    {
        public override object PreInvoke(InvocationContext context)
        {
            return new TransactionScope();
        }

        public override void PostInvoke(InvocationContext context, object correlationState)
        {
            TransactionScope transactionScope = (TransactionScope)correlationState;
            if (context.Reply.Exception == null)
            {
                transactionScope.Complete();
            }
            transactionScope.Dispose();
        }
    }

    public class TransactionScopeCallHandlerAttribute : HandlerAttribute
    {
        public override ICallHandler CreateCallHandler()
        {
            return new TransactionScopeCallHandler() { Ordinal = this.Ordinal, ReturnIfError = this.ReturnIfError };
        }
    }

    public class ExceptionCallHandler : CallHandlerBase
    {
        public string MessageTemplate { get; set; }
        public bool Rethrow { get; set; }

        public ExceptionCallHandler()
        {
            this.MessageTemplate = "{Message}";
        }

        public override object PreInvoke(InvocationContext context)
        {
            return null;
        }

        public override void PostInvoke(InvocationContext context, object correlationState)
        {
            if (context.Reply.Exception != null)
            {
                string message = this.MessageTemplate.Replace("{Message}", context.Reply.Exception.InnerException.Message)
                    .Replace("{Source}", context.Reply.Exception.InnerException.Source)
                    .Replace("{StackTrace}", context.Reply.Exception.InnerException.StackTrace)
                    .Replace("{HelpLink}", context.Reply.Exception.InnerException.HelpLink)
                    .Replace("{TargetSite}", context.Reply.Exception.InnerException.TargetSite.ToString());
                Console.WriteLine(message);
                if (!this.Rethrow)
                {
                    context.Reply = new ReturnMessage(null, null, 0, context.Request.LogicalCallContext, context.Request);
                }
            }
        }
    }

    public class ExceptionCallHandlerAttribute : HandlerAttribute
    {
        public string MessageTemplate { get; set; }
        public bool Rethrow { get; set; }
        public ExceptionCallHandlerAttribute()
        {
            this.MessageTemplate = "{Message}";
        }

        public override ICallHandler CreateCallHandler()
        {
            return new ExceptionCallHandler()
            {
                Ordinal = this.Ordinal,
                Rethrow = this.Rethrow,
                MessageTemplate = this.MessageTemplate,
                ReturnIfError = this.ReturnIfError
            };
        }
    }
}
