using System;
using System.Collections.Generic;

namespace AdvantShop.Web.Admin.ViewModels.Analytics
{
    public class ExportCustomersModel
    {

        public int Group { get; set; }

        public string Encoding { get; set; }
        
        public string ColumnSeparator {get;set;}

        public string PropertySeparator { get; set; }
        
        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }


        public Dictionary<string, string> Encodings { get; set; }

        public Dictionary<string, string> Separators { get; set; }

        public Dictionary<int, string> Groups { get; set; }
       
    }
}
