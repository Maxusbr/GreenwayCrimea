//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------
using AdvantShop.Customers;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface ISendCustomerNotifications
    {
        void SendOnUserRegistered(ICustomer customer);
        
        void SendOnSubscribe(ICustomer customer);

        void SendOnUnSubscribe(ICustomer customer);       
    }
}