using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Test
{
    class ThreadCollectionTest
    {
        static List<string> list = new List<string>()
        {
            "Rose","Steve","Jessoca","Fucker"
        }; 
        static AutoResetEvent autoSet = new AutoResetEvent(false);

        public static void T()
        {
            System.Threading.Thread t1 = new System.Threading.Thread((() =>
            {
                autoSet.WaitOne();
                foreach (var VARIABLE in list)
                {
                    Console.WriteLine("t:"+VARIABLE);
                    Thread.Sleep(1000);
                }
            }));
            t1.Start();
            System.Threading.Thread t2 = new System.Threading.Thread((() =>
            {
                autoSet.Set();
                Thread.Sleep(1000);
                list.RemoveAt(2);
            }));
            t2.Start();
        }

        static ArrayList list1 = new ArrayList()
        {
            "Rose","Steve","Jessoca","Fucker"
        };
        public static void T1()
        {
            Thread t1 = new Thread((() =>
            {
                autoSet.WaitOne();
                lock (list1.SyncRoot)
                {
                    foreach (var VARIABLE in list1)
                    {
                        Console.WriteLine("t1:" + VARIABLE);
                        Thread.Sleep(1500);
                    }
                }
            }));
            t1.Start();
            Thread t2 = new Thread((() =>
            {
                autoSet.Set();
                Thread.Sleep(1000);
                lock (list1.SyncRoot)
                {
                    // below codes will be blocked until t1's foreach is over
                    list1.RemoveAt(1);
                    Console.WriteLine("t2 deleted");
                }
            }));
            t2.Start();
        }

        static object sycObj = new object();
        public static void T2()
        {
            Thread t1 = new Thread((() =>
            {
                autoSet.WaitOne();
                lock (sycObj)
                {
                    foreach (var VARIABLE in list)
                    {
                        Console.WriteLine("t1:" + VARIABLE);
                        Thread.Sleep(1500);
                    }
                }
            }));
            t1.Start();
            Thread t2 = new Thread((() =>
            {
                autoSet.Set();
                Thread.Sleep(1000);
                lock (sycObj)
                {
                    // below codes will be blocked until t1's foreach is over
                    list.RemoveAt(1);
                    Console.WriteLine("t2 deleted");
                }
            }));
            t2.Start();
        }
    }
}
