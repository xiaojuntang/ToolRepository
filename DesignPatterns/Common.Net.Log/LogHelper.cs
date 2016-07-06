using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using log4net.Config;

namespace Common.Net.Log
{
    /// <summary>
    /// 基于log4net日志组件
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="appender">配置结点名称</param>
        public static void Error(string message, string appender = "ErrorFileAppender")
        {
            Logg.GetLogByName(appender).Error(message);
        }

        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="appender">配置结点名称</param>
        public static void Error(Exception message, string appender = "ErrorFileAppender")
        {
            Logg.GetLogByName(appender).Error(message.Message);
        }

        /// <summary>
        /// 输出调试信息
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="appender">配置结点名称</param>
        public static void Debug(string message, string appender = "DebugFileAppender")
        {
            Logg.GetLogByName(appender).Debug(message);
        }

        /// <summary>
        /// 输出说明
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="appender">配置结点名称</param>
        public static void Info(string message, string appender = "DebugFileAppender")
        {
            Logg.GetLogByName(appender).Info(message);
        }
    }

    /// <summary>
    /// 处理每一条日志委托方法 
    /// </summary>
    /// <param name="msg"></param>
    public delegate void LogHandler(string msg);

    /// <summary>
    /// 日志操作类
    /// 使用本类必须保证配置节上存在log4net的配置（web/app.config中）
    /// </summary>  
    internal class Logg
    {
        /// <summary>
        /// 日志实例字典
        /// </summary>
        private static Dictionary<string, ILog> _logbyConfig;
        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object LogbyConfiglock = new object();
        /// <summary>
        /// 当前日志实例
        /// </summary>
        private static ILog _log;

        /// <summary>
        /// 日志记录实体类
        /// </summary>
        public static ILog Log
        {
            get { return Logg._log; }
            set { Logg._log = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        static Logg()
        {
            _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            XmlConfigurator.Configure();
        }

        /// <summary>
        /// 加载日志记录配置
        /// </summary>
        /// <param name="configFilePath"></param>
        public static void Config(string configFilePath)
        {
            XmlConfigurator.Configure(new FileInfo(configFilePath));
        }

        /// <summary>
        /// 根据配置的log名称获取Log实例
        /// </summary>
        /// <param name="logName">log配置节点名称</param>
        /// <returns></returns>
        public static ILog GetLogByName(string logName)
        {
            if (_logbyConfig == null)
            {
                lock (LogbyConfiglock)
                {
                    _logbyConfig = new Dictionary<string, ILog>();
                }
            }
            if (!_logbyConfig.ContainsKey(logName))
            {
                lock (LogbyConfiglock)
                {
                    if (!_logbyConfig.ContainsKey(logName))
                    {
                        ILog log = log4net.LogManager.GetLogger(logName);
                        _logbyConfig.Add(logName, log);
                    }
                }
            }
            return _logbyConfig[logName];
        }

        /// <summary>
        /// 读取txt日志文件
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="delHandleLog"></param>
        public static void ReadLog(string filepath, LogHandler delHandleLog)
        {
            if (!File.Exists(filepath))
                return;
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filepath, System.Text.Encoding.UTF8);
                while (!sr.EndOfStream)
                {
                    var everyLineLog = sr.ReadLine();
                    if (everyLineLog != null)
                    {
                        delHandleLog(everyLineLog);
                    }
                }
                sr.Close();
                sr.Dispose();
            }
            catch (Exception ex)
            {
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
            }
        }
    }
}
