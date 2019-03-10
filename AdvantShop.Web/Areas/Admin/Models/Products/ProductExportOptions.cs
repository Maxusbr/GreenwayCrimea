namespace AdvantShop.Web.Admin.Models.Products
{
    public class ProductExportOptions
    {
        public int ProductId { get; set; }
        public string SalesNote { get; set; }
        public string Gtin { get; set; }
        public string GoogleProductCategory { get; set; }
        public string YandexMarketCategory { get; set; }
        public string YandexTypePrefix { get; set; }
        public string YandexModel { get; set; }
        public string YandexSizeUnit { get; set; }
        public string YandexName { get; set; }


        public bool Adult { get; set; }
        public bool ManufacturerWarranty { get; set; }
        
        public float Fee { get; set; }
        public float Cbid { get; set; }
    }
}
