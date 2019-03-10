using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.SQL2;
using AdvantShop.SEO;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.Settings.System
{
    public class Get301Redirect
    {
        private Admin301RedirectFilterModel _filterModel;
        private SqlPaging _paging;

        public Get301Redirect(Admin301RedirectFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<RedirectSeo> Execute()
        {
            var model = new FilterResult<RedirectSeo>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = string.Format("Найдено позиций: {0}", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<RedirectSeo>();
            
            return model;
        }

        public List<int> GetItemsIds()
        {
            GetPaging();

            return _paging.ItemsIds<int>("ID");
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };
            
            _paging.Select(
                "ID",
                "RedirectFrom",
                "RedirectTo",
                "ProductArtNo");

            _paging.From("[Settings].[Redirect]");
            
            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrWhiteSpace(_filterModel.Search))
            {
                _paging.Where("RedirectFrom LIKE '%'+{0}+'%' OR RedirectTo LIKE '%'+{0}+'%'", _filterModel.Search);
            }
            if (_filterModel.RedirectFrom != null)
            {
                _paging.Where("RedirectFrom LIKE '%'+{0}+'%'", _filterModel.RedirectFrom);
            }
            if (!string.IsNullOrWhiteSpace(_filterModel.RedirectTo))
            {
                _paging.Where("RedirectTo LIKE '%'+{0}+'%'", _filterModel.RedirectTo);
            }
            if (_filterModel.ProductArtNo != null)
            {
                _paging.Where("ProductArtNo LIKE '%'+{0}+'%'", _filterModel.ProductArtNo);
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy(new SqlCritera("RedirectFrom", "", SqlSort.Asc));
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
