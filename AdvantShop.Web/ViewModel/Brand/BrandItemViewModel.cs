using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.CMS;
using AdvantShop.ViewModel.Catalog;

namespace AdvantShop.ViewModel.Brand
{
    public class BrandItemViewModel
    {
        public AdvantShop.Catalog.Brand Brand { get; set; }

        public List<SelectListItem> Countries { get; set; }

        public List<BrandLetter> EnLetters { get; set; }

        public List<BrandLetter> RuLetters { get; set; }

        public List<BrandCategoryViewModel> Categories { get; set; }

        public List<BreadCrumbs> BreadCrumbs { get; set; }

        public ProductListViewModel ProductsList { get; set; }
        public int CurentCountyId { get; set; }

        public BrandItemViewModel()
        {
            Countries = new List<SelectListItem>();
        }
    }

    public class BrandCategoryViewModel
    {
        public string Name { get; set; }

        public int  Count { get; set; }

        public string Url { get; set; }

        public List<BrandCategoryViewModel> SubCategories { get; set; }
    }
}