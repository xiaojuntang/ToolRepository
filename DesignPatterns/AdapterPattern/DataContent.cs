using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AdapterPattern
{
    public class DataContent : IDBHelper
    {
        private IDBHelper DbHelper = GetDBHelper();
        public static IDBHelper GetDBHelper()
        {
            string strClass = ConfigurationSettings.AppSettings["DBHeper"].ToString();
            Assembly assembly = Assembly.Load("AdapterPattern");
            IDBHelper dbHelper = assembly.CreateInstance(strClass) as IDBHelper;
            return dbHelper;
        }

        #region IDBHelper 成员

        public string DBConnectString()
        {
            return DbHelper.DBConnectString();
        }

        public System.Data.DataSet GetUserGroup(string strsql)
        {
            return DbHelper.GetUserGroup(strsql);
        }

        public int InsertUser(User user)
        {
            return DbHelper.InsertUser(user);
        }

        #endregion
    }
}
