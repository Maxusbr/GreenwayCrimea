using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Web.Admin.Models.Catalog;
using AdvantShop.Web.Admin.ViewModels.Catalog;

namespace AdvantShop.Web.Admin.Handlers.Catalog
{
    public class GetCatalogIndexHandler
    {
        private CatalogFilterModel _model;
        private UrlHelper _url;

        public GetCatalogIndexHandler(CatalogFilterModel model)
        {
            _model = model;
            _url = new UrlHelper(HttpContext.Current.Request.RequestContext);
        }

        public AdminCatalog Execute()
        {
            var viewModel = new AdminCatalog() { ShowMethod = _model.ShowMethod, Search = _model.Search, CategorySearch = _model.CategorySearch };

            switch (_model.ShowMethod)
            {
                case ECatalogShowMethod.Normal:
                    {
                        var category = CategoryService.GetCategory(_model.CategoryId ?? 0);
                        if (category == null)
                            return null;

                        viewModel.CategoryId = category.CategoryId;
                        viewModel.Title = category.Name;
                        viewModel.Category = category;
                        viewModel.HasChildCategories = CategoryService.GetChildCategoriesByCategoryId(category.CategoryId, false).Any();

                        if (category.CategoryId != 0)
                        {
                            viewModel.BreadCrumbs =
                                CategoryService.GetParentCategories(category.CategoryId)
                                    .Select(x => new BreadCrumbs(x.Name, _url.Action("Index", "Catalog", new { categoryId = x.CategoryId })))
                                    .Reverse()
                                    .ToList();
                            viewModel.BreadCrumbs.Insert(0, new BreadCrumbs(LocalizationService.GetResource("Admin.Catalog.Index.CatalogTitle"), "catalog"));
                        }
                        break;
                    }

                case ECatalogShowMethod.AllProducts:
                    viewModel.Title = LocalizationService.GetResource("Admin.Catalog.Index.AllProductsTitle");
                    break;

                case ECatalogShowMethod.OnlyInCategories:
                    viewModel.Title = LocalizationService.GetResource("Admin.Catalog.Index.OnlyInCategoriesTitle");
                    break;

                case ECatalogShowMethod.OnlyWithoutCategories:
                    viewModel.Title = LocalizationService.GetResource("Admin.Catalog.Index.OnlyWithoutCategoriesTitle");
                    break;
            }

            return viewModel;
        }
    }
}
