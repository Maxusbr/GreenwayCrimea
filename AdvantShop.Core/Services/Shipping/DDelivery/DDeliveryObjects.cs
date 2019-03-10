using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AdvantShop.Core.Services.Shipping.DDelivery
{
    public enum EDeliveryType
    {
        Pickup = 1,
        Courier = 2,
        Post = 3
    }

    [Serializable]
    public class DDeliveryObjectResponse<T>
    {
        //"status": "ok",
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }


        //"message": "",
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        //"data": 
        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }

        [JsonIgnore]
        public bool Success { get { return string.Equals(Status, "ok"); } }
    }

    [Serializable]
    public class DDeliveryObjectCalculatorAnswer
    {
        [JsonProperty(PropertyName = "1")]
        public DDeliveryObjectDeliveryType Pickup { get; set; }

        [JsonProperty(PropertyName = "2")]
        public DDeliveryObjectDeliveryType Courier { get; set; }

        [JsonProperty(PropertyName = "3")]
        public DDeliveryObjectDeliveryType Post { get; set; }
    }

    [Serializable]
    public class DDeliveryObjectDeliveryType
    {
        //"type": 3,
        [JsonProperty(PropertyName = "type")]
        public int DeliveryTypeId { get; set; }

        //"type_name": "Почта",
        [JsonProperty(PropertyName = "type_name")]
        public string DeliveryTypeName { get; set; }

        //"delivery":
        [JsonProperty(PropertyName = "delivery")]
        public List<DDeliveryObjectDelivery> Delivery { get; set; }

        //"points":        
        [JsonProperty(PropertyName = "points")]
        public List<DDeliveryObjectPoint> Points { get; set; }
    }

    [Serializable]
    public class DDeliveryObjectDelivery
    {
        //  "delivery_company_id": 1,
        [JsonProperty(PropertyName = "delivery_company_id")]
        public int DeliveryCompanyId { get; set; }
        //  "delivery_company_name": "PickPoint",
        [JsonProperty(PropertyName = "delivery_company_name")]
        public string DeliveryCompanyName { get; set; }
        //  "delivery_company_abbr": "PICK",
        [JsonProperty(PropertyName = "delivery_company_abbr")]
        public string DeliveryCompanyAbbr { get; set; }
        //  "pickup_company_id": 62,
        [JsonProperty(PropertyName = "pickup_company_id")]
        public int PickupCompanyId { get; set; }
        //  "delivery_days": 3,
        [JsonProperty(PropertyName = "delivery_days")]
        public int DeliveryDays { get; set; }
        //  "delivery_date": "21.09.2017",
        [JsonProperty(PropertyName = "delivery_date")]
        public string DeliveryDate { get; set; }
        //  "delivery_hold_date": "27.09.2017",
        [JsonProperty(PropertyName = "delivery_hold_date")]
        public string DeliveryHoldDate { get; set; }
        //  "service": {
        //    "4": {
        //      "id": 4,
        //      "name": "Осмотр вложения",
        //      "price": 25,
        //      "is_check": 1
        //    }
        //  },
        //[JsonProperty(PropertyName = "service")]
        //public string Service { get; set; }

        //  "is_cash": true,
        [JsonProperty(PropertyName = "is_cash")]
        public bool IsCash { get; set; }
        //  "price_delivery": 225.85,
        [JsonProperty(PropertyName = "price_delivery")]
        public float PriceDelivery { get; set; }
        //  "price_commission_declared": 5,
        [JsonProperty(PropertyName = "price_commission_declared")]
        public float PriceCommissionDeclared { get; set; }
        //  "percent_commission_declared": 0.5,
        [JsonProperty(PropertyName = "percent_commission_declared")]
        public float PercentCommissionDeclared { get; set; }
        //  "price_commission_payment": 25.27,
        [JsonProperty(PropertyName = "price_commission_payment")]
        public float PriceCommissionPayment { get; set; }
        //  "percent_commission_payment": 1.5,
        [JsonProperty(PropertyName = "percent_commission_payment")]
        public float PercentCommissionPayment { get; set; }
        //  "price_commission_payment_card": 35.27,
        [JsonProperty(PropertyName = "price_commission_payment_card")]
        public float PriceCommissionPaymentCard { get; set; }
        //  "percent_commission_payment_card": 1.8,
        [JsonProperty(PropertyName = "percent_commission_payment_card")]
        public float PercentCommissionPaymentCard { get; set; }
        //  "total_price": 264.9
        [JsonProperty(PropertyName = "total_price")]
        public float TotalPrice { get; set; }
    }

    [Serializable]
    public class DDeliveryObjectPoint
    {
        //"id": 151185,
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        //"name": "Невинномысск",
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        //"longitude": "41.946725",
        [JsonProperty(PropertyName = "longitude")]
        public string Longitude { get; set; }
        //"latitude": "44.624142",
        [JsonProperty(PropertyName = "latitude")]
        public string Latitude { get; set; }
        //"delivery_company_id": 4,
        [JsonProperty(PropertyName = "delivery_company_id")]
        public int DeliveryCompanyId { get; set; }
        //"delivery_company_name": "Boxberry",
        [JsonProperty(PropertyName = "delivery_company_name")]
        public string DeliveryCompanyName { get; set; }
        //"type": 2,
        [JsonProperty(PropertyName = "type")]
        public int DeliveryTypeId { get; set; }

        //"description_in": "Войти в Торговый центр через первый вход",
        [JsonProperty(PropertyName = "description_in")]
        public string DescriptionIn { get; set; }

        //"description_out": "Возле остановки",
        [JsonProperty(PropertyName = "description_out")]
        public string DescriptionOut { get; set; }

        //"address": "357100, г. Невинномысск, ул. Гагарина, д. 72",
        [JsonProperty(PropertyName = "address")]
        public string Adress { get; set; }

        //"schedule": "пн-пт:10.00-18.00 сб:10.00-18.00",
        [JsonProperty(PropertyName = "schedule")]
        public string Schedule { get; set; }
        //"is_fitting": 0,
        [JsonProperty(PropertyName = "is_fitting")]
        public int IsFitting { get; set; }
        //"is_cash": 1,
        [JsonProperty(PropertyName = "is_cash")]
        public int IsCash { get; set; }
        //"is_card": 0
        [JsonProperty(PropertyName = "is_card")]
        public int IsCard { get; set; }


        //"price_delivery": 264.9,
        [JsonProperty(PropertyName = "price_delivery")]
        public float PriceDelivery { get; set; }

        //"delivery_date": "21.09.2017",
        [JsonProperty(PropertyName = "delivery_date")]
        public string DeliveryDate { get; set; }

        //"delivery_hold_date": "27.09.2017"
        [JsonProperty(PropertyName = "delivery_hold_date")]
        public string DeliveryHoldDate { get; set; }
    }

    [Serializable]
    public class DDeliveryObjectCity
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    [Serializable]
    public class DDeliveryObjectCompany
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    [Serializable]
    public class DDeliveryObjectNewOrder
    {
        //"order_id": 299081,
        [JsonProperty(PropertyName = "order_id")]
        public string OrderId { get; set; }

        //"delivery_date": "2017-09-22"
        [JsonProperty(PropertyName = "delivery_date")]
        public string DeliveryDate { get; set; }
    }


    [Serializable]
    public class DDeliveryObjectOrderInfo
    {
        //"order_id": 362517
        [JsonProperty(PropertyName = "order_id")]
        public string OrderId { get; set; }

        //"type": 1,
        [JsonProperty(PropertyName = "type")]
        public EDeliveryType Type { get; set; }

        //"site": "test.advantshop.net",
        [JsonProperty(PropertyName = "site")]
        public string Site { get; set; }

        //"company": "IML",
        [JsonProperty(PropertyName = "company")]
        public string Company { get; set; }

        //"delivery_date": "20.12.2017",
        [JsonProperty(PropertyName = "delivery_date")]
        public string DeliveryDate { get; set; }

        //"logo": "data:image/png;base64,/9j/4AAQSkZJR ... ",
        //[JsonProperty(PropertyName = "logo")]
        //public string Logo { get; set; }

        //"status": "11. Заявка создана как Черновик",
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        //  "status_history": [ {} ],
        //[JsonProperty(PropertyName = "status_history")]
        //public List<string> StatusHistory { get; set; }

        //"track_number": "11. Заявка создана как Черновик",
        [JsonProperty(PropertyName = "track_number")]
        public string TrackNumber { get; set; }
        //"track_number": "11. Заявка создана как Черновик",
        [JsonProperty(PropertyName = "tracking_url")]
        public string TrackingUrl { get; set; }

        //"point": [ {} ]
        [JsonProperty(PropertyName = "point")]
        public DDeliveryObjectPoint Point { get; set; }
    }

    [Serializable]
    public class DDeliveryObjectWidgetConfig
    {        
        [JsonProperty(PropertyName = "products")]
        public List<DDeliveryObjectProduct> Products { get; set; }
        
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "width")]
        public float Width { get; set; }

        [JsonProperty(PropertyName = "height")]
        public float Height { get; set; }

        [JsonProperty(PropertyName = "env")]
        public string Env { get; set; }        
    }

    [Serializable]
    public class DDeliveryObjectProduct
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "sku")]
        public string Sku { get; set; }

        [JsonProperty(PropertyName = "price")]
        public float Price { get; set; }

        [JsonProperty(PropertyName = "weight")]
        public float Weight { get; set; }

        [JsonProperty(PropertyName = "width")]
        public float Width { get; set; }

        [JsonProperty(PropertyName = "height")]
        public float Height { get; set; }

        [JsonProperty(PropertyName = "length")]
        public float Length { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }        
    }


}
