/***************************************************************************** 
*        filename :          MsSqlHelper 
*        描述 :              MSSQL数据库访问抽象基础类库
*        创建者              唐晓军（QQ:417281862）
*        CLR版本:            4.0.30319.42000 
*        新建项输入的名称:   MsSqlHelper 
*        机器名称:           LD 
*        注册组织名:         无
*        命名空间名称:       Common.Net.DbProvider
*        文件名:             MsSqlHelper 
*        创建系统时间:       2016/07/05 10:13:10 
*        创建年份:           2016 
/*****************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Common.Net.DbProvider
{
    /// <summary>
    /// MSSQL数据库访问抽象基础类库
    /// <remarks>作者：唐晓军 QQ:417281862</remarks>
    /// </summary>
    public abstract class MsSqlHelper : IDisposable
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        protected MsSqlHelper() { }

        /// <summary>
        /// 创建自定义数据库连接
        /// </summary>
        /// <param name="db">数据库配置</param>
        /// <returns>SqlConnection对象</returns>
        private static SqlConnection MsSqlConnection(string db = DBConnect.ConnStr)
        {
            SqlConnection connection = new SqlConnection();
            var conn = ConfigurationManager.ConnectionStrings[db];
            if (conn != null)
                connection.ConnectionString = conn.ConnectionString;
            else
                throw new ConfigurationErrorsException("配置文件中没有名为" + db + "的数据库连接字符串");
            return connection;
        }

        #region Exists
        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="db">数据库配置</param>
        /// <returns></returns>
        public static bool Exists(string sql, string db = DBConnect.ConnStr)
        {
            var obj = ExecuteScalar(sql, db);
            int cmdresult;
            if ((object.Equals(obj, null)) || (object.Equals(obj, DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            return cmdresult != 0;
        }

        /// <summary>
        /// 判断记录是否存在（基于SqlParameter）
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="db">数据库配置</param>
        /// <param name="cmdParms">动态参数</param>
        /// <returns></returns>
        public static bool Exists(string sql, string db = DBConnect.ConnStr, params SqlParameter[] cmdParms)
        {
            var obj = ExecuteScalar(sql, db, cmdParms);
            int cmdresult;
            if ((object.Equals(obj, null)) || (object.Equals(obj, DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            return cmdresult != 0;
        } 
        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// 执行SQL语句返回影响的记录数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="db">数据库配置字符</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteNonQuery(string sql, string db = DBConnect.ConnStr)
        {
            using (SqlConnection connection = MsSqlConnection(db))
            {
                using (SqlCommand cmd = new SqlCommand(sql, connection))
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
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 返回影响的记录数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="db"></param>
        /// <param name="cmdParms"></param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteNonQuery(string sql, string db = DBConnect.ConnStr, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = MsSqlConnection(db))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, sql, cmdParms);
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
        /// <param name="sql">SQL</param>
        /// <param name="parameters">参数</param>
        /// <param name="commandtext">执行类型</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, List<SqlParameter> parameters, string db = DBConnect.ConnStr, CommandType commandtext = CommandType.Text)
        {
            int results;
            using (SqlConnection connection = MsSqlConnection(db))
            {
                connection.Open();
                var command = new SqlCommand(sql, connection);
                command.CommandType = commandtext;
                if (parameters != null)
                    foreach (var p in parameters)
                        command.Parameters.Add(p);
                results = command.ExecuteNonQuery();
                command.Parameters.Clear();
            }
            return results;
        }
        #endregion

        #region ExecuteScalar
        /// <summary>
        /// 获取首行首列
        /// </summary>
        /// <param name="sql">计算查询结果语句</param>
        /// <param name="db"></param>
        /// <returns>查询结果（object）</returns>
        public static object ExecuteScalar(string sql, string db = DBConnect.ConnStr)
        {
            using (SqlConnection connection = MsSqlConnection(db))
            {
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Equals(obj, null)) || (object.Equals(obj, System.DBNull.Value)))
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
        /// <param name="sql">>SQL</param>
        /// <param name="db">数据库</param>
        /// <param name="cmdParms">参数列表</param>
        /// <returns>返回结果</returns>
        public static object ExecuteScalar(string sql, string db = DBConnect.ConnStr, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = MsSqlConnection(db))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, sql, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((object.Equals(obj, null)) || (object.Equals(obj, System.DBNull.Value)))
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
        #endregion

        #region SqlTransaction
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqlList">Sql列表</param>
        /// <param name="db">数据库配置字符</param>
        /// <returns></returns>
        public static int ExecuteSqlTran(List<string> sqlList, string db = DBConnect.ConnStr)
        {
            using (SqlConnection connection = MsSqlConnection(db))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                SqlTransaction tx = connection.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    int count = 0;
                    for (int n = 0; n < sqlList.Count; n++)
                    {
                        string strsql = sqlList[n];
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
        /// <param name="sqlList">多条SQL语句</param>
        /// <param name="sqlParameterList">多条SQL参数</param>
        /// <param name="db"></param>
        /// <returns>影响的行数</returns>
        public static int ExecuteSqlTran(List<string> sqlList, List<SqlParameter[]> sqlParameterList, string db = DBConnect.ConnStr)
        {
            using (SqlConnection connection = MsSqlConnection(db))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                SqlTransaction tx = connection.BeginTransaction();
                int ac = 0;
                try
                {
                    int count = 0;
                    for (int n = 0; n < sqlList.Count; n++)
                    {
                        string strsql = sqlList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            PrepareCommand(cmd, connection, tx, strsql, sqlParameterList == null ? null : sqlParameterList[n]);
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
                    var b = sqlList[ac];
                    var c = sqlParameterList[ac];
                    tx.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <param name="db"></param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSqlInsertImg(string sql, byte[] fs, string db = DBConnect.ConnStr)
        {
            using (SqlConnection connection = MsSqlConnection(db))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
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
        /// 使用Hashtable执行多条SQL语句实现数据库事务。
        /// </summary>
        /// <param name="sqlList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        /// <param name="db">数据库配置</param>
        public static void ExecuteSqlTran(Hashtable sqlList, string db = DBConnect.ConnStr)
        {
            using (SqlConnection connection = MsSqlConnection(db))
            {
                connection.Open();
                using (SqlTransaction trans = connection.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        foreach (DictionaryEntry item in sqlList)
                        {
                            string cmdText = item.Key.ToString();
                            SqlParameter[] cmdParms = (SqlParameter[])item.Value;
                            PrepareCommand(cmd, connection, trans, cmdText, cmdParms);
                            cmd.ExecuteNonQuery();
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
        /// <param name="cmdList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        /// <param name="db"></param>
        public static int ExecuteSqlTran(List<CommandInfo> cmdList, string db = DBConnect.ConnStr)
        {
            using (SqlConnection connection = MsSqlConnection(db))
            {
                connection.Open();
                using (SqlTransaction trans = connection.BeginTransaction())
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
                            PrepareCommand(cmd, connection, trans, cmdText, cmdParms);

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
        /// <param name="cmdList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        /// <param name="db"></param>
        public static void ExecuteSqlTranWithIndentity(List<CommandInfo> cmdList, string db = DBConnect.ConnStr)
        {
            using (SqlConnection connection = MsSqlConnection(db))
            {
                connection.Open();
                using (SqlTransaction trans = connection.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        int indentity = 0;
                        //循环
                        foreach (CommandInfo myDE in cmdList)
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
                            PrepareCommand(cmd, connection, trans, cmdText, cmdParms);
                            cmd.ExecuteNonQuery();
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
        /// <param name="sqlList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        /// <param name="db"></param>
        public static void ExecuteSqlTranWithIndentity(Hashtable sqlList, string db = DBConnect.ConnStr)
        {
            using (SqlConnection connection = MsSqlConnection(db))
            {
                connection.Open();
                using (SqlTransaction trans = connection.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        int indentity = 0;
                        //循环
                        foreach (DictionaryEntry myDE in sqlList)
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
                            PrepareCommand(cmd, connection, trans, cmdText, cmdParms);
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
        #endregion

        /// <summary>
        /// 执行查询语句，返回SqlDataReader
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string sql, string db = DBConnect.ConnStr)
        {
            using (SqlConnection connection = MsSqlConnection(db))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
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

        #region 返回DataSet方法

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="db">数据库连接字符串</param>
        /// <returns></returns>
        public static DataSet FindDataSet(string sql, string db = DBConnect.ConnStr)
        {
            using (SqlConnection connection = MsSqlConnection(db))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(sql, connection);
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
        /// <param name="sql">SQL</param>
        /// <param name="db"></param>
        /// <param name="cmdParms">参数列表</param>
        /// <returns></returns>
        public static DataSet FindDataSet(string sql, string db = DBConnect.ConnStr, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = MsSqlConnection(db))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, sql, cmdParms);
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
        /// <param name="sql">查询语句</param>
        /// <param name="cmdParms"></param>
        /// <param name="db"></param>
        /// <returns>DataSet</returns>
        public static DataSet FindDataSet(string sql, List<SqlParameter> cmdParms, string db = DBConnect.ConnStr)
        {
            using (SqlConnection connection = MsSqlConnection(db))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, sql, cmdParms.ToArray());
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

        #region FindList查询
        /// <summary>
        /// 返回数据集合
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="sql">SQL</param>
        /// <param name="readFunc">FUN</param>
        /// <param name="parameters">参数</param>
        /// <param name="commandtext">执行类型</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static List<T> FindList<T>(string sql, Func<SqlDataReader, T> readFunc, List<SqlParameter> parameters, string db = DBConnect.ConnStr, CommandType commandtext = CommandType.Text)
        {
            List<T> results = new List<T>();
            using (SqlConnection connection = MsSqlConnection(db))
            {
                connection.Open();
                var command = new SqlCommand(sql, connection);
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
        /// <param name="sql">Sql语句</param>
        /// <param name="readFunc">DataReader</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static T Find<T>(string sql, Func<SqlDataReader, T> readFunc, List<SqlParameter> parameters, string db = DBConnect.ConnStr, CommandType commandType = CommandType.Text)
        {
            T results = default(T);
            using (SqlConnection connection = MsSqlConnection(db))
            {
                connection.Open();
                var command = new SqlCommand(sql, connection);
                command.CommandType = commandType;
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
        /// 返回一个实例对象
        /// </summary>
        /// <typeparam name="T">返回对象</typeparam>
        /// <param name="sql">Sql语句</param>
        /// <param name="readFunc">DataReader</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="db">数据库名称</param>
        /// <returns></returns>
        public static T Find<T>(string sql, Func<SqlDataReader, T> readFunc, List<SqlParameter> parameters, string db = DBConnect.ConnStr)
        {
            T results = default(T);
            using (SqlConnection connection = MsSqlConnection(db))
            {
                connection.Open();
                var command = new SqlCommand(sql, connection);
                command.CommandType = CommandType.Text;
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
        /// <param name="sql">Sql语句</param>
        /// <param name="readFunc">DataReader</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="commandtext">命令类型</param>
        /// <param name="db"></param>
        public static void FindList(string sql, Action<SqlDataReader> readFunc, List<SqlParameter> parameters, CommandType commandtext = CommandType.Text, string db = DBConnect.ConnStr)
        {
            using (SqlConnection connection = MsSqlConnection(db))
            {
                connection.Open();
                var command = new SqlCommand(sql, connection);
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

        /// <summary>
        /// 执行带参数的SQL语句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="readFunc">DataReader</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="db">命令类型</param>
        public static void FindList(string sql, Action<SqlDataReader> readFunc, List<SqlParameter> parameters, string db = DBConnect.ConnStr)
        {
            using (SqlConnection connection = MsSqlConnection(db))
            {
                connection.Open();
                var command = new SqlCommand(sql, connection);
                command.CommandType = CommandType.Text;
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
        #endregion

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
        /// 大批量数据插入,返回成功插入行数
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="db"></param>
        /// <returns>返回成功插入行数</returns>
        public static int BulkInsert(DataTable table, string db = DBConnect.ConnStr)
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
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    var colum = table.Columns[i];
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
        /// 使用SqlDataAdapter批量更新数据
        /// </summary>
        /// <param name="table"></param>
        /// <param name="db"></param>
        public static void BatchUpdate(DataTable table, string db = DBConnect.ConnStr)
        {
            using (SqlConnection connection = MsSqlConnection(db))
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

        /// <summary> 
        /// 大批量插入数据(2000每批次) 已采用整体事物控制 
        /// </summary> 
        /// <param name="dt">含有和目标数据库表结构完全一致(所包含的字段名完全一致即可)的DataTable</param>
        /// <param name="db"></param> 
        public static void BulkCopy(DataTable dt, string db = DBConnect.ConnStr)
        {
            if (string.IsNullOrEmpty(dt.TableName)) throw new Exception("请给DataTable的TableName属性附上表名称");
            using (SqlConnection connection = MsSqlConnection(db))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.BatchSize = 2000;
                        bulkCopy.BulkCopyTimeout = 120;
                        bulkCopy.DestinationTableName = dt.TableName;
                        try
                        {
                            foreach (DataColumn col in dt.Columns)
                            {
                                bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                            }
                            bulkCopy.WriteToServer(dt);
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
        }

        /// <summary> 
        /// 批量更新数据(每批次5000) 
        /// 若只是需要大批量插入数据使用bcp是最好的，若同时需要插入、删除、更新建议使用SqlDataAdapter我测试过有很高的效率，一般情况下这两种就满足需求了 
        /// </summary> 
        /// <param name="dt"></param>
        /// <param name="db"></param> 
        public static void BulkUpdate(DataTable dt, string db = DBConnect.ConnStr)
        {
            using (SqlConnection connection = MsSqlConnection(db))
            {
                SqlCommand comm = connection.CreateCommand();
                comm.CommandTimeout = 120;
                comm.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter(comm);
                SqlCommandBuilder commandBulider = new SqlCommandBuilder(adapter);
                commandBulider.ConflictOption = ConflictOption.OverwriteChanges;
                try
                {
                    connection.Open();
                    //设置批量更新的每次处理条数 
                    adapter.UpdateBatchSize = 5000;
                    adapter.SelectCommand.Transaction = connection.BeginTransaction();
                    if (dt.ExtendedProperties["SQL"] != null)
                    {
                        adapter.SelectCommand.CommandText = dt.ExtendedProperties["SQL"].ToString();
                    }
                    adapter.Update(dt);
                    adapter.SelectCommand.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    if (adapter.SelectCommand != null && adapter.SelectCommand.Transaction != null)
                    {
                        adapter.SelectCommand.Transaction.Rollback();
                    }
                    throw;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            };
        }
        #endregion 批量操作
    }
}
