
using System;
using Newtonsoft.Json;

namespace AdvantShop.Security.OAuth.Vk
{
    [Serializable]
    public class VkOAuthToken : OAuthToken
    {
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }


        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
    }
}
