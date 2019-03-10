using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.ViewModel.MyAccount;

namespace AdvantShop.Handlers.MyAccount
{
    public class CommonInfoHandler
    {
        public CommonInfoViewModel Get()
        {
            var customer = CustomerContext.CurrentCustomer;

            var model = new CommonInfoViewModel()
            {
                Email = customer.EMail,
                RegistrationDate = customer.RegistrationDateTime.ToString("dd.MM.yyyy"),
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Patronymic = customer.Patronymic,
                Phone = customer.Phone,
                ShowSubscription = SettingsDesign.NewsSubscriptionVisibility,
                SubscribedForNews = SubscriptionService.IsSubscribe(customer.EMail),
                ShowCustomerGroup = customer.CustomerGroup.CustomerGroupId != CustomerGroupService.DefaultCustomerGroup,
                CustomerGroup = customer.CustomerGroup.GroupName,
                CustomerFields = CustomerFieldService.GetCustomerFieldsWithValue(customer.Id).Where(x => x.ShowInClient).ToList()
            };

            switch (customer.CustomerRole)
            {
                case Role.User:
                    model.CustomerType = LocalizationService.GetResource("MyAccount.CommonInfo.User");
                    model.ShowCustomerRole = false;
                    break;
                case Role.Moderator:
                    model.CustomerType = LocalizationService.GetResource("MyAccount.CommonInfo.Moderator");
                    model.ShowCustomerRole = true;
                    break;
                case Role.Administrator:
                    model.CustomerType = LocalizationService.GetResource("MyAccount.CommonInfo.Administrator");
                    model.ShowCustomerRole = true;
                    break;
            }

            return model;
        }
    }
}