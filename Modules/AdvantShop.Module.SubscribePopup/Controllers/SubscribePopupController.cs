using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Customers;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Module.SubscribePopup.Controllers
{
    [Module(Type = "SubscribePopup")]
    public class SubscribePopupController : ModuleController
    {
        private const string CookieName = "SubscribePopup";

        private const string Script =
            "<link rel=\"stylesheet\" href=\"modules/subscribepopup/styles/styles.css\"> " +
            "<div data-oc-lazy-load=\"['modules/subscribepopup/scripts/subscribepopup.js']\">" +
                "<div data-subscribe-popup  data-modal-title=\"{0}\" data-modal-top-text=\"{1}\" data-modal-bottom-text=\"{2}\" data-modal-final-text=\"{3}\" data-start-open-delay=\"{4}\" is-show-user-agreement-text=\"{5}\" user-agreement-text=\"{6}\"></div>" +
            "</div>";

        private enum ETimeSpan
        {
            Once = 0,
            OnePerMonth = 1,
            OnePerWeek = 2,
            OnePerDay = 3,
            TwoPerDay = 4,
            OnePerHour = 5,
            ThirtyMinutes = 6,

            OnePerMinute = 7,
            OnePerFiveMinutes = 8,
            OnePerTenMinutes = 9,
            Now = 10,
            ThirtySeconds = 11,
            EveryTime = 12
        }

        public ActionResult SubscribePopupScript()
        {
            var curcookie = System.Web.HttpContext.Current.Request.Cookies.Get(CookieName);
            if (curcookie != null && curcookie.Value == "show")
                return new EmptyResult();

            var html = SubscribePopup.SettingStaticHtml;
            var timeSpan = SubscribePopup.SettingTimeSpan;

            var delay = SubscribePopup.SettingDelayShowPopup;

            var showInMainPage = SubscribePopup.SettingShowOnMain;
            var showInDetails = SubscribePopup.SettingShowInDetails;
            var showInOtherPages = SubscribePopup.SettingShowInOtherPages;

            var controllerName = Request.RequestContext.RouteData.Values["controller"].ToString();

            if (controllerName == "Home" && !showInMainPage)
            {
                return new EmptyResult();
            }

            if (controllerName == "Product" && !showInDetails)
            {
                return new EmptyResult();
            }
            if (controllerName != "Home" && controllerName != "Product" && !showInOtherPages)
            {
                return new EmptyResult();
            }

            if (string.IsNullOrEmpty(html))
                return new EmptyResult();

            byte[] hash = null;
            using (HashAlgorithm hashAlg = new SHA1Managed())
            {
                hash = hashAlg.ComputeHash(Encoding.UTF8.GetBytes(html + timeSpan + delay + "show"));

                if (curcookie != null && BitConverter.ToString(hash) == curcookie.Value && (ETimeSpan)timeSpan != ETimeSpan.EveryTime)
                {
                    return new EmptyResult();
                }
            }

            var delayMilliseconds = 0;
            switch ((ETimeSpan)delay)
            {
                case ETimeSpan.Now:
                    delayMilliseconds = 0;
                    break;
                case ETimeSpan.ThirtySeconds:
                    delayMilliseconds = 30000;
                    break;
                case ETimeSpan.OnePerMinute:
                    delayMilliseconds = 60000;
                    break;
                case ETimeSpan.OnePerFiveMinutes:
                    delayMilliseconds = 300000;
                    break;
                case ETimeSpan.OnePerTenMinutes:
                    delayMilliseconds = 600000;
                    break;
            }
            var cookie = new HttpCookie(CookieName, BitConverter.ToString(hash)) { HttpOnly = true };

            switch ((ETimeSpan)timeSpan)
            {
                case ETimeSpan.Once:
                    cookie.Expires = DateTime.Now.AddYears(10);
                    break;
                case ETimeSpan.OnePerMonth:
                    cookie.Expires = DateTime.Now.AddMonths(1);
                    break;
                case ETimeSpan.OnePerWeek:
                    cookie.Expires = DateTime.Now.AddDays(7);
                    break;
                case ETimeSpan.OnePerDay:
                    cookie.Expires = DateTime.Now.AddDays(1);
                    break;
                case ETimeSpan.TwoPerDay:
                    cookie.Expires = DateTime.Now.AddHours(6);
                    break;
                case ETimeSpan.OnePerHour:
                    cookie.Expires = DateTime.Now.AddHours(1);
                    break;
                case ETimeSpan.ThirtyMinutes:
                    cookie.Expires = DateTime.Now.AddMinutes(30);
                    break;
            }

            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            return Content(
                string.Format(Script,
                    SubscribePopup.SettingPopupTitle,
                    SubscribePopup.SettingPopupTopHtml.Replace("\"", "'"),
                    SubscribePopup.SettingPopupBottomHtml.Replace("\"", "'"),
                    SubscribePopup.SettingPopupFinalHtml.Replace("\"", "'"),
                    delayMilliseconds, 
                    SettingsCheckout.IsShowUserAgreementText,
                    SettingsCheckout.UserAgreementText));
        }

        [HttpPost]
        public ActionResult Subscribe(string email)
        {
            SubscriptionService.Subscribe(email);

            System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie(CookieName, "show")
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddMonths(3)
            });

            if (SubscribePopup.SettingNotifyAdmin && CustomerContext.CurrentCustomer != null)
            {
                Mails.SendMail.SendMailNow(CustomerContext.CustomerId, SettingsMail.EmailForRegReport,
                    "Подписка на новости", string.Format("Пользователь {0} подписался на новости", email), true);
            }
            return new EmptyResult();
        }
    }
}
