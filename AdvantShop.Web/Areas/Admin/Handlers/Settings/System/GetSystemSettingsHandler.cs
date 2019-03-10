using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Saas;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.System
{
    public class GetSystemSettingsHandler
    {
        public SystemSettingsModel Execute()
        {
            var model = new SystemSettingsModel
            {
                AdditionalHeadMetaTag = SettingsSEO.CustomMetaString,

                LogoImageAlt = SettingsMain.LogoImageAlt,
                AdminDateFormat = SettingsMain.AdminDateFormat,
                ShortDateFormat = SettingsMain.ShortDateFormat,
                IsStoreClosed = SettingsMain.IsStoreClosed,
                EnableInplace = SettingsMain.EnableInplace,
                DisplayToolBarBottom = SettingsDesign.DisplayToolBarBottom,
                DisplayCityInTopPanel = SettingsDesign.DisplayCityInTopPanel,
                ShowCopyright = SettingsDesign.ShowCopyright,

                EnableCaptcha = SettingsMain.EnableCaptcha,
                EnableCaptchaInCheckout = SettingsMain.EnableCaptchaInCheckout,
                EnableCaptchaInRegistration = SettingsMain.EnableCaptchaInRegistration,
                EnableCaptchaInFeedback = SettingsMain.EnableCaptchaInFeedback,
                EnableCaptchaInPreOrder = SettingsMain.EnableCaptchaInPreOrder,
                EnableCaptchaInGiftCerticate = SettingsMain.EnableCaptchaInGiftCerticate,
                CaptchaMode = (int)SettingsMain.CaptchaMode,
                CaptchaLength = SettingsMain.CaptchaLength,

                GoogleActive = SettingsOAuth.GoogleActive,
                MailActive = SettingsOAuth.MailActive,
                YandexActive = SettingsOAuth.YandexActive,
                VkontakteActive = SettingsOAuth.VkontakteActive,
                FacebookActive = SettingsOAuth.FacebookActive,
                OdnoklassnikiActive = SettingsOAuth.OdnoklassnikiActive,
                GoogleClientId = SettingsOAuth.GoogleClientId,
                GoogleClientSecret = SettingsOAuth.GoogleClientSecret,
                VkontakeClientId = SettingsOAuth.VkontakeClientId,
                VkontakeSecret = SettingsOAuth.VkontakeSecret,
                OdnoklassnikiClientId = SettingsOAuth.OdnoklassnikiClientId,
                OdnoklassnikiSecret = SettingsOAuth.OdnoklassnikiSecret,
                OdnoklassnikiPublicApiKey = SettingsOAuth.OdnoklassnikiPublicApiKey,
                FacebookClientId = SettingsOAuth.FacebookClientId,
                FacebookApplicationSecret = SettingsOAuth.FacebookApplicationSecret,
                MailAppId = SettingsOAuth.MailAppId,
                MailClientId = SettingsOAuth.MailClientId,
                MailClientSecret = SettingsOAuth.MailClientSecret,
                YandexClientId = SettingsOAuth.YandexClientId,
                YandexClientSecret = SettingsOAuth.YandexClientSecret,

                CallbackUrl = SettingsMain.SiteUrl + "/login",

                IsSaas = SaasDataService.IsSaasEnabled,
                IsTrial = Trial.TrialService.IsTrialEnabled,
                LicKey = SettingsLic.LicKey,
                ActiveLic = SettingsLic.ActiveLic,

                FilesSize = SettingsMain.CurrentFilesStorageSize, //AttachmentService.GetAllAttachmentsSize(),

                ShowUserAgreementText = SettingsCheckout.IsShowUserAgreementText,
                UserAgreementText = SettingsCheckout.UserAgreementText,
                DisplayCityBubble = SettingsDesign.DisplayCityBubble,
                ShowCookiesPolicyMessage = SettingsNotifications.ShowCookiesPolicyMessage,
                CookiesPolicyMessage = SettingsNotifications.CookiesPolicyMessage,
            };

            model.CaptchaModes = new List<SelectListItem>();
            foreach (CaptchaMode value in Enum.GetValues(typeof(CaptchaMode)))
            {
                model.CaptchaModes.Add(new SelectListItem()
                {
                    Text = value.Localize(),
                    Value = ((int)value).ToString()
                });
            }


            var sitemapFilePathXml = SettingsGeneral.AbsolutePath + "sitemap.xml";
            var sitemapFilePathHtml = SettingsGeneral.AbsolutePath + "sitemap.html";

            if (File.Exists(sitemapFilePathXml))
            {
                model.SiteMapFileXmlDate = Localization.Culture.ConvertDate(new FileInfo(sitemapFilePathXml).LastWriteTime);
                model.SiteMapFileXmlLink = SettingsMain.SiteUrl + "/sitemap.xml";
            }

            if (File.Exists(sitemapFilePathHtml))
            {
                model.SiteMapFileHtmlDate = Localization.Culture.ConvertDate(new FileInfo(sitemapFilePathHtml).LastWriteTime);
                model.SiteMapFileHtmlLink = SettingsMain.SiteUrl + "/sitemap.html";
            }

            return model;
        }
    }
}
