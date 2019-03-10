using AdvantShop.Customers;

namespace AdvantShop.Areas.AdminMobile.Models.Sidebar
{
    public class SidebarModel
    {
        public Customer Customer { get; set; }
        public bool ShowOrders { get; set; }
        public int OrdersCount { get; set; }
        public bool ShowTasks { get; set; }
        public int TasksCount { get; set; }
        public bool ShowLeads { get; set; }
        public int LeadsCount { get; set; }
    }
}