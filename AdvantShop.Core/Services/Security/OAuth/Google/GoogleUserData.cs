//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdvantShop.Security.OAuth.Goolgle
{
    [Serializable]
    public class GoogleUserData
    {
        [JsonProperty(PropertyName = "sub")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "nickname")]
        public string Login { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string FullName { get; set; }
        
        [JsonProperty(PropertyName = "givenName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "familyName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
    }
}
