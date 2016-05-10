using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;

namespace DataPatterns
{
    public enum EffentNextType
    {
        /// <summary>
        /// 对其他语句无任何影响 
        /// </summary>
        None,
        /// <summary>
        /// 当前语句必须为"select count(1) from .."格式，如果存在则继续执行，不存在回滚事务
        /// </summary>
        WhenHaveContine,
        /// <summary>
        /// 当前语句必须为"select count(1) from .."格式，如果不存在则继续执行，存在回滚事务
        /// </summary>
        WhenNoHaveContine,
        /// <summary>
        /// 当前语句影响到的行数必须大于0，否则回滚事务
        /// </summary>
        ExcuteEffectRows,
        /// <summary>
        /// 引发事件-当前语句必须为"select count(1) from .."格式，如果不存在则继续执行，存在回滚事务
        /// </summary>
        SolicitationEvent
    }

    /// <summary>
    /// 命令
    /// </summary>
    public class CommandInfo
    {
        public object ShareObject = null;
        public object OriginalData = null;
        event EventHandler _solicitationEvent;
        public event EventHandler SolicitationEvent
        {
            add
            {
                _solicitationEvent += value;
            }
            remove
            {
                _solicitationEvent -= value;
            }
        }
        public void OnSolicitationEvent()
        {
            if (_solicitationEvent != null)
            {
                _solicitationEvent(this, new EventArgs());
            }
        }
        public string CommandText;
        public System.Data.Common.DbParameter[] Parameters;
        public EffentNextType EffentNextType = EffentNextType.None;

        public CommandInfo()
        {

        }

        /// <summary>
        /// 命令对象
        /// </summary>
        /// <param name="sqlText">SQL</param>
        /// <param name="para">参数列表</param>
        public CommandInfo(string sqlText, MySqlParameter[] para)
        {
            this.CommandText = sqlText;
            this.Parameters = para;
        }

        /// <summary>
        /// 命令对象
        /// </summary>
        /// <param name="sqlText">SQL</param>
        /// <param name="para">参数列表</param>
        /// <param name="type"></param>
        public CommandInfo(string sqlText, MySqlParameter[] para, EffentNextType type)
        {
            this.CommandText = sqlText;
            this.Parameters = para;
            this.EffentNextType = type;
        }
    }

    /// <summary>
    /// 数据库连接枚举
    /// </summary>
    public enum DataBase
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 数据库一 192.168.200.103
        /// </summary>
        CResource,
        /// <summary>
        /// 数据库二 192.168.200.72
        /// </summary>
        Jg,
        /// <summary>
        /// 数据库三
        /// </summary>
        DB3
    }

    /// <summary>
    /// MySQL数据访问抽象基础类
    /// </summary>
    public abstract class MySQLHelper : IDisposable
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>	
        private static string connectionString = string.Empty;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MySQLHelper()
        {

        }

        /// <summary>
        /// 自定义数据库连接
        /// </summary>
        /// <param name="db">数据库名称</param>
        /// <returns></returns>
        public static MySqlConnection MySelfSqlConnection(DataBase db)
        {
            MySqlConnection connection = new MySqlConnection();
            var conn = ConfigurationManager.ConnectionStrings[db.ToString()];
            if (conn != null)
                connection.ConnectionString = conn.ConnectionString;
            else
                throw new ConfigurationErrorsException("配置文件中没有名为" + db.ToString() + "的数据库连接字符串");
            return connection;
        }

        #region 公用方法

        /// <summary>
        /// 得到最大值,使用该方法有线程安全问题 使用LAST_INSERT_ID
        /// </summary>
        /// <param name="FieldName">字段</param>
        /// <param name="TableName">表名</param>
        /// <returns>增长后的值</returns>
        public static int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = ExecuteScalar(strsql);
            return obj == null ? 1 : int.Parse(obj.ToString());
        }

        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="SQLString">SQL</param>
        /// <returns></returns>
        public static bool Exists(string SQLString)
        {
            object obj = ExecuteScalar(SQLString);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 判断记录是否存在（基于MySqlParameter）
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static bool Exists(string SQLString, params MySqlParameter[] cmdParms)
        {
            object obj = ExecuteScalar(SQLString, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region  执行简单SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="db">数据库配置字符</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, DataBase db = DataBase.None)
        {
            using (MySqlConnection connection = (db == DataBase.None) ? new MySqlConnection(connectionString) : MySelfSqlConnection(db))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public static int ExecuteSqlTran(List<String> SQLStringList, DataBase db = DataBase.None)
        {
            using (MySqlConnection conn = (db == DataBase.None) ? new MySqlConnection(connectionString) : MySelfSqlConnection(db))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                MySqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    int count = 0;
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return count;
                }
                catch
                {
                    tx.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>
        /// <param name="SqlParameterList">多条SQL参数</param>
        /// <returns>影响的行数</returns>
        public static int ExecuteSqlTran(List<String> SQLStringList, List<MySqlParameter[]> SqlParameterList, DataBase db = DataBase.None)
        {
            using (MySqlConnection conn = (db == DataBase.None) ? new MySqlConnection(connectionString) : MySelfSqlConnection(db))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                MySqlTransaction tx = conn.BeginTransaction();
                int ac = 0;
                try
                {
                    int count = 0;
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            PrepareCommand(cmd, conn, tx, strsql, SqlParameterList[n]);
                            count += cmd.ExecuteNonQuery();
                            ac++;
                        }
                    }
                    tx.Commit();
                    return count;
                }
                catch (Exception ex)
                {
                    var a = ac;
                    var b = SQLStringList[ac];
                    var c = SqlParameterList[ac];
                    tx.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object ExecuteScalar(string SQLString, DataBase db = DataBase.None)
        {
            using (MySqlConnection connection = (db == DataBase.None) ? new MySqlConnection(connectionString) : MySelfSqlConnection(db))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSqlInsertImg(string SQLString, byte[] fs)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(SQLString, connection);
                MySql.Data.MySqlClient.MySqlParameter myParameter = new MySql.Data.MySqlClient.MySqlParameter("@fs", SqlDbType.Image);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (MySql.Data.MySqlClient.MySqlException e)
                {
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回MySqlDataReader ( 注意：调用该方法后，一定要对MySqlDataReader进行Close )
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>MySqlDataReader</returns>
        public static MySqlDataReader ExecuteReader(string SQLString)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand(SQLString, connection);
            try
            {
                connection.Open();
                MySqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (MySql.Data.MySqlClient.MySqlException e)
            {
                throw e;
            }
        }

        #endregion

        #region 返回DataSet方法

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet FindList(string SQLString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <param name="db">数据库连接字符串</param>
        /// <returns></returns>
        public static DataSet FindList(string SQLString, DataBase db)
        {
            using (MySqlConnection connection = MySelfSqlConnection(db))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">SQL</param>
        /// <param name="Times">超时时间</param>
        /// <returns></returns>
        public static DataSet FindList(string SQLString, int Times)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(SQLString, connection);
                    command.SelectCommand.CommandTimeout = Times;
                    command.Fill(ds, "ds");
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        /// <summary>
        /// 执行带参数查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">SQL</param>
        /// <param name="cmdParms">参数列表</param>
        /// <returns></returns>
        public static DataSet FindList(string SQLString, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (MySql.Data.MySqlClient.MySqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet FindList(string SQLString, List<MySqlParameter> cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms.ToArray());
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (MySql.Data.MySqlClient.MySqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }

        #endregion

        /// <summary>
        /// 返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteNonQuery(string SQLString, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的MySqlParameter[]）</param>
        public static void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlTransaction trans = conn.BeginTransaction())
                {
                    MySqlCommand cmd = new MySqlCommand();
                    try
                    {
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            MySqlParameter[] cmdParms = (MySqlParameter[])myDE.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的MySqlParameter[]）</param>
        public static int ExecuteSqlTran(System.Collections.Generic.List<CommandInfo> cmdList)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlTransaction trans = conn.BeginTransaction())
                {
                    MySqlCommand cmd = new MySqlCommand();
                    try
                    {
                        int count = 0;
                        //循环
                        foreach (CommandInfo myDE in cmdList)
                        {
                            string cmdText = myDE.CommandText;
                            MySqlParameter[] cmdParms = (MySqlParameter[])myDE.Parameters;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);

                            if (myDE.EffentNextType == EffentNextType.WhenHaveContine || myDE.EffentNextType == EffentNextType.WhenNoHaveContine)
                            {
                                if (myDE.CommandText.ToLower().IndexOf("count(") == -1)
                                {
                                    trans.Rollback();
                                    return 0;
                                }

                                object obj = cmd.ExecuteScalar();
                                bool isHave = false;
                                if (obj == null && obj == DBNull.Value)
                                {
                                    isHave = false;
                                }
                                isHave = Convert.ToInt32(obj) > 0;

                                if (myDE.EffentNextType == EffentNextType.WhenHaveContine && !isHave)
                                {
                                    trans.Rollback();
                                    return 0;
                                }
                                if (myDE.EffentNextType == EffentNextType.WhenNoHaveContine && isHave)
                                {
                                    trans.Rollback();
                                    return 0;
                                }
                                continue;
                            }
                            int val = cmd.ExecuteNonQuery();
                            count += val;
                            if (myDE.EffentNextType == EffentNextType.ExcuteEffectRows && val == 0)
                            {
                                trans.Rollback();
                                return 0;
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                        return count;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的MySqlParameter[]）</param>
        public static void ExecuteSqlTranWithIndentity(System.Collections.Generic.List<CommandInfo> SQLStringList)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlTransaction trans = conn.BeginTransaction())
                {
                    MySqlCommand cmd = new MySqlCommand();
                    try
                    {
                        int indentity = 0;
                        //循环
                        foreach (CommandInfo myDE in SQLStringList)
                        {
                            string cmdText = myDE.CommandText;
                            MySqlParameter[] cmdParms = (MySqlParameter[])myDE.Parameters;
                            foreach (MySqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.InputOutput)
                                {
                                    q.Value = indentity;
                                }
                            }
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            foreach (MySqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.Output)
                                {
                                    indentity = Convert.ToInt32(q.Value);
                                }
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句.实现数据库事务.
        /// </summary>
        /// <param name="SqlList">SQL语句的哈希表（key为sql语句，value是该语句的MySqlParameter[]）</param>
        public static void ExecuteSqlTranWithIndentity(Hashtable SqlList)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlTransaction trans = conn.BeginTransaction())
                {
                    MySqlCommand cmd = new MySqlCommand();
                    try
                    {
                        int indentity = 0;
                        //循环
                        foreach (DictionaryEntry myDE in SqlList)
                        {
                            string cmdText = myDE.Key.ToString();
                            MySqlParameter[] cmdParms = (MySqlParameter[])myDE.Value;
                            foreach (MySqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.InputOutput)
                                {
                                    q.Value = indentity;
                                }
                            }
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            foreach (MySqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.Output)
                                {
                                    indentity = Convert.ToInt32(q.Value);
                                }
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句.实现数据库事务.
        /// </summary>
        /// <param name="SqlList">SQL语句的哈希表（key为sql语句，value是该语句的MySqlParameter[]）</param>
        /// <param name="db">数据库名称</param>
        public static void ExecuteSqlTranWithIndentity(Hashtable SqlList, DataBase db)
        {
            using (MySqlConnection conn = MySelfSqlConnection(db))
            {
                conn.Open();
                using (MySqlTransaction trans = conn.BeginTransaction())
                {
                    MySqlCommand cmd = new MySqlCommand();
                    try
                    {
                        int indentity = 0;
                        //循环
                        foreach (DictionaryEntry myDE in SqlList)
                        {
                            string cmdText = myDE.Key.ToString();
                            MySqlParameter[] cmdParms = (MySqlParameter[])myDE.Value;
                            foreach (MySqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.InputOutput)
                                {
                                    q.Value = indentity;
                                }
                            }
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            foreach (MySqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.Output)
                                {
                                    indentity = Convert.ToInt32(q.Value);
                                }
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 获取首行首列
        /// </summary>
        /// <param name="SQLString">SQL</param>
        /// <returns>返回结果</returns>
        public static object ExecuteScalar(string SQLString, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 获取首行首列
        /// </summary>
        /// <param name="SQLString">>SQL</param>
        /// <param name="db">数据库</param>
        /// <param name="cmdParms">参数列表</param>
        /// <returns>返回结果</returns>
        public static object ExecuteScalar(string SQLString, DataBase db, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = MySelfSqlConnection(db))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回MySqlDataReader ( 注意：调用该方法后，一定要对MySqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>MySqlDataReader</returns>
        public static MySqlDataReader ExecuteReader(string SQLString, params MySqlParameter[] cmdParms)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                MySqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (MySql.Data.MySqlClient.MySqlException e)
            {
                throw e;
            }
            //			finally
            //			{
            //				cmd.Dispose();
            //				connection.Close();
            //			}	

        }

        /// <summary>
        /// 填充Command参数
        /// </summary>
        /// <param name="cmd">SqlCommand</param>
        /// <param name="conn">SqlConnection</param>
        /// <param name="trans">SqlTransaction</param>
        /// <param name="cmdText">Sql</param>
        /// <param name="cmdParms">参数</param>
        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, string cmdText, MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (MySqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        /// <summary>
        /// 执行带参数返回受影响的行数Sql
        /// </summary>
        /// <param name="SQLString">SQL</param>
        /// <param name="parameters">参数</param>
        /// <param name="commandtext">执行类型</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string SQLString, List<MySqlParameter> parameters, CommandType commandtext = CommandType.Text)
        {
            int results = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand(SQLString, connection);
                command.CommandType = commandtext;
                if (parameters != null)
                    foreach (var p in parameters)
                        command.Parameters.Add(p);
                results = command.ExecuteNonQuery();
                command.Parameters.Clear();
            }
            return results;
        }

        /// <summary>
        /// 返回数据集合
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="SQLString">SQL</param>
        /// <param name="readFunc">FUN</param>
        /// <param name="parameters">参数</param>
        /// <param name="commandtext">执行类型</param>
        /// <returns></returns>
        public static List<T> FindList<T>(string SQLString, Func<MySqlDataReader, T> readFunc, List<MySqlParameter> parameters, CommandType commandtext = CommandType.Text, DataBase db = DataBase.None)
        {
            List<T> results = new List<T>();
            using (MySqlConnection connection = (db == DataBase.None) ? new MySqlConnection(connectionString) : MySelfSqlConnection(db))
            {
                connection.Open();
                var command = new MySqlCommand(SQLString, connection);
                command.CommandType = commandtext;
                if (parameters != null)
                    foreach (var p in parameters)
                        command.Parameters.Add(p);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                        while (reader.Read())
                            results.Add(readFunc(reader));
                }
            }
            return results;
        }

        /// <summary>
        /// 返回一个实例对象
        /// </summary>
        /// <typeparam name="T">返回对象</typeparam>
        /// <param name="SQLString">Sql语句</param>
        /// <param name="readFunc">DataReader</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="commandtext">命令类型</param>
        /// <returns></returns>
        public static T Find<T>(string SQLString, Func<MySqlDataReader, T> readFunc, List<MySqlParameter> parameters, CommandType commandtext = CommandType.Text, DataBase db = DataBase.None)
        {
            T results = default(T);
            using (MySqlConnection connection = (db == DataBase.None) ? new MySqlConnection(connectionString) : MySelfSqlConnection(db))
            {
                connection.Open();
                var command = new MySqlCommand(SQLString, connection);
                command.CommandType = commandtext;
                if (parameters != null)
                    foreach (var p in parameters)
                        command.Parameters.Add(p);
                using (var reader = command.ExecuteReader())
                {
                    results = readFunc(reader);
                }
            }
            return results;
        }

        /// <summary>
        /// 执行带参数的SQL语句
        /// </summary>
        /// <param name="SQLString">Sql语句</param>
        /// <param name="readFunc">DataReader</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="commandtext">命令类型</param>
        public static void FindList(string SQLString, Action<MySqlDataReader> readFunc, List<MySqlParameter> parameters, CommandType commandtext = CommandType.Text, DataBase db = DataBase.None)
        {
            using (MySqlConnection connection = (db == DataBase.None) ? new MySqlConnection(connectionString) : MySelfSqlConnection(db))
            {
                connection.Open();
                var command = new MySqlCommand(SQLString, connection);
                command.CommandType = commandtext;
                if (parameters != null)
                    foreach (var p in parameters)
                        command.Parameters.Add(p);
                using (var reader = command.ExecuteReader())
                {
                    readFunc(reader);
                }
            }
        }

        #region 其它扩展公用代码
        /// <summary>
        /// 生成更新语句
        /// </summary>
        /// <param name="updateDictionary">要更新字段的字典</param>
        /// <param name="dbDataParameters">参数列表</param>
        /// <returns></returns>
        public static string ConvertSql(Dictionary<string, object> updateDictionary, List<IDbDataParameter> dbDataParameters)
        {
            StringBuilder setSql = new StringBuilder();
            if (updateDictionary != null && updateDictionary.Count > 0)
            {
                foreach (var item in updateDictionary)
                {
                    setSql.Append(",");
                    setSql.Append(item.Key);
                    setSql.Append("=");
                    setSql.Append("@");
                    setSql.Append(item.Key);
                    dbDataParameters.Add(new MySqlParameter("@" + item.Key, item.Value));// 构建参数化列表
                }
            }
            string sql = string.Empty;
            if (setSql.Length > 0)
            {
                sql = setSql.Remove(0, 1).ToString();
            }
            setSql.Clear();
            return sql;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
