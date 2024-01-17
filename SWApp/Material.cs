using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWApp
{
    public class Material
    {
        public string Name { get; set; }
        public decimal Price {  get; set; }
        public string Description { get; set; }
        public int ID { get; set; }
        public List<SWTreeNode> Nodes { get; set; }
        public decimal PricePetSet { get; set; }

    }
}
