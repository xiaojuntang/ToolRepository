using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Common.Tools
{
    /// <summary>
    /// Oralce数据库访问
    /// </summary>
    public abstract class OracleHelper
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["OraConn"].ConnectionString;

        protected OracleHelper() {
        }

        #region 公用方法

        /// <summary>
        /// 获取NextId
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static int GetMaxId(string fieldName, string tableName) {
            string strsql = "select max(" + fieldName + ")+1 from " + tableName;
            object obj = OracleHelper.GetSingle(strsql);
            return obj == null ? 1 : int.Parse(obj.ToString());
        }

        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static bool Exists(string strSql, params OracleParameter[] cmdParms) {
            object obj = OracleHelper.GetSingle(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value))) {
                cmdresult = 0;
            }
            else {
                cmdresult = int.Parse(obj.ToString());
            }
            return cmdresult != 0;
        }

        #endregion

        #region  执行简单SQL语句
        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool Exists(string sql) {
            using (OracleConnection connection = new OracleConnection(ConnectionString)) {
                using (OracleCommand cmd = new OracleCommand(sql, connection)) {
                    try {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        return Convert.ToInt32(obj) > 0;
                    }
                    catch (Exception e) {
                        connection.Close();
                        throw new Exception(e.Message);
                    }
                    finally {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string sql) {
            using (OracleConnection connection = new OracleConnection(ConnectionString)) {
                using (OracleCommand cmd = new OracleCommand(sql, connection)) {
                    try {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (Exception E) {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
                    finally {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqlList">多条SQL语句</param>  
        public static bool ExecuteSqlTran(ArrayList sqlList)
        {
            bool re = false;
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            {
                connection.Open();
                OracleCommand cmd = new OracleCommand {Connection = connection};
                OracleTransaction tx = connection.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    foreach (object sql in sqlList)
                    {
                        string strsql = sql.ToString();
                        if (strsql.Trim().Length <= 1) continue;
                        cmd.CommandText = strsql;
                        cmd.ExecuteNonQuery();
                    }
                    tx.Commit();
                    re = true;
                }
                catch (Exception e)
                {
                    re = false;
                    tx.Rollback();
                    throw new Exception(e.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
            return re;
        }

        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string sql, string content) {
            using (OracleConnection connection = new OracleConnection(ConnectionString)) {
                OracleCommand cmd = new OracleCommand(sql, connection);
                OracleParameter myParameter = new OracleParameter("@content", OracleDbType.NVarchar2);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (Exception E) {
                    throw new Exception(E.Message);
                }
                finally {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSqlInsertImg(string strSql, byte[] fs) {
            using (OracleConnection connection = new OracleConnection(ConnectionString)) {
                OracleCommand cmd = new OracleCommand(strSql, connection);
                OracleParameter myParameter = new OracleParameter("@fs", OracleDbType.LongRaw);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (Exception e) {
                    throw new Exception(e.Message);
                }
                finally {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="sql">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string sql) {
            using (OracleConnection connection = new OracleConnection(ConnectionString)) {
                using (OracleCommand cmd = new OracleCommand(sql, connection)) {
                    try {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value))) {
                            return null;
                        }
                        else {
                            return obj;
                        }
                    }
                    catch (Exception e) {
                        connection.Close();
                        throw new Exception(e.Message);
                    }
                    finally {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回OracleDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <returns>OracleDataReader</returns>
        public static OracleDataReader ExecuteReader(string strSql) {
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand cmd = new OracleCommand(strSql, connection);
            try {
                connection.Open();
                OracleDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (OracleException e) {
                throw new Exception(e.Message);
            }


        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string sqlString) {
            using (OracleConnection connection = new OracleConnection(ConnectionString)) {
                DataSet ds = new DataSet();
                try {
                    connection.Open();
                    OracleDataAdapter command = new OracleDataAdapter(sqlString, connection);
                    command.Fill(ds, "ds");
                }
                catch (OracleException ex) {
                    throw new Exception(ex.Message);
                }
                finally {
                    connection.Close();
                }
                return ds;
            }
        }


        #endregion

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="cmdParms"></param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string sqlString, params OracleParameter[] cmdParms) {
            using (OracleConnection connection = new OracleConnection(ConnectionString)) {
                using (OracleCommand cmd = new OracleCommand()) {
                    try {
                        PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (OracleException E) {
                        throw new Exception(E.Message);
                    }
                    finally {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }


        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqlList">SQL语句的哈希表（key为sql语句，value是该语句的OracleParameter[]）</param>
        public static void ExecuteSqlTran(Hashtable sqlList) {
            using (OracleConnection conn = new OracleConnection(ConnectionString)) {
                conn.Open();
                using (OracleTransaction trans = conn.BeginTransaction()) {
                    OracleCommand cmd = new OracleCommand();
                    try {
                        //循环
                        foreach (DictionaryEntry sql in sqlList) {
                            string cmdText = sql.Key.ToString();
                            OracleParameter[] cmdParms = (OracleParameter[])sql.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                            trans.Commit();
                        }
                    }
                    catch {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }


        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="sqlString">计算查询结果语句</param>
        /// <param name="cmdParms"></param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string sqlString, params OracleParameter[] cmdParms) {
            using (OracleConnection connection = new OracleConnection(ConnectionString)) {
                using (OracleCommand cmd = new OracleCommand()) {
                    try {
                        PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value))) {
                            return null;
                        }
                        else {
                            return obj;
                        }
                    }
                    catch (OracleException e) {
                        throw new Exception(e.Message);
                    }
                    finally {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回OracleDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <param name="cmdParms"></param>
        /// <returns>OracleDataReader</returns>
        public static OracleDataReader ExecuteReader(string sqlString, params OracleParameter[] cmdParms) {
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand cmd = new OracleCommand();
            try {
                PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                OracleDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (OracleException e) {
                throw new Exception(e.Message);
            }

        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <param name="cmdParms"></param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string sqlString, params OracleParameter[] cmdParms) {
            using (OracleConnection connection = new OracleConnection(ConnectionString)) {
                OracleCommand cmd = new OracleCommand();
                PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                using (OracleDataAdapter da = new OracleDataAdapter(cmd)) {
                    DataSet ds = new DataSet();
                    try {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (OracleException ex) {
                        throw new Exception(ex.Message);
                    }
                    finally {
                        cmd.Dispose();
                        connection.Close();
                    }
                    return ds;
                }
            }
        }

        private static void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans,
            string cmdText, OracleParameter[] cmdParms) {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;
            if (cmdParms == null) return;
            foreach (OracleParameter parm in cmdParms)
                cmd.Parameters.Add(parm);
            }

        #endregion

        #region 存储过程操作

        /// <summary>
        /// 执行存储过程 返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>OracleDataReader</returns>
        public static OracleDataReader RunProcedureReader(string storedProcName, IDataParameter[] parameters) {
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleDataReader returnReader;
            connection.Open();
            OracleCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.CommandType = CommandType.StoredProcedure;
            returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            return returnReader;
        }

        /// <summary>
        /// 执行存储过程，返回影响的行数  
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected) {
            using (OracleConnection connection = new OracleConnection(ConnectionString)) {
                int result;
                connection.Open();
                OracleCommand command = BuildIntCommand(connection, storedProcName, parameters);
                rowsAffected = command.ExecuteNonQuery();
                result = (int)command.Parameters["ReturnValue"].Value;
                return result;
            }
        }

        /// <summary>
        /// 执行存储过程，什么值也不返回 
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <param name="parameters"></param>
        public static void RunProcedure(string storedProcName, OracleParameter[] parameters) {
            using (OracleConnection connection = new OracleConnection(ConnectionString)) {
                OracleCommand cmd = BuildQueryCommand(connection, storedProcName, parameters);
                try {
                    if (connection.State != ConnectionState.Open) {
                        connection.Open();
                    }
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex) {
                    throw new Exception(ex.Message);
                }
                finally {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行存储过程,返回数据集
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public static DataSet RunProcedureGetDataSet(string storedProcName, OracleParameter[] parameters) {
            using (OracleConnection connection = new OracleConnection(ConnectionString)) {
                DataSet dataSet = new DataSet();
                connection.Open();
                OracleDataAdapter sqlDA = new OracleDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dataSet, "dt");
                connection.Close();
                return dataSet;
            }
        }

        /// <summary>
        /// 构建 OracleCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>OracleCommand</returns>
        private static OracleCommand BuildQueryCommand(OracleConnection connection, string storedProcName,
            IDataParameter[] parameters) {
            OracleCommand command = new OracleCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (OracleParameter parameter in parameters) {
                command.Parameters.Add(parameter);
            }
            return command;
        }

        /// <summary>
        /// 执行存储过程，返回影响的行数  
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public static int RunProcedure_rowsAffected(string storedProcName, IDataParameter[] parameters,
            out int rowsAffected) {
            using (OracleConnection connection = new OracleConnection(ConnectionString)) {
                int result;
                connection.Open();
                OracleCommand command = BuildIntCommand(connection, storedProcName, parameters);
                rowsAffected = command.ExecuteNonQuery();
                result = (int)command.Parameters["ReturnValue"].Value;
                //Connection.Close();
                return result;
            }
        }

        /// <summary>
        /// 创建 OracleCommand 对象实例(用来返回一个整数值) 
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>OracleCommand 对象实例</returns>
        private static OracleCommand BuildIntCommand(OracleConnection connection, string storedProcName,
            IDataParameter[] parameters) {
            OracleCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.Parameters.Add(new OracleParameter("ReturnValue",
                OracleDbType.Int32, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }
        #endregion
    }
}
