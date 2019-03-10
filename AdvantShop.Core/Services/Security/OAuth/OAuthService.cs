//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.Mails;

namespace AdvantShop.Security.OpenAuth
{
    public class OAuthService
    {
        public static void AuthOrRegCustomer(Customer customer)
        {
            AuthOrRegCustomer(customer, customer.EMail);
        }

        public static void AuthOrRegCustomer(Customer customer, string identifier)
        {
            if (!CustomerService.IsExistOpenIdLinkCustomer(identifier))
            {
                if (customer.EMail == null)
                {
                    customer.EMail = Guid.NewGuid().ToString() + "@temp.adv";
                }

                if (!CustomerService.CheckCustomerExist(customer.EMail))
                {

                    CustomerService.InsertNewCustomer(customer);
                    customer = CustomerService.GetCustomerByEmail(customer.EMail);

                    var registrationMail = new RegistrationMailTemplate(SettingsMain.SiteUrl, customer.FirstName,
                                                                        customer.LastName,
                                                                        Localization.Culture.ConvertDate(DateTime.Now),
                                                                        customer.Password,
                                                                        customer.SubscribedForNews
                                                                                ? LocalizationService.GetResource("User.Registration.Yes")
                                                                                : LocalizationService.GetResource("User.Registration.No"),
                                                                        customer.EMail, 
                                                                        customer.Phone,
                                                                        SettingsCheckout.IsShowPatronymic ? customer.Patronymic.ToString() : string.Empty);
                    registrationMail.BuildMail();

                    SendMail.SendMailNow(Guid.Empty, SettingsMail.EmailForRegReport, registrationMail.Subject, registrationMail.Body, true);
                }
                else
                {
                    customer = CustomerService.GetCustomerByEmail(customer.EMail);
                }
                CustomerService.AddOpenIdLinkCustomer(customer.Id, identifier);
            }
            else
            {
                customer = CustomerService.GetCustomerByOpenAuthIdentifier(identifier);
            }

            AuthorizeService.SignIn(customer.EMail, customer.Password, true, true);
        }
    }
}