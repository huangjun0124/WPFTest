using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    class ContraVariant
    {
        public static void T()
        {
            ISalary<PP> s = new BaseSalaryCounter<PP>();
            ISalary<MM> t = new BaseSalaryCounter<MM>();
            PrintSalary(s);
            PrintSalary(t);
        }

        static void PrintSalary(ISalary<Employee> s)
        {
            s.Pay();
        }

        // OUT 关键字在反省接口和委托中，用来让类型参数支持协变性。
        // 通过协变，可以使用比声明的参数的派生类更大的参数
        // 去掉 此处的 out， 看情况！！！！！！！！！！！！！！！！！！
        interface ISalary<out T>
        {
            void Pay();
        }

        class BaseSalaryCounter<T> : ISalary<T>
        {
            public void Pay()
            {
                Console.WriteLine("Fuck salary base");
            }
        }

        class Employee
        {
            public string Name;
        }

        class PP : Employee
        {
            public string skill;
        }

        class MM : Employee
        {
            public string manage;
        }
    }

    class InVariant
    {
        //Remove this  IN , see what happens
        interface IMyComparable<in T>
        {
            int Compare(T other);
        }

        class Employee : IMyComparable<Employee>
        {
            public string Name { get; set; }
           
            public int Compare(Employee other)
            {
                return Name.CompareTo(other.Name);
            }
        }

        class PP : Employee, IMyComparable<PP>
        {
            public int Compare(PP other)
            {
                return Name.CompareTo(other.Name);
            }
        }

        class MM : Employee
        {
        }

        public static void T()
        {
            PP p = new PP(){Name = "Mike"};
            MM m = new MM(){Name = "Steve"};
            Test(p, m);
        }

        static void Test<T>(IMyComparable<T> t1, T t2 )
        {
        }
    }
    
}
