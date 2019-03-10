using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Web.Admin.ViewModels.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Helpers;
using AdvantShop.Core.Services.FullSearch;
using AdvantShop.FullSearch;

namespace AdvantShop.Web.Admin.Handlers.Catalog
{
    public class GetCategoryList
    {
        private int _categoryId;
        private string _categorySearch;
        private UrlHelper _url;

        public GetCategoryList(int categoryId, string categorySearch)
        {
            _categoryId = categoryId;
            _url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            _categorySearch = categorySearch;
        }

        public List<AdminCategoryListItemViewModel> Execute()
        {
            List<Category> categories;

            if (_categorySearch.IsNullOrEmpty())
            {
                categories = CategoryService.GetChildCategoriesByCategoryId(_categoryId, false);
            }
            else
            {
                var tanslitQ = StringHelper.TranslitToRusKeyboard(_categorySearch);
                var categoryIds = CategorySeacher.Search(_categorySearch).SearchResultItems.Select(item => item.Id)
                    .Union(CategorySeacher.Search(tanslitQ).SearchResultItems.Select(item => item.Id)).Distinct();
                categories = categoryIds.Select(id => CategoryService.GetCategory(id)).Where(x => x != null).ToList();
            }

            return
                categories != null
                    ? categories.Select(x => new AdminCategoryListItemViewModel()
                    {
                        CategoryId = x.CategoryId,
                        Name = x.Name,
                        Sorting = x.Sorting.ToString(),
                        MiniPictureSrc = x.MiniPicture.ImageSrcSmall(),
                        Url = _url.Action("Edit", "Category", new {id = x.CategoryId}),
                        ProductsCount = x.ProductsCount
                    }).ToList()
                    : new List<AdminCategoryListItemViewModel>();
        }
    }
}
