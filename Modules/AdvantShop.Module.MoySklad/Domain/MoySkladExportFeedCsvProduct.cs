using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.ExportImport;

namespace AdvantShop.Module.MoySklad.Domain
{
    public class MoySkladExportFeedCsvProduct : ExportFeedCsvProduct
    {
        public bool HasMultiOffer { get; set; }
        public List<Offer> Offers { get; set; }

        public string ExternalCode { get; set; }
    }
}
