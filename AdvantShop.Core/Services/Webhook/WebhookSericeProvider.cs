using System;
using System.Collections.Generic;
using AdvantShop.Configuration;
using Newtonsoft.Json;

namespace AdvantShop.Core.Services.Webhook
{
    public class WebhookSerice
    {
        public string ApiKey { get; set; }
        public bool Enabled { get; set; }
    }

    public class WebhookSericeProvider : Dictionary<EWebhookType, WebhookSerice>
    {
        private static WebhookSericeProvider _services;
        private static object _sync = new object();
        public static WebhookSericeProvider Services
        {
            get
            {
                if (_services == null)
                {
                    _services = JsonConvert.DeserializeObject<WebhookSericeProvider>(SettingsWebhook.WebhookSerices ?? string.Empty) ?? new WebhookSericeProvider();
                }
                foreach (EWebhookType type in Enum.GetValues(typeof(EWebhookType)))
                {
                    if (type == EWebhookType.None)
                        continue;

                    lock (_sync)
                    {
                        if (!_services.ContainsKey(type))
                        {
                            _services.Add(type, new WebhookSerice());
                        }
                    }
                }
                return _services;
            }
            set
            {
                _services = value;
                SettingsWebhook.WebhookSerices = JsonConvert.SerializeObject(_services);
            }
        }

        public WebhookSerice Get(EWebhookType type)
        {
            return Services.ContainsKey(type) ? Services[type] : new WebhookSerice();
        }

        public WebhookSericeProvider Set(EWebhookType type, WebhookSerice service)
        {
            if (Services.ContainsKey(type))
                Services[type] = service;
            else
                Services.Add(type, service);
            return Services;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(Services);
        }
    }
}
