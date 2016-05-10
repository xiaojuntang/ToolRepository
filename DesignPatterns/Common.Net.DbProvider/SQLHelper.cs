using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.DbProvider
{
    /// <summary>
    /// MySQL数据访问抽象基础类
    /// </summary>
    public abstract class SQLHelper : IDisposable
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>	
        private static string connectionString = string.Empty;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SQLHelper() { }

        /// <summary>
        /// 自定义数据库连接
        /// </summary>
        /// <param name="db">数据库名称</param>
        /// <returns></returns>
        private static SqlConnection MySelfSqlConnection(DataBase db)
        {
            SqlConnection connection = new SqlConnection();
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
        /// 判断记录是否存在（基于SqlParameter）
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static bool Exists(string SQLString, DataBase db, params SqlParameter[] cmdParms)
        {
            object obj = ExecuteScalar(SQLString, db, cmdParms);
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
            using (SqlConnection connection = (db == DataBase.None) ? new SqlConnection(connectionString) : MySelfSqlConnection(db))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (SqlException e)
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
            using (SqlConnection conn = (db == DataBase.None) ? new SqlConnection(connectionString) : MySelfSqlConnection(db))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
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
        public static int ExecuteSqlTran(List<String> SQLStringList, List<SqlParameter[]> SqlParameterList, DataBase db = DataBase.None)
        {
            using (SqlConnection conn = (db == DataBase.None) ? new SqlConnection(connectionString) : MySelfSqlConnection(db))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                SqlTransaction tx = conn.BeginTransaction();
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
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSqlInsertImg(string SQLString, byte[] fs)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(SQLString, connection);
                SqlParameter myParameter = new SqlParameter("@fs", SqlDbType.Image);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (SqlException e)
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
        /// 执行查询语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string SQLString)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(SQLString, connection);
            try
            {
                connection.Open();
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (SqlException e)
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
        public static DataSet FindDataSet(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (SqlException ex)
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
        public static DataSet FindDataSet(string SQLString, DataBase db)
        {
            using (SqlConnection connection = MySelfSqlConnection(db))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.SelectCommand.CommandTimeout = CommandTimeOut;
                    command.Fill(ds, "ds");
                }
                catch (SqlException ex)
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
        public static DataSet FindDataSet(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (SqlException ex)
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
        public static DataSet FindDataSet(string SQLString, List<SqlParameter> cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms.ToArray());
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (SqlException ex)
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
        public static int ExecuteNonQuery(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (SqlException e)
                    {
                        throw e;
                    }
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
        public static int ExecuteNonQuery(string SQLString, List<SqlParameter> parameters, CommandType commandtext = CommandType.Text)
        {
            int results = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(SQLString, connection);
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
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public static void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;
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
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public static int ExecuteSqlTran(System.Collections.Generic.List<CommandInfo> cmdList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        int count = 0;
                        //循环
                        foreach (CommandInfo myDE in cmdList)
                        {
                            string cmdText = myDE.CommandText;
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.Parameters;
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
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public static void ExecuteSqlTranWithIndentity(System.Collections.Generic.List<CommandInfo> SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        int indentity = 0;
                        //循环
                        foreach (CommandInfo myDE in SQLStringList)
                        {
                            string cmdText = myDE.CommandText;
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.Parameters;
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.InputOutput)
                                {
                                    q.Value = indentity;
                                }
                            }
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            foreach (SqlParameter q in cmdParms)
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
        /// <param name="SqlList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public static void ExecuteSqlTranWithIndentity(Hashtable SqlList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        int indentity = 0;
                        //循环
                        foreach (DictionaryEntry myDE in SqlList)
                        {
                            string cmdText = myDE.Key.ToString();
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.InputOutput)
                                {
                                    q.Value = indentity;
                                }
                            }
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            foreach (SqlParameter q in cmdParms)
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
        /// <param name="SqlList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        /// <param name="db">数据库名称</param>
        public static void ExecuteSqlTranWithIndentity(Hashtable SqlList, DataBase db)
        {
            using (SqlConnection conn = MySelfSqlConnection(db))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        int indentity = 0;
                        //循环
                        foreach (DictionaryEntry myDE in SqlList)
                        {
                            string cmdText = myDE.Key.ToString();
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.InputOutput)
                                {
                                    q.Value = indentity;
                                }
                            }
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            foreach (SqlParameter q in cmdParms)
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
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object ExecuteScalar(string SQLString, DataBase db = DataBase.None)
        {
            using (SqlConnection connection = (db == DataBase.None) ? new SqlConnection(connectionString) : MySelfSqlConnection(db))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
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
                    catch (SqlException e)
                    {
                        connection.Close();
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
        public static object ExecuteScalar(string SQLString, DataBase db, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = (db == DataBase.None) ? new SqlConnection(connectionString) : MySelfSqlConnection(db))
            {
                using (SqlCommand cmd = new SqlCommand())
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
                    catch (SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string SQLString, params SqlParameter[] cmdParms)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (SqlException e)
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
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
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
                foreach (SqlParameter parameter in cmdParms)
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
        /// 返回数据集合
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="SQLString">SQL</param>
        /// <param name="readFunc">FUN</param>
        /// <param name="parameters">参数</param>
        /// <param name="commandtext">执行类型</param>
        /// <returns></returns>
        public static List<T> FindList<T>(string SQLString, Func<SqlDataReader, T> readFunc, List<SqlParameter> parameters, CommandType commandtext = CommandType.Text, DataBase db = DataBase.None)
        {
            List<T> results = new List<T>();
            using (SqlConnection connection = (db == DataBase.None) ? new SqlConnection(connectionString) : MySelfSqlConnection(db))
            {
                connection.Open();
                var command = new SqlCommand(SQLString, connection);
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
        public static T Find<T>(string SQLString, Func<SqlDataReader, T> readFunc, List<SqlParameter> parameters, CommandType commandtext = CommandType.Text, DataBase db = DataBase.None)
        {
            T results = default(T);
            using (SqlConnection connection = (db == DataBase.None) ? new SqlConnection(connectionString) : MySelfSqlConnection(db))
            {
                connection.Open();
                var command = new SqlCommand(SQLString, connection);
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
        public static void FindList(string SQLString, Action<SqlDataReader> readFunc, List<SqlParameter> parameters, CommandType commandtext = CommandType.Text, DataBase db = DataBase.None)
        {
            using (SqlConnection connection = (db == DataBase.None) ? new SqlConnection(connectionString) : MySelfSqlConnection(db))
            {
                connection.Open();
                var command = new SqlCommand(SQLString, connection);
                command.CommandType = commandtext;
                command.CommandTimeout = CommandTimeOut;
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
                    dbDataParameters.Add(new SqlParameter("@" + item.Key, item.Value));// 构建参数化列表
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
            //Dispose();
        }
        #endregion

        #region 数据库批量操作
        /// <summary>
        /// 批量操作每批次记录数
        /// </summary>
        public static int BatchSize = 2000;

        /// <summary>
        /// 超时时间
        /// </summary>
        public static int CommandTimeOut = 600;
        /// <summary>
        ///大批量数据插入,返回成功插入行数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="table">数据表</param>
        /// <returns>返回成功插入行数</returns>
        public static int BulkInsert(DataTable table, DataBase db)
        {
            if (string.IsNullOrEmpty(table.TableName)) throw new Exception("请给DataTable的TableName属性附上表名称");
            if (table.Rows.Count == 0) return 0;
            int insertCount = 0;
            //string tmpPath = Path.GetTempFileName();
            //string csv = DataTableToCsv(table);
            //File.WriteAllText(tmpPath, csv);
            //using (SqlConnection conn = (db == DataBase.None) ? new SqlConnection(connectionString) : MySelfSqlConnection(db))
            //{
            //    SqlTransaction tran = null;
            //    try
            //    {
            //        conn.Open();
            //        tran = conn.BeginTransaction();
            //        SqlBulkLoader bulk = new SqlBulkLoader(conn)
            //        {
            //            FieldTerminator = ",",
            //            FieldQuotationCharacter = '"',
            //            EscapeCharacter = '"',
            //            LineTerminator = "\n",
            //            FileName = tmpPath,
            //            NumberOfLinesToSkip = 0,
            //            TableName = table.TableName,
            //        };
            //        bulk.Columns.AddRange(table.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList());
            //        insertCount = bulk.Load();
            //        tran.Commit();
            //    }
            //    catch (SqlException ex)
            //    {
            //        if (tran != null) tran.Rollback();
            //        throw ex;
            //    }
            //}
            //File.Delete(tmpPath);
            return insertCount;
        }
        /// <summary>
        ///将DataTable转换为标准的CSV
        /// </summary>
        /// <param name="table">数据表</param>
        /// <returns>返回标准的CSV</returns>
        private static string DataTableToCsv(DataTable table)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    colum = table.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else if (colum.DataType == typeof(DateTime))
                    {
                        sb.Append(Convert.ToDateTime(row[colum]).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
        /// <summary>
        ///使用SqlDataAdapter批量更新数据
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="table">数据表</param>
        public static void BatchUpdate(DataTable table, DataBase db)
        {
            using (SqlConnection connection = (db == DataBase.None) ? new SqlConnection(connectionString) : MySelfSqlConnection(db))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandTimeout = CommandTimeOut;
                command.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                SqlCommandBuilder commandBulider = new SqlCommandBuilder(adapter);
                commandBulider.ConflictOption = ConflictOption.OverwriteChanges;

                SqlTransaction transaction = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    //设置批量更新的每次处理条数
                    adapter.UpdateBatchSize = BatchSize;
                    //设置事物
                    adapter.SelectCommand.Transaction = transaction;

                    if (table.ExtendedProperties["SQL"] != null)
                    {
                        adapter.SelectCommand.CommandText = table.ExtendedProperties["SQL"].ToString();
                    }
                    adapter.Update(table);
                    transaction.Commit();/////提交事务
                }
                catch (SqlException ex)
                {
                    if (transaction != null) transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        #endregion 批量操作

        #region 数据分页方法

        /// <summary>
        /// 高效主键分页--适用于大数据
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="fieldList">需查询字段列表逗号分隔</param>
        /// <param name="tableList">表列表逗号分隔</param>
        /// <param name="whereList">where条件 不用带where关键字没有请写"",</param>
        /// <param name="keyList">主键</param>
        /// <param name="orderList">排序列表</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="_paras">参数</param>
        /// <returns></returns>
        public static DataTable GetPageTable(string conn, string fieldList, string tableList, string whereList, string orderList, int pageSize, int pageIndex, string keyList, params SqlParameter[] _paras)
        {
            //  string _sqlPageById = @" select {0} FROM {1} INNER JOIN( SELECT {2} FROM {1} {6}  {3} LIMIT {4}, {5} ) as lims using({2}}) ";//高效分页
            if (whereList.Trim().Length > 0)
            {
                whereList = " where " + whereList;
            }
            if (orderList.Trim().Length > 0)
            {
                orderList = " ORDER BY   " + orderList;
            }
            int PageStar = pageSize * (pageIndex - 1);
            string _sqlPageById = @" select " + fieldList + " FROM " + tableList + " INNER JOIN( SELECT " + keyList + " FROM " + tableList + whereList + orderList + " LIMIT " + PageStar + ", " + pageSize + " ) as lims using(" + keyList + ") ";//高效分页
            //object[] pd =
            //{
            //   fieldList, tableList, keyList, orderList, PageStar.ToString() ,
            //              pageSize.ToString(),whereList
            //};
            // _sqlPageById = string.Format(_sqlPageById, pd);
            return FindDataSet(_sqlPageById, _paras).Tables[0];//ExecuteDataTable(_sqlPageById, conn, _paras);
        }
        /// <summary>
        /// 简单分页--适用于少量数据
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="fieldList">查询字段列表</param>
        /// <param name="tableList">表列表</param>
        /// <param name="whereList">where条件</param>
        /// <param name="orderList">排序条件</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="_paras">参数</param>
        /// <returns></returns>
        public static DataTable GetPageTable(string conn, string fieldList, string tableList, string whereList, string orderList, int pageSize, int pageIndex, params SqlParameter[] _paras)
        {
            string _sqlPage = "SELECT  {0} FROM {1}  {2} {3} limit {4},{5};";//简单分页
            if (whereList.Trim().Length > 0)
            {
                whereList = " where " + whereList;
            }
            if (orderList.Trim().Length > 0)
            {
                orderList = " ORDER BY   " + orderList;
            }
            int PageStar = pageSize * (pageIndex - 1);
            _sqlPage = string.Format(_sqlPage, fieldList, tableList, whereList, orderList, PageStar, pageSize);
            return FindDataSet(_sqlPage, _paras).Tables[0];// ExecuteDataTable(_sqlPage, conn, _paras);
        }

        /// <summary>
        /// 高效主键分页--适用于大数据
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="fieldList">需查询字段列表逗号分隔</param>
        /// <param name="tableList">表列表逗号分隔</param>
        /// <param name="whereList">where条件 不用带where关键字没有请写"",</param>
        /// <param name="keyList">主键</param>
        /// <param name="orderList">排序列表</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="_paras">参数</param>
        /// <returns></returns>
        public static DataTable GetPageTable(DataBase db, string fieldList, string tableList, string whereList, string orderList, int pageSize, int pageIndex, string keyList, ref int allct, params SqlParameter[] _paras)
        {
            //  string _sqlPageById = @" select {0} FROM {1} INNER JOIN( SELECT {2} FROM {1} {6}  {3} LIMIT {4}, {5} ) as lims using({2}}) ";//高效分页
            if (whereList.Trim().Length > 0)
            {
                whereList = " where " + whereList;
            }
            if (orderList.Trim().Length > 0)
            {
                orderList = " ORDER BY   " + orderList;
            }
            int PageStar = pageSize * (pageIndex - 1);
            string _sqlPageById = @" select " + fieldList + " FROM " + tableList + " INNER JOIN( SELECT " + keyList + " FROM " + tableList + whereList + orderList + " LIMIT " + PageStar + ", " + pageSize + " ) as lims using(" + keyList + ") ";//高效分页
            allct = Convert.ToInt32(ExecuteScalar("SELECT count(*) FROM  " + tableList + "  " + whereList + "", db, _paras));
            //object[] pd =
            //{
            //   fieldList, tableList, keyList, orderList, PageStar.ToString() ,
            //              pageSize.ToString(),whereList
            //};
            // _sqlPageById = string.Format(_sqlPageById, pd);
            return FindDataSet(_sqlPageById, _paras).Tables[0];//ExecuteDataTable(_sqlPageById, conn, _paras);
        }
        /// <summary>
        /// 简单分页--适用于少量数据
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="fieldList">查询字段列表</param>
        /// <param name="tableList">表列表</param>
        /// <param name="whereList">where条件</param>
        /// <param name="orderList">排序条件</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="allct">总行数</param>
        /// <param name="_paras">参数</param>
        /// <returns></returns>
        public static DataTable GetPageTable(DataBase db, string fieldList, string tableList, string whereList, string orderList, int pageSize, int pageIndex, ref int allct, params SqlParameter[] _paras)
        {
            string _sqlPage = "SELECT  {0} FROM {1}  {2} {3} limit {4},{5};";//简单分页
            if (whereList.Trim().Length > 0)
            {
                whereList = " where " + whereList;
            }
            if (orderList.Trim().Length > 0)
            {
                orderList = " ORDER BY   " + orderList;
            }
            int PageStar = pageSize * (pageIndex - 1);
            _sqlPage = string.Format(_sqlPage, fieldList, tableList, whereList, orderList, PageStar, pageSize);
            allct = Convert.ToInt32(ExecuteScalar("SELECT count(*) FROM  " + tableList + "  " + whereList, db, _paras));
            return FindDataSet(_sqlPage, _paras).Tables[0];//ExecuteDataTable(_sqlPage, conn, _paras);
        }
        #endregion
    }


}
