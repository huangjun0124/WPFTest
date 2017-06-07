using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Test.MD5;

namespace Test
{
    class Program
    {
        [DllImport("SamleCpp.dll")]
        private extern static void Method1();

        [DllImport("SamleCpp.dll",CallingConvention = CallingConvention.Cdecl)]
        private extern static Int32 Method2(int i);

        static void CallCppT()
        {
            try
            {
                int r = Method2(5);
            }
            catch (Exception e)
            {
            }
        }

        static void Main(string[] args)
        {
            CallCppT();
            //EncryptFile();
            Console.Read();
        }

        static void EncryptFile()
        {
            EncryptDemo.T();
        }

        static void Md5Tes()
        {
            //Md5Test.T();
            Md5Test.T1();
        }

        static void CallVirtualInBaseConstructer()
        {
            VirtualInConstructer.T();
        }

        static void MemberDesignT()
        {
            MemberDesign.T();
        }

        static void TaskExceptionT()
        {
            //TaskException.T();
            TaskException.T2();
        }

        static void PLINQT()
        {
            PLINQ.T();
        }

        static void TaskTestT()
        {
            //TaskTest.T3();
            //TaskTest.ParallelT1();
            TaskTest.ParallelException();
        }

        static void BackgroundWorkerTest()
        {
            BackgroundWorkerT.T();
        }

        static void CancelThreadT()
        {
            CancelThread.StartTest();
        }

        static void ResetEventTest()
        {
            AutoResetEventT.btnStartClick();
            Thread.Sleep(2000);
            AutoResetEventT.btnSetClick();
            HeartBeatTest.StartBeatCheck();
            Thread.Sleep(2000);
            HeartBeatTest.SendBackBeatPack();
        }

        static void ThreadAndAsyncTest()
        {
            ThreadAndAsync.T();
        }

        static void SerializeT()
        {
            SerializeTest.T();
            SerializeTest.T2();
            SerializeTest.T3();
        }

        static void CustomeEvengHanTest()
        {
            CustomeEvengHan.T();
        }

        static void GenericAndDelegateTest()
        {
            GenericAndDelegate.T();
        }

        static void LinqEnumTest()
        {
            LinQEnumTest.T();
        }

        static void CustomeListTest()
        {
            CustomeList.T();
        }

        static void TreadCollectionTest()
        {
            // would popup error: Collection was modified: InvalidOperationException
            //ThreadCollectionTest.T();

            //ThreadCollectionTest.T1();
            ThreadCollectionTest.T2();
        }

        static void InheritPolyTest()
        {
            Father Fa = new Father();
            A aa = new A();
            B bb = new B();
            Fa.Fuck();
            Fa.FuckFuck();
            aa.Fuck();
            aa.FuckFuck();
            bb.Fuck();
            bb.FuckFuck();
            Fa = aa;
            Fa.Fuck();
            Fa.FuckFuck();
        }
    }
}
