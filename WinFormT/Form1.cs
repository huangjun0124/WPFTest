using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetCurrentDirectory();
        }

        private void GetCurrentDirectory()
        {
            //获取当前进程的完整路径，包含文件名(进程名)
            string path = this.GetType().Assembly.Location;
            //获取新的Process组件并将其与当前活动的进程关联的主模块的完整路径，包含文件名(进程名)
            path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            //获取和设置当前目录(即该进程从中启动的目录)的完全限定路径
            path = System.Environment.CurrentDirectory;
            //获取当前thread的当前应用程序域的基目录，它由程序集冲突解决程序用来探测程序集
            path = System.AppDomain.CurrentDomain.BaseDirectory;
            //获取和设置包含该应用程序的目录的名称(推荐)
            path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            //获取启动了应用程序的可执行文件的路径，包括可执行文件的名称
            path = System.Windows.Forms.Application.ExecutablePath;
            //获取应用程序的当前工作目录(不可靠)
            path = System.IO.Directory.GetCurrentDirectory();
        }
    }
}
