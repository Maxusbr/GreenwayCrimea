using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.ViewModel.Home;

namespace AdvantShop.Handlers.Home
{
    public class MainPageProductsHandler
    {
        private ProductViewModel PrepareProductModel(EProductOnMain type, int itemsCount)
        {
            var products = ProductOnMain.GetProductsByType(type, itemsCount);

            var model = new ProductViewModel(products)
            {
                DisplayCategory = true,
                CountProductsInLine = SettingsDesign.CountMainPageProductInLine,
                DisplayQuickView = false
            };
            return model;
        }

        public MainPageProductsViewModel Get()
        {
            var model = new MainPageProductsViewModel();

            var countNew = ProductOnMain.GetProductCountByType(EProductOnMain.New);
            var countDiscount = ProductOnMain.GetProductCountByType(EProductOnMain.Sale);
            var countBestseller = ProductOnMain.GetProductCountByType(EProductOnMain.Best);

            var itemsCount = 0;
            var mode = SettingsDesign.eMainPageMode.TwoColumns;
            switch (mode)
            {
                case SettingsDesign.eMainPageMode.Default:

                    itemsCount = SettingsDesign.CountMainPageProductInSection;

                    if (countBestseller == 0)
                        itemsCount = 3;

                    if (countNew == 0 || countDiscount == 0)
                        itemsCount = itemsCount == 2 ? 3 : 6;

                    if (itemsCount > 0)
                    {
                        model.BestSellers = PrepareProductModel(EProductOnMain.Best, itemsCount);
                        model.NewProducts = PrepareProductModel(EProductOnMain.New, itemsCount);
                        model.Sales = PrepareProductModel(EProductOnMain.Sale, itemsCount);
                    }
                    break;

                case SettingsDesign.eMainPageMode.TwoColumns:
                case SettingsDesign.eMainPageMode.ThreeColumns:

                    itemsCount = SettingsDesign.CountMainPageProductInSection;

                    model.BestSellers = PrepareProductModel(EProductOnMain.Best, itemsCount);
                    model.NewProducts = PrepareProductModel(EProductOnMain.New, itemsCount);
                    model.Sales = PrepareProductModel(EProductOnMain.Sale, itemsCount);
                    break;
            }

            var productLists = ProductListService.GetMainPageList();
            foreach (var productList in productLists)
            {
                var products = ProductListService.GetProducts(productList.Id, itemsCount);
                if (products.Count > 0)
                {
                    var productListModel = new ProductViewModel(products)
                    {
                        Id = productList.Id,
                        Title = productList.Name,
                        DisplayCategory = true,
                        CountProductsInLine = SettingsDesign.CountMainPageProductInLine,
                        DisplayQuickView = false
                    };
                    model.ProductLists.Add(productListModel);
                }
            }

            return model;
        }

    }
}