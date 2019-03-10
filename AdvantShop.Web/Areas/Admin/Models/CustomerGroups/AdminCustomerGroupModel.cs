using AdvantShop.Customers;

namespace AdvantShop.Web.Admin.Models.CustomerGroups
{
    public partial class AdminCustomerGroupModel
    {
        public int CustomerGroupId { get; set; }
        public string GroupName { get; set; }
        public float GroupDiscount { get; set; }
        public float MinimumOrderPrice { get; set; }

        public bool CanBeDeleted
        {
            get { return CustomerGroupId != CustomerGroupService.DefaultCustomerGroup; }
        }
    }
}
