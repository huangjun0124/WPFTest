using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Test
{
    public class Binaryserializer
    {
        /*
         * 将类型序列化为字符串
         */
        public static string Serialize<T>(T t)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, t);
                return System.Text.Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /*
         * 将类型序列化为文件
         */
        public static void SerializeToFile<T>(T t, string path, string fullName)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fullPath = string.Format(@"{0}\{1}", path, fullName);
            using (FileStream stream = new FileStream(fullPath,FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, t);
                stream.Flush();
            }
        }

        /*
         *将字符串反序列化为类型
         */
        public static TResult Deserialize<TResult>(string s) where TResult : class
        {
            byte[] bb = System.Text.Encoding.UTF8.GetBytes(s);
            using (MemoryStream stream = new MemoryStream(bb))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(stream) as TResult;
            }
        }

        /*
         *将文件反序列化为类型
         */
        public static TResult DeserializeFromFile<TResult>(string fullPath) where TResult : class
        {
            using (FileStream stream = new FileStream(fullPath,FileMode.Open))
            {
                BinaryFormatter formmater = new BinaryFormatter();
                return formmater.Deserialize(stream) as TResult;
            }
        }
    }

    class SerializeTest
    {
        static void mike_FieldChanged(object sender, EventArgs e)
        {
            Console.WriteLine("field changed..." + (sender as Person).FirstName + (sender as Person).Age);   
        }

        public static void T()
        {
            Person mike = new Person() {Age = 21, FirstName = "Mike", LastName = "Fuck",ChineaseName = "What fuck",Department = "FuckDeris"};
            mike.NameChanged += new EventHandler(mike_FieldChanged);
            mike.AgeChanged += new EventHandler(mike_FieldChanged);
            mike.Age = 55;
            Binaryserializer.SerializeToFile(mike,@".\","person.txt");
            Person p = Binaryserializer.DeserializeFromFile<Person>(@".\person.txt");
            p.FirstName = "Rose";
            p.Age = 23;
        }

        public static void T2()
        {
            Person2 mike = new Person2() { FirstName = "Mike", LastName = "Fuck", ChineaseName = "What fuck" };
            Binaryserializer.SerializeToFile(mike, @".\", "person.txt");
            PersonAnother p = Binaryserializer.DeserializeFromFile<PersonAnother>(@".\person.txt");
        }

        public static void T3()
        {
            Employee mike = new Employee() { Name = "What fuck", Salary = 5000};
            Binaryserializer.SerializeToFile(mike, @".\", "person.txt");
            Employee p = Binaryserializer.DeserializeFromFile<Employee>(@".\person.txt");
        }
    }

    [Serializable]
    class Person
    {
        /*
         *（1）属性本质上是方法，所以不能将  NonSerialized  特性应用于属性上
         *（2）要让事件不能被序列化，需使用改进的特性语法  field:NonSerialized
         *
         * (3) OnDeserializedAttribute 当他应用于方法时，会指定对象反序列化后立即调用此方法
         * (4) OnDeserializingAttribute ~~~~~~~~~~~~~, 会指定对象反序列化时调用此方法
         * (5) OnSerializedAttribute    序列化该对象后调用此方法
         * (6) OnSerializingAttribute   序列化对象时调用此方法
         */

        private string firstName;
        public string LastName;
        private int age;

        [NonSerialized] private string department;
        public string Department
        {
            set { department = value; }
            get{return department;}
        }

        public string FirstName
        {
            set
            {
                firstName = value;
                if (NameChanged != null)
                {
                    NameChanged(this, null);
                }
            }
            get { return firstName; }
        }

        public int Age
        {
            set
            {
                age = value;
                if (AgeChanged != null)
                {
                    AgeChanged(this, null);
                }
            }
            get { return age; }
        }

        public event EventHandler NameChanged;

        [field:NonSerialized]
        public event EventHandler AgeChanged;

        [NonSerialized]
        public string ChineaseName;

        [OnDeserializedAttribute]
        void OnSerialized(StreamingContext context)
        {
            ChineaseName = string.Format("{0}{1}", LastName, firstName);
        }
    }

    [Serializable]
    class PersonAnother : ISerializable
    {
        public PersonAnother()
        {
            
        }

        public string Name { get; set; }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name",Name);
        }

        protected PersonAnother(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("Name");
        }
    }

    [Serializable]
    class Person2 : ISerializable
    {
        public string FirstName;
        public string LastName;
        public string ChineaseName;

        public Person2()
        {
        }

        protected Person2(SerializationInfo info, StreamingContext context)
        {
            
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // below tells the serializer that i need to be DeSerialized into class PersonAnother
            info.SetType(typeof(PersonAnother)); // this is very import ............................................
            info.AddValue("Name",string.Format("{0}{1}",LastName,FirstName));
        }
    }

    [Serializable]
    class Employee:PersonAnother, ISerializable
    {
        public int Salary;

        public Employee()
        {
            
        }

        protected Employee(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Salary = info.GetInt32("Salary");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);   
            info.AddValue("Salary", Salary);
        }
    }


}
