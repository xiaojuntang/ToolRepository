using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //自动更新
            Process p = new Process();
            p.StartInfo.FileName = Application.StartupPath + "\\AutoUpdate.exe";
            p.StartInfo.Arguments = "auto";
            p.Start();
        }
    }
}
