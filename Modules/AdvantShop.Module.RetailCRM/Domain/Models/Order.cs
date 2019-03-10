using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdvantShop.Modules.RetailCRM.Models
{
    public class SerializedOrder
    {
        public int? id { get; set; }

        public string number { get; set; }
        public string externalId { get; set; }
        public DateTime? createdAt { get; set; }

        public float? discount { get; set; }
        public float? discountPercent { get; set; }

        public string lastName { get; set; }
        public string firstName { get; set; }
        public string patronymic { get; set; }
        public string phone { get; set; }
        public string email { get; set; }

        public string customerComment { get; set; }
        public string managerComment { get; set; }

        public string orderType { get; set; }
        public string orderMethod { get; set; }

        public string customerId { get; set; }
        //public int managerId { get; set; }

        public string paymentType { get; set; }
        public string paymentStatus { get; set; }

        public string status { get; set; }

        public object customer { get; set; } // Владимир: не используется, нужен только чтобы правильно десерилизовалось
        //public string sourceId { get; set; }

        public List<SerializedOrderProduct> items { get; set; }
        public SerializedOrderDelivery delivery { get; set; }

        public Dictionary<string, string> customFields { get; set; }

    }

    public class SerializedOrderDelivery
    {
        public string code { get; set; }
        public object data { get; set; } // Владимир: не используется, нужен только чтобы правильно десерилизовалось
        public SerializedDeliveryService service { get; set; }
        public float cost { get; set; }
        //public DateTime date { get; set; }

        public CustomerAddress address { get; set; }
    }

    public class SerializedDeliveryService
    {
        public string name { get; set; }
        public string code { get; set; }
        public string deliveryType { get; set; }

    }

    public class SerializedOrderProduct
    {
        public float initialPrice { get; set; }
        //public float discount { get; set; }
        //public float discountPercent { get; set; }
        //public DateTime createdAt { get; set; }
        public float quantity { get; set; }
        public string productId { get; set; }
        //public string comment { get; set; }
        public float purchasePrice { get; set; }
        public bool deleted { get; set; }
        public bool created { get; set; }

        public string xmlId { get; set; }
        public string productName { get; set; }     // vladimir: такое вот говнище приходит. Отправлять надо в данны ев полях xmlId, productName
        public Offer offer { get; set; }            // а приходят обратно в offer.xmlId, offer.name, offer.externalId
        public string id { get; set; }              // в случае удаления позиции из заказа в поле id придет АРТИКУЛ !

        [JsonConverter(typeof(RetailDictionaryConverter<Property>))]
        public List<Property> properties { get; set; } // отправляем массив, возвращается дермище
    }

    public class Property : BaseRetailDictionary
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Offer
    {
        public string externalId { get; set; }
        public string xmlId { get; set; }
        public string name { get; set; }

    }
}   