/***************************************************************************** 
*        filename :XmlHelper 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.42000 
*        新建项输入的名称:   XmlHelper 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Common.Net.Helper 
*        文件名:             XmlHelper 
*        创建系统时间:       2016/2/1 15:52:11 
*        创建年份:           2016 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Common.Net.Helper
{
    public abstract class XmlHelper
    {
        #region 实体类序列化成xml
        /// <summary>
        ///  实体类序列化成xml
        /// </summary>
        /// <param name="enitities">The enitities.</param>
        /// <param name="headtag">The headtag.</param>
        /// <returns></returns>
        public static string ObjListToXml<T>(List<T> enitities, string headtag)
        {
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] propinfos = null;
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine("<" + headtag + ">");
            foreach (T obj in enitities)
            {
                //初始化propertyinfo
                if (propinfos == null)
                {
                    Type objtype = obj.GetType();
                    propinfos = objtype.GetProperties();
                }
                sb.AppendLine("<item>");
                foreach (PropertyInfo propinfo in propinfos)
                {
                    sb.Append("<");
                    sb.Append(propinfo.Name);
                    sb.Append(">");
                    sb.Append(propinfo.GetValue(obj, null));
                    sb.Append("</");
                    sb.Append(propinfo.Name);
                    sb.AppendLine(">");
                }
                sb.AppendLine("</item>");
            }
            sb.AppendLine("</" + headtag + ">");
            return sb.ToString();
        }
        #endregion

        #region 使用XML初始化实体类容器
        /// <summary>
        ///  使用XML初始化实体类容器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typename">The typename.</param>
        /// <param name="xml">The XML.</param>
        /// <param name="headtag">The headtag.</param>
        /// <returns></returns>
        public static List<T> XmlToObjListByNode<T>(string xml, string headtag)
           where T : new()
        {
            List<T> list = new List<T>();
            XmlDocument doc = new XmlDocument();
            PropertyInfo[] propinfos = null;
            doc.LoadXml(xml);
            XmlNodeList nodelist = doc.GetElementsByTagName(headtag);
            foreach (XmlNode node in nodelist)
            {
                T entity = new T();
                //初始化propertyinfo
                if (propinfos == null)
                {
                    Type objtype = entity.GetType();
                    propinfos = objtype.GetProperties();
                }
                //填充entity类的属性
                foreach (PropertyInfo propinfo in propinfos)
                {
                    XmlNode cnode = node.SelectSingleNode(propinfo.Name);
                    if (cnode == null)
                        continue;
                    string v = cnode.InnerText;
                    if (v != null)
                        propinfo.SetValue(entity, Convert.ChangeType(v, propinfo.PropertyType), null);
                }
                list.Add(entity);
            }
            return list;
        }
        #endregion

        #region 使用XML初始化实体类容器
        /// <summary>
        ///  使用XML初始化实体类容器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typename">The typename.</param>
        /// <param name="xml">The XML.</param>
        /// <param name="headtag">The headtag.</param>
        /// <returns></returns>
        public static List<T> XmlToObjListByAttr<T>(string xml, string headtag)
           where T : new()
        {
            List<T> list = new List<T>();
            XmlDocument doc = new XmlDocument();
            PropertyInfo[] propinfos = null;
            doc.LoadXml(xml);
            XmlNodeList nodelist = doc.GetElementsByTagName(headtag);
            foreach (XmlNode node in nodelist)
            {
                T entity = new T();
                //初始化propertyinfo
                if (propinfos == null)
                {
                    Type objtype = entity.GetType();
                    propinfos = objtype.GetProperties();
                }
                //填充entity类的属性
                foreach (PropertyInfo propinfo in propinfos)
                {
                    XmlNode cnode = node.Attributes[propinfo.Name.ToLower()];
                    if (cnode == null)
                        continue;
                    string v = cnode.InnerText;
                    if (v != null)
                        propinfo.SetValue(entity, Convert.ChangeType(v, propinfo.PropertyType), null);
                }
                list.Add(entity);
            }
            return list;
        }
        #endregion

        #region 将Xml内容字符串转换成DataSet对象
        /**/
        /// <summary>
        /// 将Xml内容字符串转换成DataSet对象
        /// </summary>
        /// <param name="xmlStr">Xml内容字符串</param>
        /// <returns>DataSet对象</returns>
        public static DataSet CXmlToDataSet(string xmlStr)
        {
            if (!string.IsNullOrEmpty(xmlStr))
            {
                StringReader StrStream = null;
                XmlTextReader Xmlrdr = null;
                try
                {
                    DataSet ds = new DataSet();
                    //读取字符串中的信息
                    StrStream = new StringReader(xmlStr);
                    //获取StrStream中的数据
                    Xmlrdr = new XmlTextReader(StrStream);
                    //ds获取Xmlrdr中的数据                
                    ds.ReadXml(Xmlrdr);
                    return ds;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    //释放资源
                    if (Xmlrdr != null)
                    {
                        Xmlrdr.Close();
                        StrStream.Close();
                        StrStream.Dispose();
                    }
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 将Xml字符串转换成DataTable对象
        /**/
        /// <summary>
        /// 将Xml字符串转换成DataTable对象
        /// </summary>
        /// <param name="xmlStr">Xml字符串</param>
        /// <param name="tableIndex">Table表索引</param>
        /// <returns>DataTable对象</returns>
        public static DataTable CXmlToDatatTable(string xmlStr, int tableIndex)
        {
            return CXmlToDataSet(xmlStr).Tables[tableIndex];
        }
        #endregion

        #region 将Xml字符串转换成DataTable对象
        /**/
        /// <summary>
        /// 将Xml字符串转换成DataTable对象
        /// </summary>
        /// <param name="xmlStr">Xml字符串</param>
        /// <returns>DataTable对象</returns>
        public static DataTable CXmlToDatatTable(string xmlStr)
        {
            return CXmlToDataSet(xmlStr).Tables[0];
        }
        #endregion

        #region 读取Xml文件信息,并转换成DataSet对象
        /**/
        /// <summary>
        /// 读取Xml文件信息,并转换成DataSet对象
        /// </summary>
        /// <remarks>
        /// DataSet ds = new DataSet();
        /// ds = CXmlFileToDataSet("/XML/upload.xml");
        /// </remarks>
        /// <param name="xmlFilePath">Xml文件地址</param>
        /// <returns>DataSet对象</returns>
        public static DataSet CXmlFileToDataSet(string xmlFilePath)
        {
            if (!string.IsNullOrEmpty(xmlFilePath))
            {
                string path = HttpContext.Current.Server.MapPath(xmlFilePath);
                StringReader StrStream = null;
                XmlTextReader Xmlrdr = null;
                try
                {
                    XmlDocument xmldoc = new XmlDocument();
                    //根据地址加载Xml文件
                    xmldoc.Load(path);

                    DataSet ds = new DataSet();
                    //读取文件中的字符流
                    StrStream = new StringReader(xmldoc.InnerXml);
                    //获取StrStream中的数据
                    Xmlrdr = new XmlTextReader(StrStream);
                    //ds获取Xmlrdr中的数据
                    ds.ReadXml(Xmlrdr);
                    return ds;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    //释放资源
                    if (Xmlrdr != null)
                    {
                        Xmlrdr.Close();
                        StrStream.Close();
                        StrStream.Dispose();
                    }
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 读取Xml文件信息,并转换成DataTable对象
        /**/
        /// <summary>
        /// 读取Xml文件信息,并转换成DataTable对象
        /// </summary>
        /// <param name="xmlFilePath">xml文江路径</param>
        /// <param name="tableIndex">Table索引</param>
        /// <returns>DataTable对象</returns>
        public static DataTable CXmlToDataTable(string xmlFilePath, int tableIndex)
        {
            return CXmlFileToDataSet(xmlFilePath).Tables[tableIndex];
        }
        #endregion

        #region 读取Xml文件信息,并转换成DataTable对象
        /**/
        /// <summary>
        /// 读取Xml文件信息,并转换成DataTable对象
        /// </summary>
        /// <param name="xmlFilePath">xml文江路径</param>
        /// <returns>DataTable对象</returns>
        public static DataTable CXmlToDataTable(string xmlFilePath)
        {
            return CXmlFileToDataSet(xmlFilePath).Tables[0];
        }
        #endregion

        /// <summary>
        /// 摘要:对象转XML 无命名空间 [XmlElement("Head")]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string SerializeData<T>(T model)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                string xml;
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, string.Empty);

                StreamWriter sw = new StreamWriter(ms);
                XmlWriterSettings settings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true };
                using (XmlWriter writer = XmlWriter.Create(sw, settings))
                {
                    XmlSerializer serializer = new XmlSerializer(model.GetType());
                    serializer.Serialize(writer, model, ns);
                    writer.Flush();
                    writer.Close();
                }
                using (StreamReader sr = new StreamReader(ms))
                {
                    ms.Position = 0;
                    xml = sr.ReadToEnd();
                    sr.Close();
                }
                return xml;
            }
        }

        /// <summary> 
        /// 摘要:XML反序列化 Xml->Object
        /// </summary> 
        /// <param name="type">类型</param> 
        /// <param name="xml">XML字符串</param> 
        /// <returns></returns> 
        public static object Deserialize(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary> 
        /// 摘要:获取对应XML节点的值 
        /// </summary> 
        /// <param name="node">XML节点的标记</param> 
        /// <returns>返回获取对应XML节点的值</returns> 
        public static string XmlAnalysis(string node, string xml)
        {
            if (node.Equals("") == false)
            {
                try
                {
                    XmlDocument xmlLoad = new XmlDocument();
                    xmlLoad.LoadXml(xml);
                    if (xmlLoad.DocumentElement != null)
                    {
                        var selectSingleNode = xmlLoad.DocumentElement.SelectSingleNode(node);
                        if (selectSingleNode != null)
                            return selectSingleNode.InnerXml.Trim();
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return "";
        }

        /// <summary>
        /// 格式化Xml
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string FormatXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            StringWriter sw = new StringWriter();
            using (XmlTextWriter writer = new XmlTextWriter(sw))
            {
                writer.Indentation = 2;
                writer.Formatting = Formatting.Indented;
                doc.WriteContentTo(writer);
                writer.Close();
            }
            return sw.ToString();
        }
    }
}
