using AdvantShop.Configuration;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.System
{
    public class SaveSystemSettingsHandler
    {
        private readonly SystemSettingsModel _model;

        public SaveSystemSettingsHandler(SystemSettingsModel model)
        {
            _model = model;
        }

        public void Execute()
        {
            #region Common

            SettingsMain.LogoImageAlt = _model.LogoImageAlt;
            SettingsMain.AdminDateFormat = _model.AdminDateFormat;
            SettingsMain.ShortDateFormat = _model.ShortDateFormat;

            SettingsMain.IsStoreClosed = _model.IsStoreClosed;
            SettingsMain.EnableInplace = _model.EnableInplace;
            SettingsDesign.DisplayToolBarBottom = _model.DisplayToolBarBottom;
            SettingsDesign.DisplayCityInTopPanel = _model.DisplayCityInTopPanel;
            SettingsDesign.ShowCopyright = _model.ShowCopyright;

            #endregion

            #region Captcha

            SettingsMain.EnableCaptcha = _model.EnableCaptcha;
            SettingsMain.EnableCaptchaInCheckout = _model.EnableCaptchaInCheckout;
            SettingsMain.EnableCaptchaInRegistration = _model.EnableCaptchaInRegistration;
            SettingsMain.EnableCaptchaInFeedback = _model.EnableCaptchaInFeedback;
            SettingsMain.EnableCaptchaInPreOrder = _model.EnableCaptchaInPreOrder;
            SettingsMain.EnableCaptchaInGiftCerticate = _model.EnableCaptchaInGiftCerticate;
            SettingsMain.CaptchaMode = (CaptchaMode)_model.CaptchaMode;
            SettingsMain.CaptchaLength = _model.CaptchaLength;

            #endregion

            #region Auth

            SettingsOAuth.GoogleActive = _model.GoogleActive;
            SettingsOAuth.MailActive = _model.MailActive;
            SettingsOAuth.YandexActive = _model.YandexActive;
            SettingsOAuth.VkontakteActive = _model.VkontakteActive;
            SettingsOAuth.FacebookActive = _model.FacebookActive;
            SettingsOAuth.OdnoklassnikiActive = _model.OdnoklassnikiActive;

            SettingsOAuth.GoogleClientId = _model.GoogleClientId;
            SettingsOAuth.GoogleClientSecret = _model.GoogleClientSecret;

            SettingsOAuth.VkontakeClientId = _model.VkontakeClientId;
            SettingsOAuth.VkontakeSecret = _model.VkontakeSecret;

            SettingsOAuth.OdnoklassnikiClientId = _model.OdnoklassnikiClientId;
            SettingsOAuth.OdnoklassnikiSecret = _model.OdnoklassnikiSecret;
            SettingsOAuth.OdnoklassnikiPublicApiKey = _model.OdnoklassnikiPublicApiKey;

            SettingsOAuth.FacebookClientId = _model.FacebookClientId;
            SettingsOAuth.FacebookApplicationSecret = _model.FacebookApplicationSecret;

            SettingsOAuth.MailAppId = _model.MailAppId;
            SettingsOAuth.MailClientId = _model.MailClientId;
            SettingsOAuth.MailClientSecret = _model.MailClientSecret;

            SettingsOAuth.YandexClientId = _model.YandexClientId;
            SettingsOAuth.YandexClientSecret = _model.YandexClientSecret;

            #endregion
            
            #region License
            if (!Saas.SaasDataService.IsSaasEnabled && !Trial.TrialService.IsTrialEnabled)
            {
                SettingsLic.LicKey = _model.LicKey;
                SettingsLic.ActiveLic = _model.ActiveLic;
            }
            #endregion

            #region Customers Notifications

            SettingsCheckout.IsShowUserAgreementText = _model.ShowUserAgreementText;
            SettingsCheckout.UserAgreementText = _model.UserAgreementText;
            SettingsDesign.DisplayCityBubble = _model.DisplayCityBubble;
            SettingsNotifications.ShowCookiesPolicyMessage = _model.ShowCookiesPolicyMessage;
            SettingsNotifications.CookiesPolicyMessage = _model.CookiesPolicyMessage;

            #endregion

            SettingsSEO.CustomMetaString = _model.AdditionalHeadMetaTag;
        }
    }
}
