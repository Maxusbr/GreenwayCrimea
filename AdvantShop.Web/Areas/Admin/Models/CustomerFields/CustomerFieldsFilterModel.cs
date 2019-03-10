using AdvantShop.Customers;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.CustomerFields
{
    public class CustomerFieldsFilterModel : BaseFilterModel<int>
    {
        public string Name { get; set; }
        public CustomerFieldType? FieldType { get; set; }
        public bool? Required { get; set; }
        public bool? ShowInClient { get; set; }
        public bool? Enabled { get; set; }
    }
}
