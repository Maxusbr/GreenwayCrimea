//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Customers;
using AdvantShop.Orders;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface ICustomerAction
    {
        void AddToCart(ShoppingCartItem item, string url = "");

        void AddToCompare(ShoppingCartItem item, string url = "");

        void AddToWishList(ShoppingCartItem item, string url = "");

        void Subscribe(string email);

        void UnSubscribe(string email);

        void Search(string searchTerm, int resultsCount);

        void Register(Customer customer);

        void ViewMyAccount(Customer customer);

        void FilterCatalog();

        void Login(Customer customer);

        void Vote();
    }
}