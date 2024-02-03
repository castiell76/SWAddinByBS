using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWApp.Models
{
    public class Operation
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public double Rate { get; set; }

        public double Time { get; set; }

        public double PricePerItem
        {
            get
            {
                if (Unit == "s")
                {
                    Time = Time / 3600;

                }
                else if (Unit == "min")
                {
                    Time = Time / 60;
                }
                return Time * Rate * QuantityPerItem;
            }
            set { PricePerItem = value; }
        }

        public int QuantityPerItem { get; set; }
        public double TPZ { get; set; }
        public string Unit { get; set; }

    }
}
