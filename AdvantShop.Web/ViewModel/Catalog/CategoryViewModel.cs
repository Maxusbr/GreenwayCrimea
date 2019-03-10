using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Core.Models;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Models;
using AdvantShop.Models.Catalog;

namespace AdvantShop.ViewModel.Catalog
{
    public partial class CategoryViewModel : BaseModel
    {
        public CategoryViewModel(Category category)
        {
            Category = category;
            Pager = new Pager();
        }

        public Category Category { get; set; }

        public Tag Tag { get; set; }

        public ProductViewModel Products { get; set; }

        public Pager Pager { get; set; }

        public CategoryFiltering Filter { get; set; }

        public List<BreadCrumbs> BreadCrumbs { get; set; }

        public bool HasProducts
        {
            get { return Filter != null && Products != null && Products.Products.Count > 0; }
        }

        public TagViewModel TagView { get; set; }
    }

    public class TagView
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }

    public class TagViewModel
    {
        public string CategoryUrl { get; set; }
        //for more need enum
        public bool NonCategoryView { get; set; }
        public List<TagView> Tags { get; set; }
    }

}