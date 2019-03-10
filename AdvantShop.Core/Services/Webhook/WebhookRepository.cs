using AdvantShop.Configuration;
using AdvantShop.Helpers;

namespace AdvantShop.Core.Services.Webhook
{
    public class WebhookRepository
    {
        public static string SystemApiKey
        {
            get { return SecurityHelper.EncodeWithHmac(SettingsLic.LicKey ?? string.Empty , SettingsLic.ClientCode); }
        }

        public static bool IsSystemKey(string apiKey)
        {
            return SystemApiKey == apiKey;
        }
    }
}
