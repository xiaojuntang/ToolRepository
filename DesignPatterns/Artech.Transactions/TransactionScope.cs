using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace Artech.Transactions
{
    public class TransactionScope : IDisposable
    {
        private Transaction transaction = Transaction.Current;
        public bool Completed { get; private set; }

        public TransactionScope(string connectionStringName, IsolationLevel isolationLevel = IsolationLevel.Unspecified,
            Func<string, DbProviderFactory> getFactory = null)
        {
            if (null == transaction)
            {
                if (null == getFactory)
                {
                    getFactory = cnnstringName => DbHelper.GetFactory(cnnstringName);
                }
                DbProviderFactory factory = getFactory(connectionStringName);
                DbConnection connection = factory.CreateConnection();
                connection.ConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
                connection.Open();
                DbTransaction dbTransaction = connection.BeginTransaction(isolationLevel);
                Transaction.Current = new CommittableTransaction(dbTransaction);
            }
            else
            {
                Transaction.Current = transaction.DependentClone();
            }
        }

        public void Complete()
        {
            this.Completed = true;
        }
        public void Dispose()
        {
            Transaction current = Transaction.Current;
            Transaction.Current = transaction;
            if (!this.Completed)
            {
                current.Rollback();
            }
            CommittableTransaction committableTransaction = current as CommittableTransaction;
            if (null != committableTransaction)
            {
                if (this.Completed)
                {
                    committableTransaction.Commit();
                }
                committableTransaction.Dispose();
            }
        }
    }

    public class DbHelper
    {
        public DbProviderFactory DbProviderFactory { get; private set; }
        public string ConnectionString { get; private set; }
        public virtual DbParameter BuildDbParameter(string name, object value)
        {
            DbParameter parameter = this.DbProviderFactory.CreateParameter();
            parameter.ParameterName = "@" + name;
            parameter.Value = value;
            return parameter;
        }

        public DbHelper(string cnnStringName)
        {
            var cnnStringSection = ConfigurationManager.ConnectionStrings[cnnStringName];
            this.DbProviderFactory = DbProviderFactories.GetFactory(cnnStringSection.ProviderName);
            this.ConnectionString = cnnStringSection.ConnectionString;
        }
        public static DbProviderFactory GetFactory(string cnnStringName)
        {
            var cnnStringSection = ConfigurationManager.ConnectionStrings[cnnStringName];
            return DbProviderFactories.GetFactory(cnnStringSection.ProviderName);
        }

        public int ExecuteNonQuery(string commandText, IDictionary<string, object> parameters)
        {
            DbConnection connection = null;
            DbCommand command = this.DbProviderFactory.CreateCommand();
            DbTransaction dbTransaction = null;
            try
            {
                command.CommandText = commandText;
                parameters = parameters ?? new Dictionary<string, object>();
                foreach (var item in parameters)
                {
                    command.Parameters.Add(this.BuildDbParameter(item.Key, item.Value));
                }
                if (null != Artech.Transactions.Transaction.Current)
                {
                    command.Connection = Artech.Transactions.Transaction.Current.DbTransactionWrapper.DbTransaction.Connection;
                    command.Transaction = Artech.Transactions.Transaction.Current.DbTransactionWrapper.DbTransaction;
                }
                else
                {
                    connection = this.DbProviderFactory.CreateConnection();
                    connection.ConnectionString = this.ConnectionString;
                    command.Connection = connection;
                    connection.Open();
                    if (System.Transactions.Transaction.Current == null)
                    {
                        dbTransaction = connection.BeginTransaction();
                        command.Transaction = dbTransaction;
                    }
                }
                int result = command.ExecuteNonQuery();
                if (null != dbTransaction)
                {
                    dbTransaction.Commit();
                }
                return result;
            }
            catch
            {
                if (null != dbTransaction)
                {
                    dbTransaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (null != connection)
                {
                    connection.Dispose();
                }
                if (null != dbTransaction)
                {
                    dbTransaction.Dispose();
                }
                command.Dispose();
            }
        }
    }
}
