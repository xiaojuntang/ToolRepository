using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns
{
    public delegate double CalculateDelegate(double a, double b);

    public class 计算器委托
    {
        static void M()
        {
            double a = 1;
            double b = 2;
            Console.WriteLine("Result:{0}", Calculate(a, b, Add));
        }

        public static double Calculate(double a,double b, CalculateDelegate cd)
        {
           return cd.Invoke(a, b);
        }

        public static double Add(double a, double b)
        {
            return a + b;
        }
        public static double Subtract(double a, double b)
        {
            return a - b;
        }
        public static double Multiply(double a, double b)
        {
            return a * b;
        }
        public static double Divide(double a, double b)
        {
            if (b == 0) throw new DivideByZeroException();
            return a / b;
        }
    }
}
