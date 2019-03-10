using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Areas.Mobile.Handlers.Catalog;
using AdvantShop.Areas.Mobile.Models.Catalog;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Statistic;
using AdvantShop.Customers;
using AdvantShop.Models.Catalog;
using AdvantShop.Saas;
using AdvantShop.SEO;
using AdvantShop.ViewModel.Catalog;
using AdvantShop.Core.Services.SEO;

namespace AdvantShop.Areas.Mobile.Controllers
{
    public class CatalogController : BaseMobileController
    {
        #region Catalog page

        // GET: Mobile/Catalog
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

            var model = new CategoryMobileViewModel(category);

            if (!SaasDataService.IsSaasEnabled || (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveTags))
            {
                model.TagView = new TagViewModel
                {
                    CategoryUrl = category.UrlPath,
                    Tags = category.Tags.Where(x => x.Enabled && x.VisibilityForUsers).Select(x => new TagView
                    {
                        Name = x.Name,
                        Url = x.UrlPath,
                        Selected = x.Id == (tag != null ? tag.Id : 0),
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
                var paging = new CategoryMobileHandler(category, indepth, categoryModel).Get();

                if ((paging.Pager.TotalPages < paging.Pager.CurrentPage && paging.Pager.CurrentPage > 1) ||
                    paging.Pager.CurrentPage < 0)
                {
                    return Error404();
                }

                model.Pager = paging.Pager;
                model.Products = paging.Products;
                model.Filter = paging.Filter;
            }

            model.SortingList = new List<SelectListItem>();
            foreach (ESortOrder sorting in Enum.GetValues(typeof(ESortOrder)))
            {
                if (sorting == ESortOrder.AscByAddingDate)
                    continue;

                model.SortingList.Add(new SelectListItem()
                {
                    Text = sorting.Localize(),
                    Value = sorting.StrName(),
                    Selected = categoryModel.Sort != null && categoryModel.Sort == sorting
                });
            }

            model.ParentCategory = model.Category.CategoryId == 0
                ? model.Category
                : CategoryService.GetCategory(model.Category.ParentCategoryId);

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
                        tagManager.ProdIds.Add(product.ArtNo);
                    }
                }
            }

            SetTitle(T("Catalog.Index.CatalogTitle"));
            SetMetaInformation(category.Meta, category.Name, page: categoryModel.Page ?? 1);

            return View(model);
        }

        [ChildActionOnly]
        public ActionResult CategoryList(int categoryId)
        {
            var category = CategoryService.GetCategory(categoryId);
            var categories =
                CategoryService.GetChildCategoriesByCategoryId(categoryId, false)
                    .Where(cat => cat.Enabled && cat.ParentsEnabled && !cat.Hidden)
                    .ToList();

            if (categories.Count == 0)
                return new EmptyResult();

            var model = new CategoryListMobileViewModel()
            {
                Categories = categories,
                DisplayProductCount = SettingsCatalog.ShowProductsCount,
                PhotoHeight = SettingsPictureSize.SmallCategoryImageHeight,
                DisplayStyle = category.DisplayStyle
            };

            return PartialView(model);
        }

        #endregion

        #region Product list

        public ActionResult ProductList(EProductOnMain type, CategoryModel categoryModel, int? list)
        {
            if (type == EProductOnMain.None)
                return Error404();

            if (categoryModel.Page != null && categoryModel.Page < 0)
                return Error404();

            ProductList productList = null;
            if (type == EProductOnMain.List)
            {
                if (list == null)
                    return Error404();

                productList = ProductListService.Get(list.Value);
                if (productList == null)
                    return Error404();
            }

            var model = new ProductListMobileViewModel()
            {
                Type = type.ToString().ToLower(),
            };

            var paging =
                new ProductListMobileHandler(type, categoryModel.Page, categoryModel.Sort ?? ESortOrder.NoSorting, list)
                    .Get();

            if ((paging.Pager.TotalPages < paging.Pager.CurrentPage && paging.Pager.CurrentPage > 1) ||
                paging.Pager.CurrentPage < 0)
            {
                return Error404();
            }

            model.Pager = paging.Pager;
            model.Products = paging.Products;
            model.Filter = paging.Filter;
            //model.Filter.TypeFlag1 = type;

            MetaInfo meta = null;

            switch (type)
            {
                case EProductOnMain.Best:
                    model.Title = T("Catalog.ProductList.AllBestSellers");
                    meta = new MetaInfo(SettingsMain.ShopName + " - " + T("Catalog.ProductList.BestHeader"));
                    break;
                case EProductOnMain.New:
                    model.Title = T("Catalog.ProductList.AllNewProducts");
                    meta = new MetaInfo(SettingsMain.ShopName + " - " + T("Catalog.ProductList.NewHeader"));
                    break;
                case EProductOnMain.Sale:
                    model.Title = T("Catalog.ProductList.AllSales");
                    meta = new MetaInfo(SettingsMain.ShopName + " - " + T("Catalog.ProductList.SalesHeader"));
                    break;
                case EProductOnMain.List:
                    var title = productList != null ? productList.Name : "";
                    model.ListId = productList != null ? productList.Id : 0;
                    model.Title = title;
                    meta = new MetaInfo(SettingsMain.ShopName + " - " + title);
                    break;
            }

            model.SortingList = new List<SelectListItem>();
            foreach (ESortOrder sorting in Enum.GetValues(typeof(ESortOrder)))
            {
                if (sorting == ESortOrder.AscByAddingDate)
                    continue;

                model.SortingList.Add(new SelectListItem()
                {
                    Text = sorting.Localize(),
                    Value = sorting.StrName(),
                    Selected = categoryModel.Sort.ToString().ToLower() == sorting.StrName().ToLower()
                });
            }

            //SetTitle(model.Title);
            SetMetaInformation(meta, "", page: categoryModel.Page ?? 1);

            return View(model);
        }

        #endregion

        #region Search

        public ActionResult Search(SearchMobileModel model)
        {
            if ((string.IsNullOrWhiteSpace(model.Q)) || (model.Page != null && model.Page < 1))
                return Error404();

            var viewModel = new SearchPagingMobileHandler(model).Get();

            viewModel.SortingList = new List<SelectListItem>();
            foreach (ESortOrder sorting in Enum.GetValues(typeof(ESortOrder)))
            {
                if (sorting == ESortOrder.AscByAddingDate)
                    continue;

                viewModel.SortingList.Add(new SelectListItem()
                {
                    Text = sorting.Localize(),
                    Value = sorting.StrName(),
                    Selected = model.Sort == sorting
                });
            }

            SetTitle(T("Search.Index.SearchTitle"));
            SetMetaInformation(
                new MetaInfo(string.Format("{0} - {1}", SettingsMain.ShopName, T("Search.Index.SearchTitle"))),
                model.Q, page: model.Page ?? 1);

            var url = Request.Url.ToString();
            url = url.Substring(url.LastIndexOf("/"), url.Length - url.LastIndexOf("/"));

            StatisticService.AddSearchStatistic(url, model.Q, 
                string.Format(T("Search.Index.SearchIn"), "", 0, "∞"),
                viewModel.Pager.TotalItemsCount, 
                CustomerContext.CurrentCustomer.Id);

            return View(viewModel);
        }

        #endregion
    }
}