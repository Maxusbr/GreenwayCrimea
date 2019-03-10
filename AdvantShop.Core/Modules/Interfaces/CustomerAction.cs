//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Customers;
using AdvantShop.Orders;

namespace AdvantShop.Core.Modules.Interfaces
{
    public abstract class CustomerAction : ICustomerAction
    {
        public virtual void AddToCart(ShoppingCartItem item, string url = "")
        {
            return;
        }

        public virtual void AddToCompare(ShoppingCartItem item, string url = "")
        {
            return;
        }

        public virtual void AddToWishList(ShoppingCartItem item, string url = "")
        {
            return;
        }

        public virtual void Subscribe(string email)
        {
            return;
        }

        public virtual void UnSubscribe(string email)
        {
            return;
        }

        public virtual void Search(string searchTerm, int resultsCount)
        {
            return;
        }

        public virtual void Register(Customer customer)
        {
            return;
        }

        public virtual void ViewMyAccount(Customer customer)
        {
            return;
        }

        public virtual void FilterCatalog()
        {
            return;
        }

        public virtual void Login(Customer customer)
        {
            return;
        }

        public virtual void Vote()
        {
            return;
        }
    }
}