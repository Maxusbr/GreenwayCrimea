using AdvantShop.Orders;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface IShoppingCartDiscount : IModule
    {
        float GetDiscount(ShoppingCart cart);
    }
}