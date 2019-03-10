//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Orders;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface IShoppingCart : IModule
    {
        void AddToCart(ShoppingCartItem cartItem);

        void RemoveFromCart(ShoppingCartItem cartItem);

        void UpdateCart(ShoppingCartItem cartItem);

        void UpdateCart(ShoppingCart cart);

        bool ShowConfirmButtons { get; }
    }
}
