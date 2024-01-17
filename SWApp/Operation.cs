using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWApp
{
    public class Operation
    {
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public DateTime PreparationTime { get; set; }
        public decimal PricePerHour { get; set; }
        public string Description { get; set; }
        public int ID { get; set; }
        public List<SWTreeNode> Nodes { get; set; }
        public decimal PricePetSet { get; set; }

    }
}
