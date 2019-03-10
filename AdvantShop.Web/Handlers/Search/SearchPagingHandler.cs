using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Models;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.FullSearch;
using AdvantShop.Core.SQL2;
using AdvantShop.Helpers;
using AdvantShop.Models.Catalog;
using AdvantShop.Repository.Currencies;
using AdvantShop.ViewModel.Catalog;

namespace AdvantShop.Handlers.Search
{
    public class SearchPagingHandler
    {
        #region Fields

        private readonly int _currentPageIndex = 1;

        private SearchCatalogViewModel _model;

        #endregion

        #region Constructor

        public SearchPagingHandler(SearchCatalogViewModel model)
        {
            _model = model;
            _currentPageIndex = model.Page ?? 1;
        }

        #endregion

        public SearchCatalogViewModel Get()
        {
            _model = GetProduct(_model);
            _model = GetCategory(_model);

            return _model;
        }


        private SqlPaging BuildPaging()
        {
            var paging = new SqlPaging();
            paging.Select(
                "Product.ProductID",
                "CountPhoto",
                "Photo.PhotoId",
                "PhotoName",
                "Photo.Description".AsSqlField("PhotoDescription"),
                "Product.ArtNo",
                "Product.Name",
                "Recomended".AsSqlField("Recomend"),
                "Product.Bestseller",
                "Product.New",
                "Product.OnSale".AsSqlField("Sales"),
                "Product.Discount",
                "Product.DiscountAmount",
                "Product.BriefDescription",
                "Product.MinAmount",
                "Product.MaxAmount",
                "Product.Enabled",
                "Product.AllowPreOrder",
                "Product.Ratio",
                "Product.UrlPath",
                "Product.DateAdded",
                "Product.BrandID",
                "Brand.BrandName",
                "Offer.OfferID",
                "Offer.ColorID",
                "MaxAvailable".AsSqlField("Amount"),
                "Comments",
                "CurrencyValue",
                "Gifts"
                );

            paging.From("[Catalog].[Product]");
            paging.Left_Join("[Catalog].[ProductExt] ON [Product].[ProductID] = [ProductExt].[ProductID]");
            paging.Left_Join("[Catalog].[Photo] ON [Photo].[PhotoId] = [ProductExt].[PhotoId]");
            paging.Left_Join("[Catalog].[Offer] ON [ProductExt].[OfferID] = [Offer].[OfferID]");
            paging.Left_Join("[Catalog].[Brand] ON [Brand].[BrandID] = [Product].[BrandID]");
            paging.Inner_Join("[Catalog].[Currency] ON [Currency].[CurrencyID] = [Product].[CurrencyID]");
            
            if (SettingsCatalog.ComplexFilter)
            {
                paging.Select(
                      "Colors",
                      "NotSamePrices".AsSqlField("MultiPrices"),
                      "MinPrice".AsSqlField("BasePrice")
                 );
            }
            else
            {
                paging.Select(
                        "null".AsSqlField("Colors"),
                        "0".AsSqlField("MultiPrices"),
                        "Price".AsSqlField("BasePrice")
                   );
            }

            paging = BuildFilter(paging);
            paging = BuildSorting(paging);
            
            paging.ItemsPerPage = _currentPageIndex != 0 ? SettingsCatalog.ProductsPerPage : int.MaxValue;
            paging.CurrentPageIndex = _currentPageIndex != 0 ? _currentPageIndex : 1;
            return paging;
        }

        private SqlPaging BuildSorting(SqlPaging paging)
        {
            var sort = _model.Sort;
            
            if (SettingsCatalog.MoveNotAvaliableToEnd)
            {
                paging.OrderByDesc("(CASE WHEN Price=0 THEN 0 ELSE 1 END)".AsSqlField("TempSort"));
                paging.OrderByDesc("AmountSort".AsSqlField("TempAmountSort"));
            }

            switch (sort)
            {
                case ESortOrder.AscByName:
                    paging.OrderBy("Product.Name".AsSqlField("NameSort"));
                    break;
                case ESortOrder.DescByName:
                    paging.OrderByDesc("Product.Name".AsSqlField("NameSort"));
                    break;

                case ESortOrder.AscByPrice:
                    paging.OrderBy("PriceTemp");
                    break;

                case ESortOrder.DescByPrice:
                    paging.OrderByDesc("PriceTemp");
                    break;

                case ESortOrder.AscByRatio:
                    paging.OrderBy("Ratio".AsSqlField("RatioSort"));
                    break;

                case ESortOrder.DescByRatio:
                    paging.OrderByDesc("Ratio".AsSqlField("RatioSort"));
                    break;

                case ESortOrder.AscByAddingDate:
                    paging.OrderBy("DateAdded".AsSqlField("DateAddedSort"));
                    break;

                case ESortOrder.DescByAddingDate:
                    paging.OrderByDesc("DateAdded".AsSqlField("DateAddedSort"));
                    break;
            }
            return paging;
        }

        private SqlPaging BuildFilter(SqlPaging paging)
        {
            paging.Where("Product.Enabled={0}", true);
            paging.Where("AND CategoryEnabled={0}", true);
            paging.Where("AND (Offer.Main={0} OR Offer.Main IS NULL)", true);

            paging.Where(
                "AND Exists( select 1 from [Catalog].[ProductCategories] INNER JOIN [Settings].[GetChildCategoryByParent]({0}) AS hCat ON hCat.id = [ProductCategories].[CategoryID] and  ProductCategories.ProductId = [Product].[ProductID])",
                _model.CategoryId);

            if (SettingsCatalog.ShowOnlyAvalible)
            {
                paging.Where("AND MaxAvailable>{0}", 0);
            }

            if (!string.IsNullOrEmpty(_model.Q))
            {
                var productIds = ProductSeacher.Search(_model.Q).SearchResultItems.Select(x => x.Id).ToList();

                var tanslitQ = StringHelper.TranslitToRusKeyboard(_model.Q);
                var translitProductIds = ProductSeacher.Search(tanslitQ).SearchResultItems.Select(x => x.Id).ToList();
                
                var resultIds = productIds.Union(translitProductIds).Distinct();

                paging.Inner_Join(
                    "(select item, sort from [Settings].[ParsingBySeperator]({0},'/') ) as dtt on Product.ProductId=convert(int, dtt.item)",
                    String.Join("/", resultIds));

                if (_model.Sort == ESortOrder.NoSorting)
                {
                    paging.OrderBy("dtt.sort");
                }
            }

            var currency = CurrencyService.CurrentCurrency;
            if (SettingsCatalog.DefaultCurrencyIso3 != currency.Iso3)
            {
                paging.Where("", currency.Iso3);
            }

            if (_model.PriceFrom.HasValue || _model.PriceTo.HasValue)
            {
                var pricefrom = _model.PriceFrom ?? 0;
                var priceto = _model.PriceTo ?? int.MaxValue;

                paging.Where("AND (ProductExt.PriceTemp >= {0} ", pricefrom * currency.Rate);
                paging.Where("AND  ProductExt.PriceTemp <= {0})", priceto * currency.Rate);
            }

            if (!string.IsNullOrEmpty(_model.Brand))
            {
                var brandIds = _model.Brand.Split(',').Select(item => item.TryParseInt()).Where(id => id != 0).ToList();
                if (brandIds.Count > 0)
                {
                   // _model..Filter.BrandIds = brandIds;
                    paging.Where("AND Product.BrandID IN ({0})", brandIds.ToArray());
                }
            }

            return paging;
        }

        private string GetViewMode()
        {
            var cookieMode = CommonHelper.GetCookieString("search_viewmode");
            var mode = SettingsCatalog.EnabledSearchViewChange
                ? cookieMode.Parse<ProductViewMode>(SettingsCatalog.DefaultSearchView)
                : SettingsCatalog.DefaultSearchView;
            return mode.ToString().ToLower();
        }

        private SearchCatalogViewModel GetProduct(SearchCatalogViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Q))
            {
                model.Pager = new Pager();
                model.Products = new ProductViewModel(new List<ProductModel>());
                model.ViewMode = GetViewMode();
                model.AllowChangeViewMode = SettingsCatalog.EnabledSearchViewChange;
                return model;
            }

            var paging = BuildPaging();

            var totalCount = paging.TotalRowsCount;
            var totalPages = paging.PageCount(totalCount);

            model.Pager = new Pager()
            {
                TotalItemsCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = _currentPageIndex
            };

            if ((totalPages < _currentPageIndex && _currentPageIndex > 1) || _currentPageIndex < 0)
            {
                return model;
            }

            var products = paging.PageItemsList<ProductModel>();

            model.Products = new ProductViewModel(products);
            model.ViewMode = GetViewMode();
            model.AllowChangeViewMode = SettingsCatalog.EnabledSearchViewChange;
            return model;
        }

        private SearchCatalogViewModel GetCategory(SearchCatalogViewModel model)
        {
            if (string.IsNullOrEmpty(_model.Q))
            {
                model.Categories = new CategoryListViewModel()
                {
                    Categories = new List<Category>(),
                };
                return model;
            }
            
            var categoryIds = CategorySeacher.Search(_model.Q).SearchResultItems.Select(x => x.Id).ToList();

            var tanslitQ = StringHelper.TranslitToRusKeyboard(_model.Q);
            var translitCategoryIds = CategorySeacher.Search(tanslitQ).SearchResultItems.Select(x => x.Id).ToList();
            
            var categories =
                categoryIds.Union(translitCategoryIds)
                    .Distinct()
                    .Select(CategoryService.GetCategory)
                    .Where(cat => cat != null && cat.Enabled && cat.ParentsEnabled && !cat.Hidden)
                    .Take(10)
                    .ToList();

            model.Categories = new CategoryListViewModel()
            {
                Categories = categories,
                CountCategoriesInLine = SettingsDesign.CountCategoriesInLine
            };

            return model;
        }
    }
}