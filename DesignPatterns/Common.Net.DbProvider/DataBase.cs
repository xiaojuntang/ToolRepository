using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Common.Net.DbProvider
{
    /// <summary>
    /// 数据库连接对象项目可继承该类
    /// </summary>
    public abstract class DataBase
    {
        /// <summary>
        /// 默认数据连接
        /// </summary>
        public const string ConnStr = "ConnStr";
    }

    /// <summary>
    /// SQL命令对象
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
    /// 执行SQL语句的类别
    /// </summary>
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
}
