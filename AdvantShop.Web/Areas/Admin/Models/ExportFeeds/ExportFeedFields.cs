using System.Collections.Generic;
using AdvantShop.ExportImport;

namespace AdvantShop.Web.Admin.Models.ExportFeeds
{
    public class ExportFeedFields
    {
        public int Id { get; set; }

        public List<ProductFields> FieldMapping { get; set; }
        public List<CSVField> ModuleFieldMapping { get; set; }

        public Dictionary<string, string> AllFields { get; set; }

        public string DefaultExportFields { get; set; }
        
    }
}
