using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.ViewModel.Checkout
{
    public class CheckoutUserViewModel
    {
        public CheckoutData Data { get; set; }
        public Customer Customer { get; set; }
        public bool IsBonusSystemActive { get; set; }
        public float BonusPlus { get; set; }

        public Currency Currency { get; set; }
    }
}