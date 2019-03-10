using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.Reviews;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.Reviews
{
    public class GetReviewsHandler
    {
        private readonly ReviewsFilterModel _filterModel;
        private SqlPaging _paging;

        public GetReviewsHandler(ReviewsFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<ReviewItemModel> Execute()
        {
            var model = new FilterResult<ReviewItemModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<ReviewItemModel>();
            
            return model;
        }

        public List<int> GetItemsIds(string fieldName)
        {
            GetPaging();

            return _paging.ItemsIds<int>(fieldName);
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };

            _paging.Select(
                "ReviewId",
                "PhotoName",
                "Review.Name",
                "Email",
                "AddDate",
                "Checked",
                "Text",
                "Review.Type",
                "EntityId",
                "ProductId",
                "Product.Name".AsSqlField("ProductName"),
                "ArtNo"
                );

            _paging.From("[CMS].[Review]");
            _paging.Left_Join("[Catalog].[Photo] ON Photo.ObjId = Review.ReviewId AND Photo.Type = {0} AND Main = 1", PhotoType.Review.ToString());
            _paging.Left_Join("Catalog.Product ON Product.ProductId = Review.EntityId AND Review.Type = {0}", EntityType.Product);
            
            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrEmpty(_filterModel.Search))
            {
                _paging.Where("Text LIKE '%'+{0}+'%' OR Review.Name LIKE '%'+{0}+'%' OR Email LIKE '%'+{0}+'%' OR Product.Name LIKE '%'+{0}+'%'", _filterModel.Search);
            }

            if (_filterModel.EntityId != 0)
            {
                _paging.Where("EntityId = {0}", _filterModel.EntityId);
            }

            if (!string.IsNullOrEmpty(_filterModel.Text))
            {
                _paging.Where("Text LIKE '%'+{0}+'%'", _filterModel.Text);
            }

            if (!string.IsNullOrEmpty(_filterModel.Name))
            {
                _paging.Where("Review.Name LIKE '%'+{0}+'%'", _filterModel.Name);
            }

            if (!string.IsNullOrEmpty(_filterModel.Email))
            {
                _paging.Where("Email LIKE '%'+{0}+'%'", _filterModel.Email);
            }

            if (!string.IsNullOrEmpty(_filterModel.ArtNo))
            {
                _paging.Where("Product.ArtNo LIKE '%'+{0}+'%'", _filterModel.ArtNo);
            }

            if (_filterModel.Checked != null)
            {
                _paging.Where("Checked = {0}", Convert.ToInt32(_filterModel.Checked.Value));
            }

            if (_filterModel.HasPhoto != null)
            {
                _paging.Where("PhotoName is " + (_filterModel.HasPhoto.Value ? "not null" : "null"));
            }

            DateTime from, to;

            if (!string.IsNullOrWhiteSpace(_filterModel.DateFrom) && DateTime.TryParse(_filterModel.DateFrom, out from))
            {
                _paging.Where("AddDate >= {0}", from);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.DateTo) && DateTime.TryParse(_filterModel.DateTo, out to))
            {
                _paging.Where("AddDate <= {0}", to);
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderByDesc("AddDate");
                return;
            }

            var sorting = _filterModel.Sorting.ToLower().Replace("formatted", "");

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
    }
}
