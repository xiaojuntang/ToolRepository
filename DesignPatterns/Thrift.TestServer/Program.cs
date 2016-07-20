using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thrift.TestServer
{
    class Program
    {
        static int Main(string[] args)
        {if (args.Length == 0)
            {
                Console.WriteLine("must provide 'server' or 'client' arg");
                return -1;
            }

            string[] subArgs = new string[args.Length - 1];
            for (int i = 1; i < args.Length; i++)
            {
                subArgs[i - 1] = args[i];
            }
            if (args[0] == "client")
            {
                return TestClient.Execute(subArgs) ? 0 : 1;
            }
            else if (args[0] == "server")
            {
                return TestServer.Execute(subArgs) ? 0 : 1;
            }
            else
            {
                Console.WriteLine("first argument must be 'server' or 'client'");
            }
            return 0;
        }
    }
}
