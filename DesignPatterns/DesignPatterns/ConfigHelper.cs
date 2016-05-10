using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Common.Net.Func
{
    public class ConfigHelper
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private readonly string _configPath = string.Empty;

        public ConfigHelper() {

        }

        /// <summary>
        /// 配置文件XML
        /// </summary>
        /// <param name="xmlName">配置文件名称</param>
        public ConfigHelper(string xmlName) {
            _configPath = string.Format("{0}\\{1}.xml", Application.StartupPath, xmlName);
            if (!File.Exists(_configPath)) {
                throw new Exception("The file is not exists.");
            }
        }

        /// <summary>
        /// 写操作
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="appValue"></param>
        public void SetValue(string nodeName, string appValue) {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(_configPath);
            XmlNode xNode = xDoc.SelectSingleNode("//" + nodeName);
            xNode.InnerText = appValue;
            xDoc.Save(_configPath);
        }

        /// <summary>
        /// 写操作
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="appKey"></param>
        /// <param name="appValue"></param>
        public void SetValue(string nodeName, string appKey, string appValue) {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(_configPath);

            XmlNode xNode;
            XmlElement xElem1;
            XmlElement xElem2;
            xNode = xDoc.SelectSingleNode("//" + nodeName);
            if (xNode != null)
            {
                xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@key='" + appKey + "']");
                if (xElem1 != null)
                    xElem1.SetAttribute("value", appValue);
                else {
                    xElem2 = xDoc.CreateElement("add");
                    xElem2.SetAttribute("Key", appKey);
                    xElem2.SetAttribute("value", appValue);
                    xNode.AppendChild(xElem2);
                }
            }
            xDoc.Save(_configPath);
        }

        /// <summary>
        /// 读取XML结点内容<set>123</set>
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public string GetValue(string nodeName) {
            XmlDocument xDoc = new XmlDocument();
            try {
                xDoc.Load(_configPath);
                XmlNode xNode = xDoc.SelectSingleNode("//" + nodeName);
                return xNode.InnerText;
            }
            catch (Exception) {
                return string.Empty;
            }
        }

        /// <summary>
        /// 读取XML结点内容<add key=1 value=1/>
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="appKey"></param>
        /// <returns></returns>
        public string GetValue(string nodeName, string appKey) {
            XmlDocument xDoc = new XmlDocument();
            try {
                xDoc.Load(_configPath);
                XmlNode xNode;
                XmlElement xElem;
                xNode = xDoc.SelectSingleNode("//" + nodeName);
                xElem = (XmlElement)xNode.SelectSingleNode("//add[@key='" + appKey + "']");
                if (xElem != null)
                    return xElem.GetAttribute("value");
                else
                    return string.Empty;
            }
            catch (Exception) {
                return string.Empty;
            }
        }

        public List<object> GetValueToArray(string NodeName) {
            XmlDocument xDoc = new XmlDocument();
            try {
                List<object> array = new List<object>();
                xDoc.Load(_configPath);
                XmlNode xNode = xDoc.SelectSingleNode("//" + NodeName);
                if (xNode.ChildNodes.Count > 0) {
                    foreach (XmlNode item in xNode.ChildNodes) {
                        array.Add(item.InnerText);
                    }
                }
                return array;
            }
            catch (Exception) {
                return null;
            }
        }
    }
}
