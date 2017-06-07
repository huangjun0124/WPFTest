using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    class EnumClass
    {

        public static void T()
        {
            Week monday = Week.Monday;
            Week fuck = null;
            //switch (fuck)
            //{
            //    case Week.Monday:
            //        Util.Print("it is fuck");
            //        return;
            //    default:
            //        Util.Print("no");
            //}
        }


        class Week
        {
            public static readonly Week Monday = new Week(0);
            public static readonly Week Tuesday = new Week(1);
            private int _infoType;

            private Week(int infoType)
            {
                _infoType = infoType;
            }

            public override string ToString()
            {
                switch (_infoType)
                {
                    case 0:
                        return "星期一";
                    case 1:
                        return "星期二";
                    default:
                        throw new Exception("不正确的星期信息！");
                }
            }
        }
    }

   
}
