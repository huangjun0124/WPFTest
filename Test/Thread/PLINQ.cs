using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    class PLINQ
    {
        public static void T()
        {
            List<int> intList = new List<int>(){0,1,2,3,4,5,6,7,8,9,10};
            var query = from p in intList.AsParallel().AsOrdered() select p;
            foreach (int item in query)
            {
                Util.Print(item.ToString());
            }
            Util.Print();
            query.ForAll((item) =>
            {
                Util.Print(item.ToString());
            });
            Util.Print();
            foreach (int item in query.Take(5))
            {
                Util.Print(item.ToString());
            }
        }
    }
}
