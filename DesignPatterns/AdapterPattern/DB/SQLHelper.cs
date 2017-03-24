using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AdapterPattern
{
    public class SQLHelper : IDBHelper
    {
        #region IDBHelper 成员

        public string DBConnectString()
        {
            return "SQL Connect String";
        }

        public DataSet GetUserGroup(string strsql)
        {
            DataSet ds = new DataSet();
            return ds;
        }

        public int InsertUser(User user)
        {
            return 1;
        }

        #endregion
    }
}
