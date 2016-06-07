using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Common.Net.Core
{
    public class XmlSerialize
    {
        /// <summary>
        /// 转化SVCT对象到xml格式的字符串
        /// </summary>
        /// <param name="obj">The SVCT object.</param>
        /// <returns>Return the xml text.</returns>
        public static string Serialize(object obj)
        {
            string gwtaXml;
            byte[] streamBytes;
            Type type = obj.GetType();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            try
            {
                ns.Add("name", "http://union/xml");
                XmlSerializer formatter = new XmlSerializer(type);
                using (MemoryStream stream = new MemoryStream())
                {
                    formatter.Serialize(stream, obj, ns);
                    streamBytes = stream.ToArray();
                    gwtaXml = Encoding.UTF8.GetString(streamBytes);
                }
            }
            catch (Exception e)
            {
                string errorMessage = string.Format("Error occur when serializing {0} object to SVCT-XML.", type.Name);
                throw new SerializationException(errorMessage, e);
            }
            return gwtaXml;
        }


        /// <summary>
        /// 转化xml格式的字符串为SVCT
        /// </summary>
        /// <param name="gwtaXml">The xml text.</param>
        /// <returns>Return the GWTA object.</returns>
        public static T Deserialize<T>(string xmlPath)
        {
            T gata;
            Type type = typeof(T);
            XmlSerializer formatter;
            //byte[] streamBytes = Encoding.UTF8.GetBytes(svctXml);
            formatter = new XmlSerializer(type);
            try
            {
                using (FileStream stream = new FileStream("xmlPath", FileMode.OpenOrCreate))
                {
                    object obj = formatter.Deserialize(stream);
                    gata = (T)obj;
                }
            }
            catch (Exception e)
            {
                if (e is InvalidOperationException)
                {
                    throw e;
                }
                else
                {
                    string errorMessage = string.Format("Error occur when deserializing SVCT-XML to {0} object.", type.Name);
                    throw new SerializationException(errorMessage, e);
                }
            }
            return gata;
        }
    }
}
