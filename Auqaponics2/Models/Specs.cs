using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auqaponics2.Models
{
    public class Specs
    {
        public int SpecsId { get; set; }

        public int TankSize { get; set; }

        public string Plants { get; set; }

        public string Fish { get; set; }

        public int WaterTemp { get; set; }

        public int pH { get; set; }


    }
}
