using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    class CustomeList
    {
        class Employee
        {
            public string Name { get; set; }
        }

        class Employees : IEnumerable<Employee>, ICollection<Employee>
        {
            List<Employee> items = new List<Employee>();
            public IEnumerator<Employee> GetEnumerator()
            {
                return items.GetEnumerator();
            }

            public void Add(Employee item)
            {
                item.Name += "Changed!";
                items.Add(item);
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(Employee item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(Employee[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public bool Remove(Employee item)
            {
                throw new NotImplementedException();
            }

            public int Count { get; private set; }
            public bool IsReadOnly { get; private set; }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public static void T()
        {
            Employees lEmployees = new Employees()
            {
                new Employee() {Name = "Mike"},
                new Employee() {Name = "Rose"}
            };
            lEmployees.Add(new Employee() {Name = "Fuck"});
            ICollection<Employee> collTest = lEmployees;
            collTest.Add(new Employee() {Name = "kkk"});
            foreach (var VARIABLE in lEmployees)
            {
                Console.WriteLine(VARIABLE.Name);
            }
        }
    }
}
