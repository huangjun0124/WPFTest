using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Test
{
    class CancelThread
    {
        public static void StartTest()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.Token.Register(() =>
            {
                // this code gets run right after Cancel called
                Console.WriteLine("Worker thread is terminated....");
            });
            Thread t = new Thread((() =>
            {
                while (true)
                {
                    if (cts.Token.IsCancellationRequested)
                    {
                        Console.WriteLine("Thread is now canceled!");
                        break;
                    }
                    Console.WriteLine(DateTime.Now.ToString());
                    Thread.Sleep(1000);
                }
            }));
            t.Start();
            Console.WriteLine("Input any key to calcel thread");
            Console.ReadLine();
            cts.Cancel();
        }
    }
}
