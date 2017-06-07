using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormT
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += new ThreadExceptionEventHandler(UIThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //Application.ApplicationExit += new EventHandler(Application_ApplicationExit);

            
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            
            
            


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void UIThreadException(object sender, ThreadExceptionEventArgs t)
        {
            try
            {
                string msg = @"Windows窗体线程异常:\n\n";
                MessageBox.Show(msg + t.Exception.Message + Environment.NewLine + t.Exception.StackTrace);
            }
            catch 
            {
                try
                {
                    MessageBox.Show("不可恢复的Windows窗体异常，应用程序将退出！");
                }
                finally
                {
                    Application.Exit();
                }
                
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception) e.ExceptionObject;
                string msg = @"非窗体线程异常:\n\n";
                MessageBox.Show(msg + ex.Message + Environment.NewLine + ex.StackTrace);
            }
            catch
            {
                try
                {
                    MessageBox.Show("不可恢复的非Windows窗体线程异常，应用程序将退出！");
                }
                finally
                {
                    Application.Exit();
                }

            }
        }
    }
}
