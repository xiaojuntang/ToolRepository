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
    internal interface IDbHelper
    {
        DbParameter AddInParameter(string parameterName, DbType dbType, object value);
        DbParameter AddOutParameter(string parameterName, DbType dbType, int size);
        DbParameter AddReturnParameter(string parameterName, DbType dbType);
        void BeginTrancation();
        void Close();
        void Commit();
        void Dispose();
        int ExecSql(string sqlStr, params IDataParameter[] dbParameters);
        void ExecSqlProc(string procName, params IDataParameter[] dbParameters);
        int ExecuteSqlTran(List<string> sqlStrList);
        void ExecuteSqlTran(Hashtable sqlStrList);
        bool Exists(string sqlStr);
        bool Exists(string strSql, params IDataParameter[] dbParameters);
        T GetDataInfo<T>(string sqlStr, Func<IDataReader, T> func, params IDataParameter[] dbParameters);
        List<T> GetDataInfolList<T>(string sqlStr, Func<IDataReader, T> func, params IDataParameter[] dbParameters);
        DataSet GetDataSet(string sqlStr, params IDataParameter[] dbParameters);
        int GetMaxID(string fieldName, string tableName);
        int GetMaxID(string sqlStr, params IDataParameter[] dbParameters);
        List<T> GetPageList<T>(PageEnum pageEnum, DbPageEntity dbPage, Func<IDataReader, T> tFunc);
        DataSet GetPageSet(PageEnum pageEnum, DbPageEntity dbPage);
        object GetScalarFile(string sqlStr);
        object GetScalarFile(string sqlStr, params IDataParameter[] dbParameters);
        DataSet GetSetSqlProc(string procName, params IDataParameter[] dbParameters);
        void Rollback();
    }
}
