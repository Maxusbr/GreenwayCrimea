using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.ViewModel.Checkout;

namespace AdvantShop.Handlers.Checkout
{
    public class CheckoutBonusHandler
    {
        public CheckoutBonusViewModel Execute()
        {
            if (!BonusSystem.IsActive)
                return null;

            var current = MyCheckout.Factory(CustomerContext.CustomerId);
            var bonusCard = BonusSystemService.GetCard(CustomerContext.CurrentCustomer.Id);

            return new CheckoutBonusViewModel()
            {
                ApplyBonuses = current.Data.Bonus.UseIt,
                HasCard = bonusCard != null,
                BonusPlus = BonusSystemService.GetBonusCost(ShoppingCartService.CurrentShoppingCart).BonusPlus
            };
        }
    }
}