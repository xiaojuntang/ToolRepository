using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    public partial class DataBase
    {
        private string constr = string.Empty;

        public DataBase()
            : this(ConfigurationManager.AppSettings["ConStr"].ToString())
        {
        }
        public DataBase(string connectionString)
        {
            constr = connectionString;
        }


    }
}
