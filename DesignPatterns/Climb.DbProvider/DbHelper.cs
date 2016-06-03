using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Climb.DbProvider
{
    /// <summary>
    /// 对数据库的二次封装
    /// </summary>
    public class DbHelper : IDisposable, IDbHelper
    {
        private string _queryDetaiLog = string.Empty;
        protected DbProviderFactory _dbProviderFactory;
        protected DbDataAdapter DbaAdapter;
        protected DbCommand DbCommand;
        protected DbConnection DbConnection;
        protected string dbConnectionString;
        protected static readonly ILog DbLog = LogHelper.GetLogByName("DbLog");
        protected DbTransaction DbTransaction;
        protected bool Disposed;
        private static readonly ILog DbQueryLog = LogHelper.GetLogByName("DbQueryLog");

        protected DbHelper(DbProviderFactory dbfFactory, string connectionString)
        {
            this._dbProviderFactory = dbfFactory;
            this.dbConnectionString = connectionString;
            this.DbConnection = this._dbProviderFactory.CreateConnection();
            if (this.DbConnection != null)
            {
                this.DbConnection.ConnectionString = connectionString;
            }
        }

        public DbParameter AddInParameter(string parameterName, DbType dbType, object value)
        {
            DbParameter dbParameter = this.GetDbParameter();
            if (dbParameter == null)
            {
                return null;
            }
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Value = value;
            dbParameter.Direction = ParameterDirection.Input;
            return dbParameter;
        }

        public DbParameter AddOutParameter(string parameterName, DbType dbType, int size)
        {
            DbParameter dbParameter = this.GetDbParameter();
            if (dbParameter == null)
            {
                return null;
            }
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Size = size;
            dbParameter.Direction = ParameterDirection.Output;
            return dbParameter;
        }

        public DbParameter AddParameterWithValue(string parameterName, object value)
        {
            DbParameter dbParameter = this.GetDbParameter();
            if (dbParameter == null)
            {
                return null;
            }
            dbParameter.ParameterName = parameterName;
            dbParameter.Value = value;
            dbParameter.Direction = ParameterDirection.Input;
            return dbParameter;
        }

        public DbParameter AddReturnParameter(string parameterName, DbType dbType)
        {
            DbParameter dbParameter = this.GetDbParameter();
            if (dbParameter == null)
            {
                return null;
            }
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Direction = ParameterDirection.ReturnValue;
            return dbParameter;
        }

        public void BeginTrancation()
        {
            if (this.DbConnection.State != ConnectionState.Open)
            {
                this.DbConnection.Open();
            }
            this.DbTransaction = this.DbConnection.BeginTransaction();
        }

        public void Close()
        {
            this.Dispose();
        }

        public void Commit()
        {
            this.DbTransaction.Commit();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.Disposed)
            {
                if (disposing)
                {
                    if (this.DbConnection != null)
                    {
                        this.DbConnection.Dispose();
                    }
                    if (this.DbCommand != null)
                    {
                        this.DbCommand.Dispose();
                    }
                    if (this.DbTransaction != null)
                    {
                        this.DbTransaction.Dispose();
                    }
                    if (this.DbaAdapter != null)
                    {
                        this.DbaAdapter.Dispose();
                    }
                }
                this.DbaAdapter = null;
                this.DbConnection = null;
                this.DbCommand = null;
                this.DbTransaction = null;
                this.Disposed = true;
            }
        }

        public int ExecSql(string sqlStr, params IDataParameter[] dbParameters)
        {
            int num = -1;
            this.SetDbCommandOpen(sqlStr, CommandType.Text, dbParameters);
            try
            {
                DateTime now = DateTime.Now;
                num = this.DbCommand.ExecuteNonQuery();
                DateTime dtEnd = DateTime.Now;
                this._queryDetaiLog = this._queryDetaiLog + this.GetQueryDetail(now, dtEnd);
            }
            catch (DbException exception)
            {
                this.ShowException(exception);
            }
            return num;
        }

        public void ExecSqlProc(string procName, params IDataParameter[] dbParameters)
        {
            this.SetDbCommandOpen(procName, CommandType.StoredProcedure, dbParameters);
            try
            {
                DateTime now = DateTime.Now;
                this.DbCommand.ExecuteNonQuery();
                DateTime dtEnd = DateTime.Now;
                this._queryDetaiLog = this._queryDetaiLog + this.GetQueryDetail(now, dtEnd);
            }
            catch (DbException exception)
            {
                this.ShowException(exception);
            }
        }

        public int ExecuteSqlTran(List<string> SQLStringList)
        {
            int num3;
            using (this.DbConnection)
            {
                this.BeginTrancation();
                this.DbCommand = this._dbProviderFactory.CreateCommand();
                try
                {
                    int num = 0;
                    for (int i = 0; i < SQLStringList.Count; i++)
                    {
                        string str = SQLStringList[i];
                        if (str.Trim().Length > 1)
                        {
                            this.DbCommand.Connection = this.DbConnection;
                            this.DbCommand.CommandText = str;
                            this.DbCommand.CommandType = CommandType.Text;
                            if (this.DbConnection.State != ConnectionState.Open)
                            {
                                this.DbConnection.Open();
                            }
                            num += this.DbCommand.ExecuteNonQuery();
                        }
                    }
                    this.Commit();
                    num3 = num;
                }
                catch
                {
                    this.Rollback();
                    num3 = 0;
                }
                finally
                {
                    if (this.DbConnection.State != ConnectionState.Closed)
                    {
                        this.DbConnection.Close();
                    }
                }
            }
            return num3;
        }

        public void ExecuteSqlTran(Hashtable sqlStrList)
        {
            using (this.DbConnection)
            {
                this.BeginTrancation();
                this.DbCommand = this._dbProviderFactory.CreateCommand();
                try
                {
                    foreach (DictionaryEntry entry in sqlStrList)
                    {
                        string sqlStr = entry.Key.ToString();
                        IDataParameter[] dbParameters = (IDataParameter[])entry.Value;
                        this.SetDbCommandOpen(sqlStr, CommandType.Text, dbParameters);
                        int num = this.DbCommand.ExecuteNonQuery();
                        this.DbCommand.Parameters.Clear();
                    }
                    this.Commit();
                }
                catch
                {
                    this.Rollback();
                    throw;
                }
            }
        }

        public bool Exists(string sqlStr)
        {
            object scalarFile = this.GetScalarFile(sqlStr);
            int num = 0;
            if (!object.Equals(scalarFile, null) && !object.Equals(scalarFile, DBNull.Value))
            {
                num = int.Parse(scalarFile.ToString());
            }
            return (num != 0);
        }

        public bool Exists(string sqlStr, params IDataParameter[] dbParameters)
        {
            object scalarFile = this.GetScalarFile(sqlStr, dbParameters);
            int num = 0;
            if (!object.Equals(scalarFile, null) && !object.Equals(scalarFile, DBNull.Value))
            {
                num = int.Parse(scalarFile.ToString());
            }
            return (num != 0);
        }

        ~DbHelper()
        {
            this.Dispose(false);
        }

        public T GetDataInfo<T>(string sqlStr, Func<IDataReader, T> func, params IDataParameter[] dbParameters)
        {
            List<T> source = this.GetDataInfolList<T>(sqlStr, func, dbParameters);
            T local = default(T);
            if ((source != null) && source.Any<T>())
            {
                local = source.First<T>();
            }
            return local;
        }

        public List<T> GetDataInfolList<T>(string sqlStr, Func<IDataReader, T> func, params IDataParameter[] dbParameters)
        {
            DbDataReader dataReader = this.GetDataReader(sqlStr, dbParameters);
            List<T> list = new List<T>();
            if ((dataReader != null) && dataReader.HasRows)
            {
                using (dataReader)
                {
                    while (dataReader.Read())
                    {
                        list.Add(func(dataReader));
                    }
                }
            }
            return list;
        }

        private DbDataReader GetDataReader(string sqlStr, params IDataParameter[] dbParameters)
        {
            DbDataReader reader = null;
            this.SetDbCommandOpen(sqlStr, CommandType.Text, dbParameters);
            try
            {
                DateTime now = DateTime.Now;
                reader = this.DbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                DateTime dtEnd = DateTime.Now;
                this._queryDetaiLog = this._queryDetaiLog + this.GetQueryDetail(now, dtEnd);
            }
            catch (DbException exception)
            {
                this.ShowException(exception);
            }
            return reader;
        }

        public DataSet GetDataSet(string sqlStr, params IDataParameter[] dbParameters)
        {
            DataSet dataSet = new DataSet();
            this.SetDbAdapter(sqlStr, CommandType.Text, dbParameters);
            try
            {
                DateTime now = DateTime.Now;
                if (this.DbaAdapter != null)
                {
                    this.DbaAdapter.Fill(dataSet);
                }
                DateTime dtEnd = DateTime.Now;
                this._queryDetaiLog = this._queryDetaiLog + this.GetQueryDetail(now, dtEnd);
            }
            catch (DbException exception)
            {
                this.ShowException(exception);
            }
            return dataSet;
        }

        protected virtual DbParameter GetDbParameter()
        {
            return null;
        }

        public int GetMaxID(string fieldName, string tableName)
        {
            int num = 0;
            string sqlStr = string.Format(" SELECT MAX({0}) AS MAXID FROM {1} ", fieldName, tableName);
            object scalarFile = this.GetScalarFile(sqlStr);
            if (object.Equals(scalarFile, null) || object.Equals(scalarFile, DBNull.Value))
            {
                return num;
            }
            return Convert.ToInt32(scalarFile);
        }

        public int GetMaxID(string sql, params IDataParameter[] dbParameters)
        {
            int num = 0;
            object scalarFile = this.GetScalarFile(sql, dbParameters);
            if (object.Equals(scalarFile, null) || object.Equals(scalarFile, DBNull.Value))
            {
                return num;
            }
            return Convert.ToInt32(scalarFile);
        }

        public List<T> GetPageList<T>(PageEnum pageEnum, DbPageEntity dbPage, Func<IDataReader, T> tFunc)
        {
            string comandSqlStr = dbPage.GetComandSqlStr(pageEnum);
            IDataParameter[] pageParams = dbPage.GetPageParams(pageEnum);
            return this.GetDataInfolList<T>(comandSqlStr, tFunc, pageParams);
        }

        public List<T> GetPageList<T>(PageEnum pageEnum, DbPageEntity dbPage, Func<IDataReader, T> tFunc, out int allct)
        {
            string comandCountSqlStr = dbPage.GetComandCountSqlStr();
            object scalarFile = this.GetScalarFile(comandCountSqlStr, dbPage.DbParameters);
            allct = 0;
            if (scalarFile != null)
            {
                int.TryParse(scalarFile.ToString(), out allct);
            }
            return this.GetPageList<T>(pageEnum, dbPage, tFunc);
        }

        public DataSet GetPageSet(PageEnum pageEnum, DbPageEntity dbPage)
        {
            string comandSqlStr = dbPage.GetComandSqlStr(pageEnum);
            IDataParameter[] pageParams = dbPage.GetPageParams(pageEnum);
            return this.GetDataSet(comandSqlStr, pageParams);
        }

        public DataSet GetPageSet(PageEnum pageEnum, DbPageEntity dbPage, out int allct)
        {
            string comandCountSqlStr = dbPage.GetComandCountSqlStr();
            object scalarFile = this.GetScalarFile(comandCountSqlStr, dbPage.DbParameters);
            allct = 0;
            if (scalarFile != null)
            {
                int.TryParse(scalarFile.ToString(), out allct);
            }
            return this.GetPageSet(pageEnum, dbPage);
        }

        /// <summary>
        /// 生成Sql日志
        /// </summary>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        private string GetQueryDetail(DateTime dtStart, DateTime dtEnd)
        {
            string str = "<tr style=\"background: rgb(255, 255, 255) none repeat scroll 0%; -moz-background-clip: -moz-initial; -moz-background-origin: -moz-initial; -moz-background-inline-policy: -moz-initial;\">";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            if (this.DbCommand.Parameters.Count > 0)
            {
                foreach (DbParameter parameter in this.DbCommand.Parameters)
                {
                    if (parameter != null)
                    {
                        str2 = str2 + "<td>" + parameter.ParameterName + "</td>";
                        str3 = str3 + "<td>" + parameter.DbType.ToString() + "</td>";
                        object obj2 = str4;
                        str4 = string.Concat(new object[] { obj2, "<td>", parameter.Value, "</td>" });
                    }
                }
                str5 = string.Format("<table width=\"100%\" cellspacing=\"1\" cellpadding=\"0\" style=\"background: rgb(255, 255, 255) none repeat scroll 0%; margin-top: 5px; font-size: 12px; display: block; -moz-background-clip: -moz-initial; -moz-background-origin: -moz-initial; -moz-background-inline-policy: -moz-initial;\">{0}{1}</tr>{0}{2}</tr>{0}{3}</tr></table>", new object[] { str, str2, str3, str4 });
            }
            string message = string.Format("<center><div style=\"border: 1px solid black; background:#FFF; margin: 2px; padding: 1em; text-align: left; width: 96%; clear: both;\"><div style=\"font-size: 12px; float: right; width: 100px; margin-bottom: 5px;\"><b>TIME:</b> {0}</div><span style=\"font-size: 12px;\">{1}{2}</span></div><br /></center>", dtEnd.Subtract(dtStart).TotalMilliseconds / 1000.0, this.DbCommand.CommandText, str5);
            DbQueryLog.Warn(message);
            return message;
        }

        private string GetQueryRowDetail(DateTime dtStart, DateTime dtEnd)
        {
            StringBuilder build = new StringBuilder();
            string str = "<tr style=\"background: rgb(255, 255, 255) none repeat scroll 0%; -moz-background-clip: -moz-initial; -moz-background-origin: -moz-initial; -moz-background-inline-policy: -moz-initial;\">";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            if (this.DbCommand.Parameters.Count > 0)
            {
                foreach (DbParameter parameter in this.DbCommand.Parameters)
                {
                    if (parameter != null)
                    {
                        str2 = str2 + "<td>" + parameter.ParameterName + "</td>";
                        str3 = str3 + "<td>" + parameter.DbType.ToString() + "</td>";
                        object obj2 = str4;
                        str4 = string.Concat(new object[] { obj2, "<td>", parameter.Value, "</td>" });
                    }
                }
                str5 = string.Format("<table width=\"100%\" cellspacing=\"1\" cellpadding=\"0\" style=\"background: rgb(255, 255, 255) none repeat scroll 0%; margin-top: 5px; font-size: 12px; display: block; -moz-background-clip: -moz-initial; -moz-background-origin: -moz-initial; -moz-background-inline-policy: -moz-initial;\">{0}{1}</tr>{0}{2}</tr>{0}{3}</tr></table>", new object[] { str, str2, str3, str4 });



            }
            string message = string.Format("<center><div style=\"border: 1px solid black; background:#FFF; margin: 2px; padding: 1em; text-align: left; width: 96%; clear: both;\"><div style=\"font-size: 12px; float: right; width: 100px; margin-bottom: 5px;\"><b>TIME:</b> {0}</div><span style=\"font-size: 12px;\">{1}{2}</span></div><br /></center>", dtEnd.Subtract(dtStart).TotalMilliseconds / 1000.0, this.DbCommand.CommandText, str5);
            DbQueryLog.Warn(message);
            return message;
        }

        public object GetScalarFile(string sqlStr)
        {
            object obj2 = null;
            try
            {
                DateTime now = DateTime.Now;
                obj2 = this.DbCommand.ExecuteScalar();
                DateTime dtEnd = DateTime.Now;
                this._queryDetaiLog = this._queryDetaiLog + this.GetQueryDetail(now, dtEnd);
            }
            catch (DbException exception)
            {
                this.ShowException(exception);
            }
            return obj2;
        }

        public object GetScalarFile(string sqlStr, params IDataParameter[] dbParameters)
        {
            object obj2 = null;
            this.SetDbCommandOpen(sqlStr, CommandType.Text, dbParameters);
            try
            {
                DateTime now = DateTime.Now;
                obj2 = this.DbCommand.ExecuteScalar();
                DateTime dtEnd = DateTime.Now;
                this._queryDetaiLog = this._queryDetaiLog + this.GetQueryDetail(now, dtEnd);
            }
            catch (DbException exception)
            {
                this.ShowException(exception);
            }
            return obj2;
        }

        public DataSet GetSetSqlProc(string procName, params IDataParameter[] dbParameters)
        {
            DataSet dataSet = new DataSet();
            this.SetDbCommandOpen(procName, CommandType.StoredProcedure, dbParameters);
            this.DbaAdapter = this._dbProviderFactory.CreateDataAdapter();
            if (this.DbaAdapter != null)
            {
                this.DbaAdapter.SelectCommand = this.DbCommand;
                try
                {
                    DateTime now = DateTime.Now;
                    if (this.DbaAdapter != null)
                    {
                        this.DbaAdapter.Fill(dataSet);
                    }
                    DateTime dtEnd = DateTime.Now;
                    this._queryDetaiLog = this._queryDetaiLog + this.GetQueryDetail(now, dtEnd);
                }
                catch (DbException exception)
                {
                    this.ShowException(exception);
                }
            }
            return dataSet;
        }

        public void Rollback()
        {
            this.DbTransaction.Rollback();
        }

        private void SetDbAdapter(string sqlStr, CommandType commandType, IDataParameter[] dbParameters)
        {
            this.SetDbCommandOpen(sqlStr, commandType, dbParameters);
            this.DbaAdapter = this._dbProviderFactory.CreateDataAdapter();
            if (this.DbaAdapter != null)
            {
                this.DbaAdapter.SelectCommand = this.DbCommand;
            }
        }

        private void SetDbCommandOpen(string sqlStr, CommandType commandType, params IDataParameter[] dbParameters)
        {
            this.DbCommand = this._dbProviderFactory.CreateCommand();
            if (this.DbCommand != null)
            {
                this.DbCommand.Connection = this.DbConnection;
                this.DbCommand.CommandText = sqlStr;
                this.DbCommand.CommandType = commandType;
                if (dbParameters.Length > 0)
                {
                    foreach (IDataParameter parameter in dbParameters)
                    {
                        this.DbCommand.Parameters.Add(parameter);
                    }
                }
            }
            if (this.DbConnection.State != ConnectionState.Open)
            {
                try
                {
                    this.DbConnection.Open();
                }
                catch (DbException exception)
                {
                    this.ShowException(exception);
                }
            }
        }

        private void ShowException(DbException dbException)
        {
            StringBuilder message = new StringBuilder();
            message.Append(DateTime.Now);
            message.Append("错误信息：");
            message.Append(dbException.Message);
            message.Append("\t");
            message.AppendLine("Sql语句：");
            message.Append(this.DbCommand.CommandText);
            message.AppendLine("参数信息：");
            message.AppendLine("参数名字\t参数类型\t参数值");
            foreach (DbParameter parameter in this.DbCommand.Parameters)
            {
                if (parameter != null)
                {
                    message.AppendLine(parameter.ParameterName);
                    message.Append("\t");
                    message.Append(parameter.DbType);
                    message.Append("\t");
                    message.Append(parameter.Value);
                }
            }
            DbLog.Error(message);
        }

        protected string DbConnectionString
        {
            get
            {
                return this.dbConnectionString;
            }
        }

        public string QueryDetaiLog
        {
            get
            {
                return this._queryDetaiLog;
            }
            set
            {
                this._queryDetaiLog = value;
            }
        }
    }
}
