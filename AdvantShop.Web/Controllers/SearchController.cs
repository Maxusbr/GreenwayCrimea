using System;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.FullSearch;
using AdvantShop.Core.Services.SEO;
using AdvantShop.Core.Services.Statistic;
using AdvantShop.Customers;
using AdvantShop.Handlers.Catalog;
using AdvantShop.Models.Catalog;
using AdvantShop.SEO;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AdvantShop.Handlers.Search;
using AdvantShop.Models.Search;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Helpers;

namespace AdvantShop.Controllers
{
    public partial class SearchController : BaseClientController
    {
        public ActionResult Index(SearchCatalogViewModel model)
        {
            if (model.Page != null && model.Page < 1)
                return Error404();

            var viewModel = new SearchPagingHandler(model).Get();

            ModulesExecuter.Search(model.Q, viewModel.Pager.TotalItemsCount);

            var tagManager = GoogleTagManagerContext.Current;
            if (tagManager.Enabled)
            {
                tagManager.PageType = ePageType.searchresults;
                tagManager.ProdIds = new List<string>();
                foreach (var item in viewModel.Products.Products)
                {
                    tagManager.ProdIds.Add(item.ArtNo);
                }
            }

            SetNgController(NgControllers.NgControllersTypes.CatalogCtrl);
            SetMetaInformation(new MetaInfo(string.Format("{0} - {1}", SettingsMain.ShopName, T("Search.Index.SearchTitle"))), model.Q, page: model.Page ?? 1);

            if (model.Q.IsNotEmpty())
            {
                var url = Request.Url.ToString();
                url = url.Substring(url.LastIndexOf("/"), url.Length - url.LastIndexOf("/"));
                var cat = CategoryService.GetCategory(viewModel.CategoryId);
                var catname = "все категории";
                if (cat != null)
                    catname = cat.Name;
                StatisticService.AddSearchStatistic(url, model.Q, string.Format(T("Search.Index.SearchIn"),
                        catname,
                        model.PriceFrom,
                        !model.PriceTo.HasValue ? "∞" : model.PriceTo.ToString()),
                        viewModel.Pager.TotalItemsCount,
                        CustomerContext.CurrentCustomer.Id);
                WriteLog(model.Q, Request.Url.AbsoluteUri, ePageType.searchresults);
            }
            return View(viewModel);
        }

        public JsonResult Filter(SearchCatalogViewModel categoryModel)
        {
            if (categoryModel.Page != null && categoryModel.Page < 1)
                return Json(null);

            var category = CategoryService.GetCategory(categoryModel.CategoryId);
            if (category == null)
                return Json(null);

            var sqlTasks = new List<Task<List<FilterItemModel>>>();
            
            sqlTasks.Add(new FilterInputHandler(categoryModel.CategoryId, false, categoryModel.Q).GetAsync());

            sqlTasks.Add(new FilterSelectCategoryHandler(categoryModel.CategoryId).GetAsync());

            sqlTasks.Add(new FilterPriceHandler(categoryModel.CategoryId, true, categoryModel.PriceFrom ?? 0.0f, categoryModel.PriceTo ?? 0.0f).GetAsync());

            if (SettingsCatalog.ShowProducerFilter)
            {
                var brands = BrandService.GetBrands();

                sqlTasks.Add(new FilterBrandHandler(
                    categoryModel.CategoryId,
                    true,
                    string.IsNullOrEmpty(categoryModel.Brand) ? new List<int>() : categoryModel.Brand.Split(",").Select(item => item.TryParseInt()).Where(id => id != 0).ToList(),
                    brands != null ? brands.Select(x => x.BrandId).ToList() : new List<int>(),
                    categoryModel.TypeFlag).GetAsync());
            }

            var resultFilter = sqlTasks.Select(x => x.Result).SelectMany(x => x).Where(x => x != null).ToList();

            return Json(resultFilter);
        }

        public JsonResult FilterProductCount(SearchCatalogViewModel model)
        {
            if ((string.IsNullOrWhiteSpace(model.Q)) || (model.Page != null && model.Page < 1))
                return Json(null);

            var viewModel = new SearchPagingHandler(model).Get();

            return Json(viewModel.Pager.TotalItemsCount);
        }

        [HttpGet]
        public JsonResult Autocomplete(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Json(null);

            var tanslitQ = StringHelper.TranslitToRusKeyboard(q);
            
            var categoryIds = CategorySeacher.Search(q).SearchResultItems.Select(x => x.Id).ToList();
            var translitСategoryIds = CategorySeacher.Search(tanslitQ).SearchResultItems.Select(x => x.Id).ToList();

            var cats =
                categoryIds.Union(translitСategoryIds)
                    .Distinct()
                    .Select(CategoryService.GetCategory)
                    .Where(x => x != null && x.Enabled && x.ParentsEnabled)
                    .Take(5)
                    .ToList();
            

            var productIds = ProductSeacher.Search(q).SearchResultItems.Select(x => x.Id).ToList();
            var translitProductIds = ProductSeacher.Search(tanslitQ).SearchResultItems.Select(x => x.Id).ToList();

            var resultIds =
                productIds.Union(translitProductIds)
                    .Distinct()
                    .Take(10)
                    .Aggregate("", (current, item) => current + (item + "/"));

            var categories =
                cats.Select(x => new
                    {
                        Name = x.Name,
                        Photo = x.Icon != null ? x.Icon.IconSrc() : "",
                        Url = Url.AbsoluteRouteUrl("Category", new { url = x.UrlPath }),
                        Template = "\\scripts\\_common\\autocompleter\\templates\\categories.html"
                    }).ToList();

            //for preprare discount
            var tempModel = new ProductViewModel(ProductService.GetForAutoCompleteProducts(resultIds));
            var products = tempModel.Products.Select(x => new
                    {
                        Name = x.Name,
                        Photo = x.Photo.ImageSrcXSmall(),
                        Amount = x.Amount,
                        Price = x.PreparedPrice,
                        Rating = x.Ratio,
                        Url = Url.AbsoluteRouteUrl("Product", new { url = x.UrlPath }),
                        Template = "\\scripts\\_common\\autocompleter\\templates\\products.html",
                        Gifts = x.Gifts
                    }).ToList();

            return Json(new
            {
                Categories = new
                {
                    Title = T("Search.Autocomplete.Categories"),
                    Items = categories
                },
                Products = new
                {
                    Title = T("Search.Autocomplete.Products"),
                    Items = products
                },
                Empty = !products.Any() && !categories.Any()
            });
        }

        [ChildActionOnly]
        public ActionResult SearchBlock(SearchBlockModel search)
        {
            search = search ?? new SearchBlockModel();
            if (!String.IsNullOrEmpty(SettingsCatalog.SearchExample))
            {
                var examples = SettingsCatalog.SearchExample.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (examples.Length > 0)
                {
                    var example = examples[(new Random()).Next(0, examples.Length)];
                    example = example.TrimEnd('\r').Trim();
                    search.ExampleText = example;
                    search.ExampleLink = Url.AbsoluteRouteUrl("Search", new { q = example });
                }
            }

            return PartialView("SearchBlock", search);
        }
    }
}