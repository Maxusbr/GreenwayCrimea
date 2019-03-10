namespace AdvantShop.Web.Admin.Models.Leads
{
    public class LeadItemModel
    {
        public int LeadItemId { get; set; }
        public int LeadId { get; set; }
        public string ImageSrc { get; set; }
        public string ArtNo { get; set; }
        public string Name { get; set; }
        public string ProductLink { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public float Price { get; set; }
        public float Amount { get; set; }
        public bool Available { get; set; }
        public string AvailableText { get; set; }
        public string Cost { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
    }
}
