using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;

namespace Climj.Loogn
{
    public class Logger
    {
        private bool IsSingleFile = false;
        private static Dictionary<string, Logger> CacheLoggers = new Dictionary<string, Logger>();
        private static object lockObj = new object();
        private static XmlDocument ConfigXmlDoc;
        private static Logger _empty;
        private static string Style;
        private string filepath;

        public string DateFmt { get; private set; }

        public Dictionary<string, string> TplDict { get; private set; }

        public bool FatalEnabled { get; private set; }

        public bool ErrorEnabled { get; private set; }

        public bool WarnEnabled { get; private set; }

        public bool InfoEnabled { get; private set; }

        public bool DebugEnabled { get; private set; }

        static Logger()
        {
            //<add key="loggerConfig" value="LoggerConfig.xml"/>
            Logger.LoadConfig();
        }

        private Logger()
        {
        }

        private static void LoadConfig()
        {
            foreach (string index in ConfigurationManager.AppSettings.Keys)
            {
                if (index == "loggerConfig")
                {
                    Logger.ConfigXmlDoc = new XmlDocument();
                    Logger.ConfigXmlDoc.Load(ConfigurationManager.AppSettings[index]);
                    XmlNode xmlNode = Logger.ConfigXmlDoc.DocumentElement.SelectSingleNode("style");
                    if (xmlNode != null)
                    {
                        Logger.Style = xmlNode.InnerText;
                        return;
                    }
                    Logger.Style = string.Empty;
                    return;
                }
            }
            Logger.ConfigXmlDoc = (XmlDocument)null;
        }

        public static Logger GetInstance(string loggerName = "default")
        {
            if (Logger.ConfigXmlDoc == null)
            {
                if (Logger._empty == null)
                    Logger._empty = new Logger();
                return Logger._empty;
            }
            lock (Logger.lockObj)
            {
                if (Logger.CacheLoggers.ContainsKey(loggerName))
                    return Logger.CacheLoggers[loggerName];
                lock (Logger.lockObj)
                {
                    Logger local_0 = new Logger();
                    XmlElement local_1 = Logger.ConfigXmlDoc.DocumentElement;
                    XmlNode local_2 = local_1.SelectSingleNode("tpls");
                    XmlNode local_3 = local_1.SelectSingleNode("logs/log[@name='" + loggerName + "']");
                    Logger.BuildDefaultTpl(local_0, local_2);
                    Logger.BuildPath(local_0, local_3);
                    Logger.BuildTypes(local_0, local_3);
                    Logger.BuildDateFmt(local_0, local_3);
                    Logger.BuildSelfTpl(local_0, local_3);
                    Logger.CacheLoggers[loggerName] = local_0;
                    return local_0;
                }
            }
        }

        public static void ReloadConfig()
        {
            Logger.CacheLoggers.Clear();
            Logger.LoadConfig();
        }

        private static void BuildDefaultTpl(Logger logger, XmlNode tplsNode)
        {
            logger.TplDict = new Dictionary<string, string>();
            foreach (XmlNode xmlNode in tplsNode.ChildNodes)
            {
                string index = xmlNode.Attributes["name"].Value;
                string innerText = xmlNode.InnerText;
                logger.TplDict[index] = innerText;
            }
        }

        private static void BuildPath(Logger logger, XmlNode logNode)
        {
            logger.filepath = logNode.Attributes["path"].Value.ToLower();
            if (!logger.filepath.EndsWith("html") && !logger.filepath.EndsWith("htm"))
                return;
            logger.IsSingleFile = true;
        }

        private static void BuildTypes(Logger logger, XmlNode logNode)
        {
            string str1 = logNode.Attributes["types"].Value;
            char[] separator = new char[1]
            {
        ','
            };
            int num = 1;
            foreach (string str2 in str1.Split(separator, (StringSplitOptions)num))
            {
                if (str2.Equals("all") || str2.Equals("fatal"))
                    logger.FatalEnabled = true;
                if (str2.Equals("all") || str2.Equals("debug"))
                    logger.DebugEnabled = true;
                if (str2.Equals("all") || str2.Equals("error"))
                    logger.ErrorEnabled = true;
                if (str2.Equals("all") || str2.Equals("info"))
                    logger.InfoEnabled = true;
                if (str2.Equals("all") || str2.Equals("warn"))
                    logger.WarnEnabled = true;
            }
        }

        private static void BuildDateFmt(Logger logger, XmlNode logNode)
        {
            XmlAttribute xmlAttribute = logNode.Attributes["dateFmt"];
            if (xmlAttribute == null)
                logger.DateFmt = "yyyy-MM-dd HH:mm:ss";
            else
                logger.DateFmt = xmlAttribute.Value;
        }

        private static void BuildSelfTpl(Logger logger, XmlNode logNode)
        {
            if (!logNode.HasChildNodes)
                return;
            foreach (XmlNode xmlNode in logNode.ChildNodes)
            {
                string index = xmlNode.Attributes["name"].Value;
                string innerText = xmlNode.InnerText;
                logger.TplDict[index] = innerText;
            }
        }

        public string GetPath()
        {
            if (this.IsSingleFile)
                return this.filepath;
            DateTime now = DateTime.Now;
            string path1 = this.filepath;
            int num = now.Year;
            string path2 = num.ToString();
            num = now.Month;
            string path3 = num.ToString();
            string path = Path.Combine(path1, path2, path3);
            num = now.Day;
            string str = num.ToString() + ".html";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path + "\\" + str;
        }

        public void Debug(string message, Predicate<string> filter = null)
        {
            if (!this.DebugEnabled)
                return;
            this.Log(message, "debug", filter);
        }

        public void Info(string message, Predicate<string> filter = null)
        {
            if (!this.InfoEnabled)
                return;
            this.Log(message, "info", filter);
        }

        public void Warn(string message, Predicate<string> filter = null)
        {
            if (!this.WarnEnabled)
                return;
            this.Log(message, "warn", filter);
        }

        public void Error(string message, Predicate<string> filter = null)
        {
            if (!this.ErrorEnabled)
                return;
            this.Log(message, "error", filter);
        }

        public void Fatal(string message, Predicate<string> filter = null)
        {
            if (!this.FatalEnabled)
                return;
            this.Log(message, "fatal", filter);
        }

        private void Log(string message, string type, Predicate<string> filter)
        {
            if (Logger.ConfigXmlDoc == null || filter != null && filter(message))
                return;
            lock (this)
            {
                string local_0 = this.GetPath();
                string local_2 = this.TplDict[type].Replace("{type}", type).Replace("{message}", message).Replace("{date}", DateTime.Now.ToString(this.DateFmt));
                if (!File.Exists(local_0))
                    local_2 = "<style type='text/css'>" + Logger.Style + "</style>" + local_2;
                File.AppendAllText(local_0, local_2, Encoding.UTF8);
            }
        }
    }
}
