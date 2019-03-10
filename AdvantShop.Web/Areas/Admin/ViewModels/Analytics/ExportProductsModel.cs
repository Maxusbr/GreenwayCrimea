using System;
using System.Collections.Generic;

namespace AdvantShop.Web.Admin.ViewModels.Analytics
{
    public class ExportProductsModel
    {
        public string ProductArtno { get; set; }

        public string Encoding { get; set; }

        public string ColumnSeparator { get; set; }

        public string ExportProductsType { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }


        public List<int> SelectedCategories { get; set; }

        public Dictionary<string, string> Encodings { get; set; }

        public Dictionary<string, string> Separators { get; set; }

    }
}
