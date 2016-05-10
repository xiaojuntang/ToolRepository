using log4net;
/***************************************************************************** 
*        filename :LogHelper 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.42000 
*        新建项输入的名称:   LogHelper 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       HeplerPatterns 
*        文件名:             LogHelper 
*        创建系统时间:       2016/2/1 11:36:38 
*        创建年份:           2016 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeplerPatterns
{
    //处理每一条日志委托方法 
    public delegate void LogHelper_DELHandleLog(string msg);

    /// <summary>日志操作类
    /// 使用本类必须保证配置节上存在log4net的配置（web/app.config中）
    /// </summary>  
    public class LogHelper
    {
        private static ILog log;
        /// <summary>日志记录实体类
        /// </summary>
        public static ILog Log
        {
            get { return LogHelper.log; }
            set { LogHelper.log = value; }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configPath"></param>
        static LogHelper()
        {
            log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log4net.Config.XmlConfigurator.Configure();
        }
        /// <summary>加载日志记录配置
        /// </summary>
        /// <param name="configFilePath"></param>
        public static void Config(string configFilePath)
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(configFilePath));
        }

        private static Dictionary<string, ILog> logbyConfig;
        //private static ILog logbyConfig;
        static object logbyConfiglock = new object();
        /// <summary>根据配置的log名称获取Log实例
        /// </summary>
        /// <param name="logname">log配置节点名称</param>
        /// <returns></returns>
        /// <remarks>每次调用都会创建一个实例，使用时考虑重复利用问题</remarks>
        public static ILog getLogByConfigLogName(string logname)
        {
            if (logbyConfig == null)
            {
                lock (logbyConfiglock)
                {
                    logbyConfig = new Dictionary<string, ILog>();
                }
            }
            if (!logbyConfig.ContainsKey(logname))
            {
                lock (logbyConfiglock)
                {
                    if (!logbyConfig.ContainsKey(logname))
                    {
                        ILog log = log4net.LogManager.GetLogger(logname);
                        logbyConfig.Add(logname, log);
                    }
                }
            }

            return logbyConfig[logname];
        }




        /// <summary>读取txt日志文件
        /// </summary>
        /// <param name="filepath">文件路径</param>
        public static void readlog(string filepath, LogHelper_DELHandleLog delHandleLog)
        {
            if (!File.Exists(filepath))
                return;
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filepath, System.Text.Encoding.UTF8);
                string EveryLineLog = string.Empty;

                while (!sr.EndOfStream)
                {
                    EveryLineLog = sr.ReadLine();

                    if (EveryLineLog != null)
                    {
                        delHandleLog(EveryLineLog);
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

