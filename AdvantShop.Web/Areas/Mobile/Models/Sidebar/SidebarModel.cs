using AdvantShop.Customers;

namespace AdvantShop.Areas.Mobile.Models.Sidebar
{
    public class SidebarModel
    {
        public Customer Customer { get; set; }

        public int OrdersCount { get; set; }
    }
}