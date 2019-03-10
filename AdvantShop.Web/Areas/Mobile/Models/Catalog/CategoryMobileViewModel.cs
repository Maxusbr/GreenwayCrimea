using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Core.Models;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Models.Catalog;
using AdvantShop.ViewModel.Catalog;

namespace AdvantShop.Areas.Mobile.Models.Catalog
{
    public class CategoryMobileViewModel
    {
        public CategoryMobileViewModel(Category category)
        {
            Category = category;
            Pager = new Pager();
        }

        public Category Category { get; set; }

        public Tag Tag { get; set; }

        public Category ParentCategory { get; set; }

        public ProductViewMobileModel Products { get; set; }

        public Pager Pager { get; set; }

        public CategoryFiltering Filter { get; set; }

        public List<SelectListItem> SortingList { get; set; }
        
        public bool HasProducts
        {
            get { return Filter != null && Products != null && Products.Products.Count > 0; }
        }

        public TagViewModel TagView { get; set; }
    }
}