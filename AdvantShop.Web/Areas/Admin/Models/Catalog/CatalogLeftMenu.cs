namespace AdvantShop.Web.Admin.Models.Catalog
{
    public class CatalogLeftMenu
    {
        public int ProductsCount { get; set; }
        public int ProductsWithoutCategoriesCount { get; set; }
        public int BestProductsCount { get; set; }
        public int BestProductsCountTotal { get; set; }
        public int NewProductsCount { get; set; }
        public int NewProductsCountTotal { get; set; }
        public int SaleProductsCount { get; set; }
        public int SaleProductsCountTotal { get; set; }
        public int ProductListsCount { get; set; }
        public string SelectedItem { get; set; }
        public string NgCallbackOnInit { get; set; }
    }
}
