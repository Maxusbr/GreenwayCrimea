using AdvantShop.Configuration;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.Socials
{
    public class GetSocialSettingsHandler
    {
        public SocialSettingsModel Execute()
        {
            return new SocialSettingsModel
            {
                SocialShareCustomCode = SettingsSocial.SocialShareCustomCode,
                SocialShareEnabled = SettingsSocial.SocialShareEnabled,
                SocialShareCustomEnabled = SettingsSocial.SocialShareCustomEnabled,

                VkTemplateActive = SettingsDesign.IsVkTemplateActive,
                FbTemplateActive = SettingsDesign.IsFbTemplateActive,

                SocialLinkVk = SettingsMain.SiteUrl.Replace("www.", "").Replace("http://", "http://vk.").Replace("https://", "https://vk."),
                SocialLinkFb = SettingsMain.SiteUrl.Replace("www.", "").Replace("http://", "http://fb.").Replace("https://", "https://fb.")
            };
        }
    }
}
