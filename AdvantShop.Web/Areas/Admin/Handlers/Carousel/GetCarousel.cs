using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.Carousel;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Catalog;

namespace AdvantShop.Web.Admin.Handlers.Carousel
{
    public class GetCarousel
    {
        private CarouselFilterModel _filterModel;
        private SqlPaging _paging;

        public GetCarousel(CarouselFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<CarouselResultFilterModel> Execute()
        {
            var model = new FilterResult<CarouselResultFilterModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = string.Format("Найдено изображений: {0}", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<CarouselResultFilterModel>();
            
            return model;
        }

        public List<int> GetItemsIds()
        {
            GetPaging();

            return _paging.ItemsIds<int>("CarouselID");
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };
            
            _paging.Select(
                "CarouselID",
                "URL".AsSqlField("CaruselUrl"),
                "SortOrder",
                "Enabled",
                "DisplayInOneColumn",
                "DisplayInTwoColumns",
                "DisplayInMobile",
                "Blank",
                "Description",
                "PhotoName");

            _paging.From("[CMS].[Carousel]");
            _paging.Left_Join("Catalog.Photo on Photo.ObjId=Carousel.CarouselID and Type={0}", PhotoType.Carousel.ToString());

            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrWhiteSpace(_filterModel.Search))
            {
                _filterModel.CaruselUrl = _filterModel.Search;
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.CaruselUrl))
            {
                _paging.Where("URL LIKE '%'+{0}+'%'", _filterModel.CaruselUrl);
            }
            if (!string.IsNullOrWhiteSpace(_filterModel.Description))
            {
                _paging.Where("Description LIKE '%'+{0}+'%'", _filterModel.Description);
            }
            if (!string.IsNullOrEmpty(_filterModel.SortingFrom))
            {
                _paging.Where("SortOrder >= {0}", _filterModel.SortingFrom);
            }
            if (!string.IsNullOrEmpty(_filterModel.SortingTo))
            {
                _paging.Where("SortOrder <= {0}", _filterModel.SortingTo);
            }
            if (_filterModel.Enabled != null)
            {
                _paging.Where("Enabled = {0}",(bool)_filterModel.Enabled ? "1" : "0");
            }
            if (_filterModel.DisplayInOneColumn != null)
            {
                _paging.Where("DisplayInOneColumn = {0}", (bool)_filterModel.DisplayInOneColumn ? "1" : "0");
            }
            if (_filterModel.DisplayInTwoColumns != null)
            {
                _paging.Where("DisplayInTwoColumns = {0}", (bool)_filterModel.DisplayInTwoColumns ? "1" : "0");
            }
            if (_filterModel.DisplayInMobile != null)
            {
                _paging.Where("DisplayInMobile = {0}", (bool)_filterModel.DisplayInMobile ? "1" : "0");
            }
            if (_filterModel.Blank.HasValue)
            {
                _paging.Where("Blank = {0}", _filterModel.Blank.Value);
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy(
                    new SqlCritera("SortOrder", "", SqlSort.Asc)
                    );
                return;
            }

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
    }
}
