using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWApp
{
    public class SWFileProperties
    {
        public string filepath { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string material { get; set; }
        public string thickness { get; set; }
        public string mass { get; set; }
        public string area { get; set; }
        public string paintQty { get; set; }
        public string drawingNum { get; set; }
        public string Qty { get; set; }
        public string configuration { get; set; }
        public string status { get; set; }
        public string createdBy { get; set; }
        public string checkedBy { get; set; }
        public bool dxfExist { get; set; }
        public bool stepExist { get; set; }
        public string comments { get; set; }
    }
}
