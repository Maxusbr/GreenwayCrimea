using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.Colors;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.Colors
{
    public class GetColorsHandler
    {
        private readonly ColorsFilterModel _filterModel;
        private SqlPaging _paging;

        public GetColorsHandler(ColorsFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<ColorModel> Execute()
        {
            var model = new FilterResult<ColorModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<ColorModel>();
            
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
                "Color.ColorId",
                "ColorName",
                "ColorCode",
                "PhotoName",
                "SortOrder",
                "(Select count(distinct ProductId) From Catalog.Offer Where Offer.ColorId = Color.ColorId)".AsSqlField("ProductsCount")
                );

            _paging.From("[Catalog].[Color]");
            _paging.Left_Join("[Catalog].[Photo] on [Photo].[ObjId] = [Color].[ColorID] and type = 'color'");
            
            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrEmpty(_filterModel.Search))
            {
                _paging.Where("ColorName LIKE '%'+{0}+'%'", _filterModel.Search);
            }

            if (!string.IsNullOrEmpty(_filterModel.ColorName))
            {
                _paging.Where("ColorName LIKE '%'+{0}+'%'", _filterModel.ColorName);
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy("SortOrder");
                _paging.OrderBy("ColorName");
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