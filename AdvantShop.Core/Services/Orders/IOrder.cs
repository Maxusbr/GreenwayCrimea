//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;

namespace AdvantShop.Orders
{
    public interface IOrder
    {
        int OrderID { get; set; }

        string Number { get; set; }

        float Sum { get; set; }

        float ShippingCost { get; set; }

        string StatusComment { get; set; }

        bool IsDraft { get; set; }

        bool IsFromAdminArea { get; set; }

        int? LeadId { get; set; }

        List<OrderItem> OrderItems { get; set; }

        OrderCurrency OrderCurrency { get; set; }

        OrderCoupon Coupon { get; set; }

        IOrderCustomer GetOrderCustomer();

        IOrderStatus GetOrderStatus();
    }
}