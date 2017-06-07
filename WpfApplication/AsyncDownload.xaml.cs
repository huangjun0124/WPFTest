using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for AsyncDownload.xaml
    /// </summary>
    public partial class AsyncDownload : Window
    {
        public AsyncDownload()
        {
            InitializeComponent();
        }

        private FileInfo file = null;
        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            this.barProgress.Value = 0;
            if (string.IsNullOrEmpty(this.txtURL.Text.Trim())) return;
            using (WebClient wc = new WebClient())
            {
                btnDownload.IsEnabled = false;
                file = new FileInfo() {FileName = Path.GetFileName(this.txtURL.Text.Trim())};
                wc.DownloadFileAsync(new Uri(this.txtURL.Text.Trim()), file.FileName);
                wc.DownloadProgressChanged += client_DownloadProgressChanged;
                wc.DownloadFileCompleted += client_DownloadFileCompleted;
            }
        }

        private const string TEXT_PROGRESS = "当前接收到{0}字节({1}KB)，文件大小总共{2}KB({3}MB)";
        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.lblProgress.Content = string.Format(TEXT_PROGRESS, e.BytesReceived, e.BytesReceived / 1024, e.TotalBytesToReceive/1024, Math.Round(e.TotalBytesToReceive/1024.0/1024.0,3));
            this.barProgress.Value = e.ProgressPercentage;
            file.TotalBytes = e.TotalBytesToReceive;
        }

        private void client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            btnDownload.IsEnabled = true;
            file.StopWatch();
            if (e.Cancelled)
            {
                MessageBox.Show("文件下载被取消", "提示", MessageBoxButton.OKCancel);
            }
            MessageBox.Show(string.Format("文件下载成功,总共用时{0}秒,平均下载速度{1}KB/S", file.SecondsUsedToLoad, file.AvgSpeed), "提示");
        }

        private void btnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            if (file == null)
            {
                System.Diagnostics.Process.Start(path);
            }
            else
            {
                //打开资源管理器后默认选中该文件
                System.Diagnostics.Process.Start("Explorer", "/select," + file.FileName);    
            }
        }

        class FileInfo
        {
            private long totalKBytes;
            public long TotalKBytes { get { return totalKBytes; } }

            private long totalBytes ;

            public long TotalBytes
            {
                get { return totalBytes; }
                set
                {
                    totalBytes = value;
                    totalKBytes = totalBytes/1024;
                }
            }

            public string FileName;
            private Stopwatch watch = new Stopwatch();

            private void StartWatch()
            {
                watch.Start();
            }

            public void StopWatch()
            {
                watch.Stop();
            }

            public long SecondsUsedToLoad{get { return watch.ElapsedMilliseconds/1000; }}

            public long AvgSpeed{get { return TotalKBytes/SecondsUsedToLoad; }}

            public FileInfo()
            {
                StartWatch();
            }
        }
    }
}
