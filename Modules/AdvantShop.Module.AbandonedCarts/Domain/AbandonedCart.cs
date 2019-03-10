using System;
using AdvantShop.Orders;

namespace AdvantShop.Module.AbandonedCarts.Domain
{
    public class AbandonedCart
    {
        public Guid CustomerId { get; set; }

        public CheckoutData CheckoutData { get; set; }

        public DateTime LastUpdate { get; set; }

        public int SendingCount { get; set; }

        public DateTime? SendingDate { get; set; }
    }
}