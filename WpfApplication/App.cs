using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace WpfApplication
{
    public partial class App 
    {
        public App()
        {
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(Application_DispatcherUnhandledException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = e.Exception;
                string msg = @"WPF窗体线程异常:\n\n";
                MessageBox.Show(msg + ex.Message + Environment.NewLine + ex.StackTrace);
            }
            catch
            {
                try
                {
                    MessageBox.Show("不可恢复的WPF窗体异常，应用程序将退出！");
                }
                finally
                {
                    Application.Current.Shutdown();
                }
            }
            // below is a must!!!!!!!!!!
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = e.ExceptionObject as Exception;
                string msg = @"非WPF窗体线程异常:\n\n";
                MessageBox.Show(msg + ex.Message + Environment.NewLine + ex.StackTrace);
            }
            catch
            {
                try
                {
                    MessageBox.Show("不可恢复的WPF窗体线程异常，应用程序将退出！");
                }
                finally
                {
                    Application.Current.Shutdown();
                }
            }
        }
    }
}
