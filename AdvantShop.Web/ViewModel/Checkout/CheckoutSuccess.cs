using AdvantShop.Orders;

namespace AdvantShop.ViewModel.Checkout
{
    public class CheckoutSuccess
    {
        public string OrderSuccessTopText { get; set; } 

        public string SuccessScript { get; set; }

        public string GoogleAnalyticsString { get; set; }

        public Order Order { get; set; }

        public string NewBonusAmount { get; set; }
    }
}