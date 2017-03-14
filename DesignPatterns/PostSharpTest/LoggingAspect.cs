using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

//using PostSharp.Aspects;

namespace PostSharpTest
{
    //    [Serializable]
    //    public class LoggingAspect:OnMethodBoundaryAspect
    //    {
    //        public override void OnEntry(MethodExecutionArgs args)
    //        {
    //            Console.WriteLine("{0}:{1}", args.Method.Name, DateTime.Now);
    //            foreach (var argument in args.Arguments)//遍历方法的参数
    //            {
    //                if (argument != null)
    //                {
    //                    if (typeof(ILoggable).IsAssignableFrom(argument.GetType()))
    //                    {
    //                        //Console.WriteLine((ILoggable)argument.LogInfo());
    //                        Console.WriteLine("test");
    //                    }
    //                }

    //            }
    //        }

    //        public override void OnSuccess(MethodExecutionArgs args)
    //        {
    //            Console.WriteLine("{0} complete:{1}", args.Method.Name, DateTime.Now);
    //        }
    //    }

    //    [Serializable]
    //    public class DefensiveProgramming : OnMethodBoundaryAspect
    //    {
    //        public override void OnEntry(MethodExecutionArgs args)
    //        {
    //            var parameters = args.Method.GetParameters();//获取形参
    //            var arguments = args.Arguments;//获取实参
    //            for (int i = 0; i < arguments.Count; i++)
    //            {
    //                if (arguments[i] == null)
    //                {
    //                    throw new ArgumentNullException(parameters[i].Name);
    //                }
    //                if (arguments[i] is int && (int)arguments[i] <= 0)
    //                {
    //                    throw new ArgumentException("参数非法", parameters[i].Name);
    //                }
    //            }
    //        }
    //    }




    public interface ILoyaltyAccrualService
    {
        void Accrue(RentalAgreement agreement);
    }

















    #region Entity

    public interface ILoggable
    {
        string LogInfo();
    }

    public class Invoice : ILoggable
    {
        public Guid Id { get; set; }
        public string LogInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class RentalAgreement: ILoggable
    {
        public Guid Id { get; set; }
        public Customer Customer { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LogInfo()
        {
            throw new NotImplementedException();
        }
    }
    public class Customer : ILoggable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DriversLicense { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string LogInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class Vehicle : ILoggable
    {
        public Guid Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public Size Size { get; set; }
        public string Vin { get; set; }
        public string LogInfo()
        {
            throw new NotImplementedException();
        }
    }

    public enum Size
    {
        Compact = 0,
        Midsize,
        FullSize,
        Luxury,
        Truck,
        SUV
    } 
    #endregion
}
