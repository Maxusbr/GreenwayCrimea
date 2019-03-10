using System.Collections.Generic;
using AdvantShop.Areas.Mobile.Models.Catalog;
using AdvantShop.Areas.Mobile.Models.Home;
using AdvantShop.Catalog;
using AdvantShop.Core.Services.Configuration.Settings;
using AdvantShop.Core.Services.Localization;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using AdvantShop.Customers;
using AdvantShop.Trial;

namespace AdvantShop.Areas.Mobile.Handlers.Home
{
    public class HomeMobileHandler
    {
        public HomeMobileViewModel Get()
        {
            var model = new HomeMobileViewModel();

            model.CategoriesUrl = new List<SelectListItem>
            {
                new SelectListItem {Text = LocalizationService.GetResource("Home.Index.Catalog"), Value = "catalog/"}
            };

            var url = new UrlHelper(HttpContext.Current.Request.RequestContext);

            foreach (var category in CategoryService.GetChildCategoriesByCategoryId(0, false).Where(cat => cat.Enabled && ! cat.Hidden))
            {
                model.CategoriesUrl.Add(new SelectListItem
                {
                    Text = category.Name,
                    Value = url.RouteUrl("category", new { url = category.UrlPath }, url.RequestContext.HttpContext.Request.Url.Scheme)
                });
            }

            var itemsCount = SettingsMobile.MainPageProductsCount;
            if (itemsCount > 0)
            {
                model.Bestsellers = new ProductViewMobileModel(ProductOnMain.GetProductsByTypeMobile(EProductOnMain.Best, itemsCount));
                model.NewProducts = new ProductViewMobileModel(ProductOnMain.GetProductsByTypeMobile(EProductOnMain.New, itemsCount));
                model.Sales = new ProductViewMobileModel(ProductOnMain.GetProductsByTypeMobile(EProductOnMain.Sale, itemsCount));

                var productLists = ProductListService.GetMainPageList();
                foreach (var productList in productLists)
                {

                    var products = ProductListService.GetProducts(productList.Id, itemsCount);
                    if (products.Count > 0)
                    {
                        var productListModel = new ProductViewMobileModel(products)
                        {
                            Id = productList.Id,
                            Title = productList.Name,
                            DisplayCategory = true
                        };
                        model.ProductLists.Add(productListModel);
                    }
                }

            }

            if (CustomerContext.CurrentCustomer.IsAdmin)
                TrialService.TrackEvent(ETrackEvent.Trial_VisitMobileVersion);

            return model;
        }

    }
}