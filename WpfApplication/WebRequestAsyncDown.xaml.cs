using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using Path=System.IO.Path;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for WebRequestAsyncDown.xaml
    /// </summary>
    public partial class WebRequestAsyncDown : Window
    {

        public WebRequestAsyncDown()
        {
            InitializeComponent();
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            string url = this.txtURL.Text.Trim();
            if (string.IsNullOrEmpty(url)) return;
            btnDownload.IsEnabled = false;
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(new Uri(url));
            RequestState requestState = new RequestState();
            requestState.BUFFER_SIZE = 1024;
            requestState.BufferRead = new byte[requestState.BUFFER_SIZE];
            requestState.Request = request;
            requestState.SavePath = Path.Combine("C:\\Users\\huang\\Desktop\\", Path.GetFileName(url));
            requestState.FileStream = new FileStream(requestState.SavePath,FileMode.OpenOrCreate);

            request.BeginGetResponse(new AsyncCallback(ResponseCallback), requestState);
        }

        private void ResponseCallback(IAsyncResult asyncResult)
        {
            RequestState requestState = asyncResult.AsyncState as RequestState;
            requestState.Response = (HttpWebResponse) requestState.Request.EndGetResponse(asyncResult);
            Stream responseStream = requestState.Response.GetResponseStream();
            requestState.ResponseStream = responseStream;

            responseStream.BeginRead(requestState.BufferRead, 0, requestState.BufferRead.Length, ReadCallBack,
                requestState);
        }

        private void ReadCallBack(IAsyncResult asyncResult)
        {
            RequestState requestState = asyncResult.AsyncState as RequestState;
            int read = requestState.ResponseStream.EndRead(asyncResult);
            if (read > 0)
            {
                requestState.Totalbytes += read;
                requestState.FileStream.Write(requestState.BufferRead,0,read);
                requestState.ResponseStream.BeginRead(requestState.BufferRead, 0, requestState.BufferRead.Length,
                    ReadCallBack, requestState);
            }
            else
            {
                requestState.StopWatch();
                requestState.Response.Close();
                requestState.FileStream.Close();
                MessageBox.Show(string.Format("文件下载成功,总共用时{0}秒,平均下载速度{1}KB/S", requestState.SecondsUsedToLoad,
                    requestState.Totalbytes / 1024 / requestState.SecondsUsedToLoad), "提示");
                btnDownload.IsEnabled = true;
            }
        }

        private void btnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            {
                //打开资源管理器后默认选中该文件
                System.Diagnostics.Process.Start("Explorer", "C:\\Users\\huang\\Desktop\\");
            }
        }
    }

    public class RequestState
    {
        /// <summary>
        /// 缓冲区大小
        /// </summary>
        public int BUFFER_SIZE { get; set; }
        /// <summary>
        /// 缓冲区
        /// </summary>
        public byte[] BufferRead { get; set; }
        /// <summary>
        /// 保存路径
        /// </summary>
        public string SavePath { get; set; }
        /// <summary>
        /// 请求流
        /// </summary>
        public HttpWebRequest Request { get; set; }
        /// <summary>
        /// 响应流
        /// </summary>
        public HttpWebResponse Response { get; set; }
        /// <summary>
        /// 流对象
        /// </summary>
        public Stream ResponseStream { get; set; }
        /// <summary>
        /// 文件流
        /// </summary>
        public FileStream FileStream { get; set; }
        /// <summary>
        /// 文件总大小
        /// </summary>
        public long Totalbytes { get; set; }
        private Stopwatch watch = new Stopwatch();
        private void StartWatch()
        {
            watch.Start();
        }

        public void StopWatch()
        {
            watch.Stop();
        }

        public long SecondsUsedToLoad { get { return watch.ElapsedMilliseconds / 1000; } }

        public RequestState()
        {
            StartWatch();
            Totalbytes = 0;
        }
    }

}
