using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Test
{
    class Temp
    {
        public static void T()
        {
            Stopwatch watch = Stopwatch.StartNew();
            int x = 0;
            for (int i = 0; i < 10000; i++)
            {
                try
                {
                    int j = i/x;
                }
                catch (Exception)
                {
                }
            }
            Console.WriteLine(watch.ElapsedMilliseconds);
            watch = Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
            {
                if (x == 0)
                {
                    continue;
                }
                int j = i/x;
            }
            Console.WriteLine(watch.ElapsedMilliseconds);
        }
    }

    interface ISalary<out T, S>
    {
        void Pay(S s);
        T PayU();
    }

}
