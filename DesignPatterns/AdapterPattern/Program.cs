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
            DataContent dataContent = new DataContent();
            var aa = dataContent.DBConnectString();

            //ITarget t = new Adapter();
            //t.Request("test");
        }
    }

    public interface ITarget
    {
        string Request(string param);
    }

    /// <summary>
    /// 需要适配的类接口
    /// </summary>
    public class Cdaptee
    {
        public string SpecificRequest()
        {
            Console.WriteLine("Called SpecificRequest()");
            return "Called SpecificRequest()";
        }
    }

    public class Adapter : Cdaptee, ITarget
    {
        public string Request(string param)
        {
            return base.SpecificRequest();
        }
    }
}
