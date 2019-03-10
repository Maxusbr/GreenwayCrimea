using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Properties
{
    public class PropertiesFilterModel : BaseFilterModel
    {
        public string Name { get; set; }
        
        public int? GroupId { get; set; }

        public bool? UseInFilter { get; set; }

        public bool? UseInDetails { get; set; }

        public bool? UseInBrief { get; set; }

        public int? SortingFrom { get; set; }

        public int? SortingTo { get; set; }
    }
}
