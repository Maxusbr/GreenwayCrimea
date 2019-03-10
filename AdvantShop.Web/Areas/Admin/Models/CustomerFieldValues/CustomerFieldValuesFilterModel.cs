using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.CustomerFieldValues
{
    public class CustomerFieldValuesFilterModel : BaseFilterModel<int>
    {
        public int FieldId { get; set; }
        public string Value { get; set; }
    }
}
