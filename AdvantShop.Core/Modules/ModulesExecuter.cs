//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Drawing;
using System.Linq;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Customers;
using AdvantShop.Orders;

namespace AdvantShop.Core.Modules
{

    public class ModulesExecuter
    {
        #region PictureModules

        public static void ProcessPhoto(Image image)
        {
            foreach (var cls in AttachedModules.GetModules<IProcessPhoto>())
            {
                var classInstance = (IProcessPhoto)Activator.CreateInstance(cls, null);
                classInstance.DoProcessPhoto(image);
            }
        }

        #endregion

        #region OrderModules

        public static void OrderAdded(IOrder order)
        {
            var modules = AttachedModules.GetModules<IOrderChanged>();
            foreach (var cls in modules)
            {
                var classInstance = (IOrderChanged)Activator.CreateInstance(cls, null);
                classInstance.DoOrderAdded(order);
            }
        }

        public static void OrderChangeStatus(int orderId)
        {
            IOrder order = null;

            var modules = AttachedModules.GetModules<IOrderChanged>();

            if (modules.Count > 0)
                order = OrderService.GetOrder(orderId);

            foreach (var cls in modules)
            {
                var classInstance = (IOrderChanged)Activator.CreateInstance(cls, null);
                classInstance.DoOrderChangeStatus(order);
            }
        }

        public static void OrderUpdated(IOrder order)
        {
            var modules = AttachedModules.GetModules<IOrderChanged>();
            foreach (var cls in modules)
            {
                var classInstance = (IOrderChanged)Activator.CreateInstance(cls, null);
                classInstance.DoOrderUpdated(order);
            }
        }

        public static void OrderDeleted(int orderId)
        {
            foreach (var cls in AttachedModules.GetModules<IOrderChanged>())
            {
                var classInstance = (IOrderChanged)Activator.CreateInstance(cls, null);
                classInstance.DoOrderDeleted(orderId);
            }
        }


        public static void PayOrder(int orderId, bool payed)
        {
            foreach (var cls in AttachedModules.GetModules<IOrderChanged>())
            {
                var classInstance = (IOrderChanged)Activator.CreateInstance(cls, null);
                classInstance.PayOrder(orderId, payed);
            }
        }


        #endregion

        #region NotificationModules

        public static void SendSms(Guid customerid, long phoneNumber, string text)
        {
            foreach (var cls in AttachedModules.GetModules<IModuleSms>())
            {
                var classInstance = (IModuleSms)Activator.CreateInstance(cls, null);
                classInstance.SendSms(customerid, phoneNumber, text);
            }
        }

        #endregion

        #region CustomerActions

        public static void AddToCart(ShoppingCartItem item, string url = "")
        {
            foreach (var cls in AttachedModules.GetModules<ICustomerAction>().Union(AttachedModules.GetCore<ICustomerAction>()))
            {
                var classInstance = (ICustomerAction)Activator.CreateInstance(cls, null);
                classInstance.AddToCart(item, url);
            }
        }
        public static void AddToCompare(ShoppingCartItem item, string url = "")
        {
            foreach (var cls in AttachedModules.GetModules<ICustomerAction>().Union(AttachedModules.GetCore<ICustomerAction>()))
            {
                var classInstance = (ICustomerAction)Activator.CreateInstance(cls, null);
                classInstance.AddToCompare(item, url);
            }
        }
        public static void AddToWishList(ShoppingCartItem item, string url = "")
        {
            foreach (var cls in AttachedModules.GetModules<ICustomerAction>().Union(AttachedModules.GetCore<ICustomerAction>()))
            {
                var classInstance = (ICustomerAction)Activator.CreateInstance(cls, null);
                classInstance.AddToWishList(item, url);
            }
        }
        public static void Subscribe(Subscription subscription)
        {
            foreach (var cls in AttachedModules.GetModules<ICustomerAction>().Union(AttachedModules.GetCore<ICustomerAction>()))
            {
                var classInstance = (ICustomerAction)Activator.CreateInstance(cls, null);
                classInstance.Subscribe(subscription.Email);
            }

            var modules = AttachedModules.GetModules<ISendMails>();
            foreach (var moduleType in modules)
            {
                var moduleObject = (ISendMails)Activator.CreateInstance(moduleType, null);
                moduleObject.SubscribeEmail(subscription);
            }
        }

        public static void UnSubscribe(string email)
        {
            foreach (var cls in AttachedModules.GetModules<ICustomerAction>().Union(AttachedModules.GetCore<ICustomerAction>()))
            {
                var classInstance = (ICustomerAction)Activator.CreateInstance(cls, null);
                classInstance.UnSubscribe(email);
            }

            var modules = AttachedModules.GetModules<ISendMails>();
            foreach (var moduleType in modules)
            {
                var moduleObject = (ISendMails)Activator.CreateInstance(moduleType, null);
                moduleObject.UnsubscribeEmail(email);
            }

        }

        public static void Search(string searchTerm, int resultsCount)
        {
            foreach (var cls in AttachedModules.GetModules<ICustomerAction>().Union(AttachedModules.GetCore<ICustomerAction>()))
            {
                var classInstance = (ICustomerAction)Activator.CreateInstance(cls, null);
                classInstance.Search(searchTerm, resultsCount);
            }
        }

        public static void Registration(Customer customer)
        {
            foreach (var cls in AttachedModules.GetModules<ICustomerAction>().Union(AttachedModules.GetCore<ICustomerAction>()))
            {
                var classInstance = (ICustomerAction)Activator.CreateInstance(cls, null);
                classInstance.Register(customer);
            }
        }

        public static void Login(Customer customer)
        {
            foreach (var cls in AttachedModules.GetModules<ICustomerAction>())
            {
                var classInstance = (ICustomerAction)Activator.CreateInstance(cls, null);
                classInstance.Login(customer);
            }
        }

        public static void ViewMyAccount(Customer customer)
        {
            foreach (var cls in AttachedModules.GetModules<ICustomerAction>().Union(AttachedModules.GetCore<ICustomerAction>()))
            {
                var classInstance = (ICustomerAction)Activator.CreateInstance(cls, null);
                classInstance.ViewMyAccount(customer);
            }
        }

        public static void FilterCatalog()
        {
            foreach (var cls in AttachedModules.GetModules<ICustomerAction>().Union(AttachedModules.GetCore<ICustomerAction>()))
            {
                var classInstance = (ICustomerAction)Activator.CreateInstance(cls, null);
                classInstance.FilterCatalog();
            }
        }

        public static void Vote()
        {
            foreach (var cls in AttachedModules.GetModules<ICustomerAction>().Union(AttachedModules.GetCore<ICustomerAction>()))
            {
                var classInstance = (ICustomerAction)Activator.CreateInstance(cls, null);
                classInstance.Vote();
            }
        }
        #endregion

        #region ISendOrderNotifications

        public static void SendNotificationsOnOrderAdded(IOrder order)
        {
            var modules = AttachedModules.GetModules<ISendOrderNotifications>();
            foreach (var cls in modules)
            {
                var classInstance = (ISendOrderNotifications)Activator.CreateInstance(cls, null);
                classInstance.SendOnOrderAdded(order);
            }
        }

        public static void SendNotificationsOnOrderChangeStatus(IOrder order)
        {
            var modules = AttachedModules.GetModules<ISendOrderNotifications>();
            foreach (var cls in modules)
            {
                var classInstance = (ISendOrderNotifications)Activator.CreateInstance(cls, null);
                classInstance.SendOnOrderChangeStatus(order);
            }
        }


        public static bool SendNotificationsHasTemplatesOnChangeStatus(int orderStatusId)
        {
            bool res = false;
            var modules = AttachedModules.GetModules<ISendOrderNotifications>();
            foreach (var cls in modules)
            {
                var classInstance = (ISendOrderNotifications)Activator.CreateInstance(cls, null);
                res |= classInstance.HaveSmsTemplate(orderStatusId);
            }

            return res;
        }


        public static void SendNotificationsOnOrderUpdated(IOrder order)
        {
            var modules = AttachedModules.GetModules<ISendOrderNotifications>();
            foreach (var cls in modules)
            {
                var classInstance = (ISendOrderNotifications)Activator.CreateInstance(cls, null);
                classInstance.SendOnOrderUpdated(order);
            }
        }

        public static void SendNotificationsOnOrderDeleted(int orderId)
        {
            var modules = AttachedModules.GetModules<ISendOrderNotifications>();
            foreach (var cls in modules)
            {
                var classInstance = (ISendOrderNotifications)Activator.CreateInstance(cls, null);
                classInstance.SendOnOrderDeleted(orderId);
            }
        }

        public static void SendNotificationsOnPayOrder(int orderId, bool payed)
        {
            var modules = AttachedModules.GetModules<ISendOrderNotifications>();
            foreach (var cls in modules)
            {
                var classInstance = (ISendOrderNotifications)Activator.CreateInstance(cls, null);
                classInstance.SendOnPayOrder(orderId, payed);
            }
        }

        #endregion

        #region ICustomerChange

        public static void AddCustomer(Customer customer)
        {
            foreach (var cls in AttachedModules.GetModules<ICustomerChange>().Union(AttachedModules.GetCore<ICustomerChange>()))
            {
                var classInstance = (ICustomerChange)Activator.CreateInstance(cls, null);
                classInstance.Add(customer);
            }
        }

        public static void UpdateCustomer(Customer customer)
        {
            foreach (var cls in AttachedModules.GetModules<ICustomerChange>().Union(AttachedModules.GetCore<ICustomerChange>()))
            {
                var classInstance = (ICustomerChange)Activator.CreateInstance(cls, null);
                classInstance.Update(customer);
            }
        }

        public static void UpdateCustomer(Guid customerId)
        {
            Customer customer = null;
            foreach (var cls in AttachedModules.GetModules<ICustomerChange>().Union(AttachedModules.GetCore<ICustomerChange>()))
            {
                var classInstance = (ICustomerChange)Activator.CreateInstance(cls, null);
                classInstance.Update(customer ?? (customer = CustomerService.GetCustomer(customerId)));
            }
        }

        public static void DeleteCustomer(Guid customerId)
        {
            foreach (var cls in AttachedModules.GetModules<ICustomerChange>().Union(AttachedModules.GetCore<ICustomerChange>()))
            {
                var classInstance = (ICustomerChange)Activator.CreateInstance(cls, null);
                classInstance.Delete(customerId);
            }
        }

        #endregion

        #region IContactChange

        public static void AddContact(CustomerContact contact)
        {
            foreach (var cls in AttachedModules.GetModules<IContactChange>().Union(AttachedModules.GetCore<IContactChange>()))
            {
                var classInstance = (IContactChange)Activator.CreateInstance(cls, null);
                classInstance.Add(contact);
            }
        }

        public static void UpdateContact(CustomerContact contact)
        {
            foreach (var cls in AttachedModules.GetModules<IContactChange>().Union(AttachedModules.GetCore<IContactChange>()))
            {
                var classInstance = (IContactChange)Activator.CreateInstance(cls, null);
                classInstance.Update(contact);
            }
        }

        public static void DeleteContact(Guid contactId)
        {
            foreach (var cls in AttachedModules.GetModules<IContactChange>().Union(AttachedModules.GetCore<IContactChange>()))
            {
                var classInstance = (IContactChange)Activator.CreateInstance(cls, null);
                classInstance.Delete(contactId);
            }
        }

        #endregion

        #region Lead

        public static void LeadAdded(Lead lead)
        {
            var modules = AttachedModules.GetModules<ILeadChanged>();
            foreach (var cls in modules)
            {
                var classInstance = (ILeadChanged)Activator.CreateInstance(cls, null);
                classInstance.LeadAdded(lead);
            }
        }

        public static void LeadUpdated(Lead lead)
        {
            var modules = AttachedModules.GetModules<ILeadChanged>();
            foreach (var cls in modules)
            {
                var classInstance = (ILeadChanged)Activator.CreateInstance(cls, null);
                classInstance.LeadUpdated(lead);
            }
        }

        public static void LeadDeleted(int leadId)
        {
            var modules = AttachedModules.GetModules<ILeadChanged>();
            foreach (var cls in modules)
            {
                var classInstance = (ILeadChanged)Activator.CreateInstance(cls, null);
                classInstance.LeadDeleted(leadId);
            }
        }

        #endregion
    }
}