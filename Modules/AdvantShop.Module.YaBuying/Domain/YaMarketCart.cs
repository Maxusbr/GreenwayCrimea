using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdvantShop.Module.YaBuying.Domain
{
    /* 
     * http://api.yandex.ru/market/partner/doc/dg/reference/post-cart.xml
     * 
     */

    public class YaCart
    {
        public YaMarketCartRequest cart { get; set; }
    }


    public class YaMarketCartRequest
    {
        /// <summary>
        /// Валюта, в которой выражены цены товаров в заказе. Возможные значения:
        /// RUR — российский рубль; UAH — украинская гривна.
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        /// Товары в корзине.
        /// </summary>
        public List<YaMarketItem> items { get; set; }

        /// <summary>
        /// Информация для определения опций доставки.
        /// </summary>
        public YaMarketDelivery delivery { get; set; }
    }

    public class YaMarketItem
    {
        public YaMarketItem()
        {
            
        }

        public YaMarketItem(YaMarketItem item)
        {
            feedId = item.feedId;
            offerId = item.offerId;
            feedCategoryId = item.feedCategoryId;
            offerName = item.offerName;
            count = item.count;
            price = item.price;
            delivery = item.delivery;
        }

        /// <summary>
        /// Идентификатор прайс-листа, в котором указан товар
        /// </summary>
        public float feedId { get; set; }

        /// <summary>
        /// Идентификатор товара из прайс-листа.
        /// </summary>
        public string offerId { get; set; }

        /// <summary>
        /// Идентификатор товарной категории из прайс-листа.
        /// </summary>
        [JsonIgnore]
        public string feedCategoryId { get; set; }

        /// <summary>
        /// Название товара.
        /// </summary>
        [JsonIgnore]
        public string offerName { get; set; }

        /// <summary>
        /// Количество товара, находящегося в корзине.
        /// </summary>
        public float count { get; set; }
        
        /// <summary>
        /// Актуальная цена товара в валюте корзины. Для отделения целой части от дробной используется точка.
        /// </summary>
        public float price { get; set; }

        /// <summary>
        /// Признак возможности доставки товара в указанный в запросе регион либо по указанному в запросе адресу. Возможные значения:
        /// false — товар не доставляется в указанный регион либо по указанному адресу;
        /// true — значение по умолчанию, товар доставляется в указанный регион либо по указанному адресу.
        /// </summary>
        public bool delivery { get; set; }
    }

    public class YaMarketDelivery
    {
        public YaMarketRegion region { get; set; }
        public YaMarketAddress address { get; set; }
    }

    public class YaMarketRegion
    {
        public int id { get; set; }
        public string name { get; set; }

        /// <summary>
        /// Тип региона. Возможные значения:
        /// REGION — регион;
        /// COUNTRY — страна;
        /// COUNTRY_DISTRICT — федеральный округ;
        /// SUBJECT_FEDERATION — субъект федерации;
        /// SUBJECT_FEDERATION_DISTRICT — район субъекта федерации;
        /// CITY — город;
        /// VILLAGE — поселок или село;
        /// CITY_DISTRICT — район города;
        /// SUBWAY_STATION — станция метро;
        /// OTHER — дополнительный тип для регионов, отличных от перечисленных.
        /// </summary>
        public string type { get; set; }

        public YaMarketRegion parent { get; set; }
    }

    public class YaMarketAddress
    {
        // Обязательные
        public string country { get; set; }
        public string city { get; set; }
        public string house { get; set; }

        // Необязательные
        public string postcode { get; set; }
        public string entrance { get; set; }
        public string entryphone { get; set; }
        public string apartment { get; set; }
        public string recipient { get; set; }
        public string phone { get; set; }
        public string street { get; set; }
        public string subway { get; set; }
        public string block { get; set; }
        public string floor { get; set; }
    }
    


    public class YaMarketCartResponse
    {
        public YaMarketCartResponse()
        {
            items = new List<YaMarketItem>();
            paymentMethods = new List<string>();
            deliveryOptions = new List<YaMarketDeliveryResponse>();
        }

        /// <summary>
        /// Товары в корзине.
        /// </summary>
        public List<YaMarketItem> items { get; set; }

        public List<string> paymentMethods { get; set; }

        public List<YaMarketDeliveryResponse> deliveryOptions { get; set; }
    }

    public class YaMarketDeliveryResponse
    {
        public string id { get; set; }
        public string type { get; set; }
        public string serviceName { get; set; }
        public float price { get; set; }
        public YaMarketDate dates { get; set; }
        public List<YaMarketOutlet> outlets { get; set; }
    }

    public struct YaMarketOutlet
    {
        public int id { get; set; }
    }

    public class YaMarketDate
    {
        public string fromDate { get; set; }
        public string toDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string reservedUntil { get; set; }
    }
}