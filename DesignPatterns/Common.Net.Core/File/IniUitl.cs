using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Common.Net.Core
{
    class IniUitl
    {
        public string filePath;

        /// <summary>
        /// 传入INI文件路径构造对象
        /// </summary>
        /// <param name="path">INI文件路径</param>
        public IniUitl(string path) {
            this.filePath = path;
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);


        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defVal, Byte[] retVal, int size, string filePath);


        /// <summary>
        /// 写INI文件
        /// </summary>
        /// <param name="Section">分组节点</param>
        /// <param name="Key">关键字</param>
        /// <param name="Value">值</param>
        public void IniWriteValue(string Section, string Key, string Value) {
            WritePrivateProfileString(Section, Key, Value, this.filePath);
        }

        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="Section">分组节点</param>
        /// <param name="Key">关键字</param>
        /// <returns></returns>
        public string IniReadValue(string Section, string Key) {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.filePath);
            return temp.ToString();
        }

        public byte[] IniReadValues(string section, string key) {
            byte[] temp = new byte[255];
            int i = GetPrivateProfileString(section, key, "", temp, 255, this.filePath);
            return temp;

        }

        /// <summary>
        /// 删除ini文件下所有段落
        /// </summary>
        public void ClearAllSection() {
            IniWriteValue(null, null, null);
        }

        /// <summary>
        /// 删除ini文件下指定段落下的所有键
        /// </summary>
        /// <param name="Section"></param>
        public void ClearSection(string Section) {
            IniWriteValue(Section, null, null);
        }
    }
}
