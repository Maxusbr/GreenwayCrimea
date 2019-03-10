using AdvantShop.Orders;

namespace AdvantShop.ViewModel.Checkout
{
    public class CheckoutShippingAddressViewModel
    {
        public CheckoutAddress AddressContact { get; set; }

        public bool HasAddresses { get; set; }
        public bool HasCustomShippingFields { get; set; }
    }
}