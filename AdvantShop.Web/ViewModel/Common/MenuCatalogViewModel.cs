namespace AdvantShop.ViewModel.Common
{
    public class MenuCatalogViewModel
    {
        public MenuCatalogViewModel()
        {
            InLayout = true;
            ViewMode = "default";
        }

        public int CategoryId { get; set; }

        public bool? IsExpanded { get; set; }

        public bool InLayout { get; set; }

        public bool InCatalog { get; set; }

        public string ViewMode { get; set; }

        public int? CountColsProductsInRow { get; set; }
    }
}