using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Core.Models;

namespace AdvantShop.Areas.Mobile.Models.Catalog
{
    public class SearchMobileModel
    {
        public string Q { get; set; }

        public int? Page { get; set; }

        public ProductViewMobileModel Products { get; set; }

        public Pager Pager { get; set; }

        public ESortOrder Sort { get; set; }

        public string ViewMode { get; set; }

        public List<SelectListItem> SortingList { get; set; }

        public bool HasProducts
        {
            get { return Products != null && Products.Products.Count > 0; }
        }

        public CategoryListMobileViewModel Categories { get; set; }
    }
}