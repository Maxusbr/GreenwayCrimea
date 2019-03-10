using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Module.Roistat.Domain;
using AdvantShop.Module.Roistat.Models.Admins;
using AdvantShop.Module.Roistat.Models.RoistatSettings;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Module.Roistat.Controllers
{
    public class RoistatSettingsController : ModuleAdminController
    {
        [ChildActionOnly]
        public ActionResult Settings()
        {
            return PartialView("~/Modules/Roistat/Views/Admin/_Settings.cshtml");
        }

        [HttpGet]
        public JsonResult GetSettings()
        {
            return Json(new SettingsModel()
            {
                RoistatLogin = RoistatSettings.RoistatLogin,
                RoistatPassword = RoistatSettings.RoistatPassword,
                RoistatScript = RoistatSettings.RoistatScript,

                Email = CustomerContext.CurrentCustomer.EMail,
                OrdersUrl = UrlService.GetUrl("roistat/getorders"),
                ClientsUrl = UrlService.GetUrl("roistat/getclients"),
                
                OrderViewUrl = UrlService.GetUrl("roistat/ordersredirect/{order_id}"),
                CustomerViewUrl = UrlService.GetUrl("adminv2/customers/edit/{contact_id}"),
            });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SaveSettings(SettingsModel model)
        {
            RoistatSettings.RoistatScript = model.RoistatScript ?? "";

            RoistatSettings.RoistatLogin = model.RoistatLogin.DefaultOrEmpty();
            RoistatSettings.RoistatPassword = model.RoistatPassword.DefaultOrEmpty();
            
            return JsonOk();
        }


        [ChildActionOnly]
        public ActionResult AdminOrderInfo(int orderId)
        {
            if (orderId == 0)
                return new EmptyResult();

            var cookieValue = RoistatService.GetRoistatOrderCookie(orderId, RoistatEntityType.Order);
            
            return PartialView("~/Modules/Roistat/Views/Admin/_OrderInfo.cshtml",
                new RoistatOrderModel()
                {
                    EntityId = orderId,
                    CookieValue = cookieValue
                });
        }
        
        [ChildActionOnly]
        public ActionResult AdminLeadDescription(int leadId)
        {
            if (leadId == 0)
                return new EmptyResult();

            var cookieValue = RoistatService.GetRoistatOrderCookie(leadId, RoistatEntityType.Lead);

            return PartialView("~/Modules/Roistat/Views/Admin/_LeadDescription.cshtml",
                new RoistatOrderModel()
                {
                    EntityId = leadId,
                    CookieValue = cookieValue
                });
        }
    }
}
