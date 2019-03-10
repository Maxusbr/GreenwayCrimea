using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Customers;
using AdvantShop.ViewModel.Install;

namespace AdvantShop.Handlers.Install
{
    public class InstallOpenIdHandler
    {
        public InstallOpenIdModel Get()
        {
            var providers = AdvantshopConfigService.GetActivityAuthProviders();

            var model = new InstallOpenIdModel()
            {
               ShowFacebook = !providers.ContainsKey("facebook") || providers["facebook"],
               ShowVk = !providers.ContainsKey("vkontakte") || providers["vkontakte"],
               ShowMailRu = !providers.ContainsKey("mail.ru") || providers["mail.ru"],
               ShowGoogle = !providers.ContainsKey("google") || providers["google"],
               ShowYandex = !providers.ContainsKey("yandex") || providers["yandex"]
            };

            var cust = CustomerService.GetCustomersbyRole(Role.Administrator).FirstOrDefault();
            if (cust != null)
            {
                model.Login = cust.EMail;
            }

            model.Google = model.ShowGoogle && SettingsOAuth.GoogleActive;
            model.MailRu = model.ShowMailRu && SettingsOAuth.MailActive;
            model.Yandex = model.ShowYandex && SettingsOAuth.YandexActive;
            model.Vk = model.ShowVk && SettingsOAuth.VkontakteActive;
            model.Facebook = model.ShowFacebook && SettingsOAuth.FacebookActive;

            if (model.ShowGoogle)
            {
                model.GoogleClientId = SettingsOAuth.GoogleClientId;
                model.GoogleClientSecret = SettingsOAuth.GoogleClientSecret;
            }

            if (model.ShowVk)
            {
                model.VkAppId = SettingsOAuth.VkontakeClientId;
                model.VkSecret = SettingsOAuth.VkontakeSecret;
            }

            if (model.ShowFacebook)
            {
                model.FacebookClientId = SettingsOAuth.FacebookClientId;
                model.FacebookApplicationSecret = SettingsOAuth.FacebookApplicationSecret;
            }

            return model;
        }

        public void Update(InstallOpenIdModel model)
        {
            SettingsOAuth.GoogleActive = model.Google;
            SettingsOAuth.MailActive = model.MailRu;
            SettingsOAuth.YandexActive = model.Yandex;
            SettingsOAuth.VkontakteActive = model.Vk;
            SettingsOAuth.FacebookActive = model.Facebook;

            SettingsOAuth.VkontakeClientId = model.VkAppId ?? string.Empty;
            SettingsOAuth.VkontakeSecret = model.VkSecret ?? string.Empty;

            SettingsOAuth.FacebookClientId = model.FacebookClientId ?? string.Empty;
            SettingsOAuth.FacebookApplicationSecret = model.FacebookApplicationSecret ?? string.Empty;
            
            var customer = CustomerService.GetCustomerByEmail("admin") ?? CustomerService.GetCustomerByEmail(model.Login);
            if (customer == null)
            {
                CustomerService.InsertNewCustomer(new Customer(CustomerGroupService.DefaultCustomerGroup)
                {
                    Password = model.Login,
                    FirstName = "admin",
                    LastName = "admin",
                    Phone = string.Empty,
                    SubscribedForNews = false,
                    EMail = model.Login,
                    CustomerRole = Role.Administrator,
                    CustomerGroupId = 1,
                });
            }
            else
            {
                customer.EMail = model.Login;
                CustomerService.UpdateCustomer(customer);
                CustomerService.ChangePassword(customer.Id, model.Pass, false);
            }
        }
    }
}