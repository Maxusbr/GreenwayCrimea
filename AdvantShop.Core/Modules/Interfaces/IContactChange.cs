using System;
using AdvantShop.Customers;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface IContactChange
    {
        void Add(CustomerContact contact);

        void Update(CustomerContact contact);

        void Delete(Guid contactId);
    }
}
