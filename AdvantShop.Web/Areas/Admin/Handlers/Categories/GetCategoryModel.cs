using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Saas;
using AdvantShop.SEO;
using AdvantShop.Web.Admin.Models.Categories;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdvantShop.Web.Admin.Handlers.Categories
{
    public class GetCategoryModel
    {
        private readonly Category _category;

        public GetCategoryModel(Category category)
        {
            _category = category;
        }

        public AdminCategoryModel Execute()
        {
            var model = new AdminCategoryModel()
            {
                IsEditMode = true,
                CategoryId = _category.CategoryId,
                Name = _category.Name,
                UrlPath = _category.UrlPath,
                BriefDescription = _category.BriefDescription,
                Description = _category.Description,
                SortOrder = _category.SortOrder,
                Enabled = _category.Enabled,

                DisplayStyle = _category.DisplayStyle,
                Sorting = _category.Sorting,
                DisplayBrandsInMenu = _category.DisplayBrandsInMenu,

                DisplaySubCategoriesInMenu = _category.DisplaySubCategoriesInMenu,
                Hidden = _category.Hidden,

                Picture = _category.Picture,
                Icon = _category.Icon,
                MiniPicture = _category.MiniPicture,

            };

            var parent = CategoryService.GetCategory(_category.ParentCategoryId);
            if (parent != null)
            {
                model.ParentCategoryId = parent.CategoryId;
                model.ParentCategoryName = parent.Name;
            }

            var meta = MetaInfoService.GetMetaInfo(_category.CategoryId, MetaType.Category);
            if (meta == null)
            {
                model.DefaultMeta = true;
            }
            else
            {
                model.DefaultMeta = false;
                model.SeoTitle = meta.Title;
                model.SeoH1 = meta.H1;
                model.SeoKeywords = meta.MetaKeywords;
                model.SeoDescription = meta.MetaDescription;
            }

            if (_category.CategoryId != 0)
            {
                var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

                model.BreadCrumbs =
                    CategoryService.GetParentCategories(_category.CategoryId)
                        .Select(x => new BreadCrumbs(x.Name, urlHelper.Action("Index", "Catalog", new { categoryId = x.CategoryId })))
                        .Reverse()
                        .ToList();
                model.BreadCrumbs.Insert(0, new BreadCrumbs(LocalizationService.GetResource("Admin.Catalog.Index.CatalogTitle"), "catalog"));
            }
            return model;
        }
    }
}
