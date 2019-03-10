using AdvantShop.Configuration;
using Newtonsoft.Json;

namespace AdvantShop.Core.Services.Webhook
{
    public class WebhookUrlListProvider<TUrlList, TUrl> where TUrlList : WebhookUrlList<TUrl>, new() where TUrl : WebhookUrl
    {
        private static EWebhookType _webhookType;

        public WebhookUrlListProvider()
        {
            _webhookType = new TUrlList().WebhookType;
        }

        public static TUrlList WebhookUrlList
        {
            get
            {
                var list = SettingProvider.Items[string.Format("SettingsWebhook.WebhookUrlList.{0}", _webhookType)];
                return !string.IsNullOrEmpty(list) ? JsonConvert.DeserializeObject<TUrlList>(list) : new TUrlList();
            }
            set
            {
                SettingProvider.Items[string.Format("SettingsWebhook.WebhookUrlList.{0}", _webhookType.ToString())] = JsonConvert.SerializeObject(value);
            }
        }
    }
}
