//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdvantShop.Security.OAuth.Vk
{
    [Serializable]
    public class VkUserDataResponse
    {
        [JsonProperty(PropertyName = "response")]
        public List<VkUserData> UsersList;
    }

    [Serializable]
    public class VkUserData
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        //Может быть отчеством пользователя
        [JsonProperty(PropertyName = "nickname")]
        public string Login { get; set; }
        
        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }
    }
}
