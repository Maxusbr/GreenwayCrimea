using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Models.MyAccount;
using AdvantShop.Repository;

namespace AdvantShop.Handlers.MyAccount
{
    public class AddUpdateContactHandler
    {
        private bool IsValidCustomerContactModel(CustomerAccountModel account)
        {
            var valid = true;

            if (SettingsCheckout.IsShowCountry && SettingsCheckout.IsRequiredCountry)
            {
                valid &= account.CountryId != 0;
                valid &= account.Country.IsNotEmpty();
            }

            if (SettingsCheckout.IsShowState && SettingsCheckout.IsRequiredState)
            {
                valid &= account.Region.IsNotEmpty();
            }

            if (SettingsCheckout.IsShowCity && SettingsCheckout.IsRequiredCity)
            {
                valid &= account.City.IsNotEmpty();
            }

            if (SettingsCheckout.IsShowAddress && SettingsCheckout.IsRequiredAddress)
            {
                valid &= account.Street.IsNotEmpty();
            }

            return valid;
        }

        public CustomerContact Execute(CustomerAccountModel account)
        {
            if (!IsValidCustomerContactModel(account) || !CustomerContext.CurrentCustomer.RegistredUser)
                return null;

            var ipZone = IpZoneContext.CurrentZone;

            var contact = account.ContactId.IsNullOrEmpty()
                                ? new CustomerContact()
                                : CustomerService.GetCustomerContact(account.ContactId);

            contact.Name = account.Fio;
            contact.City = account.City.IsNotEmpty() ? account.City : ipZone.City;
            contact.Zip = account.Zip ?? string.Empty;

            var country = CountryService.GetCountry(account.CountryId);
            contact.CountryId = country != null ? country.CountryId : ipZone.CountryId;
            contact.Country = country != null ? country.Name : ipZone.CountryName;

            contact.Street = account.Street ?? string.Empty;
            contact.House = account.House ?? string.Empty;
            contact.Apartment = account.Apartment ?? string.Empty;
            contact.Structure = account.Structure ?? string.Empty;
            contact.Entrance = account.Entrance ?? string.Empty;
            contact.Floor = account.Floor ?? string.Empty;

            if (!string.IsNullOrEmpty(account.Region))
            {
                var regionId = RegionService.GetRegionIdByName(HttpUtility.HtmlDecode(account.Region));
                contact.RegionId = regionId != 0 ? regionId : ipZone.RegionId;
                contact.Region = account.Region.IsNotEmpty() ? HttpUtility.HtmlDecode(account.Region) : ipZone.Region;
            }

            if (account.ContactId.IsNullOrEmpty())
            {
                CustomerService.AddContact(contact, CustomerContext.CurrentCustomer.Id);
            }
            else
            {
                CustomerService.UpdateContact(contact);
            }

            return contact;
        }

    }
}