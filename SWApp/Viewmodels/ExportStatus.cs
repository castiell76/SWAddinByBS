using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWApp.Models;

namespace SWApp.Viewmodels
{
    public class ExportStatus
    {
        [ColumnName("nazwa")]
        [ColumnVisibility(true)]
        public string filepath { get; set; }

        [ColumnVisibility(true)]
        [ColumnName("utworzono DXF?")]
        public bool dxfCreated { get; set; }

        [ColumnVisibility(true)]
        [ColumnName("utworzono STEP?")]
        public bool stepCreated { get; set; }

        [ColumnVisibility(false)]
        public string sigmaNote { get; set; }
        
    }
}
