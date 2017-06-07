using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserDictionary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private bool isPlaying = false;
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            SysConsole.Show();
            txt.Text = DateTime.Now.ToString("yyyy/MM/dd");
            media.Source = new Uri(@"C:\Users\jhuang153889\Videos\Lync Recordings\Video_2017-01-12_162930.wmv",
                UriKind.Absolute);
            media.Play();
            SysConsole.Print("Play...");
            isPlaying = true;
            CustomerWindow cw = new CustomerWindow();
            cw.Show();
            FontViewer fv = new FontViewer();
            fv.Show();
        }

        private void MainWindow_OnActivated(object sender, EventArgs e)
        {
            if (!isPlaying)
            {
                media.Play();
                isPlaying = true;
                SysConsole.Print("Play...");
            }
        }

        private void MainWindow_OnDeactivated(object sender, EventArgs e)
        {
            if (isPlaying)
            {
                media.Pause();
                isPlaying = false;
                SysConsole.Print("Pause...");
            }
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Quit Application?", "System information",
                MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
            else
            {
                Application.Current.Shutdown(0);
            }
        }
    }
}
