using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Models;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.SQL2;
using AdvantShop.Helpers;
using AdvantShop.Models.Catalog;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Handlers.Catalog
{
    public class CategoryProductPagingHandler
    {
        #region Fields

        private SqlPaging _paging;
        private readonly bool _indepth;
        private readonly int _currentPageIndex = 1;
        private readonly CategoryModel _categoryModel;
        private readonly Category _category;

        private CategoryPagingModel _model;

        #endregion

        #region Constructor

        public CategoryProductPagingHandler(Category category, bool indepth, CategoryModel categoryModel)
        {
            _category = category;
            _indepth = indepth;
            _categoryModel = categoryModel;
            _currentPageIndex = categoryModel.Page ?? 1;
        }

        #endregion

        public CategoryPagingModel GetForCatalog()
        {
            _model = new CategoryPagingModel(_category.CategoryId, _indepth);

            BuildPaging();

            var totalCount = _paging.TotalRowsCount;
            var totalPages = _paging.PageCount(totalCount);

            _model.Pager = new Pager()
            {
                TotalItemsCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = _currentPageIndex
            };

            if ((totalPages < _currentPageIndex && _currentPageIndex > 1) || _currentPageIndex < 0)
            {
                return _model;
            }

            var products = _paging.PageItemsList<ProductModel>();

            _model.Products = new ProductViewModel(products)
            {
                SelectedColors =
                    _model.Filter != null && _model.Filter.ColorIds.Any()
                        ? "[" + string.Join(",", _model.Filter.ColorIds) + "]"
                        : null
            };

            GetViewMode();

            return _model;
        }

        public CategoryPagingModel GetForFilter()
        {
            _model = new CategoryPagingModel(_category.CategoryId, _indepth);

            BuildPaging();
            BuildExludingFilters();

            return _model;
        }

        public CategoryPagingModel GetForFilterProductCount()
        {
            _model = new CategoryPagingModel(_category.CategoryId, _indepth);

            BuildPaging();

            var totalCount = _paging.TotalRowsCount;

            _model.Pager = new Pager()
            {
                TotalItemsCount = totalCount,
                CurrentPage = _currentPageIndex
            };

            return _model;
        }

        private void BuildPaging()
        {
            _paging = new SqlPaging("CategoryPaging");
            _paging.Select(
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
                "Offer.OfferID",
                "Offer.ArtNo".AsSqlField("OfferArtNo"),
                "Offer.ColorID",
                "MaxAvailable".AsSqlField("Amount"),
                "Comments",
                "CurrencyValue",
                "Gifts"
                );

            _paging.From("[Catalog].[Product]");
            _paging.Left_Join("[Catalog].[ProductExt] ON [Product].[ProductID] = [ProductExt].[ProductID]");
            _paging.Left_Join("[Catalog].[Photo] ON [Photo].[PhotoId] = [ProductExt].[PhotoId]");
            _paging.Left_Join("[Catalog].[Offer] ON [ProductExt].[OfferID] = [Offer].[OfferID]");
            _paging.Inner_Join("[Catalog].[Currency] ON [Currency].[CurrencyID] = [Product].[CurrencyID]");
            
            if (!_indepth)
                _paging.Inner_Join("[Catalog].[ProductCategories] ON [ProductCategories].[CategoryID] = {0} and  ProductCategories.ProductId = [Product].[ProductID]", _category.CategoryId);


            if (SettingsCatalog.ComplexFilter)
            {
                _paging.Select(
                      "Colors",
                      "NotSamePrices".AsSqlField("MultiPrices"),
                      "MinPrice".AsSqlField("BasePrice"),
						"Offer.Price"
				 );
            }
            else
            {
                _paging.Select(
                        "null".AsSqlField("Colors"),
                        "0".AsSqlField("MultiPrices"),
                        "Price".AsSqlField("BasePrice")
                   );
            }

            BuildSorting();
            BuildFilter();

            _paging.ItemsPerPage = _currentPageIndex != 0 ? SettingsCatalog.ProductsPerPage : int.MaxValue;
            _paging.CurrentPageIndex = _currentPageIndex != 0 ? _currentPageIndex : 1;
        }

        private void BuildSorting()
        {
            var sort = _categoryModel.Sort != null ? (ESortOrder)_categoryModel.Sort : _category.Sorting;
            _model.Filter.Sorting = sort;

            if (SettingsCatalog.MoveNotAvaliableToEnd)
            {
                _paging.OrderByDesc("(CASE WHEN Price=0 THEN 0 ELSE 1 END)".AsSqlField("TempSort"));
                _paging.OrderByDesc("AmountSort".AsSqlField("TempAmountSort"));
            }
            
            switch (sort)
            {
                case ESortOrder.AscByName:
                    _paging.OrderBy("Product.Name".AsSqlField("NameSort"));
                    break;

                case ESortOrder.DescByName:
                    _paging.OrderByDesc("Product.Name".AsSqlField("NameSort"));
                    break;

                case ESortOrder.AscByPrice:
                    _paging.OrderBy("PriceTemp");
                    break;

                case ESortOrder.DescByPrice:
                    _paging.OrderByDesc("PriceTemp");
                    break;

                case ESortOrder.AscByRatio:
                    _paging.OrderBy("Ratio".AsSqlField("RatioSort"));
                    break;

                case ESortOrder.DescByRatio:
                    _paging.OrderByDesc("Ratio".AsSqlField("RatioSort"));
                    break;

                case ESortOrder.AscByAddingDate:
                    _paging.OrderBy("DateAdded".AsSqlField("DateAddedSort"));
                    break;

                case ESortOrder.DescByAddingDate:
                    _paging.OrderByDesc("DateAdded".AsSqlField("DateAddedSort"));
                    break;

                default:
                    //_paging.OrderByDesc("AmountSort".AsSqlField("TempAmountSort"));
                    break;
            }

            if (!_indepth)
                _paging.OrderBy("[ProductCategories].[SortOrder]".AsSqlField("ProductCategorySortOrder"));
        }

        private void BuildFilter()
        {
            _paging.Where("Product.Enabled={0}", true);
            _paging.Where("AND CategoryEnabled={0}", true);
            _paging.Where("AND (Offer.Main={0} OR Offer.Main IS NULL)", true);

            if (_indepth)
            {
                _paging.Where("AND Exists( select 1 from [Catalog].[ProductCategories] INNER JOIN [Settings].[GetChildCategoryByParent]({0}) AS hCat ON hCat.id = [ProductCategories].[CategoryID] and  ProductCategories.ProductId = [Product].[ProductID])", _category.CategoryId);
            }

            if (!string.IsNullOrEmpty(_categoryModel.Brand))
            {
                var brandIds = _categoryModel.Brand.Split(',').Select(item => item.TryParseInt()).Where(id => id != 0).ToList();
                if (brandIds.Count > 0)
                {
                    _model.Filter.BrandIds = brandIds;
                    _paging.Where("AND Product.BrandID IN ({0})", brandIds.ToArray());
                }
            }

            if (!string.IsNullOrEmpty(_categoryModel.Size))
            {
                var sizeIds = _categoryModel.Size.Split(',').Select(item => item.TryParseInt()).Where(id => id != 0).ToList();
                if (sizeIds.Count > 0)
                {
                    _model.Filter.SizeIds = sizeIds;
                    _paging.Where(
                        SettingsCatalog.ShowOnlyAvalible || _categoryModel.Available
                            ? "and Exists(Select 1 from [Catalog].[Offer] where Offer.[SizeID] IN ({0}) and Offer.ProductId = [Product].[ProductID] AND Offer.amount > 0)"
                            : "and Exists(Select 1 from [Catalog].[Offer] where Offer.[SizeID] IN ({0}) and Offer.ProductId = [Product].[ProductID])",
                        sizeIds.ToArray());
                }
            }

            if (!string.IsNullOrEmpty(_categoryModel.Color))
            {
                var colorIds = _categoryModel.Color.Split(',').Select(item => item.TryParseInt()).Where(id => id != 0).ToList();
                if (colorIds.Count > 0)
                {
                    _model.Filter.ColorIds = colorIds;
                    _paging.Where(
                        SettingsCatalog.ShowOnlyAvalible || _categoryModel.Available
                            ? "and Exists(Select 1 from [Catalog].[Offer] where Offer.[ColorID] IN ({0}) and Offer.ProductId = [Product].[ProductID] AND Offer.amount > 0)"
                            : "and Exists(Select 1 from [Catalog].[Offer] where Offer.[ColorID] IN ({0}) and Offer.ProductId = [Product].[ProductID])",
                        colorIds.ToArray());
                }

                if (SettingsCatalog.ComplexFilter)
                {
                    _paging.Select(
                        string.Format(
                            "(select Top 1 PhotoName from catalog.Photo inner join Catalog.Offer on Photo.objid=Offer.Productid and Type='product'" +
                            " Where Offer.ProductId=Product.ProductId and Photo.ColorID in({0}) Order by Photo.Main Desc, Photo.PhotoSortOrder)",
                            _model.Filter.ColorIds.AggregateString(",")).AsSqlField("AdditionalPhoto"));
                }
                else
                {
                    _paging.Select("null".AsSqlField("AdditionalPhoto"));
                }
            }
            else
            {
                _paging.Select("null".AsSqlField("AdditionalPhoto"));
            }

            var currency = CurrencyService.CurrentCurrency;
            if (SettingsCatalog.DefaultCurrencyIso3 != currency.Iso3)
            {
                _paging.Where("", currency.Iso3);
            }

            if (_categoryModel.PriceFrom.HasValue || _categoryModel.PriceTo.HasValue)
            {
                var pricefrom = _categoryModel.PriceFrom ?? 0;
                var priceto = _categoryModel.PriceTo ?? int.MaxValue;

                _model.Filter.PriceFrom = pricefrom;
                _model.Filter.PriceTo = priceto;
                
                _paging.Where("and (ProductExt.PriceTemp>= {0} ", pricefrom * currency.Rate);
                _paging.Where("and ProductExt.PriceTemp <= {0})", priceto * currency.Rate);
            }

            if (!string.IsNullOrEmpty(_categoryModel.Prop))
            {
                var selectedPropertyIDs = new List<int>();
                var filterCollection = _categoryModel.Prop.Split('-');
                foreach (var val in filterCollection)
                {
                    var tempListIds = new List<int>();
                    foreach (int id in val.Split(',').Select(item => item.TryParseInt()).Where(id => id != 0))
                    {
                        tempListIds.Add(id);
                        selectedPropertyIDs.Add(id);
                    }
                    if (tempListIds.Count > 0)
                        _paging.Where("AND Exists(Select 1 from [Catalog].[ProductPropertyValue] where [Product].[ProductID] = [ProductID] and PropertyValueID IN ({0}))", tempListIds.ToArray());
                }
                _model.Filter.PropertyIds = selectedPropertyIDs;
            }

            var rangeIds = new Dictionary<int, KeyValuePair<float, float>>();
            var rangeQueries =
                HttpContext.Current.Request.QueryString.AllKeys.Where(
                    p => p != null && p.StartsWith("prop_") && (p.EndsWith("_min") || p.EndsWith("_max"))).ToList();

            foreach (var rangeQuery in rangeQueries)
            {
                if (rangeQuery.EndsWith("_max"))
                    continue;

                var propertyId = rangeQuery.Split('_')[1].TryParseInt();
                if (propertyId == 0)
                    continue;

                var min = HttpContext.Current.Request.QueryString[rangeQuery].TryParseFloat();
                var max = HttpContext.Current.Request.QueryString[rangeQuery.Replace("min", "max")].TryParseFloat();

                rangeIds.Add(propertyId, new KeyValuePair<float, float>(min, max));
            }

            if (rangeIds.Count > 0)
            {
                foreach (var i in rangeIds.Keys)
                {
                    _paging.Where("AND Exists( select 1 from [Catalog].[ProductPropertyValue] " +
                        "Inner Join [Catalog].[PropertyValue] on [PropertyValue].[PropertyValueID] = [ProductPropertyValue].[PropertyValueID] " +
                        "Where [Product].[ProductID] = [ProductID] and PropertyId = {0}", i);
                    _paging.Where("And RangeValue >= {0}", rangeIds[i].Key);
                    _paging.Where("And RangeValue <= {0})", rangeIds[i].Value);
                }
            }
            _model.Filter.RangePropertyIds = rangeIds;

            if (SettingsCatalog.ShowOnlyAvalible || _categoryModel.Available)
            {
                _paging.Where("AND MaxAvailable>{0}", 0);
            }
            _model.Filter.Available = _categoryModel.Available;

            if (!string.IsNullOrWhiteSpace(_categoryModel.TagUrl) && _categoryModel.TagId > 0)
            {
                _paging.Where("AND Exists(Select 1 from [Catalog].[TagMap] where TagMap.[ObjId] = Product.[ProductID] and TagMap.[Type] ='Product' and TagMap.TagId={0})", _categoryModel.TagId);
            }
        }

        private void BuildExludingFilters()
        {
            if (!SettingsCatalog.ExcludingFilters)
                return;
            var tasks = new List<Task>();
            var task = Task.Factory.StartNew(() =>
            {
                _model.Filter.AvailablePropertyIds =
                    _paging.GetCustomData("PropertyValueID",
                        " AND PropertyValueID is not null",
                        reader => SQLDataHelper.GetInt(reader, "PropertyValueID"), true,
                        "Left JOIN [Catalog].[ProductPropertyValue] ON [Product].[ProductID] = [ProductPropertyValue].[ProductID]");
            });
            tasks.Add(task);

            if (SettingsCatalog.ShowProducerFilter)
            {
                task = Task.Factory.StartNew(() =>
                {
                    _model.Filter.AvailableBrandIds =
                        _paging.GetCustomData("Product.BrandID", " AND Product.BrandID is not null",
                            reader => SQLDataHelper.GetInt(reader, "BrandID"), true);
                });
                tasks.Add(task);
            }

            if (SettingsCatalog.ShowSizeFilter)
            {
                task = Task.Factory.StartNew(() =>
                {
                    _model.Filter.AvailableSizeIds =
                        _paging.GetCustomData("sizeOffer.SizeID", " AND sizeOffer.SizeID is not null",
                            reader => SQLDataHelper.GetInt(reader, "SizeID"), true,
                            "Left JOIN [Catalog].[Offer] as sizeOffer ON [Product].[ProductID] = [sizeOffer].[ProductID]");
                });
                tasks.Add(task);
            }

            if (SettingsCatalog.ShowColorFilter)
            {
                task = Task.Factory.StartNew(() =>
                {
                    _model.Filter.AvailableColorIds =
                        _paging.GetCustomData("colorOffer.ColorID", " AND colorOffer.ColorID is not null",
                            reader => SQLDataHelper.GetInt(reader, "ColorID"), true,
                            "Left JOIN [Catalog].[Offer] as colorOffer ON [Product].[ProductID] = [colorOffer].[ProductID]");
                });
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray(), 1000);
        }

        private void GetViewMode()
        {
            var cookieMode = CommonHelper.GetCookieString("viewmode");
            var mode = SettingsCatalog.EnabledCatalogViewChange
                ? cookieMode.Parse<ProductViewMode>(SettingsCatalog.DefaultCatalogView)
                : SettingsCatalog.DefaultCatalogView;
            _model.Filter.ViewMode = mode.ToString().ToLower();
            _model.Filter.AllowChangeViewMode = SettingsCatalog.EnabledCatalogViewChange;
        }
    }
}