namespace AdvantShop.Web.Admin.Models.Properties
{
    public partial class PropertyModel
    {
        public int PropertyId { get; set; }
        public string Name { get; set; }
        public string NameDisplayed { get; set; }
        public bool UseInFilter { get; set; }
        public bool UseInDetails { get; set; }
        public bool UseInBrief { get; set; }
        public int SortOrder { get; set; }

        public string GroupName { get; set; }
        public int GroupId { get; set; }

        public int ProductsCount { get; set; }
    }
}
