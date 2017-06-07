using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    class Util
    {
        public static void Print(string s)
        {
            Console.WriteLine(s);
        }

        public static void Print()
        {
            Console.WriteLine();
        }

        public static string ReadLine()
        {
            return Console.ReadLine();
        }

        public static void Print(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }

        public static string NewLine = Environment.NewLine;
    }
}
