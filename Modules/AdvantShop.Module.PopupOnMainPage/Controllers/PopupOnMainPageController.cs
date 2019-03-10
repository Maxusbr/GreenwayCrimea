using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Helpers;
using AdvantShop.Core.Modules;
using AdvantShop.Web.Infrastructure.Controllers;

namespace AdvantShop.Module.PopupOnMainPage.Controllers
{
    public class PopupOnMainPageController : ModuleController
    {
        private const string ModuleId = "popuponmainpage";
        private const string CookieName = "advantshopModalPopup";

        private enum ETimeSpan
        {
            oneYear = 0,
            oneMinute = 1,
            tenMinute = 2,
            thirtyMinute = 3,
            oneDay = 4,
            oneMounth = 5,
            never = 6,
            fiveMinute = 7,
            now = 8,
            thirtySeconds = 9,
            everyTime = 10
        }

        public ActionResult PopupOnMainPageScript()
        {
            var html = ModuleSettingsProvider.GetSettingValue<string>("PopupOnMainPageHtml", ModuleId);
            var showInMobile = ModuleSettingsProvider.GetSettingValue<bool>("ShowInMobile", PopupOnMainPage.ModuleID);

            if (string.IsNullOrEmpty(html) || (showInMobile == false && MobileHelper.IsMobileEnabled()))
                return new EmptyResult();

            var titleCache = ModuleSettingsProvider.GetSettingValue<string>("PopupOnMainPageTitle", ModuleId);
            var title = string.IsNullOrEmpty(titleCache) ? "" : "<div class=\"modal-header\">" + titleCache + "</div>";

            var timeSpan = ModuleSettingsProvider.GetSettingValue<int>("PopupOnMainPageTimeSpan", ModuleId);

            var delay = ModuleSettingsProvider.GetSettingValue<int>("DelayShowPopup", ModuleId);

            var showInMainPage = ModuleSettingsProvider.GetSettingValue<bool>("ShowOnMain", ModuleId);
            var showInDetails = ModuleSettingsProvider.GetSettingValue<bool>("ShowInDetails", ModuleId);
            var showInOtherPages = ModuleSettingsProvider.GetSettingValue<bool>("ShowInOtherPages", ModuleId);
			var blocksBackground = ModuleSettingsProvider.GetSettingValue<bool>("BlocksBackground", PopupOnMainPage.ModuleID);

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
            
            byte[] hash = null;
            using (HashAlgorithm hashAlg = new SHA1Managed())
            {
                hash = hashAlg.ComputeHash(Encoding.UTF8.GetBytes(title + html + timeSpan + delay));
                var cookies = System.Web.HttpContext.Current.Request.Cookies.Get(CookieName);

                if (cookies != null &&
                    BitConverter.ToString(hash) == cookies.Value &&
                    (ETimeSpan)timeSpan != ETimeSpan.everyTime)
                {
                    return new EmptyResult();
                }
            }


            var delayMilliseconds = 0;
            switch ((ETimeSpan)delay)
            {
                case ETimeSpan.now:
                    delayMilliseconds = 0;
                    break;
                case ETimeSpan.thirtySeconds:
                    delayMilliseconds = 30000;
                    break;
                case ETimeSpan.oneMinute:
                    delayMilliseconds = 60000;
                    break;
                case ETimeSpan.fiveMinute:
                    delayMilliseconds = 300000;
                    break;
                case ETimeSpan.tenMinute:
                    delayMilliseconds = 600000;
                    break;
            }

            ////todo: отвязть от движкового modalPopup или сделать прослойку
			const string script = "<div id=\"popupOnMainPage\" data-modal-control data-modal-cross-enable=\"true\" {3} start-open-delay=\"{2}\">{0}<div class=\"modal-content\">{1}</div></div>";
            var cookie = new HttpCookie(CookieName, BitConverter.ToString(hash)) { HttpOnly = true };

            switch ((ETimeSpan)timeSpan)
            {
                case ETimeSpan.oneYear:
                    cookie.Expires = DateTime.Now.AddYears(1);
                    break;
                case ETimeSpan.oneMinute:
                    cookie.Expires = DateTime.Now.AddMinutes(1);
                    break;
                case ETimeSpan.tenMinute:
                    cookie.Expires = DateTime.Now.AddMinutes(10);
                    break;
                case ETimeSpan.thirtyMinute:
                    cookie.Expires = DateTime.Now.AddMinutes(30);
                    break;
                case ETimeSpan.oneDay:
                    cookie.Expires = DateTime.Now.AddDays(1);
                    break;
                case ETimeSpan.oneMounth:
                    cookie.Expires = DateTime.Now.AddMonths(1);
                    break;
                case ETimeSpan.never:
                    return new EmptyResult();
            }

            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);

			return Content(string.Format(script, title, html, delayMilliseconds, blocksBackground ? "data-close-out=\"false\"" : string.Empty));
        }
    }
}
