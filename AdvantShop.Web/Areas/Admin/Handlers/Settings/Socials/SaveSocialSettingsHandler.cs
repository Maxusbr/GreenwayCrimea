using AdvantShop.Configuration;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.Socials
{
    public class SaveSocialSettingsHandler
    {
        private SocialSettingsModel _model;

        public SaveSocialSettingsHandler(SocialSettingsModel model)
        {
            _model = model;
        }

        public void Execute()
        {
            SettingsSocial.SocialShareCustomCode = _model.SocialShareCustomCode;
            SettingsSocial.SocialShareEnabled = _model.SocialShareEnabled;
            SettingsSocial.SocialShareCustomEnabled = _model.SocialShareCustomEnabled;

            SettingsDesign.IsVkTemplateActive = _model.VkTemplateActive;
            SettingsDesign.IsFbTemplateActive = _model.FbTemplateActive;            
        }
    }
}
