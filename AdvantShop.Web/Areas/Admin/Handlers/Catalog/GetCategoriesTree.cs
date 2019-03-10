using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Web.Admin.Models.Catalog;
using AdvantShop.Web.Admin.ViewModels.Catalog;

namespace AdvantShop.Web.Admin.Handlers.Catalog
{
    public class GetCategoriesTree
    {
        private readonly string _id;
        private readonly int _categoryIdSelected;
        private readonly List<int> _excludeIds;
        private readonly List<int> _selectedIds;
        private readonly bool _showRoot;
        private List<int> allParentsIds = new List<int>();

        public GetCategoriesTree(CategoriesTree model)
        {
            _id = model.Id;
            _categoryIdSelected = model.CategoryIdSelected != null ? model.CategoryIdSelected.Value : -1;
            _excludeIds = model.ExcludeIds != null ? model.ExcludeIds.Split(',').Select(x => x.TryParseInt()).ToList() : new List<int>();
            _selectedIds = model.SelectedIds != null ? model.SelectedIds.Split(',').Select(x => x.TryParseInt()).ToList() : new List<int>();

            _showRoot = model.ShowRoot;
        }

        public List<AdminCatalogTreeViewItem> Execute()
        {
            if (_showRoot)
            {
                var category = CategoryService.GetCategory(0);
                if (category != null)
                    return new List<AdminCatalogTreeViewItem>()
                    {
                        new AdminCatalogTreeViewItem()
                        {
                            id = "0",
                            parent = "#",
                            text = String.Format("<span class=\"jstree-advantshop-name\">{0}</span>", category.Name),
                            name = category.Name,
                            children = true,
                            state = new AdminCatalogTreeViewItemState()
                            {
                                opened = true,
                                selected = _categoryIdSelected == 0 || _selectedIds.Contains(0)
                            },
                            li_attr = new Dictionary<string, string>() {
                                { "data-tree-id", "categoryItemId_" +  0}
                            },
                        }
                    };
            }

            var categories = CategoryService.GetChildCategoriesByCategoryId(_id.TryParseInt(), false);
            int categoryOpen = 0;

            foreach (var item in _selectedIds)
            {
                allParentsIds.AddRange(CategoryService.GetParentCategories(item).Select(x => x.CategoryId));
            }


            if (categories.Any(x => x.CategoryId != _categoryIdSelected))
            {
                var category = CategoryService.GetCategory(_categoryIdSelected);
                if (category != null)
                {
                    int i = 20;
                    while (category.ParentCategoryId > 0 && i > 0)
                    {
                        if (categories.Any(x => x.CategoryId == category.ParentCategoryId))
                        {
                            categoryOpen = category.ParentCategoryId;
                            break;
                        }
                        category = CategoryService.GetCategory(category.ParentCategoryId);
                        i--;
                    }
                }
            }

            return categories.Where(x => !_excludeIds.Contains(x.CategoryId)).Select(x => new AdminCatalogTreeViewItem()
            {
                id = x.CategoryId.ToString(),
                parent = _id == "#" && x.ParentCategoryId == 0 ? "#" : x.ParentCategoryId.ToString(),
                text =
                    String.Format(
                        "<span class=\"jstree-advantshop-name\">{0}</span> <span class=\"jstree-advantshop-count\">{1}/{2}</span>",
                        x.Name, x.ProductsCount, x.TotalProductsCount),
                name = x.Name,
                children = x.HasChild,
                state = new AdminCatalogTreeViewItemState()
                {
                    opened = x.CategoryId == categoryOpen || allParentsIds.Contains(x.CategoryId),
                    selected = x.CategoryId == _categoryIdSelected || _selectedIds.Contains(x.CategoryId)
                },
                li_attr = new Dictionary<string, string>() {
                        { "data-tree-id", "categoryItemId_" +  x.CategoryId}
                },
            }).ToList();
        }
    }
}
