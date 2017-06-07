using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace Test
{
    class SingletonTest
    {
    }

    /*
     * 单例和静态类是两种完全不同的概念，静态类不可以作为单件模式的一种实现方法
     * 单例是一个对象，静态类不满足这一点
     * 实际上，静态类也违反了面向对象三大特性中的两项：继承和多态
     *      无法让一个静态类从其他类型继承
     *      也不能让静态类作为参数和返回值进行传递
     */
    public sealed class Singleton
    {
        static Singleton instance = null;
        static readonly object padlock = new object();

        Singleton()
        {}

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new Singleton();
                        }
                    }
                }
                return instance;
            }
        }
    }

    static class SampleClass
    {
        static FileStream fileStream;

        static SampleClass()
        {
            try
            {
                fileStream = new FileStream(@".\temp.txt", FileMode.Open);
            }
            catch (FileNotFoundException ex)
            {
                Util.Print(ex.Message);
            }
        }

        public static void SampleMethodTest()
        {
            
        }
    }
}
