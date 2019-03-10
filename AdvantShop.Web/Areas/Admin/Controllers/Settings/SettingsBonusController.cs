using System;
using System.Web.Mvc;
using AdvantShop.Core;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Bonuses.Sms.Gateways;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Controllers;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.BonusSystem)]
    public class SettingsBonusController : BaseAdminController
    {
        public ActionResult Index()
        {
            var model = new BonusSettingsModel()
            {
                IsActive = BonusSystem.IsActive,
                BonusGradeId = BonusSystem.DefaultGrade,
                CardNumFrom = BonusSystem.CardFrom,
                CardNumTo = BonusSystem.CardTo,
                SmsEnabled = BonusSystem.SmsEnabled,
                SmsTitle = BonusSystem.SmsTitle,
                //SmsLogin = BonusSystem.SmsLogin,
                //SmsPassword = BonusSystem.SmsPassword,
                SmsProviderType = BonusSystem.SmsProviderType,
                MaxOrderPercent = BonusSystem.MaxOrderPercent,
                BonusType = BonusSystem.BonusType,
                BonusTextBlock = BonusSystem.BonusTextBlock,
                BonusRightTextBlock = BonusSystem.BonusRightTextBlock,

                Sms4BLogin = BonusSystem.Sms4BLogin,
                Sms4BPassword = BonusSystem.Sms4BPassword,
                StreamSmsLogin = BonusSystem.StreamSmsLogin,
                StreamSmsPassword = BonusSystem.StreamSmsPassword,
                EPochtaPublicKey = BonusSystem.EPochtaPublicKey,
                EPochtaPrivateKey = BonusSystem.EPochtaPrivateKey,
                UniSenderApiKey = BonusSystem.UniSenderApiKey,

                ForbidOnCoupon = BonusSystem.ForbidOnCoupon
            };


            var item = model.Grades.Find(x => x.Value == model.BonusGradeId.ToString());
            if (item != null)
                item.Selected = true;

            SetMetaInformation(T("Admin.Settings.Bonus.Title"));
            SetNgController(NgControllers.NgControllersTypes.SettingsBonusCtrl);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(BonusSettingsModel model)
        {
            if (ModelState.IsValid)
            {
                BonusSystem.IsActive = model.IsActive;
                BonusSystem.DefaultGrade = model.BonusGradeId;
                BonusSystem.CardFrom = model.CardNumFrom;
                BonusSystem.CardTo = model.CardNumTo;
                BonusSystem.SmsEnabled = model.SmsEnabled;
                BonusSystem.SmsTitle = model.SmsTitle;
                BonusSystem.SmsProviderType = model.SmsProviderType;
                BonusSystem.MaxOrderPercent = model.MaxOrderPercent;
                BonusSystem.BonusType = model.BonusType;

                BonusSystem.BonusTextBlock = model.BonusTextBlock;
                BonusSystem.BonusRightTextBlock = model.BonusRightTextBlock;

                BonusSystem.Sms4BLogin = model.Sms4BLogin;
                BonusSystem.Sms4BPassword = model.Sms4BPassword;
                BonusSystem.StreamSmsLogin = model.StreamSmsLogin;
                BonusSystem.StreamSmsPassword = model.StreamSmsPassword;
                BonusSystem.EPochtaPublicKey = model.EPochtaPublicKey;
                BonusSystem.EPochtaPrivateKey = model.EPochtaPrivateKey;
                BonusSystem.UniSenderApiKey = model.UniSenderApiKey;
                BonusSystem.ForbidOnCoupon = model.ForbidOnCoupon;

                ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));
            }
            else
            {
                ShowErrorMessages();
            }

            return Index();
        }

        [HttpPost]
        public JsonResult UniSenderRegister(string email, string login, string password)
        {
            if (email.IsNullOrEmpty() || login.IsNullOrEmpty() || password.IsNullOrEmpty())
                return JsonError("Введите данные");
            if (!ValidationHelper.IsValidEmail(email))
                return JsonError("Некорректный e-mail");
            try
            {
                var uniSender = new UniSenderGateway();
                var apiKey = uniSender.Register(email, login, password);
                if (apiKey.IsNotEmpty())
                    BonusSystem.UniSenderApiKey = apiKey;
                return JsonOk(new { apiKey });
            }
            catch (BlException ex)
            {
                return JsonError(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return JsonError("При регистрации произошла ошибка: " + ex.Message);
            }
        }
    }
}
