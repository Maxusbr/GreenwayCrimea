using AdvantShop.Catalog;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Sizes
{
    public class SizesFilterModel : BaseFilterModel
    {
        public string SizeName { get; set; }
        
    }

    public class SizeModel
    {
        public int SizeId { get; set; }
        public string SizeName { get; set; }
        public int SortOrder { get; set; }
        public int ProductsCount { get; set; }


        public bool CanBeDeleted
        {
            get { return !SizeService.IsSizeUsed(SizeId); }
        }
    }
}
