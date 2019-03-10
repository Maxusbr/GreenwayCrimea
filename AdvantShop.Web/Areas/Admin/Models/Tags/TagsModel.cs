namespace AdvantShop.Web.Admin.Models.Tags
{
    public partial class TagsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlPath { get; set; }
        public bool Enabled { get; set; }
        public int SortOrder { get; set; }
        public bool VisibilityForUsers { get; set; }
    }
}
