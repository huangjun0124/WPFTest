using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    class VirtualInConstructer
    {
        public static void T()
        {
            American american = new American();
        }
    }

    class PA
    {
        public PA()
        {
            InitSkin();
        }

        protected virtual void InitSkin()
        {
        }
    }

    class American : PA
    {
        Race race;

        public American() : base()
        {
            race = new Race(){Name = "White"};
        }

        protected override void InitSkin()
        {
            Util.Print(race.Name);
        }
    }

    class Race
    {
        public string Name { get; set; }
    }
}
