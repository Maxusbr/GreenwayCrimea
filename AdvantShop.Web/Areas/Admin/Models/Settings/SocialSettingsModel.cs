namespace AdvantShop.Web.Admin.Models.Settings
{
    public class SocialSettingsModel
    {
        public string SocialLinkVk { get; set; }
        public string SocialLinkFb { get; set; }

        public bool VkTemplateActive { get; set; }
        public bool FbTemplateActive { get; set; }

        public bool SocialShareEnabled { get; set; }
        public bool SocialShareCustomEnabled { get; set; }
        public string SocialShareCustomCode { get; set; }
    }
}