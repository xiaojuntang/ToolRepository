using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using System.Text.RegularExpressions;
using System.Management;

namespace Common.Net.Core
{
    public sealed class IpMacManager
    {
        [DllImport("Iphlpapi.dll")]
        private static extern int SendARP(Int32 dest, Int32 host, ref Int64 mac, ref Int32 length);
        [DllImport("Ws2_32.dll")]
        private static extern Int32 inet_addr(string ip);

        /// <summary>
        /// 获取本机的IPV4
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIP() {
            string strHostName = Dns.GetHostName(); //得到本机的主机名
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName); //取得本机IP
            string localIp = string.Empty;
            foreach (var ip in ipEntry.AddressList) {
                string strAddr = ip.ToString();
                if (IsValidIP(strAddr))
                    localIp = strAddr;
            }
            return localIp;
        }

        /// <summary>
        /// 获取本机的MAC
        /// </summary>
        /// <returns></returns>
        public static string GetLocalMac() {
            string mac = null;
            //using System.Management
            ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection queryCollection = query.Get();
            foreach (ManagementObject mo in queryCollection) {
                if (mo["IPEnabled"].ToString() == "True")
                    mac = mo["MacAddress"].ToString();
            }
            return (mac);
        }

        /// <summary>
        /// 获取远程主机IP
        /// </summary>
        /// <param name="RemoteHostName">主机名称</param>
        /// <returns></returns>
        public static string[] GetRemoteIP(string RemoteHostName) {
            IPHostEntry ipEntry = Dns.GetHostEntry(RemoteHostName);
            IPAddress[] IpAddr = ipEntry.AddressList;
            string[] strAddr = new string[IpAddr.Length];
            for (int i = 0; i < IpAddr.Length; i++) {
                strAddr[i] = IpAddr[i].ToString();
            }
            return (strAddr);
        }

        /// <summary>
        /// 获取远程主机MAC
        /// </summary>
        /// <param name="localIP"></param>
        /// <param name="remoteIP"></param>
        /// <returns></returns>
        public static string GetRemoteMac(string localIP, string remoteIP) {
            Int32 ldest = inet_addr(remoteIP); //目的ip 
            Int32 lhost = inet_addr(localIP); //本地ip 
            try {
                Int64 macinfo = new Int64();
                Int32 len = 6;
                int res = SendARP(ldest, 0, ref macinfo, ref len);
                return Convert.ToString(macinfo, 16);
            }
            catch (Exception err) {
                Console.WriteLine("Error:{0}", err.Message);
            }
            return 0.ToString();
        }

        /// <summary>
        /// 判断是否是IP格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsValidIP(string ip) {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }
}
