using System.Collections.Generic;

namespace AdvantShop.Web.Admin.ViewModels.Home
{
    public class OrdersDasboardViewModel
    {
        public List<OrderItemDasboardViewModel> OrderStatuses { get; set; } 
    }

    public class OrderItemDasboardViewModel
    {
        public int OrderStatusId { get; set; }
        public string StatusName { get; set; }
        public int OrdersCount { get; set; }
        public string Color { get; set; }
    }
}
