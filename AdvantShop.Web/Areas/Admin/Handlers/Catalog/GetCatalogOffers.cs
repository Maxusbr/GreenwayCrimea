using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Web.Admin.Models.Catalog;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.FullSearch;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.Catalog
{
    public class GetCatalogOffers
    {
        private readonly CatalogFilterModel _filterModel;
        private SqlPaging _paging;

        public GetCatalogOffers(CatalogFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<CatalogOfferModel> Execute()
        {
            var model = new FilterResult<CatalogOfferModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.DataItems = new List<CatalogOfferModel>();

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
                return model;
            
            // Достаем товары с главным офером и потом если нужно берем остальные
            var items = _paging.PageItemsList<CatalogOfferModel>();

            foreach (var item in items)
            {
                if (item.OffersCount == 1)
                {
                    model.DataItems.Add(item);
                    continue;
                }

                var offers = OfferService.GetProductOffers(item.ProductId).OrderByDescending(x => x.Main).ToList();

                foreach (var offer in offers)
                {

                    model.DataItems.Add(new CatalogOfferModel()
                    {
                        ProductId = offer.ProductId,
                        OfferId = offer.OfferId,
                        Name = item.Name,
                        ArtNo = offer.ArtNo,
                        Price = offer.BasePrice,
                        Main = offer.Main,
                        ColorName = offer.ColorID != null ? offer.Color.ColorName : null,
                        SizeName = offer.SizeID != null ? offer.Size.SizeName : null,
                        CurrencyCode = item.CurrencyCode,
                        CurrencyIso3 = item.CurrencyIso3,
                        CurrencyValue = item.CurrencyValue,
                        IsCodeBefore = item.IsCodeBefore
                    });
                }
            }

            return model;
        }
        
        public List<T> GetItemsIds<T>(string fieldName)
        {
            GetPaging();

            return _paging.ItemsIds<T>(fieldName);
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };

            // Достаем товары с главным офером и потом если нужно берем остальные
            _paging.Select(
                "Product.ProductID",
                "Product.Name",
                "Offer.OfferId",
                "Offer.ArtNo",
                "Offer.Price",
                "Offer.ColorID",
                "Offer.SizeID",
                "Color.ColorName",
                "Size.SizeName",
                "[Currency].Code".AsSqlField("CurrencyCode"),
                "[Currency].CurrencyIso3",
                "[Currency].CurrencyValue",
                "[Currency].IsCodeBefore",

                "(Select count (offerid) from catalog.Offer where Offer.ProductID=Product.productID)".AsSqlField("OffersCount"),

                (_filterModel.ShowMethod == ECatalogShowMethod.Normal ? "ProductCategories.SortOrder" : "-1 as SortOrder")
                );

            _paging.From("[Catalog].[Product]");
            _paging.Inner_Join("[Catalog].[Offer] ON [Product].[ProductID] = [Offer].[ProductID] and [Offer].[Main] = 1");
            _paging.Left_Join("[Catalog].[Color] ON [Color].[ColorId] = [Offer].[ColorId]");
            _paging.Left_Join("[Catalog].[Size] ON [Size].[SizeId] = [Offer].[SizeId]");
            _paging.Left_Join("[Catalog].[Currency] ON [Product].[CurrencyID] = [Currency].[CurrencyID]");

            if (_filterModel.ShowMethod == ECatalogShowMethod.Normal)
            {
                _paging.Inner_Join("[Catalog].[ProductCategories] on [ProductCategories].[ProductId] = [Product].[ProductID]");

                _paging.Where("ProductCategories.CategoryID = {0}", _filterModel.CategoryId);
            }

            Filter();
            Sorting();
        }

        private void Filter()
        {
            if (!string.IsNullOrEmpty(_filterModel.Search))
            {
                var res = ProductSeacherAdmin.Search(_filterModel.Search);
                var productIds = res.SearchResultItems.Aggregate("", (current, item) => current + (item.Id + "/"));
                _paging.Inner_Join(
                    "(select item, sort from [Settings].[ParsingBySeperator]({0},'/') ) as dtt on Product.ProductId=convert(int, dtt.item)",
                    productIds);

                if (_filterModel.SortingType == FilterSortingType.None)
                {
                    _paging.OrderBy("dtt.sort");
                }
            }

            //if (!string.IsNullOrWhiteSpace(_filterModel.ArtNo))
            //{
            //    _paging.Where("Offer.ArtNo LIKE '%'+{0}+'%'", _filterModel.ArtNo);
            //}

            //if (!string.IsNullOrWhiteSpace(_filterModel.Name))
            //{
            //    _paging.Where("Product.Name LIKE '%'+{0}+'%'", _filterModel.Name);
            //}


            //if (_filterModel.ColorId.IsNotEmpty())
            //{
            //    var color = ColorService.GetColor(_filterModel.ColorId.TryParseInt());
            //    if (color != null)
            //    {
            //        _paging.Where("Offer.ColorID={0}", _filterModel.ColorId);
            //    }
            //}

            //if (_filterModel.SizeId.IsNotEmpty())
            //{
            //    var size = SizeService.GetSize(_filterModel.SizeId.TryParseInt());
            //    if (size != null)
            //    {
            //        _paging.Where("Offer.SizeID={0}", _filterModel.SizeId);
            //    }
            //}

            //if (_filterModel.PriceFrom != 0 && _filterModel.PriceTo != 0)
            //{
            //    _paging.Where("Price >= {0} and Price <= {1}", _filterModel.PriceFrom, _filterModel.PriceTo);
            //}
            
            //if (_filterModel.AmountFrom != null)
            //{
            //    _paging.Where("Amount >= {0}", _filterModel.AmountFrom);
            //}
            
            //if (_filterModel.AmountTo != null)
            //{
            //    _paging.Where("Amount <= {1}", _filterModel.AmountTo);
            //}

            //if (_filterModel.SortingFrom != null && _filterModel.SortingTo != null && _filterModel.ShowMethod == ECatalogShowMethod.Normal)
            //{
            //    _paging.Where("ProductCategories.SortOrder >= {0} and ProductCategories.SortOrder <= {1}", _filterModel.SortingFrom, _filterModel.SortingTo);
            //}


            //if (_filterModel.HasPhoto != null)
            //{
            //    _paging.Where(string.Format("PhotoName is {0} null", _filterModel.HasPhoto.Value ? "not" : ""));
            //}

            //if (_filterModel.Enabled != null)
            //{
            //    _paging.Where("Enabled = {0}", _filterModel.Enabled.Value ? "1" : "0");
            //}

            if (_filterModel.ExcludeIds != null)
            {
                var exludeIds = _filterModel.ExcludeIds.Split(',').Select(x => x.TryParseInt()).Where(x => x != 0).ToList();
                if (exludeIds.Count > 0)
                {
                    _paging.Where("Offer.OfferId not in (" + String.Join(",", exludeIds) + ")");
                }
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy(
                    _filterModel.ShowMethod != ECatalogShowMethod.Normal
                        ? "ArtNo"
                        : "ProductCategories.SortOrder");
            }
            else
            {
                var sorting = _filterModel.Sorting.ToLower();

                var field = _paging.SelectFields().FirstOrDefault(x => x.FieldName == sorting);
                if (field != null)
                {
                    if (_filterModel.SortingType == FilterSortingType.Asc)
                    {
                        _paging.OrderBy(sorting);
                    }
                    else
                    {
                        _paging.OrderByDesc(sorting);
                    }
                }
            }

            _paging.OrderBy("Offer.ProductId");
            _paging.OrderByDesc("Offer.Main");
        }
    }
}