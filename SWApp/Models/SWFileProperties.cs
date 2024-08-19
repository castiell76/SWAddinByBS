using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWApp
{
    public class SWFileProperties
    {
        [ColumnName("typ")]
        [ColumnVisibility(true)]
        public string type { get; set; }

        [ColumnName("nazwa")]
        [ColumnVisibility(true)]
        public string filepath { get; set; }

        [ColumnName("opis")]
        [ColumnVisibility(true)]
        public string description { get; set; }

        [ColumnName("materiał")]
        [ColumnVisibility(true)]
        public string material { get; set; }

        [ColumnName("index")]
        [ColumnVisibility(true)]
        public string index { get; set; }

        [ColumnName("grubość")]
        [ColumnVisibility(true)]
        public string thickness { get; set; }

        [ColumnName("masa")]
        [ColumnVisibility(true)]
        public string mass { get; set; }

        [ColumnName("powierzchnia")]
        [ColumnVisibility(true)]
        public string area { get; set; }

        [ColumnName("ilość farby")]
        [ColumnVisibility(true)]
        public string paintQty { get; set; }

        [ColumnName("numer rysunku")]
        [ColumnVisibility(true)]
        public string drawingNum { get; set; }

        [ColumnName("sztuk na kpl")]
        [ColumnVisibility(true)]
        public string Qty { get; set; }

        [ColumnVisibility(true)]
        public string configuration { get; set; }

        [ColumnVisibility(false)]
        public string status { get; set; }

        [ColumnName("utworzył")]
        [ColumnVisibility(true)]
        public string createdBy { get; set; }

        [ColumnName("sprawdził")]
        [ColumnVisibility(true)]
        public string checkedBy { get; set; }

        [ColumnName("czy istnieje dxf?")]
        [ColumnVisibility(true)]
        public bool dxfExist { get; set; }

        [ColumnName("czy istnieje step?")]
        [ColumnVisibility(true)]
        public bool stepExist { get; set; }

        [ColumnName("uwagi")]
        [ColumnVisibility(true)]
        public string comments { get; set; }



        [ColumnVisibility(false)]
        public string assemblyFilePath {  get; set; }

        [ColumnVisibility(false)]
        public string assemblyConfig {  get; set; }

    }
}
