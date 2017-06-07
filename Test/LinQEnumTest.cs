using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    class LinQEnumTest
    {
        class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        class MyList : IEnumerable<Person>
        {
            List<Person> list = new List<Person>()
            {
                new Person(){Name = "Mike",Age = 20},
                new Person(){Name = "Mike",Age = 30},
                new Person(){Name = "Rose",Age = 25},
                new Person(){Name = "Steve",Age = 30},
                new Person(){Name = "Jessica",Age = 20}
            };
            public int IteratedNum { get; set; }

            public Person this[int i]
            {
                get { return list[i]; }
                set { list[i] = value; }
            }
            public IEnumerator<Person> GetEnumerator()
            {
                foreach (var item in list)
                {
                    IteratedNum++;
                    yield return item;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public static void T()
        {
            MyList list = new MyList();
            // AsEnumerable is still later valued; ToList is however instant valued
            var temp = (from c in list where c.Age == 20 select c).ToList(); // ToList is needed, or this won't actually work
            Console.WriteLine(list.IteratedNum);
            list.IteratedNum = 0;
            var temp2 = (from c in list where c.Age >= 25 select c).First();
            Console.WriteLine(list.IteratedNum);
            list.IteratedNum = 0;
            // below is interesting, Iter will be only 2
            var temp3 = (from c in list where c.Age >= 25 select c).Take(2).ToList();
            Console.WriteLine(list.IteratedNum);
        }
    }
}
