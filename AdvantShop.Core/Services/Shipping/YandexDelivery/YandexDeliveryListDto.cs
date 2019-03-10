using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdvantShop.Shipping.ShippingYandexDelivery
{
    public class YandexDeliveryListDto
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "data")]
        public List<YandexDeliveryListItem> Data { get; set; }
    }

    public class YandexDeliveryListItem
    {
        [JsonProperty(PropertyName = "delivery")]
        public YaDeliveryDataDelivery Delivery { get; set; }


        [JsonProperty(PropertyName = "pickupPoints")]
        public List<YaDeliveryPicpoint> PickupPoints { get; set; }
        
        /// <summary>
        /// Срок доставки. может быть число, может быть строка (1-3)
        /// </summary>
        [JsonProperty(PropertyName = "days")]
        public string Days { get; set; }

        /// <summary>
        /// Название службы доставки
        /// </summary>
        [JsonProperty(PropertyName = "delivery_name")]
        public string DeliveryName { get; set; }

        /// <summary>
        /// Итоговая стоимость доставки для покупателя
        /// </summary>
        [JsonProperty(PropertyName = "costWithRules")]
        public float CostWithRules { get; set; }


        [JsonProperty(PropertyName = "tariffId")]
        public string TariffId { get; set; }

        [JsonProperty(PropertyName = "tariffName")]
        public string TariffName { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }


        [JsonProperty(PropertyName = "direction")]
        public string Direction { get; set; }


        [JsonProperty(PropertyName = "settings")]
        public YaDeliverySettings Settings { get; set; }

    }

    public class YaDeliveryDataDelivery
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class YaDeliveryPicpoint
    {
        public string id { get; set; }
        public string delivery_id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string location_name { get; set; }
        public string full_address { get; set; }
    }

    public class YaDeliverySettings
    {
        [JsonProperty(PropertyName = "to_yd_warehouse")]
        public string ToYDWarehouse { get; set; }
    }
}