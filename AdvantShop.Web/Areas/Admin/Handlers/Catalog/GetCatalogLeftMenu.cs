using System;
using System.Collections.Generic;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Web.Admin.Models.Catalog;

namespace AdvantShop.Web.Admin.Handlers.Catalog
{
    public class GetCatalogLeftMenu
    {
        public CatalogLeftMenu Execute()
        {
            var model = new CatalogLeftMenu()
            {
                ProductsCount = ProductService.GetProductsCount(),
                ProductsWithoutCategoriesCount = CategoryService.GetTolatCounTofProductsWithoutCategories(),

                BestProductsCount = ProductOnMain.GetProductCountByType(EProductOnMain.Best),
                BestProductsCountTotal = ProductOnMain.GetProductCountByType(EProductOnMain.Best, false),

                NewProductsCount = ProductOnMain.GetProductCountByType(EProductOnMain.New),
                NewProductsCountTotal = ProductOnMain.GetProductCountByType(EProductOnMain.New, false),

                SaleProductsCount = ProductOnMain.GetProductCountByType(EProductOnMain.Sale),
                SaleProductsCountTotal = ProductOnMain.GetProductCountByType(EProductOnMain.Sale, false),

                ProductListsCount = ProductListService.GetCount()
            };

            var request = HttpContext.Current.Request;
            var controller = request.RequestContext.RouteData.Values["controller"].ToString().ToLower();

            if (controller == "catalog" && request["showmethod"].IsNotEmpty())
            {
                ECatalogShowMethod showmethod;
                if (Enum.TryParse(request["showmethod"], true, out showmethod) && showmethod != ECatalogShowMethod.Normal)
                {
                    model.SelectedItem = showmethod.ToString();
                }
            }
            else if (controller == "mainpageproducts" && request["type"].IsNotEmpty())
            {
                var item = new List<string>() {"new", "best", "sale"}.Find(x => x == request["type"].ToLower());
                if (item != null)
                    model.SelectedItem = item;
            }
            else if (controller == "productlists")
            {
                model.SelectedItem = "productlists";
            }

            return model;
        }
    }
}
