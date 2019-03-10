using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdvantShop.Module.Roistat.Models.Roistat
{
    // http://help.roistat.com/pages/viewpage.action?pageId=360599

    public class RoistatOrdersResponse
    {
        [JsonProperty(PropertyName = "pagination")]
        public RoistatPagination Pagination { get; set; }

        [JsonProperty(PropertyName = "statuses")]
        public List<RoistatStatus> Statuses { get; set; }


        [JsonProperty(PropertyName = "orders")]
        public List<RoistatOrder> Orders { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public List<RoistatOrderField> Fields { get; set; }
    }
    
    public class RoistatOrder
    {
        /// <summary>
        /// Уникальный идентификатор сделки (обяз.)
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Название сделки. Используется в интерфейсе Roistat (не обяз.)
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Дата создания сделки в формате UNIX-time или YYYY-MM-DD HH:MM (обяз.)
        /// </summary>
        [JsonProperty(PropertyName = "date_create")]
        public string DateCreate { get; set; }

        /// <summary>
        /// Уникальный идентификатор статуса из массива statuses
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Сумма сделки. Используется в показателе «Выручка» в Roistat
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public string Price { get; set; }

        /// <summary>
        /// Cебестоимость сделки. Используется в показателе «Себестоимость» в Roistat
        /// </summary>
        [JsonProperty(PropertyName = "cost")]
        public string Cost { get; set; }

        /// <summary>
        /// Номер визита, сохраненный у сделки. Значение cookie roistat_visit. Используется для определения источника сделки
        /// </summary>
        [JsonProperty(PropertyName = "roistat")]
        public string Roistat { get; set; }

        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        [JsonProperty(PropertyName = "client_id")]
        public string ClientId { get; set; }
        

        [JsonProperty(PropertyName = "fields")]
        public Dictionary<string, string> Fields { get; set; }
    }

    public class RoistatStatus
    {
        /// <summary>
        /// Уникальный идентификатор статуса. Используется в массиве orders
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Название статуса. Используется в интерфейсе Roistat
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        public RoistatStatus(string type, int id, string name, bool? paied)
        {
            Id = string.Format("{0}_{1}_{2}", type, id, paied ?? false);
            Name = string.Format("{0}{1}", name, paied != null ? (paied.Value ? " (Оплачен)" : " (Не оплачен)") : "");
        }
    }

    public class RoistatOrderField
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        public static List<RoistatOrderField> GetDefaultFields()
        {
            return new List<RoistatOrderField>()
            {
                new RoistatOrderField() {Id = "1", Name = "Менеджер"},
                new RoistatOrderField() {Id = "2", Name = "Способ доставки"},
                new RoistatOrderField() {Id = "3", Name = "Способ оплаты"},
                new RoistatOrderField() {Id = "4", Name = "Адрес доставки"},
            };
        }
    }
}
