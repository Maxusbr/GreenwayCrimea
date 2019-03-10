namespace AdvantShop.Models.Search
{
    public partial class SearchModel : BaseModel
    {
        public int CategoryId { get; set; }

        public string Url { get; set; }

        public bool Indepth { get; set; }
        
        public int? Page { get; set; }

        public string PriceFrom { get; set; }

        public string PriceTo { get; set; }
        
        public string Sort { get; set; }

        public string ViewMode { get; set; }
    }
}