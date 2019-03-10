
using System;
using Newtonsoft.Json;

namespace AdvantShop.Security.OAuth
{
    [Serializable]
    public class MailOAuthToken : OAuthToken
    {
        [JsonProperty(PropertyName = "x_mailru_vid")]
        public string Vid { get; set; }

    }
}
