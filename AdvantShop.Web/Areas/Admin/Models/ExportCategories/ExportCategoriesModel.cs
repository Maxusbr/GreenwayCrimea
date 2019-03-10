using System.Collections.Generic;
using AdvantShop.ExportImport;

namespace AdvantShop.Web.Admin.Models.ExportCategories
{
    public class ExportCategoriesModel
    {
        public List<CategoryFields> FieldMapping = new List<CategoryFields>();

        public Dictionary<string, string> AllFields { get; set; }

        public string DefaultExportFields { get; set; }

        public string CsvEncoding { get; set; }

        public string CsvSeparator { get; set; }



        public Dictionary<string, string> CsvSeparatorList { get; set; }

        public Dictionary<string, string> CsvEnconingList { get; set; }
    }
}
