using AdvantShop.Core.Common.Attributes;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Orders
{
    public enum OrdersPreFilterType
    {
        [Localize("Admin.Models.Orders.OrdersPreFilterType.None")]
        None,
        [Localize("Admin.Models.Orders.OrdersPreFilterType.New")]
        New,
        [Localize("Admin.Models.Orders.OrdersPreFilterType.Paid")]
        Paid,
        [Localize("Admin.Models.Orders.OrdersPreFilterType.NotPaid")]
        NotPaid,
        [Localize("Admin.Models.Orders.OrdersPreFilterType.Drafts")]
        Drafts,
    }

    public class OrdersFilterModel : BaseFilterModel
    {
        public string Filter { get; set; }
        
        public string Status { get; set; }

        public string Manager { get; set; }

        public string Number { get; set; }
        
        public int OrderStatusId { get; set; }
        
        public float PriceFrom { get; set; }

        public float PriceTo { get; set; }

        public string BuyerName { get; set; }

        public string BuyerPhone { get; set; }

        public string BuyerEmail { get; set; }

        public string BuyerCity { get; set; }
        
        public string PaymentMethod { get; set; }

        public string ShippingMethod { get; set; }

        public bool? IsPaid { get; set; }

        public bool IsDraft { get; set; }

        public string ManagerCustomerId { get; set; }

        public string OrderDateFrom { get; set; }

        public string OrderDateTo { get; set; }

        public int? OrderSourceId { get; set; }

        public OrdersPreFilterType FilterBy { get; set; }
    }
}
