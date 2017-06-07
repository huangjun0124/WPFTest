using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    class Father
    {
        public void Fuck()
        {
            Console.WriteLine("Fuck god");
        }

        public virtual void FuckFuck()
        {
            Console.WriteLine("virtual fuck god");
        }
    }

    class A : Father
    {
        public new  void Fuck()
        {
            Console.WriteLine("Fuck AA");
        }

        public override void FuckFuck()
        {
            Console.WriteLine("overrid fuckfuck aa");
        }
    }

    class B : Father
    {
        public void Fuck()
        {
            Console.WriteLine("Fuck BB");
        }

        public override void FuckFuck()
        {
            Console.WriteLine("overrid fuckfuck bb");
        }
    }
}
