using System;
using AdvantShop.Web.Infrastructure.Controllers;
using System.Web.Mvc;
using AdvantShop.Core.Modules;
using AdvantShop.Module.SmsNotifications.Domain;
using AdvantShop.Module.SmsNotifications.Services;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Module.SmsNotifications.Controllers
{
    [AdminAuth]
    public class SmsNotificationsController : ModuleController
    {
        [HttpGet]
        public JsonResult CheckSmsConfiguration()
        {
            ESMSSenderService serviceType;
            Enum.TryParse(ModuleSettingsProvider.GetSettingValue<string>("SmsService", SmsNotifications.ModuleId), true, out serviceType);

            if (serviceType == ESMSSenderService.None)
                return Json(new { result = false, error = "Ошибка: Не выбран смс сервис" });

            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SendSms(Guid customerId, long phone,  string text)
        {
            if (customerId == Guid.Empty)
                return Json(new { result = false });

            if (phone == 0 || string.IsNullOrWhiteSpace(text))
                return Json(new { result = false });

            SmsNotificationsService.SendNow(customerId, phone, text);

            return Json(new {result = true});
        }

    }
}
