//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Modules
{
    public class RitmzProduct
    {
        public int ProductId { get; set; }
        public int OfferId { get; set; }
        public int Amount { get; set; }
        public string UrlPath { get; set; }
        public float Price { get; set; }
        public float Discount { get; set; }
        public int ParentCategory { get; set; }
        public string Name { get; set; }
        public string BriefDescription { get; set; }
        public string Description { get; set; }

        public string Photos { get; set; }

        public string SalesNote { get; set; }

        public string ArtNo { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public int SizeId { get; set; }
        public string SizeName { get; set; }

        public string BrandName { get; set; }
        public bool Main { get; set; }

        public bool Enabled { get; set; }
        public float SupplyPrice { get; set; }
        public float Weight { get; set; }
    }
}