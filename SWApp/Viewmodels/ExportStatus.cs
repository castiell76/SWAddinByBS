using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWApp.Models;

namespace SWApp.Viewmodels
{
    public class ExportStatus : ConvertStatus
    {
        public string dxfFilepath { get; set; }
        public bool dxfCreated { get; set; }
        public bool stepCreated { get; set; }
       
        public string sigmaNote { get; set; }
        
    }
}
