/***************************************************************************** 
*        filename :AbstractFactory 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   AbstractFactory 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       AbstractFactoryPatterns 
*        文件名:             AbstractFactory 
*        创建系统时间:       2015/12/14 11:11:01 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractFactoryPatterns
{
    /// <summary>
    /// 数据库抽象工厂接口
    /// </summary>
    public interface AbstractDbFactory
    {
        /// <summary> 
        /// 建立默认连接 
        /// </summary> 
        /// <returns>数据库连接</returns> 
        IDbConnection CreateConnection();

        /// <summary> 
        /// 根据连接字符串建立Connection对象 
        /// </summary> 
        /// <param name="strConn">连接字符串</param> 
        /// <returns>Connection对象</returns> 
        IDbConnection CreateConnection(string strConn);

        /// <summary> 
        /// 建立Command对象 
        /// </summary> 
        /// <returns>Command对象</returns> 
        IDbCommand CreateCommand();

        /// <summary> 
        /// 建立DataAdapter对象 
        /// </summary> 
        /// <returns>DataAdapter对象</returns> 
        IDbDataAdapter CreateDataAdapter();

        /// <summary> 
        /// 根据Connection建立Transaction 
        /// </summary> 
        /// <param name="myDbConnection">Connection对象</param> 
        /// <returns>Transaction对象</returns> 
        IDbTransaction CreateTransaction(IDbConnection myDbConnection);

        /// <summary> 
        /// 根据Command建立DataReader 
        /// </summary> 
        /// <param name="myDbCommand">Command对象</param> 
        /// <returns>DataReader对象</returns> 
        IDataReader CreateDataReader(IDbCommand myDbCommand);

        /// <summary> 
        /// 获得连接字符串 
        /// </summary> 
        /// <returns>连接字符串</returns> 
        string GetConnectionString();
    }

    /// <summary>
    /// 工厂类
    /// </summary>
    public sealed class Factory
    {
        private static volatile Factory singleFactory = null;
        private static object syncObj = new object();

        private Factory()
        {

        }

        /// <summary>
        /// 获取Factory类的实例
        /// </summary>
        public static Factory Instance
        {
            get
            {
                if (singleFactory == null)
                {
                    lock (syncObj)
                    {
                        if (singleFactory == null)
                        {
                            singleFactory = new Factory();
                        }
                    }
                }
                return singleFactory;
            }
        }

        public AbstractDbFactory CreateInstance()
        {
            AbstractDbFactory factory = null;
            string dbType = "";
            switch (dbType)
            {
                case "sqlServer":
                    factory = new SqlFactory();
                    break;
                case "orcale":
                    factory = new OralceFactory();
                    break;
                default:
                    break;
            }
            return factory;
        }
    }

    public class SqlFactory : AbstractDbFactory
    {

        public IDbConnection CreateConnection()
        {
            throw new NotImplementedException();
        }

        public IDbConnection CreateConnection(string strConn)
        {
            throw new NotImplementedException();
        }

        public IDbCommand CreateCommand()
        {
            throw new NotImplementedException();
        }

        public IDbDataAdapter CreateDataAdapter()
        {
            throw new NotImplementedException();
        }

        public IDbTransaction CreateTransaction(IDbConnection myDbConnection)
        {
            throw new NotImplementedException();
        }

        public IDataReader CreateDataReader(IDbCommand myDbCommand)
        {
            throw new NotImplementedException();
        }

        public string GetConnectionString()
        {
            throw new NotImplementedException();
        }
    }

    public class OralceFactory : AbstractDbFactory
    {

        public IDbConnection CreateConnection()
        {
            throw new NotImplementedException();
        }

        public IDbConnection CreateConnection(string strConn)
        {
            throw new NotImplementedException();
        }

        public IDbCommand CreateCommand()
        {
            throw new NotImplementedException();
        }

        public IDbDataAdapter CreateDataAdapter()
        {
            throw new NotImplementedException();
        }

        public IDbTransaction CreateTransaction(IDbConnection myDbConnection)
        {
            throw new NotImplementedException();
        }

        public IDataReader CreateDataReader(IDbCommand myDbCommand)
        {
            throw new NotImplementedException();
        }

        public string GetConnectionString()
        {
            throw new NotImplementedException();
        }
    }
}
