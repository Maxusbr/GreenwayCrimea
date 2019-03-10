using System.Collections.Generic;

namespace AdvantShop.Shipping.ShippingYandexDelivery
{
    public class YandexDeliveryTemplate : DefaultCargoParams
    {
        public const string ClientId = "ClientId";
        public const string SenderId = "SenderId";
        public const string RequisiteId = "RequisiteId";
        public const string WarehouseId = "WarehouseId";

        public const string ApiKeys = "ApiKeys";
        public const string ApiData = "ApiData";
        public const string SearchDeliveryListKey = "SearchDeliveryListKey";
        public const string CreateOrderKey = "CreateOrderKey";
        
        public const string CityFrom = "CityFrom";
        public const string SecretKeyDelivery = "SecretKeyDelivery";
        public const string SecretKeyCreateOrder = "SecretKeyCreateOrder";
        public const string WidgetCode = "WidgetCode";
        public const string IsActive = "IsActive";

        public const string ShowAssessedValue = "ShowAssessedValue";
    }

    public class YaDeliveryConfigParams
    {
        public string client_id { get; set; }
        public List<string> sender_ids { get; set; }
        public List<string> warehouse_ids { get; set; }
        public List<string> requisite_ids { get; set; }
    }

    public class YaDeliveryJsonConfigParams
    {
        public YaDeliveryJsonConfigParamItem client { get; set; }
        public List<YaDeliveryJsonConfigParamItem> warehouses { get; set; }
        public List<YaDeliveryJsonConfigParamItem> senders { get; set; }
        public List<YaDeliveryJsonConfigParamItem> requisites { get; set; }
    }

    public class YaDeliveryJsonConfigParamItem
    {
        public string id { get; set; }
        public string name { get; set; }
    }


    public class YandexDeliveryAdditionalData
    {
        public int direction { get; set; }
        public int delivery { get; set; }
        public int price { get; set; }
        public int tariffId { get; set; }
        public int? to_ms_warehouse { get; set; }
    }
}
