using System;
using AdvantShop.Customers;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface ICustomerChange
    {
        void Add(Customer customer);

        void Update(Customer customer);

        void Delete(Guid customerId);
    }
}
