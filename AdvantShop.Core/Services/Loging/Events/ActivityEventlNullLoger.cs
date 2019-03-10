using System;
using System.Collections.Generic;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Customers;
using AdvantShop.Orders;

namespace AdvantShop.Core.Services.Loging.Events
{
    public class ActivityEventNullLoger : IEventLoger, ICustomerAction, IOrderChanged
    {
        public virtual void AddToCart(ShoppingCartItem item, string url)
        {
        }

        public virtual void AddToCompare(ShoppingCartItem item, string url)
        {
        }

        public virtual void AddToWishList(ShoppingCartItem item, string url)
        {
        }

        public virtual void Subscribe(string email)
        {
            // nothing here
        }

        public virtual void UnSubscribe(string email)
        {
            // nothing here
        }

        public virtual void Search(string searchTerm, int resultsCount)
        {
        }

        public virtual void Register(Customer customer)
        {
            // nothing here
        }

        public virtual void ViewMyAccount(Customer customer)
        {
            // nothing here
        }

        public virtual void FilterCatalog()
        {
            // nothing here
        }

        public virtual void Login(Customer customer)
        {
            // nothing here
        }

        public virtual void Vote()
        {
            // nothing here
        }

        public virtual void LogEvent(Event @event)
        {
        }

        public virtual List<Event> GetEvents(Guid customerId)
        {
            return null;
        }

        public virtual void DoOrderAdded(IOrder order)
        {
        }

        public virtual void DoOrderChangeStatus(IOrder order)
        {
        }

        public virtual void DoOrderUpdated(IOrder order)
        {
        }

        public virtual void DoOrderDeleted(int orderId)
        {
        }

        public virtual void PayOrder(int orderId, bool pay)
        {
        }
    }
}