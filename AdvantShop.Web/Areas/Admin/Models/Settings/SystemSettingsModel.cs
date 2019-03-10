using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.Helpers;

namespace AdvantShop.Web.Admin.Models.Settings
{
    public class SystemSettingsModel
    {
        #region common

        public string AdditionalHeadMetaTag { get; set; }

        public string LogoImageAlt { get; set; }
        public string AdminDateFormat { get; set; }
        public string ShortDateFormat { get; set; }

        public bool IsStoreClosed { get; set; }

        public bool EnableInplace { get; set; }
        public bool DisplayToolBarBottom { get; set; }
        public bool DisplayCityInTopPanel { get; set; }
        public bool DisplayCityBubble { get; set; }
        public bool ShowCopyright { get; set; }
        public long FilesSize { get; set; }
        public string FilesSizeFormatted
        {
            get { return FileHelpers.FileSize(FilesSize); }
        }

        #endregion

        #region captcha

        public bool EnableCaptcha { get; set; }
        public bool EnableCaptchaInCheckout { get; set; }
        public bool EnableCaptchaInRegistration { get; set; }
        public bool EnableCaptchaInPreOrder { get; set; }
        public bool EnableCaptchaInGiftCerticate { get; set; }
        public bool EnableCaptchaInFeedback { get; set; }
        public int CaptchaMode { get; set; }
        public List<SelectListItem> CaptchaModes { get; set; }
        public int CaptchaLength { get; set; }

        #endregion

        #region oauth
        public bool GoogleActive { get; set; }
        public bool MailActive { get; set; }
        public bool YandexActive { get; set; }
        public bool VkontakteActive { get; set; }
        public bool FacebookActive { get; set; }
        public bool OdnoklassnikiActive { get; set; }

        public string GoogleClientId { get; set; }
        public string GoogleClientSecret { get; set; }

        public string VkontakeClientId { get; set; }
        public string VkontakeSecret { get; set; }

        public string FacebookClientId { get; set; }
        public string FacebookApplicationSecret { get; set; }

        public string OdnoklassnikiClientId { get; set; }
        public string OdnoklassnikiPublicApiKey { get; set; }
        public string OdnoklassnikiSecret { get; set; }

        public string MailAppId { get; set; }
        public string MailClientId { get; set; }
        public string MailClientSecret { get; set; }

        public string YandexClientId { get; set; }
        public string YandexClientSecret { get; set; }

        public string CallbackUrl { get; set; }
        #endregion

        #region License
        public string LicKey { get; set; }
        public bool ActiveLic { get; set; }
        #endregion

        #region sitemap        
        public string SiteMapFileXmlLink { get; set; }
        public string SiteMapFileXmlDate { get; set; }

        public string SiteMapFileHtmlLink { get; set; }
        public string SiteMapFileHtmlDate { get; set; }
        #endregion

        #region customers notifications

        public bool ShowUserAgreementText { get; set; }
        public string UserAgreementText { get; set; }
        public bool ShowCookiesPolicyMessage { get; set; }
        public string CookiesPolicyMessage { get; set; }

        #endregion

        public bool IsSaas { get; set; }
        public bool IsTrial { get; set; }
    }
}
