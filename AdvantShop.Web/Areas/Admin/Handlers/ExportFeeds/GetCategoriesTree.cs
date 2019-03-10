using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Web.Admin.Models.Catalog;
using AdvantShop.Web.Admin.ViewModels.Catalog;
using AdvantShop.ExportImport;

namespace AdvantShop.Web.Admin.Handlers.ExportFeeds
{
    public class GetCategoriesTree
    {
        private readonly string _id;
        private readonly List<int> _categoriesIdSelected;
        private readonly List<int> _excludeIds;
        private readonly bool _showRoot;


        public GetCategoriesTree(CategoriesTree model, int exportFeedId)
        {
            _id = model.Id;
            _categoriesIdSelected = ExportFeedService.GetExportFeedCategoriesId(exportFeedId);
            _excludeIds = model.ExcludeIds != null ? model.ExcludeIds.Split(',').Select(x => x.TryParseInt()).ToList() : new List<int>();

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
                                    selected = _categoriesIdSelected.Any(item=> item == 0)
                                }
                        }
                    };
            }

            var categories = CategoryService.GetChildCategoriesByCategoryId(_id.TryParseInt(), false);
            List<int> categoryOpen = new List<int>() { 0 };
            foreach (var selectedCategoryId in _categoriesIdSelected)
            {
                if (categories.Any(x => x.CategoryId != selectedCategoryId))
                {
                    var category = CategoryService.GetCategory(selectedCategoryId);
                    if (category != null)
                    {
                        int i = 20;
                        while (category.ParentCategoryId > 0 && i > 0)
                        {
                            if (categories.Any(x => x.CategoryId == category.ParentCategoryId))
                            {
                                categoryOpen.Add(category.ParentCategoryId);
                                break;
                            }
                            category = CategoryService.GetCategory(category.ParentCategoryId);
                            i--;
                        }
                    }
                }
            }

            return categories.Where(x => !_excludeIds.Contains(x.CategoryId)).Select(x => new AdminCatalogTreeViewItem()
            {
                id = x.CategoryId.ToString(),
                parent = _id == "#" && x.ParentCategoryId == 0 ? "#" : x.ParentCategoryId.ToString(),
                text =
                    string.Format(
                        "<span class=\"jstree-advantshop-name\">{0}</span> <span class=\"jstree-advantshop-count\">{1}/{2}</span>",
                        x.Name, x.ProductsCount, x.TotalProductsCount),
                name = x.Name,
                children = x.HasChild,
                state = new AdminCatalogTreeViewItemState()
                {
                    opened = categoryOpen.Any(item => item == x.CategoryId),
                    selected = _categoriesIdSelected.Any(item => item == x.CategoryId)
                }
            }).ToList();
        }
    }
}
