using System;
using System.IO;
using System.Net;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Diagnostics;
using AdvantShop.Saas;
using AdvantShop.Trial;
using Newtonsoft.Json;

namespace AdvantShop.Core.Services.DownloadableContent
{
    public class ShopActionsService
    {
        private const string ShopActionsUrl = "http://modules.advantshop.net/actionblocks/last?id=";

        public static string GetLast()
        {
            return CacheManager.Get("AdvantshopAdminAction", 20, () =>
            {
                var action = "";

                if (!SaasDataService.IsSaasEnabled && !TrialService.IsTrialEnabled)
                    return "";

                try
                {
                    var request = WebRequest.Create(ShopActionsUrl + SettingsLic.LicKey);
                    request.Method = "GET";
                    request.Timeout = 1100;

                    using (var dataStream = request.GetResponse().GetResponseStream())
                    {
                        using (var reader = new StreamReader(dataStream))
                        {
                            var responseFromServer = reader.ReadToEnd();
                            if (!string.IsNullOrEmpty(responseFromServer))
                            {
                                action = JsonConvert.DeserializeObject<string>(responseFromServer);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                }

                return action;
            });
        }
    }
}
