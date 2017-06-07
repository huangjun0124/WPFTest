using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    class GenericAndDelegate
    {
        delegate int AddHandler(int i, int j);

        delegate void PrintHandler(string msg);

        // the last param is return value
        private static Func<int, int, int> AddFunc;
        public static void T()
        {
            AddHandler add = Add;
            PrintHandler print = Print;
            print(add(1, 2).ToString());

            Func<int, int, int> addFunc = Add;
            // Action has not return value
            Action<string> printAc = Print;
            printAc(addFunc(3, 4).ToString());

            AddFunc = Add;
            printAc(AddFunc(5, 6).ToString());
        }

        public static void AnonymousDelegateT()
        {
            Func<int, int, int> add = new Func<int, int, int>(
                delegate(int i, int j)
                {
                    return i + j;
                });
            
            Action<string> print = new Action<string>(
                delegate(string msg)
                {
                    Console.WriteLine(msg);
                });

            // Simplify
            add = delegate(int i, int j)
            {
                return i + j;
            };

            print = delegate(string msg)
                {
                    Console.WriteLine(msg);
                };

            // Lambda
            add = (i, j) =>
            {
                return i + j;
            };

            print = (msg) =>
            {
                Console.WriteLine(msg);
            };

        }

        static int Add(int i, int j)
        {
            return i + j;
        }

        static void Print(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
