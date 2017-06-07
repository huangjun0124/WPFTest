using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.MemnerDesign
{
    class ClassExtensionMethod
    {
        public static void T()
        {
            Student s = new Student();
            s.GetSexString();

            FuckStu fs = new FuckStu();
            fs.GetSexString();
        }
    }

    /*
     * 扩展方法：
     *      可以扩展密封类型
     *      可以扩展第三方程序集中的类型
     *      可以避免不必要的深度继承体系
     *    必须在静态类中，且该类不能是一个嵌套类
     *    方法必须是静态的
     *    方法的第一个参数必须是要扩展的类型，必须加上了this关键字
     *    不支持扩展属性和事件
     */
    static class StudentExtension
    {
        public static string GetSexString(this Student student)
        {
            return student.GetSex() == true ? "Male" : "Female";
        }
    }

    class Student
    {
        public bool GetSex()
        {
            return false;
        }
    }

    class FuckStu : Student
    {
    }
}
