using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Test
{
    class AutoResetEventT
    {
        /*
         * AutoResetEvent when Set(), it only wakes one thread waiting
         * ManualResetEvent when Set(), it wakes every thread waiting
         */

        //默认阻滞状态为false，意味着任何在它上面等待的线程都将被阻滞
       // static AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        static ManualResetEvent autoResetEvent = new ManualResetEvent(false);

        public static void btnStartClick()
        {
            StartThread1();
            StartThread2();
        }

        private static void StartThread1()
        {
            Thread tWork1 = new Thread(() =>
            {
                Console.WriteLine( @"线程1启动..." + Environment.NewLine);
                Console.WriteLine( @"线程1...开始等待别的线程的信号..." + Environment.NewLine);
                autoResetEvent.WaitOne();
                Console.WriteLine(@"线程1...结束.....");
            });
            tWork1.IsBackground = true;
            tWork1.Start();
        }

        private static  void StartThread2()
        {
            Thread tWork2 = new Thread(() =>
            {
                Console.WriteLine(@"线程2启动..." + Environment.NewLine);
                Console.WriteLine(@"线程2...开始等待别的线程的信号..." + Environment.NewLine);
                autoResetEvent.WaitOne();
                Console.WriteLine(@"线程2...结束.....");
            });
            tWork2.IsBackground = true;
            tWork2.Start();
        }

        public static void btnSetClick()
        {
            autoResetEvent.Set();
        }
    }

    class HeartBeatTest
    {
        static AutoResetEvent autoResetEvent = new AutoResetEvent(false);

        public static void StartBeatCheck()
        {
            Thread t = new Thread((() =>
            {
                while (true)
                {
                    // 等待3秒，3秒没有信号，则显示断开
                    //         有信号，则显示更新
                    bool re = autoResetEvent.WaitOne(3000);
                    if (re)
                    {
                        Console.WriteLine("Date:{0},{1}",DateTime.Now.ToString(),"Connection is alive...");
                    }
                    else
                    {
                        Console.WriteLine("Date:{0},{1}", DateTime.Now.ToString(), "Connection closed");
                    }
                }
            }));
            t.IsBackground = true;
            t.Start();
        }

        public static void SendBackBeatPack()
        {
            autoResetEvent.Set();
        }
    }
}
