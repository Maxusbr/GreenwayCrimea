using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Web.Admin.Models.Categories;

namespace AdvantShop.Web.Admin.Handlers.Categories
{
    public class GetRelatedAlternativeCategories
    {
        private readonly int _categoryId;

        public GetRelatedAlternativeCategories(int categoryId)
        {
            _categoryId = categoryId;
        }

        public RelatedAlternativeCategories Execute()
        {
            var model = new RelatedAlternativeCategories
            {
                Categories = GetCategories(),
                RelatedCategories = CategoryService.GetRelatedCategories(_categoryId, RelatedType.Related),
                RelatedProperties = CategoryService.GetRelatedProperties(_categoryId, RelatedType.Related),
                RelatedPropertyValues = CategoryService.GetRelatedPropertyValues(_categoryId, RelatedType.Related),

                AlternativeCategories = CategoryService.GetRelatedCategories(_categoryId, RelatedType.Alternative),
                AlternativeProperties = CategoryService.GetRelatedProperties(_categoryId, RelatedType.Alternative),
                AlternativePropertyValues = CategoryService.GetRelatedPropertyValues(_categoryId, RelatedType.Alternative),
            };

            model.Properties = PropertyService.GetAllProperties().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.PropertyId.ToString()
            }).ToList();

            model.Properties.Insert(0,
                new SelectListItem()
                {
                    Text = LocalizationService.GetResource("Resource.Admin_Catalog_AutoRecommendations_NotSelected"),
                    Value = "-1"
                });


            return model;
        }

        private List<SelectListItem> GetCategories()
        {
            var categories = CategoryService.GetCategories();
            var list = new List<SelectListItem>()
	        {
		        new SelectListItem()
		        {
			        Text = LocalizationService.GetResource("Resource.Admin_Catalog_AutoRecommendations_NotSelected"),
			        Value = "-1"
		        }
	        };
            LoadAllCategories(categories, list, 0, "");

            return list;
        }

        private void LoadAllCategories(List<Category> categories, List<SelectListItem> list, int categoryId, string offset)
        {
            foreach (var category in categories.Where(c => c.ParentCategoryId == categoryId).OrderBy(c => c.SortOrder).ToList())
            {
                list.Add(new SelectListItem() { Text = HttpUtility.HtmlDecode(offset + category.Name), Value = category.CategoryId.ToString() });

                if (categories.Any(c => c.ParentCategoryId == category.CategoryId))
                {
                    LoadAllCategories(categories, list, category.CategoryId, offset + "&nbsp;&nbsp;");
                }
            }
        }

    }
}
