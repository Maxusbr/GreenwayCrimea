using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdvantShop.Module.Roistat.Models.Roistat
{
    // http://help.roistat.com/pages/viewpage.action?pageId=360599

    public class RoistatClientsResponse
    {
        [JsonProperty(PropertyName = "clients")]
        public List<RoistatClient> Clients { get; set; }
    }

    public class RoistatClient
    {
        /// <summary>
        /// Уникальный идентификатор клиента (обяз.)
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Имя клиента (обяз.)
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Номер телефона (обяз.)
        /// </summary>
        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        /// <summary>
        /// Эл. почта (обяз.)
        /// </summary>
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        ///// <summary>
        ///// Название компании (не обяз.)
        ///// </summary>
        //[JsonProperty(PropertyName = "company")]
        //public string Company { get; set; }

        ///// <summary>
        ///// День рождения (В формате Y-m-d) (не обяз.)
        ///// </summary>
        //[JsonProperty(PropertyName = "birth_date")]
        //public string BirthDate { get; set; }
    }

}
