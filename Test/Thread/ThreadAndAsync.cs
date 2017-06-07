using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Test
{
    /*
     * 计算密集型工作 -----> 采用多线程
     * IO密集型工作   -----> 采用异步机制
     */
    class ThreadAndAsync
    {
        public static void T()
        {
            UseThread.Fuck();
            UseAsyncCall.FuckAsync();
        }

        class UseThread
        {
            /*Exp:
             * 一下程序解决了界面阻滞的问题，但是，它不高效。
             * DMA(Direct Memory Access)模式：不经过CPU而直接进行内存数据损耗的数据交换模式：硬盘，网卡，声卡，显卡等都具有此功能
             *      CLR提供的异步编程模型就是让我们充分利用硬件的DMA功能来释放CPU的压力
             * 以下代码：CLR起了一个工作线程，然后在读取网页的整个过程中，该工作线程始终被阻滞，知道网页获取完毕。
             *      在这整个过程中，工作线程被占用着。这意味着系统的资源始终被消耗和等待
             */
            public static void Fuck()
            {
                Stopwatch watch = Stopwatch.StartNew();
                // this creates a new worker thread
                System.Threading.Thread t = new System.Threading.Thread(() =>
                {
                    var request = HttpWebRequest.Create("http://www.cnblogs.com/luminji");
                    var response = request.GetResponse();
                    var stream = response.GetResponseStream();
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        var content = reader.ReadLine();
                        Console.WriteLine(content);
                        Console.WriteLine(watch.ElapsedMilliseconds);
                    }
                });
                t.Start();
            }
        }

        /*Exp:
         * 这采用了异步模式，使用线程池进行管理。
         *      新起异步操作后，CLR会将工作丢给线程池中的某个工作线程来完成。当开始I/O操作的时候，异步会将工作线程还给线程池，
         *      这时候就相当于获取网页的这个工作不会在占用任何CPU资源了。直到异步完成，异步才会通过回掉的方式通知线程池，让
         *      CLR响应异步完毕。因此，异步模式借助于线程池，极大的节约了CPU的资源。
         */
        class UseAsyncCall
        {
            private static Stopwatch watch = null;
            public static void FuckAsync()
            {
                watch = Stopwatch.StartNew();
                var request = HttpWebRequest.Create("http://www.cnblogs.com/luminji");
                request.BeginGetResponse(AsyncCallbackImpl, request);
            }

            private static void AsyncCallbackImpl(IAsyncResult ar)
            {
                WebRequest request = ar.AsyncState as WebRequest;
                var response = request.EndGetResponse(ar);
                var stream = response.GetResponseStream();
                using (StreamReader reader = new StreamReader(stream))
                {
                    var content = reader.ReadLine();
                    Console.WriteLine(content);
                    Console.WriteLine(watch.ElapsedMilliseconds);
                }
            }
        }
    }
}
