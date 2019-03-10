using AdvantShop.Orders;

namespace AdvantShop.Web.Admin.Models.Orders
{
    public class OrderDraftModel
    {
        public int OrderId { get; set; }

        public string StatusComment { get; set; }
        public string AdminOrderComment { get; set; }
        public int OrderSourceId { get; set; }
        public int? ManagerId { get; set; }
        public string TrackNumber { get; set; }

        public OrderCustomer OrderCustomer { get; set; }
    }
}
