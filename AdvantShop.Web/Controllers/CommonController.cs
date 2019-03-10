using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Captcha;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.InplaceEditor;
using AdvantShop.Core.Services.IPTelephony.CallBack;
using AdvantShop.Core.Services.SEO.MetaData;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Handlers.Common;
using AdvantShop.Handlers.Menu;
using AdvantShop.Models.Common;
using AdvantShop.Orders;
using AdvantShop.SEO;
using AdvantShop.Trial;
using AdvantShop.ViewModel.Common;
using AdvantShop.Helpers;
using AdvantShop.Repository.Currencies;
using AdvantShop.Repository;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.ViewModel.Shared;
using Debug = AdvantShop.Diagnostics.Debug;
using AdvantShop.Core.Caching;

namespace AdvantShop.Controllers
{
    public partial class CommonController : BaseClientController
    {
        public ActionResult ClosedStore()
        {
            if (!SettingsMain.IsStoreClosed)
                return RedirectToRoute("Home");

            SetMetaInformation(null, string.Empty);
            return View();
        }

        public JsonResult GetDesign()
        {
            var array = new GetDesignHandler().Get();
            return Json(array, JsonRequestBehavior.AllowGet);
        }

        public void SetCurrency(string currencyIso)
        {
            CurrencyService.CurrentCurrency = CurrencyService.Currency(currencyIso);
        }

        public JsonResult SaveDesign(string theme, string colorscheme, string structure, string background)
        {
            if (CustomerContext.CurrentCustomer.CustomerRole != Role.Administrator && !Demo.IsDemoEnabled &&
                !TrialService.IsTrialEnabled || theme.IsNullOrEmpty() || colorscheme.IsNullOrEmpty() || background.IsNullOrEmpty())
            {
                return Json("error", "text");
            }

            var handler = new SaveDesignHandler();
            handler.Save(theme, colorscheme, structure, background);

            return Json("success", "text");
        }

        [HttpPost]
        public ActionResult DebugJs(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
                Debug.Log.Error(HttpUtility.HtmlEncode(message));

            return new EmptyResult();
        }

        [ChildActionOnly]
        public ActionResult Logo(LogoModel logo)
        {
            if (!logo.Visible)
                return new EmptyResult();

            if (logo.ImgSource.IsNullOrEmpty() &&
                InplaceEditorService.CanUseInplace(RoleAction.Settings))
            {
                logo.ImgSource = UrlService.GetUrl("images/nophoto-logo.png");
            }
            
            var alt = !string.IsNullOrEmpty(logo.ImgAlt) ? string.Format(" alt=\"{0}\"", logo.ImgAlt) : string.Empty;
            var cssClass = !string.IsNullOrEmpty(logo.CssClass) ? " " + logo.CssClass : string.Empty;

            logo.LogoGeneratorEnabled = CustomerContext.CurrentCustomer.CustomerRole == Role.Administrator || Demo.IsDemoEnabled;

            if (!string.IsNullOrEmpty(logo.ImgSource))
            {
                logo.Html = string.Format("<img id=\"logo\" src=\"{0}\"{1} {3} class=\"site-head-logo-picture{2}\" {4}/>",
                    logo.ImgSource, alt, cssClass, Extensions.InplaceExtensions.InplaceImageLogo(), logo.LogoGeneratorEnabled ? "data-logo-generator-preview-img": "");
            }

            return PartialView("Logo", logo);
        }


        [ChildActionOnly]
        public ActionResult Favicon(FaviconModel model, string imgSource)
        {
            string path = imgSource.IsNotEmpty() ? imgSource  : SettingsMain.FaviconImageName.IsNotEmpty() ? SettingsMain.FaviconImageName : "favicon.ico";

            model.ImgSource = FoldersHelper.GetPathRelative(FolderType.Pictures, path, model.ForAdmin);

            if (!string.IsNullOrEmpty(model.ImgSource))
            {
                const string imgTag = "<img id=\"favicon\" src=\"{0}\" {1} />";
                const string linkTag = "<link rel=\"{0}\" href=\"{1}\"{2} />";

                //Source
                string source = UrlService.GetUrl(model.ImgSource);

                // styleClass
                string styleClass = !string.IsNullOrEmpty(model.CssClassImage) ? string.Format("class=\"{0}\"", model.CssClassImage) : string.Empty;

                //Source
                string rel = Request.Browser.Browser == "IE" ? "SHORTCUT ICON" : "shortcut icon";

                model.Html = model.GetOnlyImage ? string.Format(imgTag, source, styleClass) : string.Format(linkTag, rel, source, string.Empty);
            }


            return PartialView("Favicon", model);
        }


        [ChildActionOnly]
        public ActionResult MenuTop()
        {

            var currentMode = !Demo.IsDemoEnabled || !CommonHelper.GetCookieString("structure").IsNotEmpty()
               ? SettingsDesign.MainPageMode
               : (SettingsDesign.eMainPageMode)Enum.Parse(typeof(SettingsDesign.eMainPageMode), CommonHelper.GetCookieString("structure"));

            if (currentMode != SettingsDesign.eMainPageMode.Default)
                return new EmptyResult();

            var model = new MenuTopHanlder().GetTopMenuItems();
            return PartialView("MenuTop", model);
        }

        [ChildActionOnly]
        public ActionResult MenuBottom()
        {
            var model = new MenuBottomHanlder().Get();
            return PartialView("MenuBottom", model);
        }

        public ActionResult Copyright()
        {
            return PartialView("Copyright",
                new CopyrightModel
                {
                    ShowLink = ControllerContext.ParentActionViewContext.RouteData.GetRequiredString("Controller") == "Home",
                    Visible = SettingsDesign.ShowCopyright
                });
        }


        [ChildActionOnly]
        public ActionResult ToolBarBottom(ToolBarBottomViewModel model)
        {
            if (!SettingsDesign.DisplayToolBarBottom || MobileHelper.IsMobileBrowser())
                return new EmptyResult();

            model.isCart = (string)Request.RequestContext.RouteData.Values["controller"] == "Cart";

            //var model = new ToolBarHandler().Get();
            return PartialView(model);
        }


        [ChildActionOnly]
        public ActionResult Telephony()
        {
            var callBack = IPTelephonyOperator.Current.CallBack;
            if (callBack == null || !callBack.Enabled || (Saas.SaasDataService.IsSaasEnabled && !Saas.SaasDataService.CurrentSaasData.HaveTelephony))
                return new EmptyResult();

            var cookieValue = CommonHelper.GetCookieString("telephonyUserMode");

            var model = new TelephonyViewModel()
            {
                TimeInterval = SettingsTelephony.CallBackTimeInterval,
                IsWorkTime = callBack.IsWorkTime()
            };

            if (cookieValue.IsNotEmpty())
            {
                model.ShowMode = cookieValue.TryParseEnum<ECallBackShowMode>();
            }
            else
            {
                model.ShowMode = SettingsTelephony.CallBackShowMode;
            }

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult CallBack(string phone, bool check = false)
        {
            var callBack = IPTelephonyOperator.Current.CallBack;
            if (callBack == null || !callBack.Enabled || phone.IsNullOrEmpty())
                return new JsonResult();

            var result = callBack.MakeRequest(phone, check);
            return Json(result);
        }

        [ChildActionOnly]
        public ActionResult CookiesPolicy()
        {
            var cookieName = string.Format("{0}_CookiesPopicyAccepted", SettingsMain.SiteUrlPlain);

            if (!SettingsNotifications.ShowCookiesPolicyMessage || CommonHelper.GetCookieString(HttpUtility.UrlEncode(cookieName)) == "true")
                return new EmptyResult();

            return PartialView((object)cookieName);
        }

        [ChildActionOnly]
        public ActionResult MetaData()
        {
            if (!SettingsSEO.OpenGraphEnabled)
                return new EmptyResult();

            var ogModelContext = MetaDataContext.CurrentObject;

            if (ogModelContext == null)
                return PartialView(new OpenGraphModel());

            var ogModel = new OpenGraphModel()
            {
                SiteName = ogModelContext.SiteName,
                Url = ogModelContext.Url,
                Type = ogModelContext.Type,
                Images = ogModelContext.Images
            };

            return PartialView(ogModel);
        }

        [HttpPost]
        public JsonResult CheckOrder(string orderNumber)
        {
            if (!string.IsNullOrEmpty(orderNumber))
                return Json(new StatusInfo { StatusName = T("Checkout.CheckOrder.StatusCommentNotFound") });
            var order = OrderService.GetOrderByCode(orderNumber);
            if (order == null)
            {
                return Json(new StatusInfo { StatusName = T("Checkout.CheckOrder.StatusCommentNotFound") });
            }
            else if (order.OrderStatus.Hidden)
            {
                return Json(new StatusInfo { StatusName = order.PreviousStatus.IsNotEmpty() ? order.PreviousStatus : T("Checkout.CheckOrder.OrderNotFound") });
            }

            var statusInf = OrderService.GetStatusInfo(orderNumber);

            return Json(statusInf ?? new StatusInfo { StatusName = T("Checkout.CheckOrder.StatusCommentNotFound") });
        }

        [ChildActionOnly]
        public ActionResult GoogleAnalytics()
        {
            return Content(new GoogleAnalyticsString().GetGoogleAnalyticsString());
        }


        [ChildActionOnly]
        public ActionResult TopPanel()
        {
            var model = new TopPanelHandler().Get();
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Preview()
        {
            var model = new TopPanelHandler().Get();
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult BreadCrumbs(List<BreadCrumbs> breadCrumbs)
        {
            if (breadCrumbs == null || breadCrumbs.Count == 0)
                return new EmptyResult();

            return PartialView(breadCrumbs);
        }

        [ChildActionOnly]
        public ActionResult Rating(int objId, double rating, string url, bool readOnly = true, string binding = null)
        {
            return PartialView("_Rating", new RatingViewModel(rating)
            {
                ObjId = objId,
                Url = url,
                ReadOnly = readOnly,
                Binding = binding
            });
        }

        [ChildActionOnly]
        public ActionResult ZonePopover()
        {
            var cookieValue = CommonHelper.GetCookieString("zonePopoverVisible").ToLower();
            var settingValue = SettingsDesign.DisplayCityBubble;
            var settingValueString = settingValue.ToString().ToLower();
            var displayPopup = false;
            var expiresDate = new TimeSpan(364, 0, 0, 0, 0);

            if (string.IsNullOrEmpty(cookieValue) || cookieValue != settingValueString)
            {
                CommonHelper.SetCookie("zonePopoverVisible", settingValueString, expiresDate, false);
                displayPopup = settingValue;
            }

            if (!displayPopup)
                return new EmptyResult();

            return PartialView(new ZonePopoverViewModel() { City = IpZoneContext.CurrentZone.City });
        }

        [ChildActionOnly]
        public ActionResult Captcha(string ngModel, string NgModelSource = null)
        {
            var model = new CaptchaViewModel()
            {
                NgModel = ngModel,
                NgModelSource = NgModelSource
            };

            return PartialView(model);
        }


        [ChildActionOnly]
        public ActionResult Captcha_old(string ngModel, string NgModelSource = null)
        {
            var captcha = CaptchaService_old.GetNewCaptcha();
            if (captcha == null)
                return new EmptyResult();

            var model = new CaptchaViewModel()
            {
                CaptchaBase64Text = captcha.Base64Text,
                CaptchaEncodedBase64Text = captcha.EncodedBase64Text,
                CaptchaCode = captcha.Code,
                CaptchaSource = captcha.Source,
                NgModel = ngModel,
                NgModelSource = NgModelSource
            };

            return PartialView(model);
        }


        [HttpGet]
        public ActionResult GetCaptchaText(string captchatext)
        {
            if (string.IsNullOrWhiteSpace(captchatext))
                return new EmptyResult();

            var stream = CaptchaService_old.GetImage(captchatext);

            return new FileStreamResult(stream, "image/jpeg");
        }

        [ChildActionOnly]
        public ActionResult MenuGeneral()
        {
            var currentMode = !Demo.IsDemoEnabled || !CommonHelper.GetCookieString("structure").IsNotEmpty()
                                ? SettingsDesign.MainPageMode
                                : (SettingsDesign.eMainPageMode)Enum.Parse(typeof(SettingsDesign.eMainPageMode), CommonHelper.GetCookieString("structure"));
            var menuHandler = new MenuHandler();

            var model = new MenuViewModel()
            {
                MenuItems = currentMode != SettingsDesign.eMainPageMode.Default
                    ? menuHandler.GetMenuItems()
                    : menuHandler.GetCatalogMenuItems(0).SubItems,
                ViewMode = SettingsDesign.eMenuStyle.Classic
            };

            return PartialView("MenuGeneral", model);

        }

        [ChildActionOnly]
        public ActionResult MenuBlock()
        {
            var model = new MenuBlockViewModel();

            var currentMode = !Demo.IsDemoEnabled || !CommonHelper.GetCookieString("structure").IsNotEmpty()
                ? SettingsDesign.MainPageMode
                : (SettingsDesign.eMainPageMode)Enum.Parse(typeof(SettingsDesign.eMainPageMode), CommonHelper.GetCookieString("structure"));

            switch (currentMode)
            {
                case SettingsDesign.eMainPageMode.Default:
                    model.Layout = "_ColumnsOne";
                    break;
                case SettingsDesign.eMainPageMode.TwoColumns:
                    model.Layout = "_ColumnsTwo";
                    break;
                case SettingsDesign.eMainPageMode.ThreeColumns:
                    model.Layout = "_ColumnsThree";
                    break;
            }

            return PartialView("_Menu", model);
        }

        [ChildActionOnly]
        public ActionResult SocialButtons()
        {
            if (!SettingsSocial.SocialShareEnabled)
                return new EmptyResult();

            var model = new SocialButtonsViewModel()
            {
                Mode = SettingsSocial.SocialShareCustomEnabled
                    ? "custom"
                    : "default_" + SettingsMain.Language
            };

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult DiscountByTime()
        {
            if (DiscountByTimeService.ShowPopup &&
                CommonHelper.GetCookieString("discountbytime").IsNullOrEmpty() &&
                DiscountByTimeService.GetDiscountByTime() != 0)
            {
                CommonHelper.SetCookie("discountbytime", "true", new TimeSpan(12, 0, 0), true);

                return PartialView("DiscountByTime", DiscountByTimeService.PopupText);
            }
            return new EmptyResult();
        }

        [ChildActionOnly]
        public ActionResult TrialBuilder()
        {
            var cookieValue = CommonHelper.GetCookieString("trialBuilderShowed").TryParseBool(true);

            if ((cookieValue == null || cookieValue == false) && CustomerContext.CurrentCustomer.IsAdmin && TrialService.IsTrialEnabled)
            {
                CommonHelper.SetCookie("trialBuilderShowed", "true", true);
            }
            else
            {
                return new EmptyResult();
            }

            return PartialView();
        }

        public ActionResult CancelTemplatePreview()
        {
            if (CustomerContext.CurrentCustomer.IsAdmin)
            {
                SettingsDesign.PreviewTemplate = null;
                CacheManager.Clean();
            }

            string url = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : SettingsMain.SiteUrl;
            return Redirect(url);
        }

        public ActionResult ApplyTemplate()
        {
            var previewTemplate = SettingsDesign.PreviewTemplate;

            if (CustomerContext.CurrentCustomer.IsAdmin && previewTemplate != null)
            {
                SettingsDesign.ChangeTemplate(previewTemplate);
                CacheManager.Clean();
            }

            string url = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : SettingsMain.SiteUrl;
            return Redirect(url);
        }

        public ActionResult LiveCounter()
        {
            return Content("");
        }

        [ChildActionOnly]
        public ActionResult Inplace(bool enableInplace)
        {
            return PartialView(enableInplace);
        }
    }
}