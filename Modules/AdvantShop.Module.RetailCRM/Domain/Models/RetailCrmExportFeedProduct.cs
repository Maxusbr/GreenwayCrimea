using AdvantShop.ExportImport;

namespace AdvantShop.Module.RetailCRMs.Domain.Models
{
    public class RetailCrmExportFeedProduct : ExportFeedYandexProduct
    {
        public float Height { get; set; }
        public float Width { get; set; }
        public float Length { get; set; }
    }
}
