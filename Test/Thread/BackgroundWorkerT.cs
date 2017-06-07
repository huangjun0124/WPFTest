using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace Test
{
    class BackgroundWorkerT
    {
        public static void T()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += (sender, args) =>
            {
                BackgroundWorker woker = sender as BackgroundWorker;
                for (int i = 0; i < 100; i++)
                {
                    woker.ReportProgress(i);
                    Thread.Sleep(100);
                }
            };

            worker.ProgressChanged += (sender, args) =>
            {
                Console.WriteLine(args.ProgressPercentage + "%");
            };

            worker.RunWorkerAsync();
        }
    }
}
