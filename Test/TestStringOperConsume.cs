using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    public class TestStringOperConsume
    {
        public static void TestStarter()
        {
            ConcactTest();
            FormatTest();
            StringBuilderTest();
        }

        public static string ConcactTest()
        {
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            long start = GC.GetTotalMemory(true);
            
            // 在这里写需要被测试内存消耗的代码，例如，创建一个GcMultiRow
            string ret = "";
            for (int i = 0; i < 100000; i++)
            {
                ret += i;
            }
            watch.Stop();
            var useTime = (double)watch.ElapsedMilliseconds / 1000;

            GC.Collect();
            // 确保所有内存都被GC回收
            GC.WaitForFullGCComplete();
            long end = GC.GetTotalMemory(true);
            long useMemory = end - start;
            Console.WriteLine($"ConcactTest: memory used[{useMemory}], time used[{useTime}]");
            return ret;
        }

        public static string FormatTest()
        {
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            long start = GC.GetTotalMemory(true);

            // 在这里写需要被测试内存消耗的代码，例如，创建一个GcMultiRow
            string ret = "";
            for (int i = 0; i < 100000; i++)
            {
                ret = string.Format("{0}{1}", ret, i);
            }
            watch.Stop();
            var useTime = (double)watch.ElapsedMilliseconds / 1000;

            GC.Collect();
            // 确保所有内存都被GC回收
            GC.WaitForFullGCComplete();
            long end = GC.GetTotalMemory(true);
            long useMemory = end - start;
            Console.WriteLine($"FormatTest: memory used[{useMemory}], time used[{useTime}]");
            return ret;
        }

        public static string StringBuilderTest()
        {
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            long start = GC.GetTotalMemory(true);

            // 在这里写需要被测试内存消耗的代码，例如，创建一个GcMultiRow
            StringBuilder ret = new StringBuilder(10000);
            for (int i = 0; i < 100000; i++)
            {
                ret.Append(i);
            }
            watch.Stop();
            var useTime = (double)watch.ElapsedMilliseconds / 1000;

            GC.Collect();
            // 确保所有内存都被GC回收
            GC.WaitForFullGCComplete();
            long end = GC.GetTotalMemory(true);
            long useMemory = end - start;
            Console.WriteLine($"StringBuilderTest: memory used[{useMemory}], time used[{useTime}]");
            return ret.ToString();
        }
    }
}
