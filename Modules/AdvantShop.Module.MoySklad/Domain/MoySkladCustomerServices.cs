using System;
using System.Linq;
using AdvantShop.Customers;

namespace AdvantShop.Module.MoySklad.Domain
{
    public class MoySkladCustomerServices
    {
        private static readonly object Sync = new object();

        public static void AddOrUpdateCustomer(Customer customer)
        {
            if (customer.Id == Guid.Empty)
                return;

            lock (Sync)
            {
                var customerModel = new EntityCounterparty()
                {
                    Name = MoySklad.JoinNoEmpty(" ", customer.LastName, customer.FirstName, customer.Patronymic),
                    Email = customer.EMail,
                    Phone = customer.Phone,
                    Mobile = customer.StandardPhone.ToString(),
                    Description = customer.AdminComment,
                };

                var msId = MoySkladRestApiService.GetCounterpartyId(MoySklad.GuidToString(customer.Id));

                if (string.IsNullOrEmpty(msId))
                {
                    //add
                    customerModel.ExternalCode = MoySklad.GuidToString(customer.Id);
                    MoySkladApiService.AddCounterparty(customerModel);
                }
                else
                {
                    //update
                    customerModel.Id = msId;
                    MoySkladApiService.UpdateCounterparty(customerModel);
                }
            }
        }

        public static void AddOrUpdateCustomerContact(CustomerContact contact)
        {
            if (contact.CustomerGuid == Guid.Empty)
                return;

            lock (Sync)
            {
                var contactModel = new EntityCounterpartyContactperson()
                {
                    Name = contact.Name,
                    Description = CustomerService.ConvertToLinedAddress(contact)
                };

                var msCustomerId = MoySkladRestApiService.GetCounterpartyId(MoySklad.GuidToString(contact.CustomerGuid));

                if (string.IsNullOrEmpty(msCustomerId))
                {
                    AddOrUpdateCustomer(CustomerService.GetCustomer(contact.CustomerGuid));
                    msCustomerId = MoySkladRestApiService.GetCounterpartyId(MoySklad.GuidToString(contact.CustomerGuid));
                }

                var contactMS =
                    MoySkladApiService.GetEnumeratorCounterpartyContactpersons(msCustomerId, true)
                        .FirstOrDefault(c => c.ExternalCode.Equals(MoySklad.GuidToString(contact.ContactId), StringComparison.InvariantCultureIgnoreCase));

                if (contactMS == null)
                {
                    //add
                    contactModel.ExternalCode = MoySklad.GuidToString(contact.ContactId);
                    MoySkladApiService.AddCounterpartyContactperson(contactModel, msCustomerId);
                }
                else
                {
                    //update
                    contactModel.Id = contactMS.Id;
                    MoySkladApiService.UpdateCounterpartyContactperson(contactModel, msCustomerId);
                }
            }
        }
    }
}
