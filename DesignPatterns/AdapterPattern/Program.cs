using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdapterPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            ITarget t = new Adapter();
            t.Request("test");
        }
    }

    public interface ITarget
    {
        string Request(string param);
    }

    /// <summary>
    /// 需要适配的类接口
    /// </summary>
    public class Adaptee
    {
        public string SpecificRequest()
        {
            Console.WriteLine("Called SpecificRequest()");
            return "Called SpecificRequest()";
        }
    }

    public class Adapter : Adaptee, ITarget
    {
        public string Request(string param)
        {
            return base.SpecificRequest();
        }
    }
}
