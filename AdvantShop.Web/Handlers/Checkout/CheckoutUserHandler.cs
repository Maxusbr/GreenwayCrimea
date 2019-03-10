using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.ViewModel.Checkout;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Handlers.Checkout
{
    public class CheckoutUserHandler
    {
        public CheckoutUserViewModel Execute()
        {
            var current = MyCheckout.Factory(CustomerContext.CustomerId);

            var model = new CheckoutUserViewModel()
            {
                Customer = CustomerContext.CurrentCustomer,
                Data = current.Data,
                Currency = CurrencyService.CurrentCurrency
            };

            if (BonusSystem.IsActive)
            {
                model.IsBonusSystemActive = true;
                model.BonusPlus = BonusSystemService.GetBonusCost(ShoppingCartService.CurrentShoppingCart).BonusPlus;
            }

            return model;
        }

    }
}