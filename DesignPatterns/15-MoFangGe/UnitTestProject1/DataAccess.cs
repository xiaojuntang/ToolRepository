using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
//    public sealed partial class DataAccess<T>
//    {
//        static readonly string _assemblyName = ConfigurationManager.AppSettings["AssemblyName"];

//        /// <summary>
//        /// 建立数据库操作实例
//        /// </summary>
//        /// <returns>操作实例</returns>
//        public static T CreateDAL()
//        {
//            Type t = typeof(T);
//            string className = t.FullName.Substring(t.Assembly.FullName.Split(new char[] { ',' })[0].Length + 1);
//            string[] names = className.Split(new char[] { '.' });
//            names[names.Length - 1] = names[names.Length - 1].Substring(1);
//            string fullName = _assemblyName;
//            foreach (string s in names)
//            {
//                fullName += "." + s;
//            }
//            T obj = (T)Assembly.Load(_assemblyName).CreateInstance(fullName);
//            if (obj == null)
//            {
//                throw new Exception("使用反射从程序集 " + _assemblyName + " 里创建对象 " + fullName + " 时出错");
//            }
//            return obj;
//        }
//    }
}
