using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AdapterPattern
{
    public interface IDBHelper
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <returns></returns>
        string DBConnectString();
        /// <summary>
        /// 返回结果集
        /// </summary>
        /// <param name="strsql"></param>
        /// <returns></returns>
        DataSet GetUserGroup(string strsql);
        /// <summary>
        /// 返回插入的行数
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int InsertUser(User user);
    }
}
