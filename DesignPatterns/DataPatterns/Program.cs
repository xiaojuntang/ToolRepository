using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DataPatterns
{
    class Program
    {
        static void Main(string[] args)
        {

            using (TransactionScope scope = new TransactionScope())
            {
                //SqlConnection connection1 = new SqlConnection(connectString1)
                //SqlConnection connection2 = new SqlConnection(connectString2)
                scope.Complete();
            }
        }
    }
}
