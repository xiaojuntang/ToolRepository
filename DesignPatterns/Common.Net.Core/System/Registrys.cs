using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Common.Net.Core
{
    public class Registrys
    {
        #region 注册表

        private const string Reg_Name = "System_Reg";
        private static RegistryKey _regKey = null;

        /// <summary>
        /// 获取是否自动启动服务器
        /// </summary>
        /// <returns></returns>
        public static bool GetAutoRun() {
            if (!OpenRegister())
                return false;

            bool result = false;
            string[] ValueName = _regKey.GetValueNames();
            for (int i = 0; i < ValueName.Length; i++) {
                if (ValueName[i] == Reg_Name) {
                    result = true;
                    break;
                }
            }
            CloseRegister();
            return result;
        }

        /// <summary>
        /// 设置是否自动启动服务器
        /// </summary>
        /// <param name="app_path">程序路径</param>
        /// <param name="isRun">是否自动启动</param>
        /// <returns></returns>
        public static bool SetAutoRun(string app_path, bool isRun) {
            bool result = false;
            if (!OpenRegister())
                return false;
            try {
                if (isRun) {
                    _regKey.SetValue(Reg_Name, app_path.Trim());
                }
                else {
                    _regKey.DeleteValue(Reg_Name);
                }
                result = true;
            }
            catch {
                result = false;
            }
            finally {
                CloseRegister();
            }
            return result;
        }

        /// <summary>
        /// 打开键值
        /// </summary>
        /// <returns></returns>
        private static bool OpenRegister() {
            _regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            if (_regKey != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 关闭键值
        /// </summary>
        private static void CloseRegister() {
            _regKey.Close();
        }

        #endregion
    }
}
