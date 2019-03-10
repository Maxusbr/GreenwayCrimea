//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdvantShop.Security.OAuth.Yandex
{
    [Serializable]
    public class YandexUserData
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "login")]
        public string Login { get; set; }

        //Список всех OpenID-идентификаторов, которые пользователь мог получить от Яндекса.Список используется при миграции с OpenID Яндекса на API Паспорта.
        [JsonProperty(PropertyName = "openid_identities")]
        public List<string> OpenidIdentities { get; set; }

        [JsonProperty(PropertyName = "emails")]
        public List<string> Emails { get; set; }

        [JsonProperty(PropertyName = "default_email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "real_name")]
        public string FullName { get; set; }
    }
}
