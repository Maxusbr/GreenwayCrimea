namespace AdvantShop.Web.Admin.ViewModels.Catalog
{
    public class AdminCategoryListItemViewModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Sorting { get; set; }
        public string MiniPictureSrc { get; set; }
        public string Url { get; set; }
        public int ProductsCount { get; set; }
    }
}
