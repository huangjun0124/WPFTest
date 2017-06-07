using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Test
{
    class CustomeEvengHan
    {
        class FileUploadedEventArgs : EventArgs
        {
            public int FileProgress { get; set; }
        }

        class FileUploader
        {
            public event EventHandler<FileUploadedEventArgs> FileUploaded;

            public void Upload()
            {
                FileUploadedEventArgs e = new FileUploadedEventArgs() {FileProgress = 0};
                while (e.FileProgress < 100)
                {
                    e.FileProgress++;
                    if (FileUploaded != null)
                    {
                        FileUploaded(this, e);
                    }
                    Thread.Sleep(10);

                }
            }
        }

        public static void T()
        {
            FileUploader loader = new FileUploader();
            loader.FileUploaded += Progress;
            loader.Upload();
        }

        static void Progress(object sender, FileUploadedEventArgs e)
        {
            Console.WriteLine(e.FileProgress);
        }
    }
}
