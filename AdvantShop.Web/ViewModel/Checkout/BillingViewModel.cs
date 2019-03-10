using AdvantShop.Customers;
using AdvantShop.Orders;

namespace AdvantShop.ViewModel.Checkout
{
    public class BillingViewModel
    {
        public Order Order { get; set; }

        public bool IsMobile { get; set; }

        public string Header { get; set; }
    }
}