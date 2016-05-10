using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCF.Service
{
    public class Hello : IHello
    {
        public string SayHello(string name)
        {
            return "SayHello " + name;
        }

        public string SayHello(string name, string sex)
        {
            return "SayHelloSex " + name + sex;
        }
    }
}
