using AdvantShop.Areas.Mobile.Models.Catalog;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AdvantShop.Areas.Mobile.Models.Home
{
    public class HomeMobileViewModel
    {
        public List<SelectListItem> CategoriesUrl { get; set; }

        public ProductViewMobileModel Bestsellers { get; set; }

        public ProductViewMobileModel NewProducts { get; set; }

        public ProductViewMobileModel Sales { get; set; }

        public List<ProductViewMobileModel> ProductLists { get; set; }

        public HomeMobileViewModel()
        {
            ProductLists = new List<ProductViewMobileModel>();
        }

    }
}