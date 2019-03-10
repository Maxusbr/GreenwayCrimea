namespace AdvantShop.ExportImport
{
    public class ExportFeedGoogleMerchantCenterProduct : ExportFeedProductModel
    {
        public string OfferArtNo { get; set; }
        public int OfferId { get; set; }
        public int Amount { get; set; }
        
        public float Price { get; set; }
        public float ShippingPrice { get; set; }

        public float Discount { get; set; }
        public float DiscountAmount { get; set; }

        public int ParentCategory { get; set; }
        
        public string Photos { get; set; }
        
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public int SizeId { get; set; }
        public string SizeName { get; set; }
        
        public bool Main { get; set; }

        public string GoogleProductCategory { get; set; }
        
        public string Gtin { get; set; }
        public bool Adult { get; set; }
        
        public float CurrencyValue { get; set; }
        public bool AllowPreorder { get; set; }
    }
}