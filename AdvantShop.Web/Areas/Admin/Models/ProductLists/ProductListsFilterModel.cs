using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.ProductLists
{
    public class ProductListsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public int SortOrder { get; set; }
    }

    public class ProductListsFilterModel : BaseFilterModel
    {
        public string Name { get; set; }
    }
}
