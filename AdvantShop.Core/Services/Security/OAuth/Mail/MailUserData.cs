//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using Newtonsoft.Json;

namespace AdvantShop.Security.OAuth.Mail
{
    [Serializable]
    public class MailUserData
    {
        [JsonProperty(PropertyName = "uid")]
        public string Id { get; set; }
        
        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }
    
        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }
    
        [JsonProperty(PropertyName = "nick")]
        public string Login { get; set; }
    
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
    }
}
