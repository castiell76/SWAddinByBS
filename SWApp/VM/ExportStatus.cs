using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWApp.Models;

namespace SWApp.VM
{
    public class ExportStatus : ConvertStatus
    {
        public bool stepCreated { get; set; }
        public bool dxfCreated { get; set; }
        public string sigmaNote { get; set; }
        public string dxfFilepath { get; set; }
    }
}
