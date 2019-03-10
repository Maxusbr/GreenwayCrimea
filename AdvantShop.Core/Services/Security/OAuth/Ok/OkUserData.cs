//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using Newtonsoft.Json;

namespace AdvantShop.Security.OAuth.Mail
{
    [Serializable]
    public class OkUserData
    {
        [JsonProperty(PropertyName = "uid")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string FullName { get; set; } //composition of first and last name to render
        
    }
}
