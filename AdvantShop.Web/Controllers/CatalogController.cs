using System.Threading.Tasks;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.SEO;
using AdvantShop.Handlers.Catalog;
using AdvantShop.Handlers.Menu;
using AdvantShop.Models.Catalog;
using AdvantShop.Saas;
using AdvantShop.SEO;
using AdvantShop.ViewModel.Catalog;
using AdvantShop.ViewModel.Common;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Extensions;
using AdvantShop.Core.Services.SEO.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

namespace AdvantShop.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public partial class CatalogController : BaseClientController
    {
        #region Category page

        // GET: Category page
        public ActionResult Index(CategoryModel categoryModel)
        {
            if ((string.IsNullOrWhiteSpace(categoryModel.Url) && categoryModel.CategoryId != 0) || (categoryModel.Page != null && categoryModel.Page < 0))
                return Error404();

            var category = categoryModel.Url != null
                                ? CategoryService.GetCategory(categoryModel.Url)
                                : categoryModel.CategoryId.HasValue ? CategoryService.GetCategory(categoryModel.CategoryId.Value) : null;

            if (category == null || !category.Enabled || !category.ParentsEnabled)
                return Error404();

            var tag = TagService.GetByUrl(categoryModel.TagUrl);
            if (tag != null && !tag.Enabled)
            {
                Response.Redirect("/categories/" + category.UrlPath, true);
            }

            var model = new CategoryViewModel(category);

            if (!SaasDataService.IsSaasEnabled || (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveTags))
            {
                model.TagView = new TagViewModel
                {
                    CategoryUrl = category.UrlPath,
                    Tags = category.Tags.Where(x => x.Enabled && x.VisibilityForUsers).Select(x => new TagView
                    {
                        Name = x.Name,
                        Url = x.UrlPath,
                        Selected = x.Id == (tag != null ? tag.Id : 0)
                    }).ToList()
                };
            }
            else
            {
                model.TagView = new TagViewModel
                {
                    CategoryUrl = category.UrlPath,
                    Tags = new List<TagView>()
                };
            }

            if (tag != null)
            {
                model.Tag = tag;
                categoryModel.TagId = tag.Id;
            }

            var indepth = categoryModel.Indepth || category.DisplayChildProducts;
            var productsCount = indepth
                                ? category.ProductsCount
                                : CategoryService.GetEnabledProductsCountInCategory(category.CategoryId, false);

            if (productsCount > 0)
            {
                var paging = new CategoryProductPagingHandler(category, indepth, categoryModel).GetForCatalog();

                if ((paging.Pager.TotalPages < paging.Pager.CurrentPage && paging.Pager.CurrentPage > 1) ||
                    paging.Pager.CurrentPage < 0)
                {
                    return Error404();
                }

                model.Pager = paging.Pager;
                model.Products = paging.Products;
                model.Filter = paging.Filter;
            }

            model.BreadCrumbs =
                CategoryService.GetParentCategories(category.CategoryId)
                    .Select(x => new BreadCrumbs(x.Name, Url.RouteUrl("Category", new { url = x.UrlPath })))
                    .Reverse()
                    .ToList();

            if (category.CategoryId == 0)
            {
                model.BreadCrumbs.Add(new BreadCrumbs(T("Catalog.Index.CatalogTitle"), Url.RouteUrl("CatalogRoot")));
            }
            model.BreadCrumbs.Insert(0, new BreadCrumbs(T("MainPage"), Url.RouteUrl("Home")));

            var tagManager = GoogleTagManagerContext.Current;
            if (tagManager.Enabled)
            {
                tagManager.PageType = ePageType.category;
                tagManager.CatCurrentId = category.ID;
                tagManager.CatCurrentName = category.Name;
                tagManager.CatParentId = category.ParentCategory != null ? category.ParentCategory.ID : 0;
                tagManager.CatParentName = category.ParentCategory != null ? category.ParentCategory.Name : "";

                tagManager.ProdIds = new List<string>();
                if (model.Products != null)
                {
                    foreach (var product in model.Products.Products)
                    {
                        tagManager.ProdIds.Add(product.OfferArtNo);
                    }
                }
            }

            SetNgController(NgControllers.NgControllersTypes.CatalogCtrl);

            if (tag != null)
            {
                model.BreadCrumbs.Add(new BreadCrumbs(tag.Name, ""));
                SetMetaInformation(tag.Meta, tag.Name, page: categoryModel.Page ?? 1, totalPages: model.Pager != null ? model.Pager.TotalPages : 0);
            }
            else
            {
                SetMetaInformation(category.Meta, category.Name, page: categoryModel.Page ?? 1,
                    tags: category.Tags.Select(x => x.Name).ToList(), 
                    totalPages: model.Pager != null ? model.Pager.TotalPages : 0);
            }

            var og = new OpenGraphModel();
            og.Images.Add(category.Picture.ImageSrcSmall());

            MetaDataContext.CurrentObject = og;

            WriteLog(category.Name, Url.AbsoluteRouteUrl("Category", new { url = category.UrlPath }), ePageType.category);
            return View(model);
        }

        [HttpGet]
        public JsonResult Filter(CategoryModel categoryModel)
        {
            if (categoryModel.CategoryId == 0 || (categoryModel.Page != null && categoryModel.Page < 0))
                return Json(null);

            var category = categoryModel.CategoryId.HasValue ? CategoryService.GetCategory(categoryModel.CategoryId.Value) : null;
            if (category == null)
                return Json(null);

            var indepth = categoryModel.Indepth || category.DisplayChildProducts;
            var tag = TagService.GetByUrl(categoryModel.TagUrl);
            if (tag != null)
                categoryModel.TagId = tag.Id;

            var paging = new CategoryProductPagingHandler(category, indepth, categoryModel).GetForFilter();
            var filter = paging.Filter;

            var sqlTasks = new List<Task<List<FilterItemModel>>>();

            if (SettingsCatalog.ShowPriceFilter)
            {
                sqlTasks.Add(new FilterPriceHandler(filter.CategoryId, filter.Indepth, filter.PriceFrom, filter.PriceTo).GetAsync());
            }

            if (SettingsCatalog.ShowProducerFilter)
            {
                sqlTasks.Add(new FilterBrandHandler(filter.CategoryId, filter.Indepth, filter.BrandIds, filter.AvailableBrandIds).GetAsync());
            }

            if (SettingsCatalog.ShowColorFilter)
            {
                sqlTasks.Add(new FilterColorHandler(filter.CategoryId, filter.Indepth, filter.ColorIds, filter.AvailableColorIds, SettingsCatalog.ShowOnlyAvalible || filter.Available).GetAsync());
            }

            if (SettingsCatalog.ShowSizeFilter)
            {
                sqlTasks.Add(new FilterSizeHandler(filter.CategoryId, filter.Indepth, filter.SizeIds, filter.AvailableSizeIds, SettingsCatalog.ShowOnlyAvalible || filter.Available).GetAsync());
            }

            sqlTasks.Add(new FilterPropertyHandler(filter.CategoryId, filter.Indepth, filter.PropertyIds, filter.AvailablePropertyIds, filter.RangePropertyIds).GetAsync());


            sqlTasks.Add(new FilterAvailabilityHandler(filter.Available).GetAsync());

            var resultFilter = sqlTasks.Select(x => x.Result).SelectMany(x => x).Where(x => x != null).ToList();

            ModulesExecuter.FilterCatalog();
            
            return Json(resultFilter);
        }

        [HttpGet]
        public JsonResult FilterProductCount(CategoryModel categoryModel)
        {
            if (categoryModel.CategoryId == 0 || (categoryModel.Page != null && categoryModel.Page < 0))
                return Json(null);

            var category = categoryModel.CategoryId.HasValue ? CategoryService.GetCategory(categoryModel.CategoryId.Value) : null;
            if (category == null)
                return Json(null);

            var tag = TagService.GetByUrl(categoryModel.TagUrl);
            if (tag != null)
                categoryModel.TagId = tag.Id;

            var indepth = categoryModel.Indepth || category.DisplayChildProducts;
            var paging = new CategoryProductPagingHandler(category, indepth, categoryModel).GetForFilterProductCount();
            if (paging.Filter == null || paging.Pager == null)
                return Json(null);

            return Json(paging.Pager.TotalItemsCount);
        }

        [ChildActionOnly]
        public ActionResult CategoryList(int categoryId, ECategoryDisplayStyle type, int? countProductsInLine)
        {
            if (type == ECategoryDisplayStyle.None)
                return new EmptyResult();

            var categories =
                CategoryService.GetChildCategoriesByCategoryId(categoryId, false)
                    .Where(cat => cat.Enabled && cat.ParentsEnabled && !cat.Hidden)
                    .ToList();

            if (categories.Count == 0)
                return new EmptyResult();

            if (type == ECategoryDisplayStyle.List)
            {
                var modelWithProducts = new CategoryListViewModel()
                {
                    CategoriesWithProducts = new List<CategoryProductsViewModel>(),
                    DisplayProductCount = SettingsCatalog.ShowProductsCount
                };

                foreach (var category in categories)
                {
                    var products = ProductService.GetProductsByCategory(
                        category.CategoryId,
                        countProductsInLine != null ? (int)countProductsInLine : SettingsDesign.CountCatalogProductInLine,
                        category.Sorting,
                        category.HasChild);

                    var categoryModel = new CategoryProductsViewModel(products)
                    {
                        ProductsCount = category.ProductsCount,
                        Title = category.Name,
                        Url = Url.RouteUrl("Category", new { url = category.UrlPath }),
                        CountProductsInLine = countProductsInLine != null ? (int)countProductsInLine : SettingsDesign.CountCatalogProductInLine
                    };
                    modelWithProducts.CategoriesWithProducts.Add(categoryModel);
                }

                return PartialView("CategoryListWithProducts", modelWithProducts);
            }

            var model = new CategoryListViewModel()
            {
                Categories = categories,
                DisplayProductCount = SettingsCatalog.ShowProductsCount,
                CountCategoriesInLine = SettingsDesign.CountCategoriesInLine
            };

            return PartialView(model);
        }

        #endregion

        #region Product list page

        public ActionResult ProductList(EProductOnMain? type, CategoryModel categoryModel, int? list)
        {
            if (type == null || type == EProductOnMain.None)
                return Error404();

            if (categoryModel.Page != null && categoryModel.Page < 0)
                return Error404();

            var tag = TagService.GetByUrl(categoryModel.TagUrl);

            EProductOnMain currentType = type.Value;

            ProductList productList = null;
            if (currentType == EProductOnMain.List)
            {
                if (list == null)
                    return Error404();

                productList = ProductListService.Get(list.Value);
                if (productList == null)
                    return Error404();
            }

            var model = new ProductListViewModel()
            {
                Type = currentType,
                ShowBest = ProductOnMain.IsExistsProductByType(EProductOnMain.Best),
                ShowNew = ProductOnMain.IsExistsProductByType(EProductOnMain.New),
                ShowSale = ProductOnMain.IsExistsProductByType(EProductOnMain.Sale),
                ProductLists = ProductListService.GetMainPageList()
            };

            if (!SaasDataService.IsSaasEnabled || (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveTags && type.HasValue))
            {
                model.TagView = new TagViewModel
                {
                    CategoryUrl = type.Value.ToString().ToLower(),
                    NonCategoryView = true,
                    Tags = TagService.GetTagsByProductOnMain(type.Value,list).Select(x => new TagView
                    {
                        Name = x.Name,
                        Url = x.UrlPath,
                        Selected = x.Id == (tag != null ? tag.Id : 0)
                    }).ToList()
                };
            }
            else
            {
                model.TagView = new TagViewModel
                {
                    CategoryUrl = type.Value.ToString().ToLower(),
                    NonCategoryView = true,
                    Tags = new List<TagView>()
                };
            }

            if (tag != null)
            {
                model.Tag = tag;
                categoryModel.TagId = tag.Id;
            }

            const bool indepth = true;

            var paging = new ProductListHandler(currentType, indepth, categoryModel, list).Get();

            if ((paging.Pager.TotalPages < paging.Pager.CurrentPage && paging.Pager.CurrentPage > 1) ||
                paging.Pager.CurrentPage < 0)
            {
                return Error404();
            }

            model.Pager = paging.Pager;
            model.Products = paging.Products;
            model.Filter = paging.Filter;
            //model.Filter.TypeFlag1 = currentType;

            MetaInfo meta = null;

            switch (currentType)
            {
                case EProductOnMain.Best:
                    model.Title = T("Catalog.ProductList.AllBestSellers");
                    meta = new MetaInfo(SettingsMain.ShopName + " - " + T("Catalog.ProductList.BestHeader"));
                    model.Description = SettingsCatalog.BestDescription;
                    break;
                case EProductOnMain.New:
                    model.Title = T("Catalog.ProductList.AllNewProducts");
                    meta = new MetaInfo(SettingsMain.ShopName + " - " + T("Catalog.ProductList.NewHeader"));
                    model.Description = SettingsCatalog.NewDescription;
                    break;
                case EProductOnMain.Sale:
                    model.Title = T("Catalog.ProductList.AllSales");
                    meta = new MetaInfo(SettingsMain.ShopName + " - " + T("Catalog.ProductList.SalesHeader"));
                    model.Description = SettingsCatalog.DiscountDescription;
                    break;
                case EProductOnMain.List:
                    var title = productList != null ? productList.Name : "";
                    model.ListId = productList != null ? productList.Id : 0;
                    model.Description = productList != null ? productList.Description : string.Empty;
                    model.Title = title;
                    meta = new MetaInfo(SettingsMain.ShopName + " - " + title);
                    break;
            }

            model.BreadCrumbs = new List<BreadCrumbs>()
            {
                new BreadCrumbs(T("MainPage"), Url.RouteUrl("Home")),
                new BreadCrumbs(model.Title, string.Empty),
            };

            SetMetaInformation(meta, "", page: categoryModel.Page ?? 1, totalPages: model.Pager != null ? model.Pager.TotalPages : 0);

            return View(model);
        }

        public ActionResult ChangeMode(string name, string viewMode)
        {
            var model = new ChangeModeViewModel()
            {
                Name = name,
                ViewMode = viewMode
            };

            return PartialView(model);
        }

        public JsonResult FilterProductList(EProductOnMain type, CategoryModel modelIn, int? list)
        {
            if (modelIn.Page != null && modelIn.Page < 0)
                return Json(null);
            

            var tag = TagService.GetByUrl(modelIn.TagUrl);
            if (tag != null)
                modelIn.TagId = tag.Id;

            var paging = new ProductListHandler(type, true, modelIn, list).GetForFilter();
            var filter = paging.Filter;

            var sqlTasks = new List<Task<List<FilterItemModel>>>();

            sqlTasks.Add(new FilterSelectCategoryHandler(modelIn.CategoryId.HasValue ? modelIn.CategoryId.Value : 0).GetAsync());

            if (SettingsCatalog.ShowPriceFilter)
            {
                sqlTasks.Add(new FilterPriceHandler(0, true, filter.PriceFrom, filter.PriceTo).GetAsync());                
            }

            if (SettingsCatalog.ShowProducerFilter)
            {
                sqlTasks.Add(new FilterBrandHandler(0, true, filter.BrandIds, filter.AvailableBrandIds, type, list).GetAsync());
            }

            if (SettingsCatalog.ShowColorFilter)
            {
                sqlTasks.Add(new FilterColorHandler(filter.CategoryId, filter.Indepth, filter.ColorIds, filter.AvailableColorIds, SettingsCatalog.ShowOnlyAvalible || filter.Available).GetAsync());
            }

            if (SettingsCatalog.ShowSizeFilter)
            {
                sqlTasks.Add(new FilterSizeHandler(filter.CategoryId, filter.Indepth, filter.SizeIds, filter.AvailableSizeIds, SettingsCatalog.ShowOnlyAvalible || filter.Available).GetAsync());
            }

            var resultFilter = sqlTasks.Select(x => x.Result).SelectMany(x => x).Where(x => x != null).ToList();

            return Json(resultFilter);
        }

        public JsonResult FilterProductListCount(EProductOnMain type, CategoryModel modelIn, int? list)
        {
            if (modelIn.Page != null && modelIn.Page < 0)
                return Json(null);
                       

            var tag = TagService.GetByUrl(modelIn.TagUrl);
            if (tag != null)
                modelIn.TagId = tag.Id;

            var paging = new ProductListHandler(type, true, modelIn, list).GetForFilterProductCount();
            if (paging.Filter == null || paging.Pager == null)
                return Json(null);

            return Json(paging.Pager.TotalItemsCount);
        }


        #endregion

        #region ProductsByIds

        public ActionResult ProductsByIds(string ids, string title, string type, int visibleItems = 0, bool enabledCarousel = true)
        {
            if (string.IsNullOrWhiteSpace(ids))
                return new EmptyResult();

            var productIds =
                ids.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.TryParseInt())
                    .Where(x => x != 0)
                    .Take(12)
                    .ToList();

            if (productIds.Count == 0)
                return new EmptyResult();

            var productModels = ProductService.GetProductsByIds(productIds);
            if (productModels == null || productModels.Count == 0)
                return new EmptyResult();

            var products = new ProductViewModel(productModels)
            {
                Title = title,
                DisplayPhotoPreviews = false
            };

            var model = new ProductByIdViewModel()
            {
                Products = products,
                RelatedType = type,
                Title = title,
                VisibleItems = visibleItems > 0 ? visibleItems : SettingsDesign.CountCatalogProductInLine,
                EnabledCarousel = enabledCarousel
            };

            if (model.Products != null)
                model.Products.CountProductsInLine = model.VisibleItems;

            return PartialView(model);
        }

        public ActionResult ProductsByOfferIds(string ids, string title, string type, int visibleItems = 0, int offerId = 0)
        {
            if (string.IsNullOrWhiteSpace(ids))
                return new EmptyResult();

            var offerIds =
                ids.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.TryParseInt())
                    .Where(x => x != 0)
                    .Take(12)
                    .ToList();

            if (offerIds.Count == 0) // пришли артикулы вместо offerId
            {
                offerIds =
                    ids.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries)
                        .Take(12)
                        .Select(x => OfferService.GetOffer(x))
                        .Where(x => x != null)
                        .Select(x => x.OfferId)
                        .ToList();
            }

            if (offerIds.Count == 0)
                return new EmptyResult();

            var excludeProductId = offerId != 0
                                    ? ProductService.GetProductIdsByOfferIds(new List<int> { offerId }).FirstOrDefault()
                                    : 0;

            var productIds = ProductService.GetProductIdsByOfferIds(offerIds).Where(x => x != excludeProductId).ToList();

            if (productIds.Count == 0)
                return new EmptyResult();

            var productModels = ProductService.GetProductsByIds(productIds);
            if (productModels == null || productModels.Count == 0)
                return new EmptyResult();

            var products = new ProductViewModel(productModels)
            {
                Title = title,
                DisplayPhotoPreviews = false
            };

            var model = new ProductByIdViewModel()
            {
                Products = products,
                RelatedType = type,
                Title = title,
                VisibleItems = visibleItems > 0 ? visibleItems : SettingsDesign.CountCatalogProductInLine,
                EnabledCarousel = !SettingsDesign.IsMobileTemplate
            };

            if (model.Products != null)
                model.Products.CountProductsInLine = model.VisibleItems;

            return PartialView("ProductsByIds", model);
        }

        #endregion

        #region MenuCatalog

        [ChildActionOnly]
        public ActionResult MenuCatalog(MenuCatalogViewModel model)
        {
            var viewModel = new MenuViewModel()
            {
                IsExpanded =
                    model.IsExpanded != null
                        ? (bool)model.IsExpanded
                        : (string)Request.RequestContext.RouteData.Values["controller"] == "Home" &&
                          (string)Request.RequestContext.RouteData.Values["action"] == "Index",
                InLayout = model.InLayout,
                ViewMode = !model.InLayout ? SettingsDesign.MenuStyle : (SettingsDesign.MenuStyle == SettingsDesign.eMenuStyle.Modern ? SettingsDesign.eMenuStyle.Modern : SettingsDesign.eMenuStyle.Classic)
            };

            if ((viewModel.IsExpanded && !viewModel.InLayout) || !viewModel.IsExpanded)
            {
                var menuHandler = new MenuHandler().GetCatalogMenuItems(model.CategoryId);
                viewModel.MenuItems = menuHandler.SubItems;

                viewModel.DisplayProductsCount = SettingsCatalog.ShowProductsCount;
                viewModel.CountColsProductsInRow = model.CountColsProductsInRow != null ? model.CountColsProductsInRow.Value : 4;
            }

            return PartialView("MenuCatalog", viewModel);
        }

        #endregion
    }
}