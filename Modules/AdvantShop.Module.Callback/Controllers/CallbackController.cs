using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Module.Callback.Services;
using AdvantShop.Saas;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Orders;

namespace AdvantShop.Module.Callback.Controllers
{
    public class CallbackController : ModuleController
    {
        private readonly List<string> _js = new List<string>()
        {
            "'modules/callback/scripts/callback.js'"
        };

        public ActionResult GetCallback()
        {
            return
                Content("<div data-oc-lazy-load=\"[" + _js.AggregateString(",") + "]\"><div data-callback-start></div></div>");
        }

        public ActionResult GetParams()
        {
            return Json(new
            {
                Title = ModuleSettingsProvider.GetSettingValue<string>("windowTitle", Callback.ModuleStringId),
                ModalText = ModuleSettingsProvider.GetSettingValue<string>("windowText", Callback.ModuleStringId),
                ShowCommentField = ModuleSettingsProvider.GetSettingValue<bool>("showcommentfield", Callback.ModuleStringId),
                SettingsCheckout.IsShowUserAgreementText,
                SettingsCheckout.UserAgreementText
            });
        }

        public ActionResult AddCallback(string name, string phone, string comment)
        {
            if (String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(phone))
                return Json(false);

            var callbackCustomer = new CallbackCustomer
            {
                Name = HttpUtility.HtmlEncode(name),
                Phone = HttpUtility.HtmlEncode(phone),
                Comment = HttpUtility.HtmlEncode(comment)
            };

            var orderSource = OrderSourceService.GetOrderSource(OrderType.Callback);

            if (ModuleSettingsProvider.GetSettingValue<bool>("createLead", Callback.ModuleStringId) &&
                ((SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveCrm) || (!SaasDataService.IsSaasEnabled)))
            {
                LeadService.AddLead(new Lead
                {
                    FirstName = callbackCustomer.Name,
                    Comment = callbackCustomer.Comment,
                    Phone = callbackCustomer.Phone,
                    OrderSourceId = orderSource.Id,
                    Customer = new Customer(CustomerGroupService.DefaultCustomerGroup)
                    {
                        FirstName = callbackCustomer.Name,
                        Phone = callbackCustomer.Phone,
                        StandardPhone = StringHelper.ConvertToStandardPhone(callbackCustomer.Phone),
                        CustomerRole = Role.User
                    }
                }, true);
            }
            else
            {
                CallbackRepository.AddCallbackCustomer(callbackCustomer);
            }

            CallbackRepository.SendEmail(callbackCustomer);

            return Json(true);
        }
    }
}
