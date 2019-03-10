namespace AdvantShop.ExportImport
{
    public class ExportFeedYandexProduct : ExportFeedProductModel
    {
        public string OfferArtNo { get; set; }
        public int OfferId { get; set; }
        public int Amount { get; set; }
        
        public float Price { get; set; }
        public float? ShippingPrice { get; set; }
        public float SupplyPrice { get; set; }

        public float Discount { get; set; }
        public float DiscountAmount { get; set; }
        public int ParentCategory { get; set; }
        
        public string Photos { get; set; }
        
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public int SizeId { get; set; }
        public string SizeName { get; set; }
        
        public bool Main { get; set; }

        public string SalesNote { get; set; }
        public string YandexMarketCategory { get; set; }
        public string YandexTypePrefix { get; set; }
        public string YandexModel { get; set; }
        public bool ManufacturerWarranty { get; set; }
        public string YandexName { get; set; }

        public bool Adult { get; set; }

        public float CurrencyValue { get; set; }
        public float Weight { get; set; }
        public bool Enabled { get; set; }

        public string BrandCountry { get; set; }

        public float Cbid { get; set; }
        public float Fee { get; set; }

        public string YandexSizeUnit { get; set; }     
        
        public bool AllowPreorder { get; set; }

        public float MinAmount { get; set; }
        public float Multiplicity { get; set; }

        public bool Cpa { get; set; }
    }
}