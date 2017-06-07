using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Test
{
    class ClosureTest
    {
        public static void T()
        {
            List<Action> list = new List<Action>();
            for (int i = 0; i < 5; i++)
            {
                Action t = () =>
                {
                    Console.WriteLine(i.ToString());
                };
                list.Add(t);
            }
            foreach (Action t in list)
            {
                t();
            }
        }

        class TempClass
        {
            public int i;

            public void TempFunc()
            {
                Console.WriteLine(i.ToString());
            }
        }

        public static void T1()
        {
            /*
             * 闭包对象就是编译器自动在这种情形中生成的TempClass对象
             * 如果匿名方法内部引用了某个局部变量，编译器就会自动将该引用提升到该闭包对象中。
             * 即将for循环中的变量i修改成了引用闭包对象的公共变量i。如此一来，即使代码执行后离开了原局部变量i的
             *      作用域，包含该闭包对象的作用域也还存在。
             */
            List<Action> list = new List<Action>();
            TempClass tempClass = new TempClass();
            for (tempClass.i = 0; tempClass.i < 5; tempClass.i++)
            {
                Action t = tempClass.TempFunc;
                list.Add(t);
            }
            foreach (Action t in list)
            {
                t();
            }
            // this function is same as T()
        }

        public static void T2()
        {
            List<Action> list = new List<Action>();
            for (int i = 0; i < 5; i++)
            {
                int temp = i;
                Action t = () =>
                {
                    Console.WriteLine(temp.ToString());
                };
                list.Add(t);
            }
            foreach (Action t in list)
            {
                t();
            }
        }

        public static void T3()
        {
            // this function is same as T2()
            List<Action> list = new List<Action>();
            for (int i = 0; i < 5; i++)
            {
                TempClass tempClass = new TempClass();
                tempClass.i = i;
                Action t = tempClass.TempFunc;
                list.Add(t);
            }
            
            foreach (Action t in list)
            {
                t();
            }
        }
    }
}
