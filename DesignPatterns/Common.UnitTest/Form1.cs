using Common.Net.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common.UnitTest
{
    public partial class Form1 : Form
    {

        Proxy _proxy;

        public Form1()
        {
            InitializeComponent();
            button1.Click += (a, b) => LoadDll("Zxxk.Task.ClassRank.dll", "Zxxk.Task.ClassRank.MRankJob", "Domain1");

            FileSystemHelper fileSystemHelper = new FileSystemHelper(Environment.CurrentDirectory.ToString(), "Zxxk.*");
            fileSystemHelper.WatcherStrat(Action(OnDeleted), () =>
            {

            }, () =>
            {

            });

        }

        private void OnDeleted(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("文件删除事件处理逻辑{0}  {1}   {2}", e.ChangeType, e.FullPath, e.Name);
        }
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("文件删除事件处理逻辑{0}  {1}   {2}", e.ChangeType, e.FullPath, e.Name);
        }


        void LoadDll(string assName, string type, string appName)
        {
            if (_proxy != null)
                _proxy.UnLoad();
            _proxy = new Proxy(assName, type, appName);
            //lblCurrentAppDomain.Text = _proxy.DefaultAppDomainName;
            //lblCurrentAssembly.Text = _proxy.DefaultAssemblyName;
            //lblMainAppDomain.Text = AppDomain.CurrentDomain.FriendlyName;
        }
    }
}
